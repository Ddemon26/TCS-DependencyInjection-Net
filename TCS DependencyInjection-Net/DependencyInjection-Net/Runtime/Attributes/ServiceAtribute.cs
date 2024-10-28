using System;

namespace TCS.DependencyInjection.Net {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ServiceAttribute : System.Attribute { }
}