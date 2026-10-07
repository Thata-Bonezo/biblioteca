using CommunityToolkit.Mvvm.ComponentModel;

namespace MinhaBiblioteca.ViewModels;

public partial class StatusFiltroChip : ObservableObject
{
    public string Nome { get; }

    [ObservableProperty]
    private bool _selecionado;

    public StatusFiltroChip(string nome, bool selecionado = false)
    {
        Nome = nome;
        Selecionado = selecionado;
    }
}
