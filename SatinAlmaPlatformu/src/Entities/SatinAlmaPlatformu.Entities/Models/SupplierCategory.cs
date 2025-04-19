using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Tedarikçiler ile kategoriler arasındaki ilişkiyi temsil eden entity sınıfı
/// </summary>
public class SupplierCategory : BaseEntity
{
    public int SupplierId { get; set; }
    
    public int CategoryId { get; set; }
    
    [ForeignKey("SupplierId")]
    public virtual Supplier Supplier { get; set; } = null!;
    
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; } = null!;
    
    // Tedarikçinin bu kategorideki uzmanlık seviyesi (1-5)
    public int? ExpertiseLevel { get; set; }
    
    // Tedarikçinin bu kategoride sunduğu hizmet/ürün açıklaması
    public string? ServiceDescription { get; set; }
} 