using System.Collections.Generic;

namespace OpenCAD.Kernel.Features
{
    public interface IFeatureFactory
    {
        IEnumerable<IFeature> Features { get; }
        void RegenerateAsync();
    }
}