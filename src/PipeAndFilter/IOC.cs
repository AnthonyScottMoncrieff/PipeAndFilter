﻿using Exceptionless;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PipeAndFilter.Logging;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain;
using PipeAndFIlter.Domain.Converters;
using PipeAndFIlter.Domain.Converters.Interfaces;
using PipeAndFIlter.Domain.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Director;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Factory;
using PipeAndFIlter.Domain.Pipelines.Factory.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Filter;
using PipeAndFIlter.Domain.Pipelines.Filter.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Helpers;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;

namespace PipeAndFilter
{
    public static class IOC
    {
        public static void RegisterDependencies(IServiceCollection services, IConfiguration config)
        {
            RegisterServices(services);
            RegisterConverters(services);
            RegisterPipelines(services);
            RegisterExceptionless(config);
            RegisterLogging(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IFulfilmentManager, FulfilmentManager>();
        }

        private static void RegisterPipelines(IServiceCollection services)
        {
            services.AddTransient<IPipelineDirector<PipelineData, PipelineResult>, FulfilmentDirectorPipeline>();
            foreach (Type pipeline in PipelineSequenceHelper.FulfilmentPipelines)
            {
                services.AddScoped(typeof(IPipeline<PipelineData, PipelineResult>), pipeline);
            }
            services.AddTransient<IPipelineFactory, PipelineFactory>();
            services.AddScoped<IFulfilmentPipelinesFilter, FulfilmentPipelinesFilter>();
        }

        private static void RegisterConverters(IServiceCollection services)
        {
            services.AddTransient<IModelConverter<RecievedOrder, PipelineData>, PipelineDataConverter>();
        }

        private static void RegisterExceptionless(IConfiguration configuration)
        {
            ExceptionlessClient.Default.Configuration.ApiKey =
                configuration["Exceptionless:ApiKey"];
            ExceptionlessClient.Default.Configuration.ServerUrl =
                configuration["Exceptionless:ServerUrl"];
        }

        private static void RegisterLogging(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILogger>(instance => new Logger(ExceptionlessClient.Default));
        }
    }
}