using System;
using System.Net.Http;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SuperERP.PDV.Services;
using Microsoft.Extensions.Configuration;
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

		// Carregar configuração de arquivo (opcional)
		builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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

		// Prioridade de resolução da URL da API:
		// 1) Variável de ambiente SUPERERP_API_BASE_URL
		// 2) appsettings.json -> "Api:BaseUrl"
		// 3) Default por plataforma (emulador Android ou localhost)
		var envBase = Environment.GetEnvironmentVariable("SUPERERP_API_BASE_URL");
		var configBase = builder.Configuration["Api:BaseUrl"];

#if ANDROID
		var defaultBase = "http://10.0.2.2:5000/"; // emulador Android
#else
		var defaultBase = "http://localhost:5277/"; // porta HTTP do backend (conforme logs)
#endif

		var baseAddress = !string.IsNullOrWhiteSpace(envBase)
			? envBase
			: !string.IsNullOrWhiteSpace(configBase)
				? configBase
				: defaultBase;

		if (!baseAddress.EndsWith("/")) baseAddress += "/";

		builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(baseAddress) });

		var app = builder.Build();

		// Inicializar banco de dados
		var dbService = app.Services.GetRequiredService<DatabaseService>();
		Task.Run(async () => await dbService.InicializarAsync()).Wait();

		return app;
	}
}
