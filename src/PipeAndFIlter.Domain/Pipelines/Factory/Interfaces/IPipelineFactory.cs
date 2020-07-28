using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System.Collections.Generic;

namespace PipeAndFIlter.Domain.Pipelines.Factory.Interfaces
{
    public interface IPipelineFactory
    {
        IEnumerable<IPipeline<PipelineData, PipelineResult>> GetOrderedPipelines();
    }
}