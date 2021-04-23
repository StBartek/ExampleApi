using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<string> AddIfNotEmpty(this ICollection<string> list, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                list.Add(value);
            }
            return list;
        }

        public static ICollection<T> AddIf<T>(this ICollection<T> list, T value, Func<T, bool> func) 
        {
            if (func(value))
            {
                list.Add(value);
            }
            return list;
        }
    }
}
