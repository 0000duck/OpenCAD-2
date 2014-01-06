using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenCAD.Desktop.Misc;

namespace OpenCAD.Desktop
{
    public static class Extensions
    {

        public static IReadOnlyObservableCollection<TResult> WrapReadOnly<TSource, TResult>(this ObservableCollection<TSource> list) where TSource : TResult
        {
            return new MyReadOnlyObservableCollection<TSource, TResult>(list);
        }
    }

}