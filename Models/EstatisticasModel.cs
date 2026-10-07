namespace MinhaBiblioteca.Models;

public class EstatisticasModel
{
    public int TotalLivros { get; set; }
    public int QueroLerCount { get; set; }
    public int LendoCount { get; set; }
    public int LidoCount { get; set; }
    public int TotalPaginasLidas { get; set; }
    public double NotaMedia { get; set; }
    public int ConcluidosNoAno { get; set; }
}
