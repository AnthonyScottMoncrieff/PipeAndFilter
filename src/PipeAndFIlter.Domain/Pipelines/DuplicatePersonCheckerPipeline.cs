using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Pipelines
{
    public class DuplicatePersonCheckerPipeline : IPipeline<PipelineData, PipelineResult>
    {
        public string Name => nameof(DuplicatePersonCheckerPipeline);

        public Task Do(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }

        public Task Undo(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }
    }
}