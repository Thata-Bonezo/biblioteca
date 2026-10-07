using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinhaBiblioteca.Models;
using MinhaBiblioteca.Services;

namespace MinhaBiblioteca.ViewModels;

public partial class MetasViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private int _anoSelecionado = DateTime.Now.Year;

    [ObservableProperty]
    private List<int> _anosDisponiveis = new();

    [ObservableProperty]
    private int _metaQuantidade = 12;

    [ObservableProperty]
    private int _livrosConcluidos = 0;

    [ObservableProperty]
    private double _progressoMeta = 0.0;

    [ObservableProperty]
    private int _percentualMeta = 0;

    [ObservableProperty]
    private string _mensagemMotivacional = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    public MetasViewModel(IDatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;

        int currentYear = DateTime.Now.Year;
        AnosDisponiveis = new List<int>
        {
            currentYear - 2,
            currentYear - 1,
            currentYear,
            currentYear + 1
        };
    }

    [RelayCommand]
    public async Task CarregarMetaAsync()
    {
        try
        {
            IsRefreshing = true;

            var meta = await _databaseService.GetMetaByAnoAsync(AnoSelecionado);
            if (meta is not null)
            {
                MetaQuantidade = meta.QuantidadeLivros;
            }
            else
            {
                MetaQuantidade = 12; // Valor padrão
            }

            // Buscar livros do banco para calcular quantos foram concluídos no ano selecionado
            var livros = await _databaseService.GetLivrosAsync();
            LivrosConcluidos = livros.Count(l =>
                l.Status == StatusLivro.Lido &&
                l.DataConclusao.HasValue &&
                l.DataConclusao.Value.Year == AnoSelecionado);

            if (MetaQuantidade > 0)
            {
                ProgressoMeta = Math.Min(1.0, (double)LivrosConcluidos / MetaQuantidade);
                PercentualMeta = (int)Math.Round(ProgressoMeta * 100);
            }
            else
            {
                ProgressoMeta = 0.0;
                PercentualMeta = 0;
            }

            AtualizarMensagemMotivacional();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar meta: {ex.Message}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    partial void OnAnoSelecionadoChanged(int value)
    {
        _ = CarregarMetaAsync();
    }

    private void AtualizarMensagemMotivacional()
    {
        if (PercentualMeta >= 100)
        {
            MensagemMotivacional = "🎉 Parabéns! Você atingiu sua meta de leitura para este ano!";
        }
        else if (PercentualMeta >= 75)
        {
            MensagemMotivacional = "🚀 Quase lá! Falta muito pouco para bater sua meta!";
        }
        else if (PercentualMeta >= 50)
        {
            MensagemMotivacional = "📖 Excelente ritmo! Você já passou da metade da meta!";
        }
        else if (PercentualMeta > 0)
        {
            MensagemMotivacional = "📚 Continue assim! Cada página lida conta para sua conquista.";
        }
        else
        {
            MensagemMotivacional = "💡 Defina sua meta e comece a registrar suas leituras!";
        }
    }

    [RelayCommand]
    private async Task SalvarMetaAsync()
    {
        if (MetaQuantidade <= 0)
        {
            await _dialogService.ShowAlertAsync("Validação", "A meta deve ser de pelo menos 1 livro.");
            return;
        }

        var meta = new MetaLeitura
        {
            Ano = AnoSelecionado,
            QuantidadeLivros = MetaQuantidade
        };

        await _databaseService.SaveMetaAsync(meta);
        await CarregarMetaAsync();
        await _dialogService.ShowAlertAsync("Sucesso", $"Meta para o ano {AnoSelecionado} atualizada com sucesso!");
    }
}
