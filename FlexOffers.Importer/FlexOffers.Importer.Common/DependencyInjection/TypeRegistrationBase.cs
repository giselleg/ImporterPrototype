
using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Linq;


namespace FlexOffers.Importer.Common.DependencyInjection
{
    public abstract class TypeRegistrationBase:ITypeRegistration
    {
        #region Implementation of ITypeRegistration

        public void Register(IUnityContainer container)
        {
            Register(container, new List<Type>());
        }
        #endregion

        protected virtual void Register(IUnityContainer container, List<Type> registeredTypes)
        {
            Type thisType = this.GetType();

            // Get all dependencies defined via the TypeRegistrationDependency attribute, and register those first.
            var dependencyTypes = (thisType.GetCustomAttributes(typeof(TypeRegistrationDependency), true) as TypeRegistrationDependency[]).Select(d => d.DependencyType);
            Register(container, registeredTypes, dependencyTypes);

            // Tehn, register the registrations defined in this class.
            Register(container, registeredTypes, RegisterTypes(container));

            // Finally, add this type registration implementation to the list of registered types.
            registeredTypes.Add(thisType);
        }

        protected void Register(IUnityContainer container, List<Type> registeredTypes, IEnumerable<Type> typesToRegister)
        {
            if (typesToRegister == null || typesToRegister.Any() == false) return;

            foreach (var type in typesToRegister)
            {
                if (!registeredTypes.Any(t => t == type))
                {
                    TypeRegistrationBase registration = ObjectBuilder.Build(type) as TypeRegistrationBase;
                    registration.Register(container, registeredTypes);
                }
            }
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The list of registered types.</returns>
        protected abstract Type[] RegisterTypes(IUnityContainer container);
    }
}
