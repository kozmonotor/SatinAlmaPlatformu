using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Roller ile yetkiler arasındaki ilişkiyi temsil eden entity sınıfı
/// </summary>
public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    
    public int PermissionId { get; set; }
    
    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; } = null!;
    
    [ForeignKey("PermissionId")]
    public virtual Permission Permission { get; set; } = null!;
} 