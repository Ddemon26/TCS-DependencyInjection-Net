using System;
namespace TCS.DependencyInjection {
    public class ServiceDescriptor {
        public Type ImplementationType { get; private set; }
        public ServiceLifetime Lifetime { get; private set; }
        public Type ServiceType { get; private set; }

        ServiceDescriptor(Type serviceType, ServiceLifetime lifetime) {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }

        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
            : this(serviceType, lifetime) {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public static ServiceDescriptor Singleton(Type serviceType, Type implementationType)
            => Describe(serviceType, implementationType, ServiceLifetime.Singleton);

        static ServiceDescriptor Describe(Type serviceType, Type implementationType, ServiceLifetime lifetime)
            => new(serviceType, implementationType, lifetime);
    }
}