using System;
using System.Windows.Controls;
using System.Windows.Threading;
using OpenCAD.Desktop.ViewModels;
using OpenCAD.Kernel.Graphics;

namespace OpenCAD.Desktop.Views
{
    public partial class RendererView : UserControl
    {

        private readonly IRenderer _renderer;
        private readonly ICamera _camera;
        private DispatcherTimer _timer;

        public RendererView(IRenderer renderer, ICamera camera)
        {
            InitializeComponent();

         
            _renderer = renderer;
            //info.Text = "Renderer: " + _renderer.Name;
            _camera = camera;
            SizeChanged += (s, e) =>
                {
                    _renderer.Resize((int) e.NewSize.Width, (int) e.NewSize.Height);
                    _camera.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
                };
            _timer = new DispatcherTimer();
            _timer.Tick += Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
        }

        private void Tick(object sender, EventArgs e)
        {
            _renderer.Update(_camera);
            image.Source = _renderer.Render();
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _renderer.Load(((RendererViewModel)DataContext).Model, _camera,(int) Width,(int) Height);
            _renderer.Resize((int) Width,(int) Height);
            _timer.Start();
        }
    }
}
