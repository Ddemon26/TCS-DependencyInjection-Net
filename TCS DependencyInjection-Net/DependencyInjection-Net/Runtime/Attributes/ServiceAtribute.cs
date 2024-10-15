using System;

namespace TCS.DependencyInjection {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ServiceAttribute : Attribute { }
}