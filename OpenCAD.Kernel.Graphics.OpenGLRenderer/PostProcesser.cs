using System;
using OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers;
using SharpGL;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public class PostProcesser
    {
        private readonly OpenGL _gl;
        private FBO _fbo;
        private FlatShader _flatProgram;

        private VAO _flat;
        private VBO _flatBuffer;

        public PostProcesser(OpenGL gl,int width,int height)
        {
            _gl = gl;
            _fbo = new FBO(gl, width, height);

            _flatProgram = new FlatShader(gl);

            _flat = new VAO(gl);
            _flatBuffer = new VBO(gl);

            using (new Bind(_flat))
            using (new Bind(_flatBuffer))
            {
                var flatData = new float[] { -1, -1, 1, -1, -1, 1, 1, 1, };
                _flatBuffer.Update(flatData, flatData.Length * sizeof(float));
                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 2, OpenGL.GL_FLOAT, false, 0, new IntPtr(0));
                gl.BindVertexArray(0);
            }
        }


        public void Resize(int width, int height)
        {
            _fbo.Resize(width, height);
        }

        public void Capture(Action func)
        {
            using (new Bind(_fbo))
            {
                func();
            }
        }

        public void Render()
        {
            using (new Bind(_flatProgram))
            using (new Bind(_flat))
            using (new Bind(_fbo.ColorTexture))
            {
                _gl.DrawArrays(OpenGL.GL_TRIANGLE_STRIP, 0, 4);
            }
        }
    }
}