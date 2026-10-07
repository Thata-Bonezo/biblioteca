using MinhaBiblioteca.ViewModels;

namespace MinhaBiblioteca.Views;

public partial class LivroFormPage : ContentPage
{
    public LivroFormPage(LivroFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
