using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;
using SharpGL;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public class OctreeRenderer : IOpenGLRenderer
    {
        private readonly OpenGL _gl;
        private OctreeShader _octreeProgram;
        private VBO _cubeBuffer;
        private VAO _cubes;
        private int _count;

        public OctreeRenderer(OctreeModel model, OpenGL gl)
        {
            _gl = gl;
            _octreeProgram = new OctreeShader(gl);
            _cubes = new VAO(gl);
            _cubeBuffer = new VBO(gl);


            var filled = model.Node.Flatten().Where(o => o.State == NodeState.Filled).ToArray();
            _count = filled.Length;
            var list = new List<float>();
            foreach (var octreeNode in filled)
            {
                list.AddRange(octreeNode.Center.ToArray().Select(d => (float)d));

                list.AddRange(new[]
                        {
                            (float)MathsHelper.Map(octreeNode.Color.R, 0, 255, 0, 1),
                            (float)MathsHelper.Map(octreeNode.Color.G, 0, 255, 0, 1), 
                            (float)MathsHelper.Map(octreeNode.Color.B, 0, 255, 0, 1), 
                            (float)MathsHelper.Map(octreeNode.Color.A, 0, 255, 0, 1), 
                            (float)octreeNode.Size
                        });
            }

            var vertices = list.ToArray();

            using (new Bind(_cubes))
            using (new Bind(_cubeBuffer))
            {
                _cubeBuffer.Update(vertices, vertices.Length * sizeof(float));
                const int stride = sizeof(float) * 8;
                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, stride, new IntPtr(0));
                
                
                gl.EnableVertexAttribArray(1);
                gl.VertexAttribPointer(1, 4, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 3));
                
                gl.EnableVertexAttribArray(2);

                gl.VertexAttribPointer(2, 1, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 7));
                gl.BindVertexArray(0);
            }


        }

        public void Update(ICamera camera)
        {
            using (new Bind(_octreeProgram))
            {
                _octreeProgram.Model = camera.Model;
                _octreeProgram.View = camera.View;
                _octreeProgram.Projection = camera.Projection;

            }
        }

        public void Render()
        {
            using (new Bind(_octreeProgram))
            using (new Bind(_cubes))
            {
        
                _gl.DrawArrays(OpenGL.GL_POINTS, 0, _count);

            }
        }
    }

    public class BackgroundRenderer : IOpenGLRenderer
    {
        private readonly OpenGL _gl;
        private VAO _flat;

        private VBO _flatBuffer;

        private BackgroundShader _program;
        public BackgroundRenderer(OpenGL gl)
        {
            _gl = gl;

            _program = new BackgroundShader(gl);
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
        public void Update(ICamera camera)
        {

        }

        public void Render()
        {
            using (new Bind(_program))
            using (new Bind(_flat))
            {
                _gl.Disable(OpenGL.GL_DEPTH_TEST);
                _gl.DrawArrays(OpenGL.GL_TRIANGLE_STRIP, 0, 4);
                _gl.Enable(OpenGL.GL_DEPTH_TEST);
            }
        }
    }
}
