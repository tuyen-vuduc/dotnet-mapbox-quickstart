using MapboxMaui;
using Microsoft.Extensions.Logging;

namespace DotnetMaui.MapboxQs;

public static partial class MauiProgram
{
	//const string ACCESS_TOKEN = "{YOUR_MAPBOX_ACCESS_TOKEN}";

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMapbox(ACCESS_TOKEN)
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

