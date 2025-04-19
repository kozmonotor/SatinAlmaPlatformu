using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Sisteme yüklenen dosya eklerini temsil eden entity sınıfı
/// </summary>
public class Attachment : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = null!;
    
    [StringLength(100)]
    public string? ContentType { get; set; }
    
    [Required]
    public long FileSize { get; set; }
    
    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = null!;
    
    // Dosya referans türü (enum)
    [Required]
    public AttachmentReferenceType ReferenceType { get; set; }
    
    // İlişkili kayıt ID'si (Talep, Teklif, vb.)
    [Required]
    public int ReferenceId { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public int UploadedById { get; set; }
    
    [ForeignKey("UploadedById")]
    public virtual User UploadedBy { get; set; } = null!;
    
    // Talep dosyası ilişkisi
    public int? PurchaseRequestId { get; set; }
    
    [ForeignKey("PurchaseRequestId")]
    public virtual PurchaseRequest? PurchaseRequest { get; set; }
    
    // Teklif isteği dosyası ilişkisi
    public int? BidRequestId { get; set; }
    
    [ForeignKey("BidRequestId")]
    public virtual BidRequest? BidRequest { get; set; }
    
    // Teklif dosyası ilişkisi
    public int? BidId { get; set; }
    
    [ForeignKey("BidId")]
    public virtual Bid? Bid { get; set; }
    
    // Sipariş dosyası ilişkisi
    public int? OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
}

/// <summary>
/// Dosya eki referans türünü belirten enum
/// </summary>
public enum AttachmentReferenceType
{
    PurchaseRequest = 0,
    BidRequest = 1,
    Bid = 2,
    Order = 3,
    Invoice = 4,
    DeliveryNote = 5,
    Other = 6
}