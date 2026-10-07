using MinhaBiblioteca.ViewModels;

namespace MinhaBiblioteca.Views;

public partial class LivrosPage : ContentPage
{
    private readonly LivrosViewModel _viewModel;

    public LivrosPage(LivrosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.CarregarLivrosCommand.Execute(null);
    }
}
