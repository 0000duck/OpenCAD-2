using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.RenderContextProviders;
using SharpGL.SceneGraph.Shaders;
using SharpGL.WPF;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public class OpenGLRenderer:BaseRenderer
    {
        public OpenGL GL;
        private ICamera _camera;
        private ShaderProgram _program = new ShaderProgram();

        
        public OpenGLRenderer()
        {
            Name = "OpenGL";
            GL = new OpenGL();
            GL.Create(RenderContextType.FBO, 1, 1, 32, null);

        }


        uint[] vbo = new uint[1];

        private int count = 0;

        public struct ShaderUniforms
        {
            public int Model;
            public int View;
            public int Projection;
        };

        private ShaderUniforms _uniforms = new ShaderUniforms();

        public override void Load(IModel model, ICamera camera)
        {
            _camera = camera;

            GL.MakeCurrent();

            GL.Enable(OpenGL.GL_CULL_FACE);
            GL.Enable(OpenGL.GL_DEPTH_TEST);
            GL.Enable(OpenGL.GL_BLEND);
            GL.Enable(OpenGL.GL_VERTEX_ARRAY);

            GL.BlendFunc(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.OneMinusSourceAlpha);

            GL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);
            var vertex = new VertexShader();
            var fragment = new FragmentShader();
            var geo = new GeometryShader();

            vertex.CreateInContext(GL);
            vertex.SetSource(ShaderSource.Vertex);
            vertex.Compile();

            fragment.CreateInContext(GL);
            fragment.SetSource(ShaderSource.Fragment);
            fragment.Compile();

            geo.CreateInContext(GL);
            geo.SetSource(ShaderSource.Geometry);
            geo.Compile();

            _program.CreateInContext(GL);
            _program.AttachShader(vertex);
            _program.AttachShader(fragment);
            _program.AttachShader(geo);
            _program.Link();

            _uniforms.Model = GL.GetUniformLocation(_program.ProgramObject, "Model");
            _uniforms.View = GL.GetUniformLocation(_program.ProgramObject, "View");
            _uniforms.Projection = GL.GetUniformLocation(_program.ProgramObject, "Projection");



            var list = new List<float>();



            var octreeModel = model as OctreeModel;
            if (octreeModel != null)
            {
                var filled = octreeModel.Node.Flatten().Where(o => o.State == NodeState.Filled).ToArray();
                count = filled.Length;

                foreach (var octreeNode in filled)
                {
                    list.AddRange(octreeNode.Center.ToArray().Select(d=>(float)d));
                }
            }
            var vertices = list.ToArray();


            GL.GenBuffers(1, vbo);
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo[0]);
            var pointer = GCHandle.Alloc(vertices, GCHandleType.Pinned).AddrOfPinnedObject();
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices.Length * sizeof(float), pointer, OpenGL.GL_STATIC_DRAW);

        }
        Random rnd = new Random();
        public override void Update(ICamera camera)
        {
            GL.MakeCurrent();
            _camera.Model *= Mat4.RotateX(Angle.FromDegrees(1 + rnd.NextDouble()));
            _camera.Model *= Mat4.RotateY(Angle.FromDegrees(1 + rnd.NextDouble()));
            _camera.Model *= Mat4.RotateZ(Angle.FromDegrees(1 + rnd.NextDouble()));
            _program.Push(GL, null);
            {
                GL.UniformMatrix4(_uniforms.Model, 1, false, _camera.Model.ToColumnMajorArrayFloat());
                GL.UniformMatrix4(_uniforms.View, 1, false, _camera.View.ToColumnMajorArrayFloat());
                GL.UniformMatrix4(_uniforms.Projection, 1, false, _camera.Projection.ToColumnMajorArrayFloat()); 
            }
            _program.Pop(GL, null);
        }

        public override ImageSource Render()
        {
            GL.MakeCurrent();
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            GL.ClearColor(0.137f, 0.121f, 0.125f, 0f);
            var gl = GL;
            gl.PointSize(5);

            
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo[0]);
            gl.EnableVertexAttribArray(0);
            gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 0, new IntPtr(0));
            _program.Push(gl, null);

            gl.DrawArrays(OpenGL.GL_POINTS, 0, count);
            _program.Pop(gl, null);

            GL.Blit(IntPtr.Zero);
            var provider = GL.RenderContextProvider as FBORenderContextProvider;
            if (provider == null) return null;

            var newFormatedBitmapSource = new FormatConvertedBitmap();
            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = BitmapConversion.HBitmapToBitmapSource(provider.InternalDIBSection.HBitmap);
            newFormatedBitmapSource.DestinationFormat = PixelFormats.Rgb24;
            newFormatedBitmapSource.EndInit();
           return newFormatedBitmapSource;
        }

        public override void Resize(int width, int height)
        {
            GL.MakeCurrent();
            GL.SetDimensions(width, height);
            GL.Viewport(0, 0, width, height);
            
        }
    }

    public class GeometryShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpGL.SceneGraph.Shaders.FragmentShader"/> class.
        /// 
        /// </summary>
        public GeometryShader()
        {
            Name = "Geometry Shader";
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// 
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void CreateInContext(OpenGL gl)
        {
            ShaderObject = gl.CreateShader(0x8DD9);
            CurrentOpenGLContext = gl;
        }
    }
}
