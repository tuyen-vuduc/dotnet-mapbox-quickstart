using AndroidX.AppCompat.App;
using Com.Mapbox.Maps;

namespace DotnetAndroid.MapboxQs;

[Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
public class MainActivity : AppCompatActivity
{
    MapView mapView;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        mapView = FindViewById<MapView>(Resource.Id.mapView);
    }

    protected override void OnStart()
    {
        base.OnStart();
        mapView?.OnStart();
    }

    protected override void OnStop()
    {
        base.OnStop();
        mapView?.OnStop();
    }

    public override void OnLowMemory()
    {
        base.OnLowMemory();
        mapView?.OnLowMemory();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        mapView?.OnDestroy();
    }
}
