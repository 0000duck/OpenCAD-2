﻿using System;
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




        private PostProcesser _postProcesser;


        private OctreeRenderer _octreeRenderer;
        private BackgroundRenderer _backgroundRenderer;

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

 
            _postProcesser = new PostProcesser(GL,width,height);
    

            _backgroundRenderer = new BackgroundRenderer(GL);

       

            var octreeModel = model as OctreeModel;
            if (octreeModel != null)
            {
                _octreeRenderer = new OctreeRenderer(octreeModel, GL);
            }

        }








        public override void Update(ICamera camera)
        {
            GL.MakeCurrent();
            _camera.Model *= Mat4.RotateX(Angle.FromDegrees(0.5)) * Mat4.RotateY(Angle.FromDegrees(0.6)) * Mat4.RotateZ(Angle.FromDegrees(0.8));

            if(_octreeRenderer != null)_octreeRenderer.Update(camera);
        }

        public override ImageSource Render()
        {
            GL.MakeCurrent();

            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            GL.ClearColor(1f, 0f, 0f, 0f);

            _postProcesser.Capture(() =>
            {
                GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                _backgroundRenderer.Render();
                _octreeRenderer.Render();
            });

            _postProcesser.Render();

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
            _postProcesser.Resize(width, height);
        }
    }
}
