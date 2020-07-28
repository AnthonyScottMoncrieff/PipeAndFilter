using Microsoft.Extensions.DependencyInjection;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Extensions;
using PipeAndFIlter.Domain.Pipelines.Factory.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Helpers;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PipeAndFIlter.Domain.Pipelines.Factory
{
    public class PipelineFactory : IPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public PipelineFactory(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IEnumerable<IPipeline<PipelineData, PipelineResult>> GetOrderedPipelines()
        {
            try
            {
                var fulfilmentPipelines = _serviceProvider.GetServices<IPipeline<PipelineData, PipelineResult>>();
                var fulfilmentPipelineNames =
                    PipelineSequenceHelper.FulfilmentPipelines.Select(x => x.GetValue<string>("Name"));

                return fulfilmentPipelines
                    .Where(x => fulfilmentPipelineNames.Contains(x.Name))
                    .SortBy(fulfilmentPipelineNames, x => x.Name);
            }
            catch (Exception ex)
            {
                _logger.AddErrorDetail(ex.Message);
                _logger.SubmitException(ex);
                throw;
            }
        }
    }
}
