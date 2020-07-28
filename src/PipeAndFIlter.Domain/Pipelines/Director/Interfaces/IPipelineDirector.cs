using PipeAndFIlter.Domain.Pipelines.Interfaces;

namespace PipeAndFIlter.Domain.Pipelines.Director.Interfaces
{
    public interface IPipelineDirector<in TData, in TResult> : IPipeline<TData, TResult>
    {
    }
}