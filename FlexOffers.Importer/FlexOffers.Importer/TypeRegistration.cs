
using System;
using FlexOffers.Importer.Common.DependencyInjection;
using Microsoft.Practices.Unity;

namespace FlexOffers.Importer.Service.Processor.Host
{
    [TypeRegistrationDependency(typeof(FlexOffers.Importer.Service.Processor.TypeRegistration))]
    public class TypeRegistration :TypeRegistrationBase
    {
        #region Overrides of TypeRegistrationBase

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The list of registered types.</returns>
        protected override Type[] RegisterTypes(IUnityContainer container)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
