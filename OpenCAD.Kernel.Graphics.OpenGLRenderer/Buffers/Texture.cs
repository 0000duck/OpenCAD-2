using System;
using SharpGL;
using SharpGL.SceneGraph.Assets;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers
{
    public class BindableTexture:IBindable
    {
        private readonly OpenGL _gl;


        public uint Handle { get { return _texture[0]; } }
        readonly uint[] _texture = new uint[1];
        public BindableTexture(OpenGL gl)
        {
            _gl = gl;
            _gl.GenTextures(1, _texture);
        }
        public void Resize(int width, int height)
        {
            _gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA, width, height, 0, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, null);
        }
        public void Bind()
        {
            _gl.ActiveTexture(OpenGL.GL_TEXTURE0);
            _gl.BindTexture(OpenGL.GL_TEXTURE_2D, Handle);

        }

        public void UnBind()
        {
            _gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
        }
    }
}