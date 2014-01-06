using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenCAD.Kernel.References;

namespace OpenCAD.Kernel.Features
{
    public abstract class BaseFeature:IFeature
    {
        protected List<IFeatureReference> _references;
        public string Name { get; private set; }
        public bool Regenerated { get; private set; }
        public RegenResult RegenStatus { get; private set; }

        public IEnumerable<IFeatureReference> References
        {
            get { return _references; }
        }

        protected BaseFeature()
        {
            _references = new List<IFeatureReference>();
            RegenStatus = RegenResult.Failed;
        }

        public abstract RegenResult Regenerate();

        public void MakeDirty()
        {
            Regenerated = false;
        }

        public async Task<RegenResult> RegenerateAsync()
        {

            if (Regenerated)
            {
                return RegenStatus;
            }
            else
            {
                                
                var referenceResults = await Task.WhenAll(References.Select(r => r.Parent.RegenerateAsync()));
                if (referenceResults.Any(r => r == RegenResult.Failed)) return RegenResult.Failed;
                RegenStatus = await Task.Run(() => Regenerate());
                Regenerated = true;
                return RegenStatus;
            }


            //if (!Regenerated)
            //{

            //    foreach (var featureReference in References)
            //    {
            //        await featureReference.Parent.RegenerateAsync();
            //    }
            //    var response = Task.Run(() =>
            //        {
            //            Thread.Sleep(1000);
            //            return Regenerate();
            //        });
            //    Regenerated = await response;
            //}
        }
    }
}