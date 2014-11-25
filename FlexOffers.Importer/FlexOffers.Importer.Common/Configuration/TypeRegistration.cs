
using System;
using FlexOffers.Importer.Common.DependencyInjection;
using Microsoft.Practices.Unity;

namespace FlexOffers.Importer.Common.Configuration
{
    public class TypeRegistration:TypeRegistrationBase
    {
        #region Overrides of TypeRegistrationBase

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The list of registered types.</returns>
        protected override Type[] RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IConfigurationProperties, ConfigurationPropertiesAdapter>();
            container.RegisterInstance<IConfiguration>(Acceller.Utility.Configuration.ConfigurationFactory.Instance, new ContainerControlledLifetimeManager());

            return null;
        }

        #endregion
    }
}
