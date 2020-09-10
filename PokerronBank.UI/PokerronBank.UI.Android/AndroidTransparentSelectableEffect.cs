using System;
using Android.Util;
using Xamarin.Forms.Platform.Android;

namespace PokerronBank.UI.Droid
{
    public class AndroidTransparentSelectableEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                TypedValue value = new TypedValue();
                Android.App.Application.Context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, value, true);
                Control.SetBackgroundResource(value.ResourceId);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {

        }
    }
}