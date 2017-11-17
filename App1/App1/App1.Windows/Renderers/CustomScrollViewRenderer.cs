using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using App1.ViewModel;
using App1.Windows.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;
using Xamarin.Forms.PlatformConfiguration;

[assembly: ExportRenderer(typeof(TLScrollView), typeof(CustomScrollViewRenderer))]
namespace App1.Windows.Renderers
{
    public class CustomScrollViewRenderer : ViewRenderer<TLScrollView, ScrollViewer>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TLScrollView> e)
        {
            base.OnElementChanged(e);
            Element.Render();
        }
    }
}
