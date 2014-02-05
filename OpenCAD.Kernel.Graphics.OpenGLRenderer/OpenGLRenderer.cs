using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private ShaderProgram _program;
        private ShaderProgram _flatprogram;

        
        public OpenGLRenderer()
        {
            Name = "OpenGL";
            GL = new OpenGL();
            GL.Create(RenderContextType.FBO, 1, 1, 32, null);

        }

        readonly uint[] _vbo = new uint[2];
        readonly uint[] _vao = new uint[2];
        readonly uint[] _fbo = new uint[1];
        private int _count;

        public struct ShaderUniforms
        {
            public int Model;
            public int View;
            public int Projection;
        };

        private ShaderUniforms _uniforms;

        public override void Load(IModel model, ICamera camera)
        {
            _camera = camera;

            GL.MakeCurrent();

            GL.Enable(OpenGL.GL_CULL_FACE);
            GL.Enable(OpenGL.GL_DEPTH_TEST);
            GL.Enable(OpenGL.GL_BLEND);
            GL.Enable(OpenGL.GL_VERTEX_ARRAY);


            GL.Hint(HintTarget.LineSmooth, HintMode.Nicest);
            GL.Enable(OpenGL.GL_LINE_SMOOTH);
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendFunc(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.OneMinusSourceAlpha);

            GL.Enable(OpenGL.GL_MULTISAMPLE);

            GL.MinSampleShading(4.0f);


            GL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Filled);

            GL.GenFramebuffersEXT(1,_fbo);
            GL.GenVertexArrays(2, _vao);
            GL.GenBuffers(2, _vbo);


            _program = GenOctProgram();
            _flatprogram = GenFlatProgram();
            

            var list = new List<float>();

            var octreeModel = model as OctreeModel;
            if (octreeModel != null)
            {
                var filled = octreeModel.Node.Flatten().Where(o => o.State == NodeState.Filled).ToArray();
                _count = filled.Length;

                foreach (var octreeNode in filled)
                {
                    list.AddRange(octreeNode.Center.ToArray().Select(d=>(float)d));

                    list.AddRange(new[]
                        {
                            (float)MathsHelper.Map(octreeNode.Color.R, 0, 255, 0, 1),
                            (float)MathsHelper.Map(octreeNode.Color.G, 0, 255, 0, 1), 
                            (float)MathsHelper.Map(octreeNode.Color.B, 0, 255, 0, 1), 
                            (float)MathsHelper.Map(octreeNode.Color.A, 0, 255, 0, 1), 
                            (float)octreeNode.Size
                        });
                }
            }
            var vertices = list.ToArray();



            GL.BindVertexArray(_vao[0]);

 
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _vbo[0]);
            var pointer = GCHandle.Alloc(vertices, GCHandleType.Pinned).AddrOfPinnedObject();
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices.Length * sizeof(float), pointer, OpenGL.GL_STATIC_DRAW);

            const int stride = sizeof(float) * 8;
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, stride, new IntPtr(0));
            
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 3));
            
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 1, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 7));
            GL.BindVertexArray(0);





            GenFlat();



        }


        private ShaderProgram GenOctProgram()
        {
            var program =  new ShaderProgram();
            var vertex = new VertexShader();
            var fragment = new FragmentShader();
            var geo = new GeometryShader();

            vertex.CreateInContext(GL);
            vertex.SetSource(File.ReadAllText("Shaders/Octree.vert.glsl"));
            vertex.Compile();

            fragment.CreateInContext(GL);
            fragment.SetSource(File.ReadAllText("Shaders/Octree.frag.glsl"));
            fragment.Compile();

            geo.CreateInContext(GL);
            geo.SetSource(File.ReadAllText("Shaders/Octree.geo.glsl"));
            geo.Compile();

            program.CreateInContext(GL);
            program.AttachShader(vertex);
            program.AttachShader(fragment);
            program.AttachShader(geo);
            program.Link();

            Debug.WriteLine(program.InfoLog);
            foreach (var attachedShader in program.AttachedShaders)
            {
                Debug.WriteLine(attachedShader.InfoLog);
            }

            _uniforms.Model = GL.GetUniformLocation(program.ProgramObject, "Model");
            _uniforms.View = GL.GetUniformLocation(program.ProgramObject, "View");
            _uniforms.Projection = GL.GetUniformLocation(program.ProgramObject, "Projection");
            return program;
        }

        private ShaderProgram GenFlatProgram()
        {
            var program = new ShaderProgram();
            var vertex = new VertexShader();
            var fragment = new FragmentShader();

            vertex.CreateInContext(GL);
            vertex.SetSource(File.ReadAllText("Shaders/Flat.vert.glsl"));
            vertex.Compile();

            fragment.CreateInContext(GL);
            fragment.SetSource(File.ReadAllText("Shaders/Flat.frag.glsl"));
            fragment.Compile();

            program.CreateInContext(GL);
            program.AttachShader(vertex);
            program.AttachShader(fragment);
            program.Link();

            Debug.WriteLine(program.InfoLog);
            foreach (var attachedShader in program.AttachedShaders)
            {
                Debug.WriteLine(attachedShader.InfoLog);
            }
            return program;
        }

        private void GenFlat()
        {
            GL.BindVertexArray(_vao[1]);
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _vbo[1]);
            var flatData = new float[] { -1, -1, 0, 0, 1, 1, -1, 0, 1, 1, 1, 1, 0, 1, 0, -1, 1, 0, 0, 0 };
            var flatPointer = GCHandle.Alloc(flatData, GCHandleType.Pinned).AddrOfPinnedObject();
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, flatData.Length * sizeof(float), flatPointer, OpenGL.GL_STATIC_DRAW);

            const int stride = sizeof(float) * 5;
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, stride, new IntPtr(0));

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 3));

            GL.BindVertexArray(0);
        }





        public override void Update(ICamera camera)
        {
            GL.MakeCurrent();
            _camera.Model *= Mat4.RotateX(Angle.FromDegrees(0.5)) * Mat4.RotateY(Angle.FromDegrees(0.6)) * Mat4.RotateZ(Angle.FromDegrees(0.8));
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

    //        GL.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, _fbo[0]);

            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            GL.ClearColor(0.137f, 0.121f, 0.125f, 0f);

            _program.Push(GL, null);
            GL.BindVertexArray(_vao[0]);
            GL.DrawArrays(OpenGL.GL_POINTS, 0, _count);
            GL.BindVertexArray(0);
            _program.Pop(GL, null);
       //     GL.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, 0);



            //_flatprogram.Push(GL,null);
            //GL.BindVertexArray(_vao[1]);
            //GL.Disable(OpenGL.GL_DEPTH_TEST);
            //GL.DrawArrays(OpenGL.GL_QUADS, 0, 4);
            //GL.BindVertexArray(0);
            //_flatprogram.Pop(GL,null);


            // Draw cube scene here

            // Bind default framebuffer and draw contents of our framebuffer
            //
            //glBindVertexArray(vaoQuad);
            //
            //glUseProgram(screenShaderProgram);

            //glActiveTexture(GL_TEXTURE0);
            //glBindTexture(GL_TEXTURE_2D, texColorBuffer);

            //glDrawArrays(GL_TRIANGLES, 0, 6);








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
        public GeometryShader()
        {
            Name = "Geometry Shader";
        }
        public override void CreateInContext(OpenGL gl)
        {
            ShaderObject = gl.CreateShader(0x8DD9);
            CurrentOpenGLContext = gl;
        }
    }
}
