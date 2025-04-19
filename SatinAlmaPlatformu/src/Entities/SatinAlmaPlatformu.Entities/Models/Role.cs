using System.ComponentModel.DataAnnotations;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Kullanıcı rollerini temsil eden entity sınıfı
/// </summary>
public class Role : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
    
    [StringLength(255)]
    public string? Description { get; set; }
    
    // Role sahip kullanıcılar ilişkisi
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    // Rol yetkilerinin ilişkisi
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
} 