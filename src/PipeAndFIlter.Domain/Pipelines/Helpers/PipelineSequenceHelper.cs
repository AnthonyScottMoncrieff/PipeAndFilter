namespace PipeAndFIlter.Domain.Pipelines.Helpers
{
    public static class PipelineSequenceHelper
    {
        public static object[] FulfilmentPipelines { get; } =
        {
            typeof(DuplicatePersonCheckerPipeline),
            typeof(PersonPipeline),
            typeof(AddressPipeline)
        };
    }
}