using SistemaCarcel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("autorizado")]
public class Autorizado
{
    [Key]
    [Column("id_autorizado")]
    public int Id { get; set; }

    [Column("id_visitante")]
    public int IdVisitante { get; set; }

    [Column("id_pabellones")]
    public int IdPabellon { get; set; }

    [Column("fecha_de_autorizacion")]
    public DateTime FechaDeAutorizacion { get; set; }

    public virtual Visitante Visitante { get; set; } = null!;
    public virtual Pabellon Pabellon { get; set; } = null!;
}