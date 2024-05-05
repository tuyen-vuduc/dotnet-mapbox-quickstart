using Android.Views;
using Com.Mapbox.Maps.Extension.Style.Layers.Properties.Generated;
using Com.Mapbox.Maps.Plugins.Annotations.Generated;
using Com.Mapbox.Maps;
using Android.Graphics;
using Point = Com.Mapbox.Geojson.Point;
using Com.Mapbox.Maps.Viewannotation;
using Com.Mapbox.Functions;

namespace Mapbox4DotnetAndroidSamples.Examples.MarkersAndCallouts.InfoWindow;

/**
 * Class representing a marker that contains marker icon and view annotation on top of it.
 */
public record class Marker
{
    public Point Position { get; }
    public Bitmap Icon { get; }
    public string? Title { get; }
    public string? Snippet { get; }

    public Marker(Point position, Bitmap icon, string? title, string? snippet = default)
    {
        if (title == null && snippet == null)
        {
            throw new ArgumentException("Marker should have either title or snippet!");
        }
        Position = position;
        Icon = icon;
        Title = title;
        Snippet = snippet;
    }

    internal ViewAnnotationAnchorConfig anchor = null;

    private string layerId = "";
    internal bool prepared = false;

    internal PointAnnotation pointAnnotation;
    internal View viewAnnotation;

    internal void prepareAnnotationMarker(PointAnnotationManager pointAnnotationManager, string layerId)
    {
        PointAnnotationOptions pointAnnotationOptions = new PointAnnotationOptions()
            .WithPoint(Position)
            .WithIconImage(Icon)
            .WithIconAnchor(IconAnchor.Bottom);
        pointAnnotation = (PointAnnotation)pointAnnotationManager.Create(pointAnnotationOptions);
        this.layerId = layerId;
    }

    internal void prepareViewAnnotation(MapView mapView)
    {
        viewAnnotation = LayoutInflater.From(mapView.Context)
          .Inflate(Resource.Layout.item_legacy_callout_view, mapView.RootView as ViewGroup, false);
        viewAnnotation.FindViewById<TextView>(Resource.Id.infowindow_title).Text = Title;
        viewAnnotation.FindViewById<TextView>(Resource.Id.infowindow_description).Text = Snippet;

        /*
         viewAnnotationOptions {
            // attach view annotation to the feature/layer ids of the annotation
            annotatedLayerFeature(layerId) {
                featureId(pointAnnotation.id)
            }
            annotationAnchor {
                // same anchor with the annotation
                anchor(ViewAnnotationAnchor.BOTTOM)
              // needed to display info window above the marker
          offsetY((pointAnnotation.iconImageBitmap?.height!! + MARKER_PADDING_PX).toDouble())
            }
        }
         */
        var viewAnnotationOptions = ViewAnnotationOptionsKtxKt.ViewAnnotationOptions(new Function1Action<ViewAnnotationOptions.Builder>(builder =>
        {
            ViewAnnotationOptionsKtxKt.AnnotatedLayerFeature(
                builder,
                layerId,
                new Function1Action<AnnotatedLayerFeature.Builder>(xbuilder =>
                {
                    xbuilder.FeatureId(pointAnnotation.Id);
                }));

            ViewAnnotationOptionsKtxKt.AnnotationAnchor(builder, new Function1Action<ViewAnnotationAnchorConfig.Builder>(xbuilder =>
            {
                xbuilder.Anchor(ViewAnnotationAnchor.Bottom)
                .OffsetY((pointAnnotation.IconImageBitmap.Height + MARKER_PADDING_PX));
            }));
        }));

        /*
         .also {
            anchor = it.variableAnchors!!.first()
        }
         */
        anchor = viewAnnotationOptions.VariableAnchors.First();

        mapView.ViewAnnotationManager.AddViewAnnotation(
          viewAnnotation,
          viewAnnotationOptions
        );
        prepared = true;
    }

    // padding between marker and info window
    const int MARKER_PADDING_PX = 10;
}
