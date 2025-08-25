using SistemaCarcel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("personal")]
public class Personal
{
    [Key]
    [Column("id_personal")]
    public int IdPersonal { get; set; }

    [Column("ci")]
    [MaxLength(20)]
    [RegularExpression(@"^[0-9]{6,8}(-[A-Za-z0-9]{1,3})?$", ErrorMessage = "Formato de CI inválido. Ej: 12345678 o 12345678-1K")]
    public string Ci { get; set; } = null!;

    [Column("nombre")]
    [MaxLength(100)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras.")]
    public string Nombre { get; set; } = null!;

    [Column("apellidos")]
    [MaxLength(100)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras.")]
    public string Apellidos { get; set; } = null!;

    [Column("sexo")]
    [MaxLength(10)]
    public string Sexo { get; set; } = null!;

    [Column("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [Column("cargo")]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El cargo solo puede contener letras.")]
    public string Cargo { get; set; } = null!;

    [NotMapped]
    public string Usuario => $"{Nombre} {Apellidos}";


    public virtual List<AsignacionPabellonPol> Asignaciones { get; set; } = new();
    public virtual List<RegistroVisita> RegistroVisitas { get; set; } = new();
    public virtual List<Visitante> Visitantes { get; set; } = new();
    public virtual List<User> Users { get; set; } = new();
}