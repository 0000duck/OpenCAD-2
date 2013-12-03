using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCAD.Kernel.Features
{
    public class FeatureFactory : IFeatureFactory
    {
        public IEnumerable<IFeature> Features { get { return _features; } }
        private readonly IList<IFeature> _features;

        public FeatureFactory()
        {
            _features = new List<IFeature>();
        }

        public void Add(IFeature feature)
        {
            _features.Add(feature);
        }
        
        public async void RegenerateAsync()
        {
            foreach (var feature in _features)
            {
                feature.MakeDirty();
            }

            //await Task.WhenAll(_features.Select(f => f.RegenerateAsync()));
            foreach (var feature in _features)
            {
                await feature.RegenerateAsync();
            }
            //Task.WaitAny(_features.Select(f => f.RegenerateAsync()).ToArray());

        }
    }
}