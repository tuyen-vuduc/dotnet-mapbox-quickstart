using Com.Mapbox.Maps;
using Com.Mapbox.Maps.Plugins.Gestures;
using Mapbox4DotnetAndroidSamples.Utils;
using Android.Graphics;
using Com.Mapbox.Maps.Dsl;
using Com.Mapbox.Functions;

namespace Mapbox4DotnetAndroidSamples.Examples.MarkersAndCallouts.InfoWindow;

[Activity(
    Label = "@string/activity_info_window",
    Description = "@string/description_info_window",
Exported = true,
    Theme = "@style/AppTheme")]
[MetaData("@string/category", Value = "@string/category_markers_and_callouts")]
public partial class InfoWindowActivity : AppCompatActivity
{
    private MapView mapView;
    private Bitmap icon;

    private MarkerManager markerManager;
    private Marker customMarker;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        mapView = new MapView(this);
        SetContentView(mapView);

        icon = BitmapUtils.BitmapFromDrawableRes(
          this,
          Resource.Drawable.ic_blue_marker
        );
        //mapView.mapboxMap.apply {
        //        setCamera(
        //          cameraOptions {
        //            center(Point.fromLngLat(-77.03655168667463, 38.897705003219784))
        //            zoom(15.0)
        //          }
        //  )
        //  getStyle {
        //            markerManager = MarkerManager(mapView)
        //    addMarkers()
        //  }
        //        addOnMapLongClickListener(this@InfoWindowActivity)
        //}
        var cameraOptions = CameraOptionsKtxKt.CameraOptions(new Function1Action<CameraOptions.Builder>(builder =>
        {
            builder
                .Center(Com.Mapbox.Geojson.Point.FromLngLat(-77.03655168667463, 38.897705003219784))
                .Zoom(new Java.Lang.Double(15.0));
        }));
        mapView.MapboxMap.SetCamera(cameraOptions);
        mapView.MapboxMap.GetStyle(style =>
        {
            markerManager = new MarkerManager(mapView);
            addMarkers();
        });

        GesturesUtils.AddOnMapLongClickListener(mapView.MapboxMap, this);
    }

    private void addMarkers()
    {
        markerManager.addMarker(
          new Marker(
            Com.Mapbox.Geojson.Point.FromLngLat(-77.03364419, 38.9002073),
            icon,
            "Intersection",
            "H St NW with 15th St NW"));

        markerManager.addMarker(
        new Marker(
          Com.Mapbox.Geojson.Point.FromLngLat(-77.03654, 38.89393),
          icon,
          "The Ellipse"
        )
      );
        var marker = markerManager.addMarker(
            new Marker(
              Com.Mapbox.Geojson.Point.FromLngLat(-77.03655168667463, 38.897705003219784),
              icon,
              "White House",
              """
            The official residence and principal workplace of the President of the United States,
            located at 1600 Pennsylvania Avenue NW in Washington, D.C. It has been the residence of every
            U.S. president since John Adams in 1800.
            """
            )
        );
        // open InfoWindow at startup
        markerManager.selectMarker(marker);
    }
    protected override void OnDestroy()
    {
        GesturesUtils.RemoveOnMapLongClickListener(mapView.MapboxMap, this);
        markerManager.destroy();
        base.OnDestroy();
    }
}

partial class InfoWindowActivity : IOnMapLongClickListener
{
    public bool OnMapLongClick(Com.Mapbox.Geojson.Point point)
    {
        if (customMarker is not null)
        {
            markerManager.removeMarker(customMarker);
        }

        customMarker = markerManager.addMarker(
          new Marker(
            point,
            icon,
            "Custom marker",
            $"{point.Latitude: #.#####}, {point.Longitude:#.#####}"
            )
        );
        return true;
    }
}
