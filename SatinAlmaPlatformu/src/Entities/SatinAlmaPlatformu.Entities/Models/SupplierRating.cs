using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Tedarikçi değerlendirmelerini temsil eden entity sınıfı
/// </summary>
public class SupplierRating : BaseEntity
{
    [Required]
    public int SupplierId { get; set; }
    
    [ForeignKey("SupplierId")]
    public virtual Supplier Supplier { get; set; } = null!;
    
    public int? OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
    
    [Required]
    public int RatedById { get; set; }
    
    [ForeignKey("RatedById")]
    public virtual User RatedBy { get; set; } = null!;
    
    // Kalite puanı (1-5)
    public int QualityRating { get; set; }
    
    // Teslimat puanı (1-5)
    public int DeliveryRating { get; set; }
    
    // Fiyat/Değer puanı (1-5)
    public int PriceRating { get; set; }
    
    // İletişim puanı (1-5)
    public int CommunicationRating { get; set; }
    
    // Genel memnuniyet puanı (1-5)
    public int OverallRating { get; set; }
    
    [StringLength(1000)]
    public string? Comments { get; set; }
    
    // Bu değerlendirme sipariş tamamlandıktan sonra mı yapıldı
    public bool IsPostDelivery { get; set; } = true;
} 