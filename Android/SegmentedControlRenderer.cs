using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using SegmentedControl.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Resource = BiblePlus.Droid.Resource;

[assembly: ExportRenderer(typeof (SegmentedControl.SegmentedControl), typeof (SegmentedControlRenderer))]

namespace SegmentedControl.Android
{
    public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, RadioGroup>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
        {
            base.OnElementChanged(e);

            var layoutInflater = (LayoutInflater) Context.GetSystemService(Context.LayoutInflaterService);

            var g = new RadioGroup(Context) {Orientation = Orientation.Horizontal};
            g.CheckedChange += (sender, eventArgs) =>
            {
                var rg = (RadioGroup) sender;
                if (rg.CheckedRadioButtonId == -1) return;
                var id = rg.CheckedRadioButtonId;
                var radioButton = rg.FindViewById(id);
                var radioId = rg.IndexOfChild(radioButton);
                var btn = (RadioButton) rg.GetChildAt(radioId);
                var selection = btn.Text;
                e.NewElement.SelectedValue = selection;
            };

            for (var i = 0; i < e.NewElement.Children.Count; i++)
            {
                var o = e.NewElement.Children[i];
                var v = (SegmentedControlButton) layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
                v.Text = o.Text;
                if (i == 0)
                    v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
                else if (i == e.NewElement.Children.Count - 1)
                    v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
                g.AddView(v);
            }

            SetNativeControl(g);
            SetChildChecked();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(SegmentedControl.SelectedValue))
            {
                SetChildChecked();
            }
        }

        private void SetChildChecked()
        {
            for (var i = 0; i < Control.ChildCount; i++)
            {
                var child = Control.GetChildAt(i) as RadioButton;
                if (child != null)
                {
                    child.Checked = child.Text == Element.SelectedValue;
                }
            }
        }
    }

    public class SegmentedControlButton : RadioButton
    {
        private int _lineHeightSelected;
        private int _lineHeightUnselected;

        private Paint _linePaint;

        public SegmentedControlButton(Context context) : this(context, null)
        {
        }

        public SegmentedControlButton(Context context, IAttributeSet attributes)
            : this(context, attributes, Resource.Attribute.segmentedControlOptionStyle)
        {
        }

        public SegmentedControlButton(Context context, IAttributeSet attributes, int defStyle)
            : base(context, attributes, defStyle)
        {
            Initialize(attributes, defStyle);
        }

        private void Initialize(IAttributeSet attributes, int defStyle)
        {
            var a = Context.ObtainStyledAttributes(attributes, Resource.Styleable.SegmentedControlOption, defStyle,
                Resource.Style.SegmentedControlOption);

            var lineColor = a.GetColor(Resource.Styleable.SegmentedControlOption_lineColor, 0);
            _linePaint = new Paint {Color = lineColor};

            _lineHeightUnselected =
                a.GetDimensionPixelSize(Resource.Styleable.SegmentedControlOption_lineHeightUnselected, 0);
            _lineHeightSelected = a.GetDimensionPixelSize(Resource.Styleable.SegmentedControlOption_lineHeightSelected,
                0);

            a.Recycle();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (_linePaint.Color != 0 && (_lineHeightSelected > 0 || _lineHeightUnselected > 0))
            {
                var lineHeight = Checked ? _lineHeightSelected : _lineHeightUnselected;

                if (lineHeight > 0)
                {
                    var rect = new Rect(0, Height - lineHeight, Width, Height);
                    canvas.DrawRect(rect, _linePaint);
                }
            }
        }
    }
}