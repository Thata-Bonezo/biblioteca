using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinhaBiblioteca.Models;
using MinhaBiblioteca.Services;

namespace MinhaBiblioteca.ViewModels;

public partial class EstatisticasViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private EstatisticasModel _estatisticas = new();

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private int _anoAtual = DateTime.Now.Year;

    public EstatisticasViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    public async Task CarregarEstatisticasAsync()
    {
        try
        {
            IsRefreshing = true;
            Estatisticas = await _databaseService.GetEstatisticasAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar estatísticas: {ex.Message}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}
