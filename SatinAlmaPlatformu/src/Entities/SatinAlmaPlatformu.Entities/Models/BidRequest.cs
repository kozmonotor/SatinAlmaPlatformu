using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Tedarikçilere gönderilen teklif isteklerini temsil eden entity sınıfı
/// </summary>
public class BidRequest : BaseEntity
{
    [Required]
    public int PurchaseRequestId { get; set; }
    
    [ForeignKey("PurchaseRequestId")]
    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;
    
    [Required]
    public int SupplierId { get; set; }
    
    [ForeignKey("SupplierId")]
    public virtual Supplier Supplier { get; set; } = null!;
    
    [Required]
    public BidRequestStatus Status { get; set; } = BidRequestStatus.Sent;
    
    public DateTime? DueDate { get; set; }
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    // Tedarikçiden gelen teklif ilişkisi (bir teklif isteğine bir teklif gelir)
    public virtual Bid? Bid { get; set; }
    
    // Teklif isteği ekleri ilişkisi
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}

/// <summary>
/// Teklif isteği durumunu belirten enum
/// </summary>
public enum BidRequestStatus
{
    Draft = 0,
    Sent = 1,
    Viewed = 2,
    InProgress = 3,
    Responded = 4,
    Declined = 5,
    Expired = 6,
    Cancelled = 7
} 