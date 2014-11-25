
using System;
using System.Linq;
using Microsoft.Practices.Unity;

namespace FlexOffers.Importer.Common.DependencyInjection
{
    public class ApplicationContainer
    {
        private static ApplicationContainer current;

        static ApplicationContainer()
        {
            IUnityContainer container = new UnityContainer();
            current = container.Resolve<ApplicationContainer>();
        }

        public ApplicationContainer(IUnityContainer container)
        {
            this.Container = container;
        }

        public IUnityContainer Container { get; private set; }

        public static ApplicationContainer Current
        {
            get { return current; }
        }

        public void RegisterTypes<T>() where T : ITypeRegistration, new()
        {
            ITypeRegistration registration = new T();
            registration.Register(Container);
        }

        public T Resolve<T>()
        {
            return Current.Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return Current.Container.Resolve(type);
        }

        public Type GetRegisteredType(Type from)
        {
            Type result = null;
            var registration = Current.Container.Registrations.SingleOrDefault(r => r.RegisteredType == from);
            if (registration != null)
            {
                result = registration.MappedToType;
            }
            return result;
        }
    }
}
