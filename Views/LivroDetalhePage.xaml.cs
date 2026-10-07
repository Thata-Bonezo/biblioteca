using MinhaBiblioteca.ViewModels;

namespace MinhaBiblioteca.Views;

public partial class LivroDetalhePage : ContentPage
{
    public LivroDetalhePage(LivroDetalheViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
