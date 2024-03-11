using AndroidX.Core.Content;
using Com.Mapbox.Functions;
using Com.Mapbox.Geojson;
using Com.Mapbox.Maps;
using Com.Mapbox.Maps.Extension.Style;
using Com.Mapbox.Maps.Extension.Style.Expressions.Dsl.Generated;
using Com.Mapbox.Maps.Extension.Style.Expressions.Generated;
using Com.Mapbox.Maps.Extension.Style.Layers;
using Com.Mapbox.Maps.Extension.Style.Layers.Generated;
using Com.Mapbox.Maps.Extension.Style.Sources;
using Com.Mapbox.Maps.Extension.Style.Sources.Generated;
using Com.Mapbox.Maps.Extension.Style.Utils;
using Com.Mapbox.Maps.Plugins.Animation;
using Mapbox4DotnetAndroidSamples.Utils;

namespace Mapbox4DotnetAndroidSamples.Examples;

[Activity(Label = "Circle Layer Clustering", MainLauncher = true, Theme = "@style/AppTheme")]
public class CircleLayerClusteringActivity : AppCompatActivity
{
    private const string GEOJSON_SOURCE_ID = "earthquakes";
    private const string CROSS_ICON_ID = "cross-icon-id";

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var mapView = new MapView(this);
        SetContentView(mapView);

        var mapboxMap = mapView.MapboxMap;

        /*
         styleExtension = style(Style.LIGHT) {
            +transition {
                duration(0)
                delay(0)
                enablePlacementTransitions(false)
            }
          }
         */
        var styleExtension = StyleExtensionImplKt.Style(
            Style.Light,
            new Function1Action<StyleExtensionImpl.Builder>(builder =>
            {
                builder.SetTransition(TypeUtilsKt.Transition(new Function1Action<TransitionOptions.Builder>(tob =>
                {
                    tob
                        .Duration(new Java.Lang.Long(0))
                        .Delay(new Java.Lang.Long(0))
                        .EnablePlacementTransitions(new Java.Lang.Boolean(false));
                })));
            })
        );

        mapboxMap.LoadStyle(
            styleExtension,
            style =>
            {
                /*
                 mapboxMap.flyTo(
                      CameraOptions.Builder()
                        .center(Point.fromLngLat(-79.045, 12.099))
                        .zoom(3.0)
                        .build()
                    )
                 */
                CameraAnimationsUtils.FlyTo(
                    mapboxMap,
                    new CameraOptions.Builder()
                        .Center(Point.FromLngLat(-79.045, 12.099))
                        .Zoom(new Java.Lang.Double(3.0))
                        .Build()
                    );

                AddClusteredGeoJsonSource(style);

                /*
                 bitmapFromDrawableRes(this, R.drawable.ic_cross)?.let { bitmap ->
                      it.addImage(CROSS_ICON_ID, bitmap, true)
                    }
                 */
                var bitmap = this.BitmapFromDrawableRes(
                    Resource.Drawable.ic_cross
                    );
                if (bitmap is not null)
                {
                    style.AddImage(CROSS_ICON_ID, bitmap, true);
                }

                Toast.MakeText(
                     this,
                     Resource.String.zoom_map_in_and_out_instruction,
                     ToastLength.Short
                   ).Show();
            });
    }

    private void AddClusteredGeoJsonSource(Style style)
    {
        /*
         // Add a new source from the GeoJSON data and set the 'cluster' option to true.
            style.addSource(
              // Point to GeoJSON data. This example visualizes all M1.0+ earthquakes from 12/22/15 to 1/21/16 as logged by USGS' Earthquake hazards program.
              geoJsonSource(GEOJSON_SOURCE_ID) {
                url("https://www.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson")
                cluster(true)
                maxzoom(14)
                clusterRadius(50)
              }
            )
         */
        var geoJsonSource = GeoJsonSourceKt.GeoJsonSource(
            GEOJSON_SOURCE_ID,
            new Function1Action<GeoJsonSource.Builder>(x =>
                x.Url("https://www.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson")
                .Cluster(true)
                .Maxzoom(14)
                .ClusterRadius(50)
                ));
        SourceUtils.AddSource(style, geoJsonSource);

        /*
        // Creating a marker layer for single data points
        style.addLayer(
          symbolLayer("unclustered-points", GEOJSON_SOURCE_ID) {
            iconImage(CROSS_ICON_ID)
            iconSize(
              division {
                get {
                  literal("mag")
                }
                literal(4.0)
              }
            )
            iconColor(
              interpolate {
                exponential {
                  literal(1)
                }
                get {
                  literal("mag")
                }
                stop {
                  literal(2.0)
                  rgb {
                    literal(0)
                    literal(255)
                    literal(0)
                  }
                }
                stop {
                  literal(4.5)
                  rgb {
                    literal(0)
                    literal(0)
                    literal(255)
                  }
                }
                stop {
                  literal(7.0)
                  rgb {
                    literal(255)
                    literal(0)
                    literal(0)
                  }
                }
              }
            )
            filter(
              has {
                literal("mag")
              }
            )
          }
        )
        */
        var symbolLayper = SymbolLayerKt.SymbolLayer(
            "unclustered-points",
            GEOJSON_SOURCE_ID,
            new Function1Action<ISymbolLayerDsl>(x => x
                .IconImage(CROSS_ICON_ID)
                .IconSize(
                    Expression.CompanionField
                        .Division(x =>
                            x
                            .Get(y => y.Literal("mag"))
                            .Literal(4.0))
                    )
                .IconColor(
                    Expression.CompanionField.Interpolate(
                        x => x
                        .Exponential(x => x.Literal(1))
                        .Get(x => x.Literal("mag"))
                        .Stop(x => x
                            .Literal(2.0)
                            .Rgb(x => x
                                .Literal(0)
                                .Literal(255)
                                .Literal(0)))
                        .Stop(x => x
                            .Literal(4.5)
                            .Rgb(x => x
                                .Literal(0)
                                .Literal(0)
                                .Literal(255)))
                        .Stop(x => x
                            .Literal(7.0)
                            .Rgb(x => x
                                .Literal(255)
                                .Literal(0)
                                .Literal(0)))
                    ))
                .Filter(Expression.CompanionField.Has(x => x
                    .Literal("mag")))
                )
            );
        LayerUtils.AddLayer(style, symbolLayper);

        /*
            // Use the earthquakes GeoJSON source to create three layers: One layer for each cluster category.
            // Each point range gets a different fill color.
            val layers = arrayOf(
              intArrayOf(150, ContextCompat.getColor(this, R.color.red)),
              intArrayOf(20, ContextCompat.getColor(this, R.color.green)),
              intArrayOf(0, ContextCompat.getColor(this, R.color.blue))
            )
         */
        var layers = new int[][]
        {
            [150, ContextCompat.GetColor(this, Resource.Color.red)],
            [20, ContextCompat.GetColor(this, Resource.Color.green)],
            [0, ContextCompat.GetColor(this, Resource.Color.blue)]
        };

        /*
            // Add clusters' circles
            style.addLayer(
              circleLayer("clusters", GEOJSON_SOURCE_ID) {
                circleColor(
                  step(
                    input = get("point_count"),
                    output = literal(ColorUtils.colorToRgbaString(layers[2][1])),
                    stops = arrayOf(
                      literal(layers[1][0].toDouble()) to literal(ColorUtils.colorToRgbaString(layers[1][1])),
                      literal(layers[0][0].toDouble()) to literal(ColorUtils.colorToRgbaString(layers[0][1]))
                    )
                  )
                )
                circleRadius(18.0)
                filter(
                  has("point_count")
                )
              }
            )
         */
        var colorExpression = Expression.CompanionField.Step(
                        Expression.Get("point_count"),
                        Expression.Literal(ColorUtils.Instance.ColorToRgbaString(layers[2][1])),
                        new[]
                        {
                            new Kotlin.Pair(Expression.Literal(layers[1][0]), Expression.Literal(ColorUtils.Instance.ColorToRgbaString(layers[1][1]))),
                            new Kotlin.Pair(Expression.Literal(layers[0][0]), Expression.Literal(ColorUtils.Instance.ColorToRgbaString(layers[0][1]))),
                        });
        var clustersCircleLayer = CircleLayerKt.CircleLayer(
            "clusters",
            GEOJSON_SOURCE_ID,
            new Function1Action<ICircleLayerDsl>(x => x
                .CircleColor(
                    colorExpression
                    )
                .CircleRadius(18.0)
                .Filter(
                    Expression.CompanionField.Has("point_count"))
            ));
        LayerUtils.AddLayer(style, clustersCircleLayer);

        /*
         symbolLayer("count", GEOJSON_SOURCE_ID) {
            textField(
              format {
                formatSection(
                  toString {
                    get {
                      literal("point_count")
                    }
                  }
                )
              }
            )
            textSize(12.0)
            textColor(Color.WHITE)
            textIgnorePlacement(true)
            textAllowOverlap(true)
          }
        )
         */
        var countSymbolLayer = SymbolLayerKt.SymbolLayer(
            "count", GEOJSON_SOURCE_ID,
            new Function1Action<ISymbolLayerDsl>(x => x
                .TextField(
                    ExpressionDslKt.Format(x => x
                        .FormatSection(
                            ExpressionDslKt.ToString(y => y
                                .Get(z => z.Literal("point_count"))
                                )
                            )
                        )
                    )
                )
            );
        LayerUtils.AddLayer(style, countSymbolLayer);
    }
}