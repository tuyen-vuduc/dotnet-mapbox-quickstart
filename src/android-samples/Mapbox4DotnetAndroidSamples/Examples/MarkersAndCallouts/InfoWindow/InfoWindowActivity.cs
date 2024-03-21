using Android.Graphics.Drawables;
using Com.Mapbox.Geojson;
using Com.Mapbox.Maps;
using Com.Mapbox.Maps.Plugins.Gestures;
using Java.Time.Format;
using Kotlin;
using Mapbox4DotnetAndroidSamples.Utils;
using static Android.Icu.Text.CaseMap;
using static Android.Icu.Text.Transliterator;
using System;
using Android.Graphics;
using static Java.Util.Jar.Pack200;

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

  override fun onCreate(savedInstanceState: Bundle?)
    {
        super.onCreate(savedInstanceState)
    mapView = MapView(this)
    setContentView(mapView)

    icon = BitmapUtils.bitmapFromDrawableRes(
      this@InfoWindowActivity,
      R.drawable.ic_blue_marker
    )!!
    mapView.mapboxMap.apply {
            setCamera(
              cameraOptions {
                center(Point.fromLngLat(-77.03655168667463, 38.897705003219784))
                zoom(15.0)
              }
      )
      getStyle {
                markerManager = MarkerManager(mapView)
        addMarkers()
      }
            addOnMapLongClickListener(this@InfoWindowActivity)
    }
    }

    private fun addMarkers()
    {
        markerManager.addMarker(
          Marker(
            title = "Intersection",
            snippet = "H St NW with 15th St NW",
            position = Point.fromLngLat(-77.03364419, 38.9002073),
            icon = icon,
    
          )
        )
      markerManager.addMarker(
      Marker(
        title = "The Ellipse",
        icon = icon,
        position = Point.fromLngLat(-77.03654, 38.89393)
      )
    )
      val marker = markerManager.addMarker(
      Marker(
        title = "White House",
        snippet = """
          The official residence and principal workplace of the President of the United States,
          located at 1600 Pennsylvania Avenue NW in Washington, D.C. It has been the residence of every
          U.S. president since John Adams in 1800.
        """.trimIndent(),
        icon = icon,
        position = Point.fromLngLat(-77.03655168667463, 38.897705003219784)
      )
    )
      // open InfoWindow at startup
    markerManager.selectMarker(marker)
    }

    override fun onMapLongClick(point: Point): Boolean {
    customMarker?.let {
      markerManager.removeMarker(it)
}
customMarker = markerManager.addMarker(
  Marker(
    position = point,
        icon = icon,
        title = "Custom marker",
        snippet = "${DecimalFormat("#.#####").format(point.latitude())}, ${DecimalFormat("#.#####").format(point.longitude())}"
      )
    )
    return true
  }

  override fun onDestroy()
{
    mapView.mapboxMap.removeOnMapLongClickListener(this)
    markerManager.destroy()
    super.onDestroy()
  }
}

partial class InfoWindowActivity : IOnMapLongClickListener
{
    public bool OnMapLongClick(Point point)
    {
    }
}
