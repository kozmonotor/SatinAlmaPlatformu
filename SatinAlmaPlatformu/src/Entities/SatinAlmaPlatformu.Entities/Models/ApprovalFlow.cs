using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Bir onay sürecinin tanımını içerir. Farklı departmanlar, bütçe aralıkları veya 
/// satın alma kategorileri için farklı onay süreçleri tanımlanabilir.
/// </summary>
public class ApprovalFlow : BaseEntity
{
    /// <summary>
    /// Onay akışının adı
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Onay akışının tanımı ve açıklaması
    /// </summary>
    [StringLength(500)]
    public string Description { get; set; } = null!;
    
    /// <summary>
    /// Hedef departman ID'si (belirli bir departman için özelleştirilmiş akış ise)
    /// </summary>
    public int? DepartmentId { get; set; }
    
    /// <summary>
    /// Hedef kategori ID'si (belirli bir kategori için özelleştirilmiş akış ise)
    /// </summary>
    public int? CategoryId { get; set; }
    
    /// <summary>
    /// Akışı tetikleyen minimum tutar (bütçe bazlı akış için)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinimumAmount { get; set; }
    
    /// <summary>
    /// Akışı tetikleyen maksimum tutar (bütçe bazlı akış için)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaximumAmount { get; set; }
    
    /// <summary>
    /// Para birimi
    /// </summary>
    [StringLength(3)]
    public string Currency { get; set; } = "TRY";
    
    /// <summary>
    /// Onay akışının öncelik sırası (birden fazla akış koşulunu sağlayan talepler için)
    /// Küçük değer daha yüksek önceliği belirtir
    /// </summary>
    public int Priority { get; set; } = 100;
    
    /// <summary>
    /// Akışın aktif olup olmadığı
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Akışın yaratılma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Akışı oluşturan kullanıcı ID'si
    /// </summary>
    public int CreatedById { get; set; }
    
    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Son güncelleyen kullanıcı ID'si
    /// </summary>
    public int? UpdatedById { get; set; }
    
    /// <summary>
    /// Bu onay akışına ait adımlar
    /// </summary>
    public virtual ICollection<ApprovalFlowStep> Steps { get; set; } = new List<ApprovalFlowStep>();
    
    /// <summary>
    /// Bu onay akışının uygulandığı talepler
    /// </summary>
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; } = new List<PurchaseRequest>();
    
    /// <summary>
    /// İlişkili departman
    /// </summary>
    [ForeignKey("DepartmentId")]
    public virtual Department? Department { get; set; }
    
    /// <summary>
    /// İlişkili kategori
    /// </summary>
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
    
    /// <summary>
    /// Akışı oluşturan kullanıcı
    /// </summary>
    [ForeignKey(nameof(CreatedById))]
    public virtual User CreatedBy { get; set; }
    
    /// <summary>
    /// Son güncelleyen kullanıcı
    /// </summary>
    [ForeignKey(nameof(UpdatedById))]
    public virtual User UpdatedBy { get; set; }
}

/// <summary>
/// Onay akışı durum tipleri
/// </summary>
public enum FlowStatus
{
    /// <summary>
    /// Pasif (kullanılamaz) durum
    /// </summary>
    Passive = 0,
    
    /// <summary>
    /// Aktif (kullanılabilir) durum
    /// </summary>
    Active = 1,
    
    /// <summary>
    /// Arşivlenmiş durum
    /// </summary>
    Archived = 2
} 