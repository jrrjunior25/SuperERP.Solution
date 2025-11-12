using Microsoft.Extensions.Logging;
using SuperERP.PDV.Services;

namespace SuperERP.PDV;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		// Services
		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<AuthService>();
		builder.Services.AddSingleton<CaixaService>();
		builder.Services.AddSingleton<ProdutoService>();
		builder.Services.AddSingleton<VendaPDVService>();
		builder.Services.AddSingleton<MainPage>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		// Inicializar banco de dados
		var dbService = app.Services.GetRequiredService<DatabaseService>();
		Task.Run(async () => await dbService.InicializarAsync()).Wait();

		return app;
	}
}
