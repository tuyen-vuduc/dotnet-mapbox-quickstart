using Android.Views;
using Com.Mapbox.Geojson;
using Com.Mapbox.Maps;
using Com.Mapbox.Maps.Plugins.Annotations;
using Com.Mapbox.Maps.Plugins.Annotations.Generated;
using Com.Mapbox.Maps.Plugins.Gestures;
using Com.Mapbox.Maps.Viewannotation;
using static Android.Icu.Text.ListFormatter;
using static Google.Android.Material.Tabs.TabLayout;
using static Java.Util.Jar.Pack200;
using System;
using Mapbox4DotnetAndroidSamples.Examples.MarkersAndCallouts.InfoWindow;
using Com.Mapbox.Functions;

namespace Mapbox4DotnetAndroidSamples.Examples.MarkersAndCallouts.InfoWindow;

internal partial class MarkerManager : Java.Lang.Object
{
    private readonly MapView mapView;

    /*
     private val pointAnnotationManager: PointAnnotationManager =
     */
    private readonly PointAnnotationManager pointAnnotationManager;

    /*     
      // using copy on write just in case as potentially remove may be called while we're iterating in on click listener
      private val markerList = CopyOnWriteArrayList<Marker>()
     */
    private readonly IList<Marker> markerList;

    public MarkerManager(MapView mapView)
    {
        this.mapView = mapView;

        /*
         mapView.annotations.createPointAnnotationManager(
          AnnotationConfig(
            layerId = LAYER_ID
          )
        )
        */
        IAnnotationPlugin annotations = AnnotationsUtils.GetAnnotations(mapView);
        pointAnnotationManager = PointAnnotationManagerKt.CreatePointAnnotationManager(
            annotations,
            new AnnotationConfig(
                belowLayerId: null,
                layerId: LAYER_ID
            ));

        markerList = new List<Marker>();

        /*
         pointAnnotationManager.addClickListener(this)
         */
        pointAnnotationManager.AddClickListener(this);

        /*         
        // by adding regular map click listener we implement deselecting all info windows on map click
        // in legacy code it was controlled by flag in API
        mapView.mapboxMap.addOnMapClickListener(this)
         */
        GesturesUtils.AddOnMapClickListener(mapView.MapboxMap, this);
    }
}

partial class MarkerManager : IOnPointAnnotationClickListener, IOnMapClickListener
{
    public bool OnAnnotationClick(Java.Lang.Object annotation)
    {
        foreach (var xmarker in markerList)
        {
            if (xmarker.pointAnnotation == annotation)
            {
                selectMarker(xmarker, true);
            }
        }
        return true;
    }

    public bool OnMapClick(Point point)
    {
        foreach (var xmarker in markerList)
        {
            deselectMarker(xmarker);
        }
        return true;
    }


    public Marker addMarker(Marker marker)
    {
        marker.prepareAnnotationMarker(pointAnnotationManager, LAYER_ID);
        marker.prepareViewAnnotation(mapView);
        markerList.Add(marker);
        // do not show info window by default
        deselectMarker(marker);
        return marker;
    }

    public void removeMarker(Marker marker)
    {
        if (!marker.prepared)
        {
            return;
        }
        markerList.Remove(marker);
        mapView.ViewAnnotationManager.RemoveViewAnnotation(marker.viewAnnotation);
        pointAnnotationManager.Delete(marker.pointAnnotation);
    }

    public void selectMarker(Marker marker, bool deselectIfSelected = false)
    {
        if (isSelected(marker))
        {
            if (deselectIfSelected)
            {
                deselectMarker(marker);
            }
            return;
        }
        // Need to deselect any currently selected annotation first
        foreach (var xmarker in markerList)
        {
            deselectMarker(xmarker);
        }

        adjustViewAnnotationXOffset(marker);
        mapView.ViewAnnotationManager.UpdateViewAnnotation(
            marker.viewAnnotation,
            /*viewAnnotationOptions {
            selected(true)
            }*/
            ViewAnnotationOptionsKtxKt.ViewAnnotationOptions(new Function1Action<ViewAnnotationOptions.Builder>(builder =>
            {
                builder.Selected(new Java.Lang.Boolean(true));
            }))
        );
        marker.viewAnnotation.Visibility = ViewStates.Visible;
    }

    private void deselectMarker(Marker marker)
    {
        mapView.ViewAnnotationManager.UpdateViewAnnotation(
            marker.viewAnnotation,
            /*viewAnnotationOptions {
                selected(false)
                marker.anchor?.let {
                    variableAnchors(
                    listOf(it.toBuilder().offsetX(0.0).build())
                    )
                }
            }*/
            ViewAnnotationOptionsKtxKt.ViewAnnotationOptions(new Function1Action<ViewAnnotationOptions.Builder>(builder =>
             {
                 builder.Selected(new Java.Lang.Boolean(false));
                 if (marker.anchor is not null)
                 {
                     builder.VariableAnchors(new List<ViewAnnotationAnchorConfig> {
                    marker.anchor.ToBuilder().OffsetX(0.0).Build(),
                     });
                 }
             })));
        marker.viewAnnotation.Visibility = ViewStates.Invisible;
    }

    public void destroy()
    {
        foreach (var xmarker in markerList)
        {
            deselectMarker(xmarker);
        }
        pointAnnotationManager.RemoveClickListener(this);

        // mapView.mapboxMap.removeOnMapClickListener(this)
        GesturesUtils.RemoveOnMapClickListener(mapView.MapboxMap, this);
    }

    // adjust offsetX to fit on the screen if the info window is shown near screen edge
    private void adjustViewAnnotationXOffset(Marker marker)
    {
        //mapView.ViewAnnotationManager.AddOnViewAnnotationUpdatedListener(
        //  object : OnViewAnnotationUpdatedListener {
        //    override fun onViewAnnotationPositionUpdated(
        //      view: View,
        //      leftTopCoordinate: ScreenCoordinate,
        //      width: Double,
        //      height: Double,
        //    ) {
        //        if (view == marker.viewAnnotation)
        //        {
        //            updateOffsetX(marker, leftTopCoordinate, width)
        //          mapView.viewAnnotationManager.removeOnViewAnnotationUpdatedListener(this)
        //        }
        //    }
        //})
        mapView.ViewAnnotationManager.OnViewAnnotationPositionUpdated((listner, args) =>
        {
            if (args.View == marker.viewAnnotation)
            {
                updateOffsetX(marker, args.LeftTopCoordinate, args.Width);
                mapView.ViewAnnotationManager.RemoveOnViewAnnotationUpdatedListener(listner);
            }
        });
    }

    private void updateOffsetX(Marker marker, ScreenCoordinate leftTop, double width)
    {
        var resultOffsetX = (leftTop.GetX() < 0)
            ? Math.Abs(leftTop.GetX()) + ADDITIONAL_EDGE_PADDING_PX
            : (leftTop.GetX() + width > mapView.MapboxMap.Size.Width)
            ? mapView.MapboxMap.Size.Width - leftTop.GetX() - width - ADDITIONAL_EDGE_PADDING_PX
            : 0.0;

        var anchor = marker.anchor?.ToBuilder()
            ?.OffsetX(resultOffsetX)
            ?.Build();

        mapView.ViewAnnotationManager.UpdateViewAnnotation(
          marker.viewAnnotation,
            //  viewAnnotationOptions {
            //    if (anchor != null)
            //    {
            //        variableAnchors(listOf(anchor))
            //    }
            //}
            ViewAnnotationOptionsKtxKt.ViewAnnotationOptions(new Function1Action<ViewAnnotationOptions.Builder>(builder =>
            {
                if (anchor is not null)
                {
                    builder.VariableAnchors(new List<ViewAnnotationAnchorConfig> { anchor });
                }
            }))
        );
    }

    private bool isSelected(Marker marker)
        => mapView.ViewAnnotationManager.GetViewAnnotationOptions(marker.viewAnnotation)?.Selected.BooleanValue() == true;

    // additional padding when offsetting view near the screen edge
    const double ADDITIONAL_EDGE_PADDING_PX = 20.0;
    const string LAYER_ID = "annotation-layer";
}


public static class ViewAnnotationManagerExtensions
{
    class XOnViewAnnotationPositionUpdated : Java.Lang.Object, IOnViewAnnotationUpdatedListener
    {
        private Action<IOnViewAnnotationUpdatedListener, ViewAnnotationPositionUpdatedEventArgs> action;

        public XOnViewAnnotationPositionUpdated(Action<IOnViewAnnotationUpdatedListener, ViewAnnotationPositionUpdatedEventArgs> action)
        {
            this.action = action;
        }

        public void OnViewAnnotationAnchorCoordinateUpdated(View view, Point anchorCoordinate)
        {
        }

        public void OnViewAnnotationAnchorUpdated(View view, ViewAnnotationAnchorConfig anchor)
        {
        }

        public void OnViewAnnotationPositionUpdated(View view, ScreenCoordinate leftTopCoordinate, double width, double height)
        {
            action?.Invoke(this, new ViewAnnotationPositionUpdatedEventArgs(view, leftTopCoordinate, width, height));
        }

        public void OnViewAnnotationVisibilityUpdated(View view, bool visible)
        {
        }
    }

    public static void OnViewAnnotationPositionUpdated(this IViewAnnotationManager viewAnnotationManager, Action<IOnViewAnnotationUpdatedListener, ViewAnnotationPositionUpdatedEventArgs> action)
    {
        var listener = new XOnViewAnnotationPositionUpdated(action);
        viewAnnotationManager.AddOnViewAnnotationUpdatedListener(listener);
    }
}