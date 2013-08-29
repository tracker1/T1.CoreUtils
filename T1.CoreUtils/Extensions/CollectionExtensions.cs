using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T1.CoreUtils
{
    public static class CollectionExtensions
    {
        public static List<int> AddRange(this List<int> items, int from, int to)
        {
            for (; from < to; from++) items.Add(from);
            for (; from >= to; from--) items.Add(from);
            return items;
        }
    }
}
