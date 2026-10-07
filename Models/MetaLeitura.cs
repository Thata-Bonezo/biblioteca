using SQLite;

namespace MinhaBiblioteca.Models;

[Table("MetasLeitura")]
public class MetaLeitura
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Unique = true)]
    public int Ano { get; set; }

    public int QuantidadeLivros { get; set; }
}
