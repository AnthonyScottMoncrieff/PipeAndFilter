using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Pipelines.Interfaces
{
    public interface IPipeline<in TData, in TResult>
    {
        string Name { get; }

        Task Do(TData pipelineData, TResult pipelineResult);

        Task Undo(TData pipelineData, TResult pipelineResult);
    }
}