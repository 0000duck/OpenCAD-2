using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenCAD.Kernel.References;

namespace OpenCAD.Kernel.Features
{
    public interface IFeature
    {
        string Name { get; }
        RegenResult Regenerate();
        bool Regenerated { get; }
        RegenResult RegenStatus { get; }
        void MakeDirty();
        Task<RegenResult> RegenerateAsync();
        IEnumerable<IFeatureReference> References { get; }
    }

    public enum RegenResult {Failed = 0, Successful = 1}

    public class DatumOrigin : BaseFeature
    {
        public override RegenResult Regenerate()
        {
            Console.WriteLine(" > Regen Origin");
            Thread.Sleep(5000);
            return RegenResult.Successful;
        }
    }
    public class DatumPlane:BaseFeature
    {
        public DatumPlane(IEnumerable<IFeatureReference> references)
        {
            _references.AddRange(references);
        }
        public override RegenResult Regenerate()
        {
            Console.WriteLine(" > Regen Plane");

            return RegenResult.Successful;
        }
    }

    public static class Empty<T>
    {
        public static Task<T> Task { get { return _task; } }

        private static readonly Task<T> _task = System.Threading.Tasks.Task.FromResult(default(T));
    }
}
