using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.References;

namespace OpenCAD.Kernel.Features
{
    public interface IFeature
    {
        string Name { get; }
        bool Regenerate();
        bool Regenerated { get; }
        void MakeDirty();
        Task RegenerateAsync();
        IEnumerable<IFeatureReference> References { get; }
    }

    public abstract class BaseFeature:IFeature
    {
        protected List<IFeatureReference> _references;
        public string Name { get; private set; }
        public bool Regenerated { get; private set; }

        public IEnumerable<IFeatureReference> References
        {
            get { return _references; }
        }

        protected BaseFeature()
        {
            _references = new List<IFeatureReference>();
        }

        public abstract bool Regenerate();

        public void MakeDirty()
        {
            Regenerated = false;
        }

        public async Task RegenerateAsync()
        {
            if (!Regenerated)
            {
                await Task.Delay(1000);
                foreach (var featureReference in References)
                {
                    await featureReference.Parent.RegenerateAsync();
                }
                var response = Task.Run(() => Regenerate());
                Regenerated = await response;
            }
        }
    }
    public class DatumOrigin : BaseFeature
    {
        public override bool Regenerate()
        {
            Console.WriteLine(" > Regen Origin");
            
            return true;
        }
    }
    public class DatumPlane:BaseFeature
    {
        public DatumPlane(IEnumerable<IFeatureReference> references)
        {
            _references.AddRange(references);
        }
        public override bool Regenerate()
        {
            Console.WriteLine(" > Regen Plane");

            return true;
        }
    }

    public static class Empty<T>
    {
        public static Task<T> Task { get { return _task; } }

        private static readonly Task<T> _task = System.Threading.Tasks.Task.FromResult(default(T));
    }
}
