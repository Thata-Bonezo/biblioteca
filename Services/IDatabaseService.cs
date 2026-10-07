using MinhaBiblioteca.Models;

namespace MinhaBiblioteca.Services;

public interface IDatabaseService
{
    Task InitAsync();
    Task<List<Livro>> GetLivrosAsync(string? busca = null, string? statusFiltro = null);
    Task<Livro?> GetLivroByIdAsync(int id);
    Task<int> SaveLivroAsync(Livro livro);
    Task<int> DeleteLivroAsync(Livro livro);
    Task AtualizarProgressoAsync(int livroId, int novaPaginaAtual);
    Task SalvarAvaliacaoAsync(int livroId, int nota, string resenha);
    Task<MetaLeitura?> GetMetaByAnoAsync(int ano);
    Task SaveMetaAsync(MetaLeitura meta);
    Task<EstatisticasModel> GetEstatisticasAsync();
}
