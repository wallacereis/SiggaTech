using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using SiggaTechAPP.Droid.CustomRenderer;
using SiggaTechAPP.CustomRenderer;
using Android.Content;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace SiggaTechAPP.Droid.CustomRenderer
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        public RoundedEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(10f);
                gradientDrawable.SetStroke(1, Android.Graphics.Color.ParseColor("#064169"));
                gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(20, 10, 0, 0);
            }
        }
    }
}