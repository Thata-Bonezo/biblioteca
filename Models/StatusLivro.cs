namespace MinhaBiblioteca.Models;

public static class StatusLivro
{
    public const string QueroLer = "Quero ler";
    public const string Lendo = "Lendo";
    public const string Lido = "Lido";

    public static List<string> TodosFiltros => new() { "Todos", QueroLer, Lendo, Lido };
    public static List<string> StatusValidos => new() { QueroLer, Lendo, Lido };
}
