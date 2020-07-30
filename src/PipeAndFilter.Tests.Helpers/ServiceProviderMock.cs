using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeAndFilter.Tests.Helpers
{
    public static class ServiceProviderMockExtensions
    {
        public static IEnumerable<T> GetServices<T>(this ServiceProviderMock provider)
        {
            return new[]
            {
                new ServiceProviderPipelineMock<PipelineData, PipelineResult>(nameof(PersonPipeline)),
                new ServiceProviderPipelineMock<PipelineData, PipelineResult>(nameof(DuplicatePersonCheckerPipeline))
            }.Cast<T>();
        }
    }

    public class ServiceProviderMock : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return this.GetServices<IPipeline<PipelineData, PipelineResult>>();
        }
    }

    public class ServiceProviderPipelineMock<T, TU> : IPipeline<T, TU>
    {
        public ServiceProviderPipelineMock(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Task Do(T data, TU result)
        {
            throw new NotImplementedException();
        }

        public Task Undo(T pipelineData, TU pipelineResult)
        {
            throw new NotImplementedException();
        }
    }
}