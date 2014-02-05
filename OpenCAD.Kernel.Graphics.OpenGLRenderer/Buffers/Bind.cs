using System;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers
{
    public class Bind : IDisposable
    {
        private readonly IBuffer _asset;
        public Bind(IBuffer asset)
        {
            _asset = asset;
            _asset.Bind();
        }

        public void Dispose()
        {
            _asset.UnBind();
        }
        public static Bind Asset(IBuffer asset)
        {
            return new Bind(asset);
        }
    }
}