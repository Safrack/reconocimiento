using SistemaCarcel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("asignacion_pabellon_pol")]
public class AsignacionPabellonPol
{
    [Key]
    [Column("id_asignacion")]
    public int Id { get; set; }

    [Column("id_personal")]
    public int IdPersonal { get; set; }

    [ForeignKey("IdPersonal")]
    public Personal Personal { get; set; } = null!; 

    [Column("id_pabellones")]
    public int IdPabellon { get; set; } 

    [ForeignKey("IdPabellones")]
    public Pabellon Pabellon { get; set; } = null!; 
}
