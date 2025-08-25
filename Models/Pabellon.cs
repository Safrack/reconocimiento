using SistemaCarcel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("pabellones")]
public class Pabellon
{
    [Key]
    [Column("id_pabellones")]
    public int Id { get; set; }

    [Column("nombre_pb")]
    [MaxLength(50)]
    public string NombrePb { get; set; } = null!;

    public virtual List<Visitante> Visitantes { get; set; } = new();
    public virtual List<Recluso> Reclusos { get; set; } = new();
    public virtual List<Autorizado> Autorizaciones { get; set; } = new();
    public virtual List<RegistroVisita> RegistroVisitas { get; set; } = new();
    public virtual List<AsignacionPabellonPol> Asignaciones { get; set; } = new();
}

