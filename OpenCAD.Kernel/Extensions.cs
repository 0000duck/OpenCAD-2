using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenCAD.Kernel.Structure;

namespace OpenCAD.Kernel
{
    public static class Extensions
    {
        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        public static IEnumerable<dynamic> DynamicSelect(this object source, Func<dynamic, dynamic> map)
        {
            foreach (dynamic item in source as dynamic)
                yield return map(item);
        }
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
        {
            return e.SelectMany(c => f(c).Flatten(f)).Concat(e);
        }

        public static IEnumerable<IProjectItem> Flatten(this IProject project)
        {
            return project.Items.Flatten(n => n.Items);
        }
    }
}
