using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using Xamarin.Essentials;

namespace VeciHelpAPK.Droid
{
    [Activity(Label = "VeciHelp", Icon = "@mipmap/Logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            IsPlayServicesAvailable();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            bool isGooglePlayService = resultCode != ConnectionResult.Success;
            Preferences.Set("isGooglePlayService", isGooglePlayService);
        }

        // Confirm with dialog
        public override void OnBackPressed()
        {
            if (((VeciHelpAPK.App)App.Current).PromptToConfirmExit)
            {
                using (var alert = new AlertDialog.Builder(this))
                {
                    alert.SetTitle("Salir de la aplicación");
                    alert.SetMessage("Esta seguro que desea salir?");
                    alert.SetPositiveButton("SI", (sender, args) => { FinishAffinity(); }); // inform Android that we are done with the activity
                    alert.SetNegativeButton("No", (sender, args) => { }); // do nothing

                    var dialog = alert.Create();
                    dialog.Show();
                }
                return;
            }
            base.OnBackPressed();
        }
    }
}