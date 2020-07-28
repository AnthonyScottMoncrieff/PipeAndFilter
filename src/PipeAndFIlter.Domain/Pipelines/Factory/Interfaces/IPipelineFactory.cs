using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipeAndFIlter.Domain.Pipelines.Factory.Interfaces
{
    public interface IPipelineFactory
    {
        IEnumerable<IPipeline<PipelineData, PipelineResult>> GetOrderedPipelines();
    }
}
