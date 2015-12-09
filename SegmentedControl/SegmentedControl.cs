using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace SegmentedControl
{
    public class SegmentedControl : View, IViewContainer<SegmentedControlOption>
    {
        public IList<SegmentedControlOption> Children { get; set; }

        public SegmentedControl()
        {
            Children = new List<SegmentedControlOption>();
        }

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create<SegmentedControl, string>(s => s.SelectedValue, default(string), defaultBindingMode:BindingMode.TwoWay);

        public string SelectedValue
        {
            get { return (string)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly BindableProperty TintColorProperty = BindableProperty.Create<SegmentedControl, Color>(s => s.TintColor, Color.Default);

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }
    }

    public class SegmentedControlOption:View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create<SegmentedControlOption, string>(p => p.Text, "");

        public string Text {
            get{ return (string)GetValue(TextProperty); }
            set{ SetValue(TextProperty, value); }
        }

        public SegmentedControlOption()
        {
        }
    }
}