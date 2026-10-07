using Microsoft.Extensions.Logging;
using MinhaBiblioteca.Services;
using MinhaBiblioteca.ViewModels;
using MinhaBiblioteca.Views;

namespace MinhaBiblioteca;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Services (Injeção de Dependência)
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();

        // ViewModels
        builder.Services.AddTransient<LivrosViewModel>();
        builder.Services.AddTransient<LivroFormViewModel>();
        builder.Services.AddTransient<LivroDetalheViewModel>();
        builder.Services.AddTransient<MetasViewModel>();
        builder.Services.AddTransient<EstatisticasViewModel>();

        // Views
        builder.Services.AddTransient<LivrosPage>();
        builder.Services.AddTransient<LivroFormPage>();
        builder.Services.AddTransient<LivroDetalhePage>();
        builder.Services.AddTransient<MetasPage>();
        builder.Services.AddTransient<EstatisticasPage>();

        return builder.Build();
    }
}
