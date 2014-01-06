using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using OpenCAD.Desktop.ViewModels;
using OpenCAD.Kernel.Graphics;

namespace OpenCAD.Desktop.Views
{
    /// <summary>
    /// Interaction logic for RendererView.xaml
    /// </summary>
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
            _renderer.Load(((RendererViewModel)DataContext).Model, _camera);
            _renderer.Resize((int) Width,(int) Height);
            _timer.Start();
        }
    }
}
