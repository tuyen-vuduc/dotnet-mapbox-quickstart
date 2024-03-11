using Android.Content.PM;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Com.Mapbox.Maps;
using Com.Mapbox.Maps.Extension.Style.Expressions.Dsl.Generated;
using Com.Mapbox.Maps.Extension.Style.Layers.Properties.Generated;
using Com.Mapbox.Maps.Extension.Style.Light;
using Com.Mapbox.Maps.Extension.Style.Projection.Generated;
using Com.Mapbox.Maps.Plugins;
using Com.Mapbox.Maps.Plugins.Gestures;
using Com.Mapbox.Maps.Plugins.Locationcomponent;
using Mapbox4DotnetAndroidSamples.Utils;
namespace Mapbox4DotnetAndroidSamples.Examples;

[Activity(Label = "@string/activity_location_component", Description = "@string/description_location_component", Exported = true, Theme = "@style/AppTheme")]
public partial class LocationComponentActivity : AppCompatActivity
{
    private string lastStyleUri = Style.Dark;
    private LocationPermissionHelper locationPermissionHelper;
    private MapView mapView;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_location_component);
        mapView = FindViewById<MapView>(Resource.Id.mapView);

        locationPermissionHelper = new LocationPermissionHelper(new WeakReference<Activity>(this));
        locationPermissionHelper.CheckPermissions(() =>
        {
            mapView.MapboxMap.LoadStyle(
                Style.Standard,
                style =>
                {
                    style.SetLight(fl =>
                    {
                        fl.Anchor(Anchor.Map)
                        .Color(Color.Yellow)
                        .Position(10, 40, 50);
                    });

                    var gestures = mapView.GetGestures();
                    gestures.ScrollEnabled = false;
                    /*
                     gestures.addOnMapClickListener { point ->
                        location
                            .isLocatedAt(point) { isPuckLocatedAtPoint ->
                            if (isPuckLocatedAtPoint) {
                                Toast.makeText(context, "Clicked on location puck", Toast.LENGTH_SHORT).show()
                            }
                            }
                        true
                     }
                     */
                    // gestures.AddOnMapClickListener(this);
                    // OR
                    gestures.OnMapClick((point) =>
                    {
                        var location = mapView.GetLocationComponent();
                        location.IsLocatedAt(point, (result) =>
                        {
                            Toast.MakeText(this, "Clicked on location puck", ToastLength.Short).Show();
                        });
                        return true;
                    });

                    /*
                      gestures.addOnMapLongClickListener { point ->
                        location.isLocatedAt(point) { isPuckLocatedAtPoint ->
                          if (isPuckLocatedAtPoint) {
                            Toast.makeText(context, "Long-clicked on location puck", Toast.LENGTH_SHORT)
                              .show()
                          }
                        }
                        true
                      }*/
                    gestures.OnMapLongClick((point) =>
                    {
                        var location = mapView.GetLocationComponent();
                        location.IsLocatedAt(point, (result) =>
                        {
                            Toast.MakeText(this, "Long-clicked on location puck", ToastLength.Short).Show();
                        });

                        return true;
                    });

                    /*
                     val locationProvider = location.getLocationProvider() as DefaultLocationProvider
                      locationProvider.addOnCompassCalibrationListener {
                        Toast.makeText(context, "Compass needs to be calibrated", Toast.LENGTH_LONG).show()
                      }
                     */
                    var location = mapView.GetLocationComponent();
                    var locationProvider = location.LocationProvider as DefaultLocationProvider;
                    locationProvider.CompassCalibration += (sender, e) =>
                    {
                        Toast.MakeText(this, "Compass needs to be calibrated", ToastLength.Short).Show();
                    };
                }
            );
        });
    }

    public override bool OnCreateOptionsMenu(IMenu? menu)
    {
        MenuInflater.Inflate(Resource.Menu.menu_location_component, menu);

        return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.action_customise_location_puck_change:
                {
                    ToggleCustomisedPuck();
                    return true;
                }
            case Resource.Id.action_map_style_change:
                {
                    ToggleMapStyle();
                    return true;
                }
            case Resource.Id.action_map_projection_change:
                {
                    ToggleMapProjection();
                    return true;
                }
            case Resource.Id.action_component_disable:
                {
                    // binding.mapView.location.enabled = false;
                    mapView.GetLocationComponent().Enabled = false;
                    return true;
                }
            case Resource.Id.action_component_enabled:
                {
                    // binding.mapView.location.enabled = true;
                    mapView.GetLocationComponent().Enabled = true;
                    return true;
                }
            case Resource.Id.action_show_bearing:
                {
                    // binding.mapView.location.puckBearingEnabled = true;
                    mapView.GetLocationComponent().PuckBearingEnabled = true;
                    // if (binding.mapView.location.locationPuck is LocationPuck2D)
                    if (mapView.GetLocationComponent().LocationPuck is LocationPuck2D locationPuck2D)
                    {
                        // binding.mapView.location.locationPuck = createDefault2DPuck(withBearing = true)
                        mapView.GetLocationComponent().LocationPuck = LocationComponentUtils.CreateDefault2DPuck(true);

                    }
                    return true;
                }
            case Resource.Id.action_hide_bearing:
                {
                    // binding.mapView.location.puckBearingEnabled = false;
                    mapView.GetLocationComponent().PuckBearingEnabled = false;

                    // if (binding.mapView.location.locationPuck is LocationPuck2D)
                    if (mapView.GetLocationComponent().LocationPuck is LocationPuck2D locationPuck2D)
                    {
                        // binding.mapView.location.locationPuck = createDefault2DPuck(withBearing = false)
                        mapView.GetLocationComponent().LocationPuck = LocationComponentUtils.CreateDefault2DPuck(false);
                    }
                    return true;
                }
            case Resource.Id.heading:
                {
                    // binding.mapView.location.puckBearing = PuckBearing.HEADING
                    mapView.GetLocationComponent().PuckBearing = PuckBearing.Heading;
                    // item.isChecked = true
                    item.SetChecked(true);
                    return true;
                }
            case Resource.Id.course:
                {
                    // binding.mapView.location.puckBearing = PuckBearing.COURSE
                    mapView.GetLocationComponent().PuckBearing = PuckBearing.Course;
                    // item.isChecked = true
                    item.SetChecked(true);
                    return true;
                }
            case Resource.Id.action_accuracy_enabled:
                {
                    // binding.mapView.location.showAccuracyRing = true
                    mapView.GetLocationComponent().ShowAccuracyRing = true;
                    // item.isChecked = true
                    item.SetChecked(true);
                    return true;
                }
            case Resource.Id.action_accuracy_disable:
                {
                    // binding.mapView.location.showAccuracyRing = false
                    mapView.GetLocationComponent().ShowAccuracyRing = false;
                    // item.isChecked = true
                    item.SetChecked(true);
                    return true;
                }
            case Resource.Id.toggle_opacity:
                {
                    //val location = binding.mapView.location;
                    var location = mapView.GetLocationComponent();
                    //      location.locationPuck = location.locationPuck.run {
                    //          when(this) {
                    //            is LocationPuck3D->copy(modelOpacity = if (modelOpacity == 1.0F) 0.5F else 1.0F)
                    //            is LocationPuck2D->copy(opacity = if (opacity == 1.0F) 0.5F else 1.0F)
                    //          }
                    //      }
                    location.LocationPuck = location.LocationPuck switch
                    {
                        LocationPuck3D lp3d => lp3d.Copy(
                            lp3d.ModelUri,
                            lp3d.Position,
                            modelOpacity: lp3d.ModelOpacity == 1.0f ? 0.5f : 1.0f,
                            lp3d.ModelScale,
                            lp3d.ModelScaleExpression,
                            lp3d.ModelTranslation,
                            lp3d.ModelRotation,
                            lp3d.ModelCastShadows,
                            lp3d.ModelReceiveShadows,
                            lp3d.ModelScaleMode,
                            lp3d.ModelEmissiveStrength,
                            lp3d.ModelEmissiveStrengthExpression
                            ),
                        LocationPuck2D lp2d => lp2d.Copy(
                            lp2d.TopImage,
                            lp2d.BearingImage,
                            lp2d.ShadowImage,
                            lp2d.ScaleExpression,
                            opacity: lp2d.Opacity == 1.0f ? 0.5f : 1.0f),
                        _ => null
                    };

                    return true;
                }
        }
        return base.OnOptionsItemSelected(item);
    }

    //private fun toggleCustomisedPuck()
    private void ToggleCustomisedPuck()
    {
        //    binding.mapView.location.let {
        var location = mapView.GetLocationComponent();
        //        when(it.locationPuck) {
        switch (location.LocationPuck)
        {
            //        is LocationPuck3D->it.locationPuck = LocationPuck2D(
            case LocationPuck3D locationPuck3D:
                //          topImage = ImageHolder.from(com.mapbox.maps.plugin.locationcomponent.R.drawable.mapbox_user_icon),
                //          bearingImage = ImageHolder.from(com.mapbox.maps.plugin.locationcomponent.R.drawable.mapbox_user_bearing_icon),
                //          shadowImage = ImageHolder.from(com.mapbox.maps.plugin.locationcomponent.R.drawable.mapbox_user_stroke_icon),
                //          scaleExpression = interpolate {
                //                linear()
                //            zoom()
                //            stop {
                //                    literal(0.0)
                //              literal(0.6)
                //            }
                //                stop {
                //                    literal(20.0)
                //                  literal(1.0)
                //                }
                //            }.toJson()
                //          )
                location.LocationPuck = new LocationPuck2D(
                    ImageHolder.From(Resource.Drawable.mapbox_user_icon),
                    ImageHolder.From(Resource.Drawable.mapbox_user_bearing_icon),
                    ImageHolder.From(Resource.Drawable.mapbox_user_stroke_icon),
                    scaleExpression: ExpressionDslKt.Interpolate(x =>
                        x.Linear()
                        .Zoom()
                        .Stop(y =>
                            y.Literal(0.0)
                            .Literal(0.6))
                        .Stop(y =>
                            y.Literal(20.0)
                            .Literal(1.0))
                        ).ToJson()
                    );

                break;
            //        is LocationPuck2D->it.locationPuck = LocationPuck3D(
            case LocationPuck2D locationPuck2D:
                //          modelUri = "asset://sportcar.glb",
                //          modelScale = listOf(10f, 10f, 10f),
                //          modelTranslation = listOf(0.1f, 0.1f, 0.1f),
                //          modelRotation = listOf(0.0f, 0.0f, 180.0f),
                //          modelCastShadows = false,
                //          modelReceiveShadows = false,
                //          modelEmissiveStrength = 1.1f
                //        )
                location.LocationPuck = new LocationPuck3D(
                    "asset://sportcar.glb",
                    new[] { new Java.Lang.Float(0f), new Java.Lang.Float(0f) },
                    1.0f,
                    new[] { new Java.Lang.Float(10f), new Java.Lang.Float(10f), new Java.Lang.Float(10f) },
                    null,
                    new[] { new Java.Lang.Float(0.1f), new Java.Lang.Float(0.1f), new Java.Lang.Float(0.1f) },
                    new[] { new Java.Lang.Float(0.0f), new Java.Lang.Float(0.0f), new Java.Lang.Float(180.0f) },
                    false,
                    false,
                    Com.Mapbox.Maps.Plugins.ModelScaleMode.Viewport,
                    1.1f
                    );
                break;
        }
    }

    //private fun toggleMapStyle()
    private void ToggleMapStyle()
    {
        //    val styleUrl = if (lastStyleUri == Style.DARK) Style.LIGHT else Style.DARK
        var styleUrl = lastStyleUri == Style.Dark
            ? Style.Light
            : Style.Dark;
        //binding.mapView.mapboxMap.loadStyle(styleUrl) {
        //        lastStyleUri = styleUrl
        //}
        mapView.MapboxMap.LoadStyle(styleUrl, style => lastStyleUri = styleUrl);
    }

    //private fun toggleMapProjection()
    private void ToggleMapProjection()
    {
        //    binding.mapView.mapboxMap.getStyle {
        //        style->
        //      style.setProjection(
        //        projection(
        //          when(style.getProjection()?.name) {
        //            ProjectionName.MERCATOR->ProjectionName.GLOBE
        //            ProjectionName.GLOBE->ProjectionName.MERCATOR
        //        else ->ProjectionName.GLOBE
        //          }
        //    )
        //  )
        //}
        mapView.MapboxMap.GetStyle(style =>
        {
            // ProjectionName.ValueOf("equalEarth");
            style.SetProjection(
                ProjectionKt.Projection(
                    style.GetProjection()?.Name == ProjectionName.Globe
                         ? ProjectionName.Globe
                         : ProjectionName.Mercator
                    )
                );
        });
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        locationPermissionHelper.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnStart()
    {
        base.OnStart();
        //binding.mapView.location
        //  .addOnIndicatorPositionChangedListener(onIndicatorPositionChangedListener)
        mapView.GetLocationComponent()
            .AddOnIndicatorPositionChangedListener(this);
    }

    protected override void OnStop()
    {
        base.OnStop();
        //binding.mapView.location
        //  .removeOnIndicatorPositionChangedListener(onIndicatorPositionChangedListener)
        mapView.GetLocationComponent()
            .RemoveOnIndicatorPositionChangedListener(this);
    }
}

//partial class LocationComponentActivity : IOnMapClickListener
//{
//    /*
//     gestures.addOnMapClickListener { point ->
//        location
//            .isLocatedAt(point) { isPuckLocatedAtPoint ->
//            if (isPuckLocatedAtPoint) {
//                Toast.makeText(context, "Clicked on location puck", Toast.LENGTH_SHORT).show()
//            }
//            }
//        true
//     }
//     */
//    public bool OnMapClick(Com.Mapbox.Geojson.Point point)
//    {
//        var location = LocationComponentUtils.GetLocationComponent(mapView);
//        location.IsLocatedAt(point, (result) => {
//            Toast.MakeText(this, "Clicked on location puck", ToastLength.Short).Show();
//        });

//        return true;
//    }
//}

partial class LocationComponentActivity : IOnIndicatorPositionChangedListener
{
    /* Kotlin
      private val onIndicatorPositionChangedListener = OnIndicatorPositionChangedListener {
        // Jump to the current indicator position
        binding.mapView.mapboxMap.setCamera(CameraOptions.Builder().center(it).build())
        // Set the gestures plugin's focal point to the current indicator location.
        binding.mapView.gestures.focalPoint = binding.mapView.mapboxMap.pixelForCoordinate(it)
      }
    */
    public void OnIndicatorPositionChanged(Com.Mapbox.Geojson.Point point)
    {
        // Jump to the current indicator position
        mapView.MapboxMap.SetCamera(new CameraOptions.Builder().Center(point).Build());

        // Set the gestures plugin's focal point to the current indicator location.
        var gestures = mapView.GetGestures();
        gestures.FocalPoint = mapView.MapboxMap.PixelForCoordinate(point);
    }
}