using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Sipariş kalemlerini temsil eden entity sınıfı
/// </summary>
public class OrderItem : BaseEntity
{
    [Required]
    public int OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;
    
    public int? PurchaseRequestItemId { get; set; }
    
    [ForeignKey("PurchaseRequestItemId")]
    public virtual PurchaseRequestItem? PurchaseRequestItem { get; set; }
    
    public int? BidItemId { get; set; }
    
    [ForeignKey("BidItemId")]
    public virtual BidItem? BidItem { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    public int? ReceivedQuantity { get; set; }
    
    [StringLength(50)]
    public string? UnitOfMeasure { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    [NotMapped]
    public decimal TotalPrice => UnitPrice * Quantity;
    
    public string? SKU { get; set; }
    
    // Sipariş kalemi durumu
    public OrderItemStatus Status { get; set; } = OrderItemStatus.Ordered;
}

/// <summary>
/// Sipariş kalemi durumunu belirten enum
/// </summary>
public enum OrderItemStatus
{
    Ordered = 0,
    PartiallyShipped = 1,
    Shipped = 2,
    PartiallyDelivered = 3,
    Delivered = 4,
    Completed = 5,
    Returned = 6,
    Cancelled = 7
}