# Mapbox Quickstart Example for .NET iOS

A very simple and short example of how to use Mapbox iOS SDK in a .NET iOS app.

You can find [my blog post here](https://tuyen-vuduc.tech/how-to-use-mapbox-in-your-net-ios-app) for a step-by-step guide.

## Prerequisites
- Visual Studio for Mac or Visual Studio for Windows
- .NET 7.0.306
- .NET iOS workload

## Steps to run the example

- 1/ Generate/grab `MAPBOX_DOWNLOADS_TOKEN` from [your Mapbox account page](https://account.mapbox.com/)
- 2/ Open `.csproj` file and replace `YOUR_MAPBOX_DOWNLOADS_TOKEN` with yours

```xml
<PropertyGroup>
  <MAPBOX_DOWNLOADS_TOKEN>YOUR_MAPBOX_DOWNLOADS_TOKEN</MAPBOX_DOWNLOADS_TOKEN>
</PropertyGroup>
```

- 3/ Grab `mapbox_access_token` from [your Mapbox account page](https://account.mapbox.com/)
- 4/ Open `MapboxViewController.cs` and replace `YOUR_MAPBOX_ACCESS_TOKEN` with yours

```cs
public class MapboxViewController : UIViewController
{
    const string MAPBOX_ACCESS_TOKEN = "YOUR_MAPBOX_ACCESS_TOKEN";

    // ...
}
```
- 5/ Check out the result