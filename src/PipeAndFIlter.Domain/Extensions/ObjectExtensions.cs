namespace PipeAndFIlter.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetValue<T>(this object obj, string propertyName)
        {
            var propValue = default(T);
            if (obj != null)
            {
                propValue = (T)obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
            }
            return propValue;
        }
    }
}