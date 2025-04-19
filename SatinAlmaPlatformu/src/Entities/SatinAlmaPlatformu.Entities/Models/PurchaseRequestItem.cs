using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Satın alma talebi kalemlerini temsil eden entity sınıfı
/// </summary>
public class PurchaseRequestItem : BaseEntity
{
    [Required]
    public int PurchaseRequestId { get; set; }
    
    [ForeignKey("PurchaseRequestId")]
    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [StringLength(50)]
    public string? UnitOfMeasure { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedUnitPrice { get; set; }
    
    [NotMapped]
    public decimal? EstimatedTotalPrice => EstimatedUnitPrice.HasValue ? EstimatedUnitPrice * Quantity : null;
    
    public string? SKU { get; set; }
    
    public string? Specifications { get; set; }
    
    // İlgili sipariş kalemleri ilişkisi
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    // İlgili teklif kalemleri ilişkisi
    public virtual ICollection<BidItem> BidItems { get; set; } = new List<BidItem>();
} 