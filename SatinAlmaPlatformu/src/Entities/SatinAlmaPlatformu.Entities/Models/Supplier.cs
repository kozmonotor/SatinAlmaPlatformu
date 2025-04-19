using System.ComponentModel.DataAnnotations;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Tedarikçileri temsil eden entity sınıfı
/// </summary>
public class Supplier : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;
    
    [StringLength(100)]
    public string? ContactPerson { get; set; }
    
    [StringLength(150)]
    [EmailAddress]
    public string? Email { get; set; }
    
    [StringLength(50)]
    public string? Phone { get; set; }
    
    [StringLength(500)]
    public string? Address { get; set; }
    
    [StringLength(50)]
    public string? TaxId { get; set; }
    
    [StringLength(100)]
    public string? Website { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    // Tedarikçi kategorileri (birden fazla kategoride hizmet verebilir)
    public virtual ICollection<SupplierCategory> SupplierCategories { get; set; } = new List<SupplierCategory>();
    
    // Tedarikçiye gönderilen teklif istekleri ilişkisi
    public virtual ICollection<BidRequest> BidRequests { get; set; } = new List<BidRequest>();
    
    // Tedarikçinin gönderdiği teklifler ilişkisi
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    
    // Tedarikçiye verilen siparişler ilişkisi
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    // Değerlendirmeler ilişkisi
    public virtual ICollection<SupplierRating> Ratings { get; set; } = new List<SupplierRating>();
} 