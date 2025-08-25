using SistemaCarcel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("visitantes")]
public class Visitante
{
    [Key]
    [Column("id_visitante")]
    public int Id { get; set; }
    
    [Column("nombre_completo")]
    [MaxLength(100)]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El nombre solo debe contener letras y espacios.")]
    public string NombreCompleto { get; set; } = null!;

    [Column("ci")]
    [MaxLength(20)]
    [RegularExpression(@"^[0-9]{6,8}(-[A-Za-z0-9]{1,3})?$", ErrorMessage = "Formato de CI inválido. Ej: 12345678 o 12345678-1K")]
    public string Ci { get; set; } = null!;

    [Column("edad")]
    [Range(18, 99, ErrorMessage = "La edad debe estar entre 18 y 99 años.")]
    public int Edad { get; set; }

    [Column("parentesco")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El parentesco solo debe contener letras y espacios.")]
    public string Parentesco { get; set; } = null!;

    [Column("nombre_privado_libertad")]
    public string NombrePrivadoLibertad { get; set; } = null!;

    [Required(ErrorMessage = "El campo Pabellon es obligatorio.")]
    [Display(Name = "Pabellon")]
    [Column("id_pabellones")]
    public int IdPabellon { get; set; }

    [Column("imagen")]
    public string Imagen { get; set; } = null!;

    [Column("hora")]
    public DateTime Hora { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [Column("id_personal")]
    public int? IdPersonal { get; set; }

    [ForeignKey("IdPabellon")]
    public virtual Pabellon Pabellon { get; set; } = null!;
    public virtual Personal? Personal { get; set; }
    public virtual List<RegistroVisita> RegistroVisitas { get; set; } = new();
    public virtual Autorizado? Autorizacion { get; set; }
}

