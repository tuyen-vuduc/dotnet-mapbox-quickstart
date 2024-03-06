using Com.Mapbox.Maps.Extension.Style.Expressions.Dsl.Generated;
using Com.Mapbox.Maps.Extension.Style.Expressions.Generated;
using Com.Mapbox.Maps.Plugins;
using Com.Mapbox.Maps.Plugins.Locationcomponent;
using Com.Mapbox.Maps.Plugins.Viewport;
using Com.Mapbox.Maps.Plugins.Viewport.Data;
using Kotlin.Jvm.Functions;
using static Com.Mapbox.Maps.Extension.Style.Expressions.Generated.Expression;

namespace DotnetAndroid.MapboxQs;

[Activity(Label = "User Location Example", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
public partial class UserLocationActivity : MapViewBaseActivity
{
    PermissionsManager permisionManager;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (PermissionsManager.AreLocationPermissionsGranted(this))
        {
            ShowUserLocation();
        }
        else
        {
            permisionManager = new PermissionsManager(this);
            permisionManager.RequestLocationPermissions(this);
        }
    }
}

partial class UserLocationActivity : IPermissionsListener
{
    public void OnExplanationNeeded(IList<string> permissionsToExplain)
    {
    }

    public void OnPermissionResult(bool granted)
    {
        if (!granted) return;

        ShowUserLocation();
    }

    private void ShowUserLocation()
    {
        /* // Kotlin code
                with(mapView) {
                  location.locationPuck = createDefault2DPuck(withBearing = true)
                  location.enabled = true
                  location.puckBearing = PuckBearing.COURSE
                  viewport.transitionTo(
                    targetState = viewport.makeFollowPuckViewportState(),
                    transition = viewport.makeImmediateViewportTransition()
                  )
                }
                 */

        var locationComponent = LocationComponentUtils.GetLocationComponent(MapView);
        locationComponent.Enabled = true;
        locationComponent.LocationPuck = LocationComponentUtils.CreateDefault2DPuck();
        locationComponent.PuckBearing = PuckBearing.Course;
        locationComponent.PulsingEnabled = true;

        var viewport = ViewportUtils.GetViewport(MapView);
        var followPuckViewportStateOptions = new FollowPuckViewportStateOptions.Builder().Build();
        viewport.TransitionTo(
            viewport.MakeFollowPuckViewportState(followPuckViewportStateOptions),
            viewport.MakeImmediateViewportTransition(),
            null
            );

        /*
         mapView.location.locationPuck = LocationPuck2D(
          topImage = ImageHolder.from(R.drawable.mapbox_user_icon), // ImageHolder also accepts Bitmap
          bearingImage = ImageHolder.from(R.drawable.mapbox_user_bearing_icon),
          shadowImage = ImageHolder.from(R.drawable.mapbox_user_stroke_icon),
          scaleExpression = interpolate {
            linear()
            zoom()
            stop {
              literal(0.0)
              literal(0.6)
            }
            stop {
              literal(20.0)
              literal(1.0)
            }
          }.toJson()
        )*/
        var locationPuck2D = new LocationPuck2D(
            ImageHolder.From(Resource.Drawable.ic_user_on_map),
            ImageHolder.From(Resource.Drawable.ic_user_bearing),
            ImageHolder.From(Resource.Drawable.ic_user_puck),
            ExpressionDslKt
                .Interpolate((builder) => builder
                    .Linear()
                    .Zoom()
                    .Stop((expressionBuilder) => expressionBuilder
                        .Literal(0.0)
                        .Literal(0.6)
                    )
                    .Stop((expressionBuilder) => expressionBuilder
                        .Literal(20.0)
                        .Literal(1.0)
                    )
                ).ToJson()
        );
        locationComponent.LocationPuck = locationPuck2D;
    }
}