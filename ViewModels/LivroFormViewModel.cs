using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinhaBiblioteca.Models;
using MinhaBiblioteca.Services;

namespace MinhaBiblioteca.ViewModels;

public partial class LivroFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly IDatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private int _livroId;

    [ObservableProperty]
    private string _titulo = string.Empty;

    [ObservableProperty]
    private string _autor = string.Empty;

    [ObservableProperty]
    private string _genero = string.Empty;

    [ObservableProperty]
    private int _totalPaginas = 100;

    [ObservableProperty]
    private int _paginaAtual = 0;

    [ObservableProperty]
    private string _status = StatusLivro.QueroLer;

    [ObservableProperty]
    private int _nota = 0;

    [ObservableProperty]
    private string _resenha = string.Empty;

    [ObservableProperty]
    private string _tituloPage = "Novo Livro";

    [ObservableProperty]
    private bool _isEditing;

    public List<string> ListaStatus => StatusLivro.StatusValidos;

    public LivroFormViewModel(IDatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out int id) && id > 0)
        {
            LivroId = id;
            IsEditing = true;
            TituloPage = "Editar Livro";

            var livro = await _databaseService.GetLivroByIdAsync(id);
            if (livro is not null)
            {
                Titulo = livro.Titulo;
                Autor = livro.Autor;
                Genero = livro.Genero;
                TotalPaginas = livro.TotalPaginas;
                PaginaAtual = livro.PaginaAtual;
                Status = livro.Status;
                Nota = livro.Nota;
                Resenha = livro.Resenha;
            }
        }
        else
        {
            LivroId = 0;
            IsEditing = false;
            TituloPage = "Novo Livro";
        }
    }

    partial void OnPaginaAtualChanged(int value)
    {
        AtualizarStatusAutomatico();
    }

    partial void OnTotalPaginasChanged(int value)
    {
        AtualizarStatusAutomatico();
    }

    private void AtualizarStatusAutomatico()
    {
        if (TotalPaginas <= 0) return;

        if (PaginaAtual <= 0)
        {
            Status = StatusLivro.QueroLer;
        }
        else if (PaginaAtual >= TotalPaginas)
        {
            Status = StatusLivro.Lido;
        }
        else
        {
            Status = StatusLivro.Lendo;
        }
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        // Validações
        if (string.IsNullOrWhiteSpace(Titulo))
        {
            await _dialogService.ShowAlertAsync("Validação", "O título do livro é obrigatório.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Autor))
        {
            await _dialogService.ShowAlertAsync("Validação", "O autor do livro é obrigatório.");
            return;
        }

        if (TotalPaginas <= 0)
        {
            await _dialogService.ShowAlertAsync("Validação", "O total de páginas deve ser maior que zero.");
            return;
        }

        if (PaginaAtual < 0)
        {
            await _dialogService.ShowAlertAsync("Validação", "A página atual não pode ser negativa.");
            return;
        }

        if (PaginaAtual > TotalPaginas)
        {
            await _dialogService.ShowAlertAsync("Validação", "A página atual não pode ser maior que o total de páginas.");
            return;
        }

        var livro = new Livro
        {
            Id = LivroId,
            Titulo = Titulo.Trim(),
            Autor = Autor.Trim(),
            Genero = Genero?.Trim() ?? string.Empty,
            TotalPaginas = TotalPaginas,
            PaginaAtual = PaginaAtual,
            Status = Status,
            Nota = Nota,
            Resenha = Resenha?.Trim() ?? string.Empty
        };

        await _databaseService.SaveLivroAsync(livro);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
