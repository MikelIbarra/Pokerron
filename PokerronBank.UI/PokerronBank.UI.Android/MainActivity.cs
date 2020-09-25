using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using PokerronBank.UI.Droid;
using PokerronBank.UI.Views.Helper;
using Sharpnado.Presentation.Forms.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: ResolutionGroupName("MobileClient")]
[assembly: ExportEffect(typeof(AndroidTransparentSelectableEffect), nameof(TransparentSelectableEffect))]
[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace PokerronBank.UI.Droid
{
    [Activity(Label = "Pokerron", Icon = "@mipmap/IconoPokerron", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            SharpnadoInitializer.Initialize();
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
      

    }
}