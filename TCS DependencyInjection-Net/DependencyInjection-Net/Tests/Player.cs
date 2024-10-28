using TCS.DependencyInjection.Net;
using UnityEngine;
namespace TCS.DependencyInjection.Tests {
    public class SomePlayer : MonoBehaviour, IDependencyInjectionListener
    {
        [Service] // This will automatically inject the weapon service
        public IWeapon Weapon { get; set; }

        // Called after dependencies are injected
        public void OnDependenciesInjected()
        {
            Debug.Log("Player's dependencies injected successfully.");
            Attack();
        }

        public void Attack()
        {
            if (Weapon != null)
            {
                Weapon.Use();
            }
            else
            {
                Debug.LogWarning("No weapon assigned.");
            }
        }
    }

}