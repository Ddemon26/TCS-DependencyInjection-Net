using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
namespace TCS.DependencyInjection {
    public sealed class ServiceProvider : IServiceProvider, IDisposable {
        readonly IServiceCollection m_services;
        readonly ConcurrentDictionary<Type, Func<object>> m_realizedServices;
        readonly ConcurrentDictionary<Type, object> m_singletons;
        internal bool Disposed { get; private set; }

        public ServiceProvider(IServiceCollection serviceCollection) {
            m_services = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            m_realizedServices = new ConcurrentDictionary<Type, Func<object>>();
            m_singletons = new ConcurrentDictionary<Type, object>();
        }

        Func<object> RealizeService(Type serviceType) {
            var desc = m_services.FirstOrDefault(d => d.ServiceType == serviceType);

            if (desc == null)
                throw new InvalidOperationException($"No service registered for type {serviceType.FullName}.");

            ConstructorInfo[] constructors = desc.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
                throw new InvalidOperationException
                (
                    $"The type {desc.ImplementationType.FullName} doesn't contain any public constructor."
                );

            var bestConstructor = constructors.FirstOrDefault(IsValidConstructor);

            if (bestConstructor == null)
                throw new InvalidOperationException
                (
                    $"The type {desc.ImplementationType.FullName} doesn't contain any valid constructor."
                );

            return () =>
            {
                if (desc.Lifetime != ServiceLifetime.Singleton) return ConstructAndInject(bestConstructor);
                if (!m_singletons.ContainsKey(serviceType))
                    m_singletons[serviceType] = ConstructAndInject(bestConstructor);
                return m_singletons[serviceType];

            };
        }

        object ConstructAndInject(ConstructorInfo bestConstructor) {
            object service = bestConstructor.Invoke(GetConstructorParameters(bestConstructor));
            var serviceType = service.GetType();

            foreach (var field in serviceType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (field.GetCustomAttribute<ServiceAttribute>() != null)
                    field.SetValue(service, GetService(field.FieldType));
            }

            foreach (var property in serviceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (property.GetCustomAttribute<ServiceAttribute>() != null)
                    property.SetValue(service, GetService(property.PropertyType));
            }

            if (service is IDependencyInjectionListener listener)
                listener.OnDependenciesInjected();

            return service;
        }

        object[] GetConstructorParameters(ConstructorInfo info) {
            ParameterInfo[] parameters = info.GetParameters();
            var result = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++) {
                var parameter = parameters[i];
                result[i] = GetService(parameter.ParameterType);
            }

            return result;
        }

        bool IsValidConstructor(ConstructorInfo info)
            => info
                .GetParameters()
                .Aggregate
                (
                    true, (current, param)
                        => current & IsValidConstructorParameter(param)
                );

        bool IsValidConstructorParameter(ParameterInfo param)
            => m_services.Any(service => service.ServiceType.IsAssignableFrom(param.ParameterType));

        public object GetService(Type serviceType) {
            if (Disposed)
                throw new ObjectDisposedException($"The {nameof(ServiceProvider)} object has already been disposed.");

            var desc = m_services.FirstOrDefault(d => d.ServiceType == serviceType);

            if (desc == null)
                throw new InvalidOperationException($"Unable to find Service Descriptor for {serviceType.FullName}.");

            Func<object> realizedService = m_realizedServices.GetOrAdd(serviceType, RealizeService);
            return realizedService?.Invoke();
        }

        public void Dispose() {
            if (Disposed)
                return;

            m_realizedServices.Clear();
            m_singletons.Clear();
            Disposed = true;
        }
    }
}