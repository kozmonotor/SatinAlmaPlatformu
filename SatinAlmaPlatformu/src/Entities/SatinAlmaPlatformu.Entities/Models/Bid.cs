using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Tedarikçilerin gönderdiği teklifleri temsil eden entity sınıfı
/// </summary>
public class Bid : BaseEntity
{
    [Required]
    public int BidRequestId { get; set; }
    
    [ForeignKey("BidRequestId")]
    public virtual BidRequest BidRequest { get; set; } = null!;
    
    [Required]
    public int SupplierId { get; set; }
    
    [ForeignKey("SupplierId")]
    public virtual Supplier Supplier { get; set; } = null!;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    public string? Currency { get; set; } = "TRY";
    
    [Required]
    public BidStatus Status { get; set; } = BidStatus.Submitted;
    
    public DateTime? ValidUntil { get; set; }
    
    public int DeliveryTimeInDays { get; set; }
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    [StringLength(500)]
    public string? PaymentTerms { get; set; }
    
    public string? DeliveryTerms { get; set; }
    
    public string? Warranty { get; set; }
    
    // Teklif kalemleri ilişkisi
    public virtual ICollection<BidItem> Items { get; set; } = new List<BidItem>();
    
    // Teklif ekleri ilişkisi
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}

/// <summary>
/// Teklif durumunu belirten enum
/// </summary>
public enum BidStatus
{
    Draft = 0,
    Submitted = 1,
    UnderReview = 2,
    NeedsClarification = 3,
    Accepted = 4,
    PartiallyAccepted = 5,
    Rejected = 6,
    Expired = 7,
    Cancelled = 8
} 