using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> AsPage<T>(this IEnumerable<T> items, int skip = 0, int take = 20){

            if(items == null){
                throw new ArgumentNullException(nameof(items));
            }

            return items.Skip(skip).Take(take);
        }
    }
}