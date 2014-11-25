
using Microsoft.Practices.Unity;

namespace FlexOffers.Importer.Common.DependencyInjection
{
    public interface ITypeRegistration
    {
        void Register(IUnityContainer container);
    }
}
