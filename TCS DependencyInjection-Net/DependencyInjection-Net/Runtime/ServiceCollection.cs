using System.Collections;
using System.Collections.Generic;
namespace TCS.DependencyInjection {
    public class ServiceCollection : IServiceCollection {
        readonly List<ServiceDescriptor> m_descriptors = new();

        public IEnumerator<ServiceDescriptor> GetEnumerator() => m_descriptors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(ServiceDescriptor item) => m_descriptors.Add(item);
        public void Clear() => m_descriptors.Clear();
        public bool Contains(ServiceDescriptor item) => m_descriptors.Contains(item);
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => m_descriptors.CopyTo(array, arrayIndex);
        public bool Remove(ServiceDescriptor item) => m_descriptors.Remove(item);
        public int Count => m_descriptors.Count;
        public bool IsReadOnly => false;
        public int IndexOf(ServiceDescriptor item) => m_descriptors.IndexOf(item);
        public void Insert(int index, ServiceDescriptor item) => m_descriptors.Insert(index, item);
        public void RemoveAt(int index) => m_descriptors.RemoveAt(index);

        public ServiceDescriptor this[int index] {
            get => m_descriptors[index];
            set => m_descriptors[index] = value;
        }
    }
}