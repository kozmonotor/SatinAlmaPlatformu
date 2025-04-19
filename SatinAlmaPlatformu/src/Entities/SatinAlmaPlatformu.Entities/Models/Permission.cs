using System.ComponentModel.DataAnnotations;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Sistemdeki yetkileri temsil eden entity sınıfı
/// </summary>
public class Permission : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string Code { get; set; } = null!;
    
    [StringLength(255)]
    public string? Description { get; set; }
    
    // Yetki-Rol ilişkisi
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
} 