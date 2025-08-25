using SistemaCarcel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("registro_visitas")]
public class RegistroVisita
{
    [Key]
    [Column("id_registro")]
    public int Id { get; set; }

    [Column("id_visitante")]
    public int IdVisitante { get; set; }

    [Column("id_pabellones")]
    public int IdPabellon { get; set; }

    [Column("fecha_entrada")]
    public DateTime FechaEntrada { get; set; }

    [Column("fecha_salida")]
    public DateTime? FechaSalida { get; set; }

    [Column("id_personal")]
    public int IdPersonal { get; set; }

    public virtual Visitante Visitante { get; set; } = null!;
    public virtual Pabellon Pabellon { get; set; } = null!;
    public virtual Personal Personal { get; set; } = null!;
}