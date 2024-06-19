namespace Mapbox4DotnetIosSamples;

[Register ("AppDelegate")]
public partial class AppDelegate : UIApplicationDelegate {
    // const string MAPBOX_ACCESS_TOKEN = "YOUR_MAPBOX_ACCESS_TOKEN";

	public override UIWindow? Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
        MapboxCommon.MBXMapboxOptions.SetAccessTokenForToken(MAPBOX_ACCESS_TOKEN);

		// create a new window instance based on the screen size
		Window = new UIWindow (UIScreen.MainScreen.Bounds);

		Window.RootViewController = new AnimateGeoJSONLineExample(); ;

		// make the window visible
		Window.MakeKeyAndVisible ();

		return true;
	}
}

