using MapboxMaui;
using Microsoft.Extensions.Logging;

namespace DotnetMaui.MapboxQs;

public static class MauiProgram
{
	const string ACCESS_TOKEN = "pk.eyJ1IjoidHV5ZW52ZCIsImEiOiJjbGcxaHRheTcxNTYxM2ltc3pzbWZqM3FxIn0.U7VIhTCPwewyPgL3vhKTwQ";

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

