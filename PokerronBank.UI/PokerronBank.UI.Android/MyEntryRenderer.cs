using Android.Content;
using Android.Text.Method;
using PokerronBank.UI.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace PokerronBank.UI.Droid
{
    public class MyEntryRenderer : EntryRenderer
    {

        public MyEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.SetSelectAllOnFocus(true);
            }

          
        }
    }
}



