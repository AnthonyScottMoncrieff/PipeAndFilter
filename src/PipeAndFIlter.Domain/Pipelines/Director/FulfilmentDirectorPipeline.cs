using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;
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

        public string Name => nameof(FulfilmentDirectorPipeline);

        public async Task Do(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }

        public async Task Undo(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }
    }
}