namespace Mapbox4DotnetIosSamples;

public class AnimateGeoJSONLineExample : UIViewController
{
    private readonly string sourceIdentifier = "route-source-identifier";
    private MapView mapView;
    private TMBGeoJSONSource routeLineSource;
    private int currentIndex = 0;
    private HashSet<TMBCancelable> cancelables = new();

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        CLLocationCoordinate2D centerLocation = new (45.5076, -122.6736);

        TMBCameraOptions cameraOptions = new (centerLocation, new(0, 0, 0, 0), new(0, 0), 11, 0 ,0);

        var options = MapInitOptionsFactory.CreateWithMapOptions(
            null, cameraOptions, null, null, 0
            );

        mapView = MapViewFactory.CreateWithFrame(View.Bounds, options);
        mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

        View.AddSubview(mapView);

        mapView.MapboxMap().OnMapLoaded((_) =>
        {
            AddLine();
            AnimatePolyline();
        });
    }

    private void AddLine()
    {
        // Create a GeoJSON data source.
        routeLineSource = new(sourceIdentifier);
        MBXGeometry geometry = new(allCoordinates[currentIndex]);
        MBXFeature feature = new(new(), geometry, new());
        routeLineSource.Data = TMBGeoJSONSourceData.FromFeature(feature);

        // Create a line layer
        TMBLineLayer lineLayer = new(@"line-layer", sourceIdentifier);
        lineLayer.LineColor = TMBValue.FromConstant(UIColor.Red);

        float lowZoomWidth = 5;
        float highZoomWidth = 20;

        // Use an expression to define the line width at different zoom extents
        TMBExpression expression = TMBExpression.Interpolate(new NSObject[] {
            TMBExpression.Linear(),
            TMBExpression.Zoom(),
            NSNumber.FromInt32(14),
            NSNumber.FromFloat(lowZoomWidth),
            NSNumber.FromInt32(18),
            NSNumber.FromFloat(highZoomWidth)
        });
        lineLayer.LineWidth = TMBValue.FromExpression(expression);
        lineLayer.LineCap = TMBValue.LineCap(TMBLineCap.Round);
        lineLayer.LineJoin = TMBValue.LineJoin(TMBLineJoin.Round);

        // Add the lineLayer to the map.
        mapView.MapboxMap().AddSource(routeLineSource, null, (NSError error) => {
            if (error != null)
            {
                Debug.WriteLine(@"addSource ERR: {0}", error.DebugDescription);
            }
        });
        mapView.MapboxMap().AddLayer(lineLayer, null, (NSError error) => {
            if (error != null)
            {
                Debug.WriteLine(@"addLayer ERR: {0}", error.DebugDescription);
            }
        });
    }

    private void AnimatePolyline()
    {
        // Start a timer that will add a new coordinate to the line and redraw it every time it repeats.
        NSTimer.CreateScheduledTimer(0.10, true, (NSTimer  timer) => {
            if (currentIndex >= allCoordinates.Length)
            {
                timer.Invalidate() ;
                return;
            }

            currentIndex++;

            NSValue[] currentCoordinates = allCoordinates.Take(currentIndex).ToArray();

            MBXGeometry geometry = new(currentCoordinates);
            MBXFeature feature = new(new(), geometry, new());

            routeLineSource.Data = TMBGeoJSONSourceData.FromFeature(feature);
            mapView.MapboxMap().UpdateGeoJSONSourceWithId(
                sourceIdentifier,
                TMBGeoJSONSourceData.FromFeature(feature),
                null);
        });
    }

    static readonly NSValue[] allCoordinates = {
        NSValue.FromMKCoordinate(new(45.52214, -122.63748)),
        NSValue.FromMKCoordinate(new(45.52218, -122.64855)),
        NSValue.FromMKCoordinate(new(45.52219, -122.6545)),
        NSValue.FromMKCoordinate(new(45.52196, -122.65497)),
        NSValue.FromMKCoordinate(new(45.52104, -122.65631)),
        NSValue.FromMKCoordinate(new(45.51935, -122.6578)),
        NSValue.FromMKCoordinate(new(45.51848, -122.65867)),
        NSValue.FromMKCoordinate(new(45.51293, -122.65872)),
        NSValue.FromMKCoordinate(new(45.51295, -122.66576)),
        NSValue.FromMKCoordinate(new(45.51252, -122.66745)),
        NSValue.FromMKCoordinate(new(45.51244, -122.66813)),
        NSValue.FromMKCoordinate(new(45.51385, -122.67359)),
        NSValue.FromMKCoordinate(new(45.51406, -122.67415)),
        NSValue.FromMKCoordinate(new(45.51484, -122.67481)),
        NSValue.FromMKCoordinate(new(45.51532, -122.676)),
        NSValue.FromMKCoordinate(new(45.51668, -122.68106)),
        NSValue.FromMKCoordinate(new(45.50934, -122.68503)),
        NSValue.FromMKCoordinate(new(45.50858, -122.68546)),
        NSValue.FromMKCoordinate(new(45.50783, -122.6852)),
        NSValue.FromMKCoordinate(new(45.50714, -122.68424)),
        NSValue.FromMKCoordinate(new(45.50585, -122.68433)),
        NSValue.FromMKCoordinate(new(45.50521, -122.68429)),
        NSValue.FromMKCoordinate(new(45.50445, -122.68456)),
        NSValue.FromMKCoordinate(new(45.50371, -122.68538)),
        NSValue.FromMKCoordinate(new(45.50311, -122.68653)),
        NSValue.FromMKCoordinate(new(45.50292, -122.68731)),
        NSValue.FromMKCoordinate(new(45.50253, -122.68742)),
        NSValue.FromMKCoordinate(new(45.50239, -122.6867)),
        NSValue.FromMKCoordinate(new(45.5026, -122.68545)),
        NSValue.FromMKCoordinate(new(45.50294, -122.68407)),
        NSValue.FromMKCoordinate(new(45.50271, -122.68357)),
        NSValue.FromMKCoordinate(new(45.50055, -122.68236)),
        NSValue.FromMKCoordinate(new(45.49994, -122.68233)),
        NSValue.FromMKCoordinate(new(45.49955, -122.68267)),
        NSValue.FromMKCoordinate(new(45.49919, -122.68257)),
        NSValue.FromMKCoordinate(new(45.49842, -122.68376)),
        NSValue.FromMKCoordinate(new(45.49821, -122.68428)),
        NSValue.FromMKCoordinate(new(45.49798, -122.68573)),
        NSValue.FromMKCoordinate(new(45.49805, -122.68923)),
        NSValue.FromMKCoordinate(new(45.49857, -122.68926)),
        NSValue.FromMKCoordinate(new(45.49911, -122.68814)),
        NSValue.FromMKCoordinate(new(45.49921, -122.68865)),
        NSValue.FromMKCoordinate(new(45.49905, -122.6897)),
        NSValue.FromMKCoordinate(new(45.49917, -122.69346)),
        NSValue.FromMKCoordinate(new(45.49902, -122.69404)),
        NSValue.FromMKCoordinate(new(45.49796, -122.69438)),
        NSValue.FromMKCoordinate(new(45.49697, -122.69504)),
        NSValue.FromMKCoordinate(new(45.49661, -122.69624)),
        NSValue.FromMKCoordinate(new(45.4955, -122.69781)),
        NSValue.FromMKCoordinate(new(45.49517, -122.69803)),
        NSValue.FromMKCoordinate(new(45.49508, -122.69711)),
        NSValue.FromMKCoordinate(new(45.4948, -122.69688)),
        NSValue.FromMKCoordinate(new(45.49368, -122.69744)),
        NSValue.FromMKCoordinate(new(45.49311, -122.69702)),
        NSValue.FromMKCoordinate(new(45.49294, -122.69665)),
        NSValue.FromMKCoordinate(new(45.49212, -122.69788)),
        NSValue.FromMKCoordinate(new(45.49264, -122.69771)),
        NSValue.FromMKCoordinate(new(45.49332, -122.69835)),
        NSValue.FromMKCoordinate(new(45.49334, -122.7007)),
        NSValue.FromMKCoordinate(new(45.49358, -122.70167)),
        NSValue.FromMKCoordinate(new(45.49401, -122.70215)),
        NSValue.FromMKCoordinate(new(45.49439, -122.70229)),
        NSValue.FromMKCoordinate(new(45.49566, -122.70185)),
        NSValue.FromMKCoordinate(new(45.49635, -122.70215)),
        NSValue.FromMKCoordinate(new(45.49674, -122.70346)),
        NSValue.FromMKCoordinate(new(45.49758, -122.70517)),
        NSValue.FromMKCoordinate(new(45.49736, -122.70614)),
        NSValue.FromMKCoordinate(new(45.49736, -122.70663)),
        NSValue.FromMKCoordinate(new(45.49767, -122.70807)),
        NSValue.FromMKCoordinate(new(45.49798, -122.70807)),
        NSValue.FromMKCoordinate(new(45.49798, -122.70717)),
        NSValue.FromMKCoordinate(new(45.4984, -122.70713)),
        NSValue.FromMKCoordinate(new(45.49893, -122.70774))
    };
}

