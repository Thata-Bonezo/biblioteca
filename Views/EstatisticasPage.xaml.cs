using MinhaBiblioteca.ViewModels;

namespace MinhaBiblioteca.Views;

public partial class EstatisticasPage : ContentPage
{
    private readonly EstatisticasViewModel _viewModel;

    public EstatisticasPage(EstatisticasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.CarregarEstatisticasCommand.Execute(null);
    }
}
