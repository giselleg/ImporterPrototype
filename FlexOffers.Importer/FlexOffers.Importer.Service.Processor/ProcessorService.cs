using System;
using FlexOffers.Importer.Engine;

namespace FlexOffers.Importer.Service.Processor
{
    public class ProcessorService :IProcessorService
    {
        private readonly IEngineManager _engineManager;

        public ProcessorService(IEngineManager engineManager)
        {
            _engineManager = engineManager;
        }

        #region Implementation of IProcessorService

        public void Start()
        {
            Execute(() => _engineManager.Start());
        }

        public void Stop()
        {
            Execute(() => _engineManager.Stop());
        }

        public void Pause()
        {
            Execute(() => _engineManager.Stop());
        }

        public void Continue()
        {
            Execute(() => _engineManager.Start());
        }

        public void ShutDown()
        {
            Execute(() => _engineManager.Stop());
        }

        #endregion

        private void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                // log exception.
            }
        }
    }
}
