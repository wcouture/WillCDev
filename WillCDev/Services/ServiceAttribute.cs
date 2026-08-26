namespace WillCDev.Services
{
    public enum ServiceType
    {
        Scoped,
        Singleton,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public Type InterfaceType { get; set; }
        public ServiceType ServiceType { get; set;  }
        public ServiceAttribute(Type interfaceType, ServiceType serviceType)
        {
            InterfaceType = interfaceType;
            ServiceType = serviceType;
        }
    }
}
