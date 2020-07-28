using System;
using System.Collections.Generic;
using System.Text;

namespace PipeAndFIlter.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetValue<T>(this object obj, string propertyName)
        {
            var foo = default(T);
            if (obj != null)
            {
                foo = (T)obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
            }
            return foo;
        }
    }
}
