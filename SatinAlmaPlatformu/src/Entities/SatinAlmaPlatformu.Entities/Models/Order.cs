using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Siparişleri temsil eden entity sınıfı
/// </summary>
public class Order : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string OrderNumber { get; set; } = null!;
    
    public int? PurchaseRequestId { get; set; }
    
    [ForeignKey("PurchaseRequestId")]
    public virtual PurchaseRequest? PurchaseRequest { get; set; }
    
    public int? BidId { get; set; }
    
    [ForeignKey("BidId")]
    public virtual Bid? Bid { get; set; }
    
    [Required]
    public int SupplierId { get; set; }
    
    [ForeignKey("SupplierId")]
    public virtual Supplier Supplier { get; set; } = null!;
    
    [Required]
    public int CreatedById { get; set; }
    
    [ForeignKey("CreatedById")]
    public virtual User CreatedBy { get; set; } = null!;
    
    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    public string? Currency { get; set; } = "TRY";
    
    public DateTime? OrderDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? ExpectedDeliveryDate { get; set; }
    
    public DateTime? ActualDeliveryDate { get; set; }
    
    [StringLength(500)]
    public string? DeliveryAddress { get; set; }
    
    [StringLength(500)]
    public string? BillingAddress { get; set; }
    
    [StringLength(500)]
    public string? PaymentTerms { get; set; }
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    public string? InvoiceNumber { get; set; }
    
    public DateTime? InvoiceDate { get; set; }
    
    // Sipariş kalemleri ilişkisi
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    
    // Sipariş ekleri ilişkisi
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}

/// <summary>
/// Sipariş durumunu belirten enum
/// </summary>
public enum OrderStatus
{
    Created = 0,
    Sent = 1,
    Confirmed = 2,
    InProgress = 3,
    PartiallyShipped = 4,
    Shipped = 5,
    PartiallyDelivered = 6,
    Delivered = 7,
    Completed = 8,
    Cancelled = 9,
    OnHold = 10,
    Disputed = 11
} 