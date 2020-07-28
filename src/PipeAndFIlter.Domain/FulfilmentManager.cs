using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain.Converters.Interfaces;
using PipeAndFIlter.Domain.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;
using System;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain
{
    public class FulfilmentManager : IFulfilmentManager
    {
        private readonly IModelConverter<RecievedOrder, PipelineData> _orderDataConverter;
        private readonly IPipelineDirector<PipelineData, PipelineResult> _pipelineDirector;
        private readonly ILogger _logger;

        public FulfilmentManager(IModelConverter<RecievedOrder, PipelineData> orderDataConverter, IPipelineDirector<PipelineData, PipelineResult> pipelineDirector, ILogger logger)
        {
            _orderDataConverter = orderDataConverter;
            _pipelineDirector = pipelineDirector;
            _logger = logger;
        }

        public async Task<PipelineResult> Manage(RecievedOrder order)
        {
            _logger.AddMessageDetail("Beginning order processing");
            var pipelineData = _orderDataConverter.Convert(order);
            var pipelineResult = new PipelineResult();
            try
            {
                await _pipelineDirector.Do(pipelineData, pipelineResult);
            }
            catch(Exception ex)
            {
                _logger.AddErrorDetail(ex.Message);
                _logger.SubmitException(ex);
                await HandlePipelineException(pipelineData, pipelineResult);
            }
            return pipelineResult;
        }

        private async Task HandlePipelineException(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            try
            {
                await _pipelineDirector.Undo(pipelineData, pipelineResult);
            }
            catch (Exception ex)
            {
                _logger.AddErrorDetail(ex.Message);
                _logger.SubmitException(ex);
            }
        }
    }
}