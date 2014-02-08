using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.RenderContextProviders;
using SharpGL.WPF;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public class OpenGLRenderer:BaseRenderer
    {
        public OpenGL GL;
        private ICamera _camera;
        //private ShaderProgram _program;
       // private ShaderProgram _flatprogram;

        
        public OpenGLRenderer()
        {
            Name = "OpenGL";
            GL = new OpenGL();
            GL.Create(RenderContextType.FBO, 1, 1, 32, null);

        }

        //readonly uint[] _fbo = new uint[1];
        private int _count;

        private OctreeShader _octreeProgram;
        private FBO _fbo;
        private VBO _cubeBuffer;
        private VAO _cubes;

        private FlatShader _flatProgram;

        public override void Load(IModel model, ICamera camera, int width, int height)
        {
            _camera = camera;

            GL.MakeCurrent();
            GL.Enable(OpenGL.GL_TEXTURE_2D);
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

            _fbo = new FBO(GL, width,height);


            _cubes = new VAO(GL);
            _cubeBuffer = new VBO(GL);
            _octreeProgram = new OctreeShader(GL);

            _flatProgram = new FlatShader(GL);
            

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

            using (new Bind(_cubes))
            using (new Bind(_cubeBuffer))
            {
                _cubeBuffer.Update(vertices, vertices.Length * sizeof(float));
                const int stride = sizeof(float) * 8;
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, stride, new IntPtr(0));

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 4, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 3));

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 1, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 7));
                GL.BindVertexArray(0);
            }

            



            _flat = new VAO(GL);
            _flatBuffer = new VBO(GL);

            using (new Bind(_flat))
            using (new Bind(_flatBuffer))
            {
                var flatData = new float[] { -1, -1, 0, 0, 1, 1, -1, 0, 1, 1, 1, 1, 0, 1, 0, -1, 1, 0, 0, 0 };
                _flatBuffer.Update(flatData, flatData.Length * sizeof(float));
                const int stride = sizeof(float) * 5;
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, stride, new IntPtr(0));

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, OpenGL.GL_FLOAT, false, stride, new IntPtr(sizeof(float) * 3));

                GL.BindVertexArray(0);
            }

        }



        private VAO _flat;
        private VBO _flatBuffer;




        public override void Update(ICamera camera)
        {
            GL.MakeCurrent();
            _camera.Model *= Mat4.RotateX(Angle.FromDegrees(0.5)) * Mat4.RotateY(Angle.FromDegrees(0.6)) * Mat4.RotateZ(Angle.FromDegrees(0.8));


            using (new Bind(_octreeProgram))
            {
                _octreeProgram.Model = _camera.Model;
                _octreeProgram.View = _camera.View;
                _octreeProgram.Projection = _camera.Projection;
            }

        }

        public override ImageSource Render()
        {
            GL.MakeCurrent();

            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            GL.ClearColor(0.137f, 0.121f, 0.125f, 0f);

            using (new Bind(_fbo))
            using (new Bind(_octreeProgram))
            using (new Bind(_cubes))
            {
                GL.DrawArrays(OpenGL.GL_POINTS, 0, _count);
            }



            using (new Bind(_flatProgram))
            using (new Bind(_flat))
            using (new Bind(_fbo.ColorBindableTexture))
            {

                GL.DrawArrays(OpenGL.GL_QUADS, 0, 4);
            }


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
            _fbo.Resize(width, height);
        }
    }
}
