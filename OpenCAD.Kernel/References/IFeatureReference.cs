using OpenCAD.Kernel.Features;

namespace OpenCAD.Kernel.References
{
    public interface IFeatureReference
    {
        IFeature Parent { get; }
    }
    public class FeatureReference:IFeatureReference
    {
        public IFeature Parent { get; private set; }
        public FeatureReference(IFeature parent)
        {
            Parent = parent;
        }
    }
}
