using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_PCL.Util
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> key)
        {
            HashSet<TKey> uqKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (uqKeys.Add(key(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
