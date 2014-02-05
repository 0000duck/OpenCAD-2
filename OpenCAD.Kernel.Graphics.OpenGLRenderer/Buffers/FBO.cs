using System;
using SharpGL;
using SharpGL.Enumerations;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers
{
    public class FBO:IBuffer
    {
        private readonly OpenGL _gl;
        readonly uint[] _fbo = new uint[1];
        readonly uint[] _renderBuffer = new uint[1];

        public Texture ColorTexture;

        public FBO(OpenGL gl, int width, int height)
        {
            _gl = gl;
            _gl.GenFramebuffersEXT(1, _fbo);

            ColorTexture = new Texture(_gl);
            using (new Bind(this))
            using (new Bind(ColorTexture))
            {
                _gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                _gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
                _gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
                _gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
                _gl.TexImage2D(OpenGL.GL_TEXTURE_2D,0,OpenGL.GL_RGBA8,width,height,0,OpenGL.GL_RGBA8,OpenGL.GL_UNSIGNED_BYTE, IntPtr.Zero);
                _gl.FramebufferTexture2DEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_COLOR_ATTACHMENT0_EXT, OpenGL.GL_TEXTURE_2D, ColorTexture.Handle, 0);


                _gl.GenRenderbuffersEXT(1, _renderBuffer);
                _gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, _renderBuffer[0]);
                _gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_DEPTH_COMPONENT32, width, height);
                _gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_DEPTH_ATTACHMENT_EXT, OpenGL.GL_RENDERBUFFER_EXT, _renderBuffer[0]);

            }

        }

        public void Resize(int width, int height)
        {
            using (new Bind(this))
            using (new Bind(ColorTexture))
            {
                _gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA8, width, height, 0, OpenGL.GL_RGBA8, OpenGL.GL_UNSIGNED_BYTE, IntPtr.Zero);
            }
        }

        public void Bind()
        {
            _gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, _fbo[0]);
        }

        public void UnBind()
        {
            _gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, 0);
        }
    }
}