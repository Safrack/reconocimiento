using SistemaCarcel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;


[Table("reclusos")]
public class Recluso
{
    [Key]
    [Column("id_recluso")]
    public int IdRecluso { get; set; }

    [Column("nombre_completo")]
    [MaxLength(100)]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El nombre solo debe contener letras y espacios.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Column("ci")]
    [MaxLength(20)]
    [Required(ErrorMessage = "El CI es obligatorio.")]
    [RegularExpression(@"^[0-9]{6,8}(-[A-Za-z0-9]{1,3})?$", ErrorMessage = "Formato de CI inválido. Ej: 12345678 o 12345678-1K")]
    public string Ci { get; set; } = string.Empty;

    [Column("edad")]
    [Range(18, 99, ErrorMessage = "La edad debe ser entre 18 y 99 años.")]
    public int Edad { get; set; }

    [Column("delito")]
    [MaxLength(100)]
    [Required(ErrorMessage = "El delito es obligatorio.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El delito solo debe contener letras y espacios.")]
    public string Delito { get; set; } = string.Empty;

    [Column("fecha_ingreso")]
    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; }

    [Column("pabellon_id")]
    public int IdPabellon { get; set; }

    [ForeignKey("IdPabellon")]
    public Pabellon? Pabellon { get; set; }
}