using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Content;
using System.Net;
using Android;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps.Android;

namespace XamarinTileWmsMap.Droid
{
    [Activity(Label = "XamarinTileWmsMap", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int RequestLocationId = 100;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            await TryToGetPermissions();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Internet) == (int)Permission.Granted)
            {
                ServicePointManager
                .ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
            }
            else
            {
                // Internet permission is not granted. If necessary display rationale & request.
            }

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig); // initialize for Xamarin.Forms.GoogleMaps

            LoadApplication(new App());
        }

        private async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }
        }

        private readonly string[] PermissionsGroupLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.RecordAudio,
            Manifest.Permission.ModifyAudioSettings,
            Manifest.Permission.WakeLock,
            Manifest.Permission.InstantAppForegroundService,
            Manifest.Permission.Internet,
            Manifest.Permission.AccessWifiState
        };

        private async Task GetPermissionsAsync()
        {
            //const string permission = Manifest.Permission.AccessFineLocation;
            foreach (var permission in PermissionsGroupLocation)
            {
                if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
                {
                    continue;
                }
                else
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                    break;
                }
            }

        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}