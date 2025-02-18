﻿using Exceptionless.Logging;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Factory.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Filter.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Pipelines.Director
{
    public class FulfilmentDirectorPipeline : IPipelineDirector<PipelineData, PipelineResult>
    {
        private IEnumerable<IPipeline<PipelineData, PipelineResult>> _stepsToProcess;

        private readonly Stack<IPipeline<PipelineData, PipelineResult>> _processedSteps;
        private readonly ILogger _logger;
        private readonly IFulfilmentPipelinesFilter _fulfilmentPipelinesFilter;

        public string Name => nameof(FulfilmentDirectorPipeline);

        public FulfilmentDirectorPipeline(ILogger logger, IPipelineFactory pipelineFactory, IFulfilmentPipelinesFilter fulfilmentPipelinesFilter)
        {
            _logger = logger;
            _fulfilmentPipelinesFilter = fulfilmentPipelinesFilter;
            _stepsToProcess = pipelineFactory.GetOrderedPipelines();
            _processedSteps = new Stack<IPipeline<PipelineData, PipelineResult>>();
        }

        public async Task Do(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            FilterStepsToProcess(pipelineData);

            foreach (var step in _stepsToProcess)
            {
                _logger.AddMessageDetail($"Processing step: {step.Name}");
                _processedSteps.Push(step);
                await step.Do(pipelineData, pipelineResult);
                _logger.AddMessageDetail($"Finished Processing step: {step.Name}");
            }
        }

        public async Task Undo(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            _logger.SetLogLevel(LogLevel.Error);
            while (_processedSteps.Any())
            {
                var step = _processedSteps.Pop();

                _logger.AddMessageDetail($"Rolling back step: {step.Name}");

                await step.Undo(pipelineData, pipelineResult);

                _logger.AddMessageDetail($"Successfully rolled back step: {step.Name}");
            }
        }

        private void FilterStepsToProcess(PipelineData pipelineData)
        {
            _stepsToProcess = _fulfilmentPipelinesFilter
                .PopulateSteps(_stepsToProcess)
                .WhenPersonExists(() => pipelineData.Person.PersonId != default)
                .Filter();
        }
    }
}