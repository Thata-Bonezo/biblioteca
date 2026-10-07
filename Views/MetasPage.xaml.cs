using MinhaBiblioteca.ViewModels;

namespace MinhaBiblioteca.Views;

public partial class MetasPage : ContentPage
{
    private readonly MetasViewModel _viewModel;

    public MetasPage(MetasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.CarregarMetaCommand.Execute(null);
    }
}
