using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinhaBiblioteca.Models;
using MinhaBiblioteca.Services;
using MinhaBiblioteca.Views;

namespace MinhaBiblioteca.ViewModels;

public partial class LivrosViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<Livro> _livros = new();

    [ObservableProperty]
    private string _buscaTexto = string.Empty;

    [ObservableProperty]
    private string _statusFiltroSelecionado = "Todos";

    [ObservableProperty]
    private ObservableCollection<StatusFiltroChip> _statusFiltros = new(
        StatusLivro.TodosFiltros.Select(s => new StatusFiltroChip(s, s == "Todos")));

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private bool _isEmpty;

    public LivrosViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    public async Task CarregarLivrosAsync()
    {
        try
        {
            IsRefreshing = true;
            var lista = await _databaseService.GetLivrosAsync(BuscaTexto, StatusFiltroSelecionado);
            
            Livros.Clear();
            foreach (var livro in lista)
            {
                Livros.Add(livro);
            }

            IsEmpty = Livros.Count == 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar livros: {ex.Message}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    partial void OnBuscaTextoChanged(string value)
    {
        _ = CarregarLivrosAsync();
    }

    partial void OnStatusFiltroSelecionadoChanged(string value)
    {
        foreach (var chip in StatusFiltros)
        {
            chip.Selecionado = chip.Nome == value;
        }

        _ = CarregarLivrosAsync();
    }

    [RelayCommand]
    private void Filtrar(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return;

        StatusFiltroSelecionado = status;
    }

    [RelayCommand]
    private async Task AdicionarLivroAsync()
    {
        await Shell.Current.GoToAsync(nameof(LivroFormPage));
    }

    [RelayCommand]
    private async Task SelecionarLivroAsync(Livro? livro)
    {
        if (livro is null) return;
        await Shell.Current.GoToAsync($"{nameof(LivroDetalhePage)}?id={livro.Id}");
    }

    [RelayCommand]
    private async Task EditarLivroAsync(Livro? livro)
    {
        if (livro is null) return;
        await Shell.Current.GoToAsync($"{nameof(LivroFormPage)}?id={livro.Id}");
    }
}
