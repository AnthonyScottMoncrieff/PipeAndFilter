using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipeAndFIlter.Domain.Pipelines.Filter.Interfaces
{
    public interface IFulfilmentPipelinesFilter
    {
        FulfilmentPipelinesFilter CreateFilter(IEnumerable<IPipeline<PipelineData, PipelineResult>> steps);

        FulfilmentPipelinesFilter WhenPersonExists(Func<bool> FilterCondition);

        IEnumerable<IPipeline<PipelineData, PipelineResult>> Filter();
    }
}
