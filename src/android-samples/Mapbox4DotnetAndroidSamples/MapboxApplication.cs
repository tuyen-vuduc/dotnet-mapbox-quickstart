
using Android.OS;
using Android.Runtime;

namespace Mapbox4DotnetAndroidSamples;

[Application(
    Theme = "@style/AppTheme",
    UsesCleartextTraffic = true,
    AllowBackup = true,
    Label = "@string/app_name",
    SupportsRtl = true)]
public class MapboxApplication : Application
{
    protected MapboxApplication(
        System.IntPtr javaReference,
        JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();

        InitializeStrictMode();
    }

    private void InitializeStrictMode()
    {
        /*
         StrictMode.setThreadPolicy(
          StrictMode.ThreadPolicy.Builder()
            .detectAll()
            .build()
        )
        StrictMode.setVmPolicy(
          StrictMode.VmPolicy.Builder()
            .detectLeakedSqlLiteObjects()
            .penaltyLog()
            .penaltyDeath()
            .build()
        )
         */
        StrictMode.SetThreadPolicy(
            new StrictMode.ThreadPolicy.Builder()
                .DetectAll()
                .Build()
            );
        StrictMode.SetVmPolicy(
            new StrictMode.VmPolicy.Builder()
                .DetectLeakedSqlLiteObjects()
                .PenaltyLog()
                .PenaltyDeath()
                .Build()
            );
    }
}
