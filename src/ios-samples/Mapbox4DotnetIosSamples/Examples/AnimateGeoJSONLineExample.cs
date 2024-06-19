using CoreLocation;
using MapboxMapsObjC;

namespace Mapbox4DotnetIosSamples;

public class AnimateGeoJSONLineExample : UIViewController
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        CLLocationCoordinate2D centerLocation = new (45.5076, -122.6736);

        TMBCameraOptions cameraOptions = new (centerLocation, new(0, 0, 0, 0), new(0, 0), 11, 0 ,0);

        var options = MapInitOptionsFactory.CreateWithMapOptions(
            null, cameraOptions, null, null, 0
            );

        var mapView = MapViewFactory.CreateWithFrame(View.Bounds, options);
        mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

        //mapView.LoadStyleWithStyleUriString(BuiltInStyles.Standard, new MBMStylePackLoadOptions(), null, null);

        View.AddSubview(mapView);
    }
}

