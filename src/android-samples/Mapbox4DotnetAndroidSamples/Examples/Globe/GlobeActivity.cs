using Com.Mapbox.Functions;
using Com.Mapbox.Geojson;
using Com.Mapbox.Maps;
using Com.Mapbox.Maps.Extension.Style;
using Com.Mapbox.Maps.Extension.Style.Atmosphere.Generated;
using Com.Mapbox.Maps.Extension.Style.Layers.Properties.Generated;
using Com.Mapbox.Maps.Extension.Style.Projection.Generated;

namespace Mapbox4DotnetAndroidSamples.Examples.Globe;

[Activity(
    Label = "@string/activity_globe", 
    Description = "@string/description_3d_globe",
    Exported = true, 
    Theme = "@style/AppTheme")]
[MetaData("@string/category", Value = "@string/category_globe")]
public class GlobeActivity : AppCompatActivity
{
    private const double ZOOM = 0.45;
    private static readonly Point CENTER = Point.FromLngLat(30.0, 50.0);

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var mapView = new MapView(this);
        SetContentView(mapView);

        // mapView.mapboxMap.apply {
        var mapboxMap = mapView.MapboxMap;

        /*
         setCamera(
            cameraOptions {
              center(CENTER)
              zoom(ZOOM)
            }
          )
        */
        mapboxMap.SetCamera(new CameraOptions.Builder()
            .Center(CENTER)
            .Zoom(new Java.Lang.Double(ZOOM))
            .Build());

        /*
         loadStyle(
            style(Style.SATELLITE_STREETS) {
              +atmosphere { }
              +projection(ProjectionName.GLOBE)
            }
          )
         */
        mapboxMap.LoadStyle(
            StyleExtensionImplKt.Style(
                Style.SatelliteStreets, 
                new Function1Action<StyleExtensionImpl.Builder>(x => {
                    x.SetAtmosphere(
                        AtmosphereKt.Atmosphere(new Function1Action<IAtmosphereDslReceiver>(x => { })
                        )
                    );
                    x.SetProjection(
                        ProjectionKt.Projection(
                            // ProjectionName.ValueOf("equalEarth") - not available for Android
                            ProjectionName.Globe
                            )
                    );
                })
            )
        );
    }
}