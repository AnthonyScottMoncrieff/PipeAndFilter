using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Filter.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PipeAndFIlter.Domain.Pipelines.Filter
{
    public class FulfilmentPipelinesFilter : IFulfilmentPipelinesFilter
    {
        private IEnumerable<IPipeline<PipelineData, PipelineResult>> _steps;
        private static readonly List<string> StepsToExclude = new List<string>
        {
            nameof(PersonPipeline),
            nameof(AddressPipeline)
        };

        private FulfilmentPipelinesFilter(IEnumerable<IPipeline<PipelineData, PipelineResult>> steps)
        {
            _steps = steps;
        }

        public IFulfilmentPipelinesFilter CreateFilter(IEnumerable<IPipeline<PipelineData, PipelineResult>> steps)
        {
            return new FulfilmentPipelinesFilter(steps);
        }

        public IEnumerable<IPipeline<PipelineData, PipelineResult>> Filter()
        {
            return _steps;
        }

        public IFulfilmentPipelinesFilter WhenPersonExists(Func<bool> filterCondition)
        {
            if (filterCondition())
                _steps = _steps.Where(x => !StepsToExclude.Contains(x.Name));

            return this;
        }
    }
}
