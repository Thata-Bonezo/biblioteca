using MinhaBiblioteca.Views;

namespace MinhaBiblioteca;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LivroFormPage), typeof(LivroFormPage));
        Routing.RegisterRoute(nameof(LivroDetalhePage), typeof(LivroDetalhePage));
    }
}
