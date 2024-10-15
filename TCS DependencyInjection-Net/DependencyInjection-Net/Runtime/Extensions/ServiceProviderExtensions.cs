using System;
using System.Reflection;
using UnityEngine;
namespace TCS.DependencyInjection {
    public static class ServiceProviderExtensions {
        public static void InjectServiceAttributes(this IServiceProvider serviceProvider, object target) {
            var type = target.GetType();

            // Inject into fields
            InjectMembers
            (
                serviceProvider, target,
                type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
                (field, instance) => field.SetValue(instance, serviceProvider.GetService(field.FieldType)),
                field => field.FieldType,
                field => field.Name, 
                field => !field.IsInitOnly
            );

            // Inject into properties
            InjectMembers
            (
                serviceProvider, target,
                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
                (property, instance) => property.SetValue(instance, serviceProvider.GetService(property.PropertyType)),
                property => property.PropertyType, 
                property => property.Name,
                property => property.CanWrite
            );
        }

        static void InjectMembers<T>(
            IServiceProvider serviceProvider,
            object target,
            T[] members,
            Action<T, object> injectAction,
            Func<T, Type> getTypeFunc,
            Func<T, string> getNameFunc,
            Func<T, bool> isWritable) where T : MemberInfo {

            foreach (var member in members) {
                if (Attribute.GetCustomAttribute(member, typeof(ServiceAttribute)) is not ServiceAttribute)
                    continue;

                var serviceType = getTypeFunc(member);
                object service = serviceProvider.GetService(serviceType);

                if (service != null && isWritable(member)) {
                    injectAction(member, target);
                    Debug.Log($"Injected {service.GetType().Name} into {target.GetType().Name}'s {getNameFunc(member)}");
                }
                else {
                    Debug.LogWarning($"Failed to inject service for {serviceType.Name} into {target.GetType().Name}'s {getNameFunc(member)}");
                }
            }
        }
        public static T GetService<T>(this IServiceProvider serviceProvider)
            where T : class {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            return serviceProvider.GetService(typeof(T)) as T;
        }

        public static T GetRequiredService<T>(this IServiceProvider serviceProvider)
            where T : class {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var service = serviceProvider.GetService<T>();

            Debug.Assert(service != null, "Service type not found in the service provider.");

            return service;
        }
    }
}