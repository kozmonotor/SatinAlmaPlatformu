using System.ComponentModel.DataAnnotations;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Satın alma taleplerinin kategorilerini temsil eden entity sınıfı
/// </summary>
public class Category : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public int? ParentCategoryId { get; set; }
    
    public virtual Category? ParentCategory { get; set; }
    
    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
    
    // Kategori ile ilişkili talepler
    public virtual ICollection<PurchaseRequest> Requests { get; set; } = new List<PurchaseRequest>();
    
    // Kategori ile ilişkili onay akışları
    public virtual ICollection<ApprovalFlow> ApprovalFlows { get; set; } = new List<ApprovalFlow>();
} 