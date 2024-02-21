# Mapbox Quickstart Example for .NET MAUI

A very simple and short example of how to use Mapbox SDKs in a .NET MAUI app.

You can find [my blog post here](https://tuyen-vuduc.tech/how-to-use-mapbox-in-your-dotnet-maui-app) for a step-by-step guide.

## Prerequisites
- Visual Studio for Mac or Visual Studio for Windows
- .NET 8.0.100
- .NET workloads for iOS, Android, MAUI

## Steps to run the example

- 1/ Generate/grab `MAPBOX_DOWNLOADS_TOKEN` from [your Mapbox account page](https://account.mapbox.com/)
- 1.a/ Put it into your local `~/.gradle/gradle.properties`

```bash
echo "MAPBOX_DOWNLOADS_TOKEN=YOUR_MAPBOX_DOWNLOADS_TOKEN" >> ~/.gradle/gradle.properties
```
- 1.b/ Replace `YOUR_MAPBOX_DOWNLOADS_TOKEN` with yours in `src/quickstart/DotnetMaui.MapboxQs/DotnetMaui.MapboxQs.csproj` file
- 2/ Grab `mapbox_access_token` from [your Mapbox account page](https://account.mapbox.com/)
- 3/ Replace `YOUR_MAPBOX_ACCESS_TOKEN` with yours in `src/quickstart/DotnetMaui.MapboxQs/MauiProgram.cs` file
- 4/ Check out the result