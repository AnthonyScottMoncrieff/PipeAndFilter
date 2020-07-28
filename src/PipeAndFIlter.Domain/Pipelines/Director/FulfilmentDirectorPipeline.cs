using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Factory.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Pipelines.Director
{
    public class FulfilmentDirectorPipeline : IPipelineDirector<PipelineData, PipelineResult>
    {
        private IEnumerable<IPipeline<PipelineData, PipelineResult>> _stepsToProcess;

        private readonly Stack<IPipeline<PipelineData, PipelineResult>> _processedSteps;
        private readonly ILogger _logger;

        public string Name => nameof(FulfilmentDirectorPipeline);

        public FulfilmentDirectorPipeline(ILogger logger, IPipelineFactory pipelineFactory)
        {
            _logger = logger;
            _stepsToProcess = pipelineFactory.GetOrderedPipelines();
        }

        public async Task Do(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            FilterStepsToProcess(pipelineData);

            foreach(var step in _stepsToProcess)
            {
                _logger.AddMessageDetail($"Processing step: {step.Name}");
                _processedSteps.Push(step);
                await step.Do(pipelineData, pipelineResult);
                _logger.AddMessageDetail($"Finished Processing step: {step.Name}");
            }
        }

        public async Task Undo(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }

        private void FilterStepsToProcess(PipelineData pipelineData)
        {
            //Add conditions to filter steps
        }
    }
}