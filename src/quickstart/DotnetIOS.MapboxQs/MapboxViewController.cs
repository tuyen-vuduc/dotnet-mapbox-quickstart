using MapboxCoreMaps;
using MapboxMapsObjC;

namespace DotnetIOS.MapboxQs;

public partial class MapboxViewController : UIViewController
{
    // const string MAPBOX_ACCESS_TOKEN = "YOUR_MAPBOX_ACCESS_TOKEN";

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        // Use all default options, except access_token
        var resouceOptions = new MBMResourceOptions(
            MAPBOX_ACCESS_TOKEN,
            null, null, null, null);
        var options = MapInitOptionsFactory.CreateWithResourceOptions(
            resouceOptions, null, null, null, null
            );

        var mapView = MapViewFactory.CreateWithFrame(View.Bounds, options);
        mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

        View.AddSubview(mapView);
    }
}

