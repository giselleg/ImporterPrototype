
using System;
using System.Diagnostics;

namespace FlexOffers.Importer.Common.DependencyInjection
{
    /// <summary>
    /// Defines an attribute to declare type registration dependencies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TypeRegistrationDependency:Attribute
    {
        private const string TypeRegistrationDependencyAssertFormat = "Class {0} is not deriving from ITypeRegistration.";
        private static readonly Type TypeRegistrationType = typeof(ITypeRegistration);

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistrationDependency"/> class.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        public TypeRegistrationDependency(Type dependencyType)
        {
            this.DependencyType = dependencyType;
        }

        public Type DependencyType { get; private set; }

        
    }
}
