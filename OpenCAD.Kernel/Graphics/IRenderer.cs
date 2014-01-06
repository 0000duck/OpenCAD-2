using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCAD.Kernel.Modeling;
using Color = System.Drawing.Color;

namespace OpenCAD.Kernel.Graphics
{
    public interface IRenderer
    {
        string Name { get; }
        void Load(IModel model, ICamera camera);
        void Update(ICamera camera);
        ImageSource Render();
        void Resize(int width, int height);
    }
    public abstract class BaseRenderer:IRenderer
    {
        public string Name { get; protected set; }
        public abstract void Load(IModel model, ICamera camera);
        public abstract void Update(ICamera camera);
        public abstract ImageSource Render();
        public abstract void Resize(int width, int height);
    }

    public class NullRenderer : BaseRenderer
    {
        private int _width = 1;
        private int _height = 1;

        public NullRenderer()
        {
            Name = "Null Renderer";
        }

        public override void Load(IModel model, ICamera camera)
        {
            
        }

        public override void Update(ICamera camera)
        {

        }

        public override ImageSource Render()
        {
            var img = new Bitmap(_width, _height);
            using (var ms = new MemoryStream())
            using (var g = System.Drawing.Graphics.FromImage(img))
            {
                g.Clear(Color.Green);
                img.Save(ms, ImageFormat.Png);
                var bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(ms.ToArray());
                bImg.EndInit();
                return bImg;
            }
        }

        public override void Resize(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}