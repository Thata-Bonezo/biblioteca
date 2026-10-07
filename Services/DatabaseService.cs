using SQLite;
using MinhaBiblioteca.Data;
using MinhaBiblioteca.Models;

namespace MinhaBiblioteca.Services;

public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection? _database;

    private async Task EnsureInitializedAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<Livro>();
        await _database.CreateTableAsync<MetaLeitura>();

        await SeedDataAsync();
    }

    public async Task InitAsync()
    {
        await EnsureInitializedAsync();
    }

    private async Task SeedDataAsync()
    {
        if (_database is null) return;

        var count = await _database.Table<Livro>().CountAsync();
        if (count == 0)
        {
            var seedLivros = new List<Livro>
            {
                new Livro
                {
                    Titulo = "Dom Casmurro",
                    Autor = "Machado de Assis",
                    Genero = "Romance Clássico",
                    TotalPaginas = 256,
                    PaginaAtual = 256,
                    Status = StatusLivro.Lido,
                    Nota = 5,
                    Resenha = "Uma obra-prima da literatura brasileira. A dúvida sobre Capitu é genialmente construída.",
                    DataCadastro = DateTime.Now.AddDays(-60),
                    DataConclusao = DateTime.Now.AddDays(-10)
                },
                new Livro
                {
                    Titulo = "1984",
                    Autor = "George Orwell",
                    Genero = "Ficção Científica / Distopia",
                    TotalPaginas = 416,
                    PaginaAtual = 180,
                    Status = StatusLivro.Lendo,
                    Nota = 0,
                    Resenha = string.Empty,
                    DataCadastro = DateTime.Now.AddDays(-30)
                },
                new Livro
                {
                    Titulo = "O Hobbit",
                    Autor = "J.R.R. Tolkien",
                    Genero = "Fantasia",
                    TotalPaginas = 336,
                    PaginaAtual = 0,
                    Status = StatusLivro.QueroLer,
                    Nota = 0,
                    Resenha = string.Empty,
                    DataCadastro = DateTime.Now.AddDays(-15)
                },
                new Livro
                {
                    Titulo = "O Pequeno Príncipe",
                    Autor = "Antoine de Saint-Exupéry",
                    Genero = "Fábula",
                    TotalPaginas = 96,
                    PaginaAtual = 96,
                    Status = StatusLivro.Lido,
                    Nota = 5,
                    Resenha = "Leitura poética e profunda sobre o que realmente importa na vida.",
                    DataCadastro = DateTime.Now.AddDays(-45),
                    DataConclusao = DateTime.Now.AddDays(-20)
                }
            };

            await _database.InsertAllAsync(seedLivros);
        }

        var metaCount = await _database.Table<MetaLeitura>().CountAsync();
        if (metaCount == 0)
        {
            var currentYear = DateTime.Now.Year;
            await _database.InsertAsync(new MetaLeitura
            {
                Ano = currentYear,
                QuantidadeLivros = 12
            });
        }
    }

    public async Task<List<Livro>> GetLivrosAsync(string? busca = null, string? statusFiltro = null)
    {
        await EnsureInitializedAsync();
        var query = _database!.Table<Livro>();

        var livros = await query.ToListAsync();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim().ToLowerInvariant();
            livros = livros.Where(l =>
                (l.Titulo != null && l.Titulo.ToLowerInvariant().Contains(termo)) ||
                (l.Autor != null && l.Autor.ToLowerInvariant().Contains(termo)) ||
                (l.Genero != null && l.Genero.ToLowerInvariant().Contains(termo))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(statusFiltro) && statusFiltro != "Todos")
        {
            livros = livros.Where(l => l.Status == statusFiltro).ToList();
        }

        return livros.OrderByDescending(l => l.DataCadastro).ToList();
    }

    public async Task<Livro?> GetLivroByIdAsync(int id)
    {
        await EnsureInitializedAsync();
        return await _database!.Table<Livro>().FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<int> SaveLivroAsync(Livro livro)
    {
        await EnsureInitializedAsync();

        // Regra de atualização automática do status
        if (livro.PaginaAtual <= 0)
        {
            livro.PaginaAtual = 0;
            livro.Status = StatusLivro.QueroLer;
            livro.DataConclusao = null;
        }
        else if (livro.PaginaAtual >= livro.TotalPaginas)
        {
            livro.PaginaAtual = livro.TotalPaginas;
            livro.Status = StatusLivro.Lido;
            if (!livro.DataConclusao.HasValue)
            {
                livro.DataConclusao = DateTime.Now;
            }
        }
        else
        {
            livro.Status = StatusLivro.Lendo;
            livro.DataConclusao = null;
        }

        if (livro.Id != 0)
        {
            return await _database!.UpdateAsync(livro);
        }
        else
        {
            livro.DataCadastro = DateTime.Now;
            return await _database!.InsertAsync(livro);
        }
    }

    public async Task<int> DeleteLivroAsync(Livro livro)
    {
        await EnsureInitializedAsync();
        return await _database!.DeleteAsync(livro);
    }

    public async Task AtualizarProgressoAsync(int livroId, int novaPaginaAtual)
    {
        await EnsureInitializedAsync();
        var livro = await GetLivroByIdAsync(livroId);
        if (livro is null) return;

        livro.PaginaAtual = Math.Clamp(novaPaginaAtual, 0, livro.TotalPaginas);
        await SaveLivroAsync(livro);
    }

    public async Task SalvarAvaliacaoAsync(int livroId, int nota, string resenha)
    {
        await EnsureInitializedAsync();
        var livro = await GetLivroByIdAsync(livroId);
        if (livro is null) return;

        livro.Nota = Math.Clamp(nota, 0, 5);
        livro.Resenha = resenha ?? string.Empty;
        await _database!.UpdateAsync(livro);
    }

    public async Task<MetaLeitura?> GetMetaByAnoAsync(int ano)
    {
        await EnsureInitializedAsync();
        return await _database!.Table<MetaLeitura>().FirstOrDefaultAsync(m => m.Ano == ano);
    }

    public async Task SaveMetaAsync(MetaLeitura meta)
    {
        await EnsureInitializedAsync();
        var existente = await GetMetaByAnoAsync(meta.Ano);
        if (existente is not null)
        {
            existente.QuantidadeLivros = meta.QuantidadeLivros;
            await _database!.UpdateAsync(existente);
        }
        else
        {
            await _database!.InsertAsync(meta);
        }
    }

    public async Task<EstatisticasModel> GetEstatisticasAsync()
    {
        await EnsureInitializedAsync();
        var livros = await _database!.Table<Livro>().ToListAsync();
        var anoAtual = DateTime.Now.Year;

        var avaliados = livros.Where(l => l.Nota > 0).ToList();
        double notaMedia = avaliados.Any() ? avaliados.Average(l => l.Nota) : 0.0;

        var concluidosNoAno = livros.Count(l =>
            l.Status == StatusLivro.Lido &&
            l.DataConclusao.HasValue &&
            l.DataConclusao.Value.Year == anoAtual);

        return new EstatisticasModel
        {
            TotalLivros = livros.Count,
            QueroLerCount = livros.Count(l => l.Status == StatusLivro.QueroLer),
            LendoCount = livros.Count(l => l.Status == StatusLivro.Lendo),
            LidoCount = livros.Count(l => l.Status == StatusLivro.Lido),
            TotalPaginasLidas = livros.Sum(l => l.PaginaAtual),
            NotaMedia = Math.Round(notaMedia, 1),
            ConcluidosNoAno = concluidosNoAno
        };
    }
}
