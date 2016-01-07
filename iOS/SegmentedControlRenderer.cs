using System;
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
            if (e.NewElement == null)
                return;

            var segmentedControl = new UISegmentedControl();

            for (var i = 0; i < e.NewElement.Children.Count; i++)
            {
                segmentedControl.InsertSegment(e.NewElement.Children[i].Text, i, false);
            }

            if (e.NewElement.TintColor != Color.Default)
                segmentedControl.TintColor = e.NewElement.TintColor.ToUIColor();

            segmentedControl.ValueChanged += (sender, eventArgs) => {
                e.NewElement.SelectedValue = segmentedControl.TitleAt(segmentedControl.SelectedSegment);
			};

            SetNativeControl(segmentedControl);
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                return Control.IntrinsicContentSize;
            }
        }

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