using SistemaCarcel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCarcel.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_personal")]
    public int IdPersonal { get; set; }

    [Column("username")]
    [MaxLength(50)]
    [Required]
    public string Username { get; set; } = null!;

    [NotMapped]
    [Required]
    public string Password { get; set; } = null!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("rol")]
    [MaxLength(20)]
    [Required]
    public string Rol { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("IdPersonal")]
    public virtual Personal? Personal { get; set; } = null!;
}
