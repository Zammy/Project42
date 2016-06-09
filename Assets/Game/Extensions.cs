using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Extensions
{
    public static void xAddUpTo<T> (this List<T> list, int count)
    {
        for (int i = list.Count; i <= count; i++)
        {
            list.Add(default(T));
        }
    }
}

