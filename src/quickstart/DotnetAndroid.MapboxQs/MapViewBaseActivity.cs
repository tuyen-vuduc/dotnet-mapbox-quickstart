namespace DotnetAndroid.MapboxQs;

public abstract class MapViewBaseActivity : AppCompatActivity
{
    protected MapView MapView { get; private set; }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        MapView = FindViewById<MapView>(Resource.Id.mapView);
    }

    protected override void OnStart()
    {
        base.OnStart();
        MapView?.OnStart();
    }

    protected override void OnStop()
    {
        base.OnStop();
        MapView?.OnStop();
    }

    public override void OnLowMemory()
    {
        base.OnLowMemory();
        MapView?.OnLowMemory();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MapView?.OnDestroy();
    }
}