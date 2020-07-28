using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeAndFIlter.Domain.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<TResult> SortBy<TResult, TKey>(
            this IEnumerable<TResult> itemsToSort,
            IEnumerable<TKey> sortKeys,
            Func<TResult, TKey> matchFunc)
        {
            return sortKeys.Join(itemsToSort,
                key => key,
                matchFunc,
                (key, iitem) => iitem);
        }
    }
}