using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Kullanıcılar ile roller arasındaki ilişkiyi temsil eden entity sınıfı
/// </summary>
public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    
    public int RoleId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    
    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; } = null!;
} 