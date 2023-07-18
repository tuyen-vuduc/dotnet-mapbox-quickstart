# Mapbox Quickstart Example for .NET Android

A very simple and short example of how to use Mapbox Android SDK in a .NET Android app.

You can find [my blog post here](https://tuyen-vuduc.tech/how-to-use-mapbox-for-your-dotnet-android-app) for a step-by-step guide.

## Prerequisites
- Visual Studio for Mac or Visual Studio for Windows
- .NET 7.0.306
- .NET Android workload

## Steps to run the example

- 1/ Generate/grab `MAPBOX_DOWNLOADS_TOKEN` from [your Mapbox account page](https://account.mapbox.com/)
- 2/ Put it into your local `~/.gradle/gradle.properties`

```bash
echo "MAPBOX_DOWNLOADS_TOKEN=YOUR_MAPBOX_DOWNLOADS_TOKEN" >> ~/gradle/gradle.properties
```

- 3/ Grab `mapbox_access_token` from [your Mapbox account page](https://account.mapbox.com/)
- 4/ Create file `resources_dev.xml` in folder `Resources/values` with the below content

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<resources>
    <string name="mapbox_access_token">YOUR_MAPBOX_ACCESS_TOKEN</string>
</resources>
```
- 5/ Check out the result