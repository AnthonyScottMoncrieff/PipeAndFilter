using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;

namespace PipeAndFIlter.Domain.Pipelines.Filter.Interfaces
{
    public interface IFulfilmentPipelinesFilter
    {
        IFulfilmentPipelinesFilter PopulateSteps(IEnumerable<IPipeline<PipelineData, PipelineResult>> steps);

        IFulfilmentPipelinesFilter WhenPersonExists(Func<bool> FilterCondition);

        IEnumerable<IPipeline<PipelineData, PipelineResult>> Filter();
    }
}