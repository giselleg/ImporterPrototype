
namespace FlexOffers.Importer.Service.Processor
{
    public interface IProcessorService
    {
        void Start();
        void Stop();
        void Pause();
        void Continue();
        void ShutDown();
    }
}
