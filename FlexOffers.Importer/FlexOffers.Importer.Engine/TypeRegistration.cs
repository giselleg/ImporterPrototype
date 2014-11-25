
using System;
using FlexOffers.Importer.Common.DependencyInjection;
using Microsoft.Practices.Unity;

namespace FlexOffers.Importer.Engine
{
    public class TypeRegistration : TypeRegistrationBase
    {
        #region Overrides of TypeRegistrationBase

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The list of registered types.</returns>
        protected override Type[] RegisterTypes(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            container.RegisterType<IEngineManager, EngineManager>(new ContainerControlledLifetimeManager()); // singleton
            return Type.EmptyTypes;
        }

        #endregion
    }
}
