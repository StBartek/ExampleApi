using System;
using System.Linq;

namespace TestApi.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsAll(this string source, params string[] values)
        {
            return values.All(x => source.Contains(x, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
