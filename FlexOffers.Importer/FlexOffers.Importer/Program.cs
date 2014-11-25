using FlexOffers.Importer.Common.DependencyInjection;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace FlexOffers.Importer.Service.Processor.Host
{
    class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<IProcessorService>(s => InitService(s));
                x.EnablePauseAndContinue();
                x.EnableShutdown();
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription(
                    "Enables Hawk Eye processing of incoming events and baselines, triggering notifications as configured.");
                //x.SetDisplayName(componentName);
                //x.SetServiceName(componentName.Replace(" ", string.Empty));
            });
        }

        private static void InitService(ServiceConfigurator<IProcessorService> s)
        {
            s.ConstructUsing(() =>
            {
                ApplicationContainer.Current.RegisterTypes<TypeRegistration>();
                return ApplicationContainer.Current.Resolve<IProcessorService>();
            });
            s.WhenStarted(theService => theService.Start());
            s.WhenStopped(theService => theService.Stop());
            s.WhenPaused(theService => theService.Pause());
            s.WhenContinued(theService => theService.Continue());
            s.WhenShutdown(theService => theService.ShutDown());
        }
    }
}
