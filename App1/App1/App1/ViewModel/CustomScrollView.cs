using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections;


namespace App1.ViewModel
{
    public class TLScrollView : ScrollView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(TLScrollView), default(IEnumerable));

       // private object _SelectedItem;
        //public object SelectedItem
        //{
        //    get { return _SelectedItem; }
        //    set
        //    {
        //        if (_SelectedItem != null)
        //        {
                    
        //            ((View) _SelectedItem).BackgroundColor = ((View) value).BackgroundColor;
                    
                   
        //        }
        //        _SelectedItem = value;
        //        ((View)_SelectedItem).BackgroundColor = Color.Aquamarine;
        //    }
        //}


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(TLScrollView), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public void Render()
        {
            if (this.ItemTemplate == null || this.ItemsSource == null)
                return;

            var layout = new StackLayout();
            layout.Orientation = this.Orientation == ScrollOrientation.Vertical
                ? StackOrientation.Vertical : StackOrientation.Horizontal;
            layout.VerticalOptions=LayoutOptions.Center;
            layout.HorizontalOptions = LayoutOptions.Center;
            layout.WidthRequest = 500;
            foreach (var item in this.ItemsSource)
            {
                var viewCell = this.ItemTemplate.CreateContent() as ViewCell;
                viewCell.View.HorizontalOptions = LayoutOptions.Center;
                viewCell.View.BindingContext = item;
             
                var a= new TapGestureRecognizer();
                a.Tapped += AOnTapped;
                viewCell.View.GestureRecognizers.Add(a);
                layout.Children.Add(viewCell.View);
            }

            this.Content = layout;
        }

        private void AOnTapped(object sender, EventArgs eventArgs)
        {
           // SelectedItem = sender;
            var _parent = ((ListView) this.Parent.Parent);
            _parent.SelectedItem = sender;
        }
    }
}
