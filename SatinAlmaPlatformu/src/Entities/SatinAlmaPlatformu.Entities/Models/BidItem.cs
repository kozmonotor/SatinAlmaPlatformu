using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Teklif kalemlerini temsil eden entity sınıfı
/// </summary>
public class BidItem : BaseEntity
{
    [Required]
    public int BidId { get; set; }
    
    [ForeignKey("BidId")]
    public virtual Bid Bid { get; set; } = null!;
    
    public int? PurchaseRequestItemId { get; set; }
    
    [ForeignKey("PurchaseRequestItemId")]
    public virtual PurchaseRequestItem? PurchaseRequestItem { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [StringLength(50)]
    public string? UnitOfMeasure { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    [NotMapped]
    public decimal TotalPrice => UnitPrice * Quantity;
    
    // Alternatif teklif (orijinal talep kaleminden farklı bir ürün teklif edildiğinde true)
    public bool IsAlternative { get; set; } = false;
    
    // Alternatif teklifin açıklaması/nedeni
    public string? AlternativeReason { get; set; }
} 