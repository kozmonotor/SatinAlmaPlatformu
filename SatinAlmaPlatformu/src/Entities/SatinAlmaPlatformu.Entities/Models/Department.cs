using System.ComponentModel.DataAnnotations;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Şirket içindeki departmanları temsil eden entity sınıfı
/// </summary>
public class Department : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(50)]
    public string? Code { get; set; }
    
    // Departmana ait kullanıcılar ilişkisi
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    
    // Departmana ait satın alma talepleri ilişkisi
    public virtual ICollection<PurchaseRequest> Requests { get; set; } = new List<PurchaseRequest>();
} 