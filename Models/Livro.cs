using SQLite;

namespace MinhaBiblioteca.Models;

[Table("Livros")]
public class Livro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Titulo { get; set; } = string.Empty;

    [NotNull]
    public string Autor { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public int TotalPaginas { get; set; }

    public int PaginaAtual { get; set; }

    public string Status { get; set; } = StatusLivro.QueroLer;

    public int Nota { get; set; } // 0 a 5

    public string Resenha { get; set; } = string.Empty;

    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public DateTime? DataConclusao { get; set; }

    [Ignore]
    public double Progresso => TotalPaginas > 0 ? Math.Min(1.0, Math.Max(0.0, (double)PaginaAtual / TotalPaginas)) : 0.0;

    [Ignore]
    public int PercentualProgresso => TotalPaginas > 0 ? (int)Math.Min(100, Math.Round((double)PaginaAtual / TotalPaginas * 100)) : 0;

    [Ignore]
    public string StarsDisplay => Nota > 0 ? new string('★', Nota) + new string('☆', 5 - Nota) : "Sem avaliação";
}
