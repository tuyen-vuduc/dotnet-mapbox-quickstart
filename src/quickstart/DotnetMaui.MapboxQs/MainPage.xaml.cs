using GeoJSON.Text.Geometry;
using MapboxMaui;
using Microsoft.Maui.ApplicationModel;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace DotnetMaui.MapboxQs;

public partial class MainPage : ContentPage
{
	public MainPage()
    {
        InitializeComponent();

        mapbox.MapReady += Map_MapReady;
    }

    private void Map_MapReady(object sender, EventArgs e)
    {
        var mapCenter = new Position(21.0278, 105.8342);
        var cameraOptions = new CameraOptions
        {
            Center = mapCenter,
            Zoom = 13.1f,
            Bearing = 80,
            Pitch = 85,
        };

        mapbox.CameraOptions = cameraOptions;
        //mapbox.MapboxStyle = (MapboxStyle)@"mapbox://styles/mapbox-map-design/ckhqrf2tz0dt119ny6azh975y";
    }
}


