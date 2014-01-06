using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenCAD.Kernel.Features
{
    public interface IFeatureFactory
    {
        IEnumerable<IFeature> Features { get; }
        Task<IEnumerable<RegenResult>> RegenerateAsync();
    }
}