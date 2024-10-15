using UnityEngine;
namespace TCS.DependencyInjection.Tests {
    public class Sword : IWeapon {
        public void Use() {
            Debug.Log("Swinging the sword!");
        }
    }
}