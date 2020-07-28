namespace PipeAndFIlter.Domain.Converters.Interfaces
{
    public interface IModelConverter<in TIn, out TOut>
    {
        TOut Convert(TIn model);
    }
}