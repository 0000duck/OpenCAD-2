using System.Diagnostics;
using System.IO;
using OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers;
using OpenCAD.Kernel.Maths;
using SharpGL;
using SharpGL.SceneGraph.Shaders;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public class OctreeShader:ShaderBindable
    {
        private readonly int _modelLocation;
        private readonly int _viewLocation;
        private readonly int _projectionLocation;

        public Mat4 Model
        {
            set
            {
                _gl.UniformMatrix4(_modelLocation, 1, false, value.ToColumnMajorArrayFloat());
            }
        }
        public Mat4 View
        {
            set
            {
                _gl.UniformMatrix4(_viewLocation, 1, false, value.ToColumnMajorArrayFloat());
            }
        }

        public Mat4 Projection
        {
            set
            {
                _gl.UniformMatrix4(_projectionLocation, 1, false, value.ToColumnMajorArrayFloat());
            }
        }



        public OctreeShader(OpenGL gl) : base(gl)
        {
            _program = new ShaderProgram();
            var vertex = new VertexShader();
            var fragment = new FragmentShader();
            var geo = new GeometryShader();

            vertex.CreateInContext(gl);
            vertex.SetSource(File.ReadAllText("Shaders/Octree.vert.glsl"));
            vertex.Compile();

            fragment.CreateInContext(gl);
            fragment.SetSource(File.ReadAllText("Shaders/Octree.frag.glsl"));
            fragment.Compile();

            geo.CreateInContext(gl);
            geo.SetSource(File.ReadAllText("Shaders/Octree.geo.glsl"));
            geo.Compile();

            _program.CreateInContext(gl);
            _program.AttachShader(vertex);
            _program.AttachShader(fragment);
            _program.AttachShader(geo);
            _program.Link();

            Debug.WriteLine(_program.InfoLog);
            foreach (var attachedShader in _program.AttachedShaders)
            {
                Debug.WriteLine(attachedShader.InfoLog);
            }

            _modelLocation = gl.GetUniformLocation(_program.ProgramObject, "Model");
            _viewLocation = gl.GetUniformLocation(_program.ProgramObject, "View");
            _projectionLocation = gl.GetUniformLocation(_program.ProgramObject, "Projection");
        }
    }
}