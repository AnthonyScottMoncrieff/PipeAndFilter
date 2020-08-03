using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Pipelines
{
    public class AddressPipeline : IPipeline<PipelineData, PipelineResult>
    {
        public string Name => nameof(AddressPipeline);

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
