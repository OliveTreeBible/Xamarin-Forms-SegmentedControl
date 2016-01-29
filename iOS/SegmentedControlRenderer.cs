using System;
using BiblePlus.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;

[assembly:ExportRenderer(typeof(SegmentedControl.SegmentedControl), typeof(SegmentedControl.iOS.SegmentedControlRenderer))]
namespace SegmentedControl.iOS
{
    public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, UISegmentedControl>
    {
        public SegmentedControlRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var segmentedControl = new UISegmentedControl();
                segmentedControl.ValueChanged += SegmentedControl_ValueChanged;

                SetNativeControl(segmentedControl);
            }

            Control.RemoveAllSegments();
            if (e.NewElement != null)
            {
                e.NewElement.Children.ForEach((c, i) =>
                {
                    Control.InsertSegment(c.Text, i, false);
                    if (c.Text == e.NewElement.SelectedValue)
                        Control.SelectedSegment = i;
                });

                if (e.NewElement.TintColor != Color.Default)
                    Control.TintColor = e.NewElement.TintColor.ToUIColor();
            }
        }

        private void SegmentedControl_ValueChanged(object sender, EventArgs e)
        {
            if (Element == null) return;
            Element.SelectedValue = Control.TitleAt(Control.SelectedSegment);
        }

        public override CGSize IntrinsicContentSize => Control.IntrinsicContentSize;

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SegmentedControl.SelectedValueProperty.PropertyName)
            {
                for (var i = 0; i < this.Control.NumberOfSegments; i++)
                {
                    if (Control.TitleAt(i) == Element.SelectedValue)
                    {
                        Control.SelectedSegment = i;
                        break;
                    }
                }
            }
            else if (e.PropertyName == SegmentedControl.TintColorProperty.PropertyName && Element.TintColor != Color.Default)
            {
                Control.TintColor = Element.TintColor.ToUIColor();
            }
        }
    }
}