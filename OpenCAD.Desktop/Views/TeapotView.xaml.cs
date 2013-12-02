using System;
using System.Diagnostics;
using System.Windows.Controls;
using Awesomium.Core;

namespace OpenCAD.Desktop.Views
{
    /// <summary>
    /// Interaction logic for TeapotView.xaml
    /// </summary>
    public partial class TeapotView : UserControl
    {
        public TeapotView()
        {
            InitializeComponent();


        }

        private void webControl_NativeViewInitialized(object sender, WebViewEventArgs e)
        {
     
        }

        private void webControl_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
         Debug.WriteLine(e.Message);   
        }
    }
}
