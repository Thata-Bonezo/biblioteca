using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinhaBiblioteca.Models;
using MinhaBiblioteca.Services;
using MinhaBiblioteca.Views;

namespace MinhaBiblioteca.ViewModels;

public partial class LivroDetalheViewModel : ObservableObject, IQueryAttributable
{
    private readonly IDatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private Livro? _livro;

    [ObservableProperty]
    private int _novaPaginaAtual;

    [ObservableProperty]
    private int _notaAvaliacao;

    [ObservableProperty]
    private string _novaResenha = string.Empty;

    public LivroDetalheViewModel(IDatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out int id) && id > 0)
        {
            await CarregarLivroAsync(id);
        }
    }

    public async Task CarregarLivroAsync(int id)
    {
        Livro = await _databaseService.GetLivroByIdAsync(id);
        if (Livro is not null)
        {
            NovaPaginaAtual = Livro.PaginaAtual;
            NotaAvaliacao = Livro.Nota;
            NovaResenha = Livro.Resenha;
        }
    }

    [RelayCommand]
    private async Task AtualizarProgressoAsync()
    {
        if (Livro is null) return;

        if (NovaPaginaAtual < 0 || NovaPaginaAtual > Livro.TotalPaginas)
        {
            await _dialogService.ShowAlertAsync("Validação", $"A página atual deve estar entre 0 e {Livro.TotalPaginas}.");
            return;
        }

        await _databaseService.AtualizarProgressoAsync(Livro.Id, NovaPaginaAtual);
        await CarregarLivroAsync(Livro.Id);
        await _dialogService.ShowAlertAsync("Sucesso", "Progresso de leitura atualizado com sucesso!");
    }

    [RelayCommand]
    private async Task SalvarAvaliacaoAsync()
    {
        if (Livro is null) return;

        if (NotaAvaliacao < 1 || NotaAvaliacao > 5)
        {
            await _dialogService.ShowAlertAsync("Validação", "Selecione uma nota de 1 a 5 estrelas.");
            return;
        }

        await _databaseService.SalvarAvaliacaoAsync(Livro.Id, NotaAvaliacao, NovaResenha);
        await CarregarLivroAsync(Livro.Id);
        await _dialogService.ShowAlertAsync("Sucesso", "Avaliação e resenha salvas!");
    }

    [RelayCommand]
    private void SelecionarNota(string notaString)
    {
        if (int.TryParse(notaString, out int nota))
        {
            NotaAvaliacao = nota;
        }
    }

    [RelayCommand]
    private async Task EditarAsync()
    {
        if (Livro is null) return;
        await Shell.Current.GoToAsync($"{nameof(LivroFormPage)}?id={Livro.Id}");
    }

    [RelayCommand]
    private async Task ExcluirAsync()
    {
        if (Livro is null) return;

        bool confirm = await _dialogService.ShowConfirmationAsync(
            "Confirmar Exclusão",
            $"Deseja realmente excluir o livro '{Livro.Titulo}'?",
            "Excluir",
            "Cancelar");

        if (confirm)
        {
            await _databaseService.DeleteLivroAsync(Livro);
            await _dialogService.ShowAlertAsync("Excluído", "Livro removido da biblioteca.");
            await Shell.Current.GoToAsync("..");
        }
    }
}
