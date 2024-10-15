using UnityEngine;
namespace TCS.DependencyInjection.Tests {
    [DefaultExecutionOrder(-1000)]
    public class GameManager : MonoBehaviour {
        ServiceProvider m_serviceProvider;

        void Awake() // Changed Start to Awake for earlier execution
        {
            // Create a new service collection
            IServiceCollection services = new ServiceCollection();

            // Register the IWeapon service with the Sword implementation
            services.AddSingleton<IWeapon, Sword>();

            // Build the service provider
            m_serviceProvider = services.BuildServiceProvider() as ServiceProvider;

            // Inject dependencies into relevant objects
            InjectDependencies();
        }

        void InjectDependencies() {
            MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var obj in allObjects) {
                if (obj is not IDependencyInjectionListener listener) continue;
                m_serviceProvider.InjectServiceAttributes(obj);
                listener.OnDependenciesInjected();
            }
        }
    }
}