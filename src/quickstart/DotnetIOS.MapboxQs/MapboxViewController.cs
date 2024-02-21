using MapboxCoreMaps;
using MapboxMapsObjC;

namespace DotnetIOS.MapboxQs;

public partial class MapboxViewController : UIViewController
{
    // const string MAPBOX_ACCESS_TOKEN = "YOUR_MAPBOX_ACCESS_TOKEN";

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        MapboxCommon.MBXMapboxOptions.SetAccessTokenForToken(MAPBOX_ACCESS_TOKEN);

        // Use all default options, except access_token
        var mapboxOptions = new MBMMapOptions(null, null, null, null, null, null, 1, null);

        var options = MapInitOptionsFactory.CreateWithMapOptions(
            mapboxOptions, null, null, null, 0
            );

        var mapView = MapViewFactory.CreateWithFrame(View.Bounds, options);
        mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

        //mapView.LoadStyleWithStyleUriString(BuiltInStyles.Standard, new MBMStylePackLoadOptions(), null, null);

        View.AddSubview(mapView);
    }
}

