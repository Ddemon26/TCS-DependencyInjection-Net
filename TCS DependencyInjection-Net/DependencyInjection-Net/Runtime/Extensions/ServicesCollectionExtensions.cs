using System;
namespace TCS.DependencyInjection {
    public static class ServicesCollectionExtensions {
        public static IServiceCollection AddSingleton<T>(this IServiceCollection serviceCollection)
            where T : class {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection.AddSingleton(typeof(T));
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class
            where TImplementation : class, TService {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection.AddSingleton(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddSingleton(this IServiceCollection serviceCollection, Type serviceType) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            return serviceCollection.AddSingleton(serviceType, serviceType);
        }

        public static IServiceCollection AddSingleton(this IServiceCollection serviceCollection, Type serviceType, Type implementationType) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            return Add(serviceCollection, serviceType, implementationType, ServiceLifetime.Singleton);
        }

        public static void TryAddSingleton<TService>(this IServiceCollection serviceCollection)
            where TService : class {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            TryAddSingleton(serviceCollection, typeof(TService), typeof(TService));
        }

        public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class
            where TImplementation : class, TService {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            TryAddSingleton(serviceCollection, typeof(TService), typeof(TImplementation));
        }

        static void TryAddSingleton(IServiceCollection serviceCollection, Type serviceType, Type implementationType) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            var descriptor = ServiceDescriptor.Singleton(serviceType, implementationType);
            TryAdd(serviceCollection, descriptor);
        }

        public static IServiceCollection AddTransient<T>(this IServiceCollection serviceCollection)
            where T : class {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection.AddTransient(typeof(T));
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class
            where TImplementation : class, TService {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection.AddTransient(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection, Type serviceType) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            return serviceCollection.AddTransient(serviceType, serviceType);
        }

        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection, Type serviceType, Type implementationType) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            return Add(serviceCollection, serviceType, implementationType, ServiceLifetime.Transient);
        }

        public static IServiceProvider BuildServiceProvider(this IServiceCollection serviceCollection) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            return new ServiceProvider(serviceCollection);
        }

        static void TryAdd(IServiceCollection serviceCollection, ServiceDescriptor descriptor) {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            int count = serviceCollection.Count;
            for (var i = 0; i < count; i++) {
                if (serviceCollection[i].ServiceType == descriptor.ServiceType)
                    return;
            }

            serviceCollection.Add(descriptor);
        }

        static IServiceCollection Add(IServiceCollection serviceCollection, Type serviceType, Type implementationType, ServiceLifetime lifetime) {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            serviceCollection.Add(descriptor);
            return serviceCollection;
        }
    }
}