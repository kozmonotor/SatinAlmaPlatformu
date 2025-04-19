using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Tedarikçi teklifinde yer alan kalemler
    /// </summary>
    public class SupplierOfferItem : BaseEntity
    {
        #region Properties

        /// <summary>
        /// İlgili tedarikçi teklifi ID'si
        /// </summary>
        [Required]
        public int SupplierOfferId { get; set; }

        /// <summary>
        /// İlgili talep kalemi ID'si (opsiyonel)
        /// </summary>
        public int? PurchaseRequestItemId { get; set; }

        /// <summary>
        /// Kalem adı/açıklaması
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ItemName { get; set; }

        /// <summary>
        /// Ürün kodu
        /// </summary>
        [StringLength(100)]
        public string ProductCode { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Ölçü birimi
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Unit { get; set; }

        /// <summary>
        /// Birim fiyat
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Toplam fiyat
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// KDV oranı
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal VatRate { get; set; } = 18.00M;

        /// <summary>
        /// KDV dahil toplam fiyat
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPriceWithVat { get; set; }

        /// <summary>
        /// Kalem notu
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// İlgili tedarikçi teklifi
        /// </summary>
        [ForeignKey(nameof(SupplierOfferId))]
        public virtual SupplierOffer SupplierOffer { get; set; }

        /// <summary>
        /// İlgili talep kalemi
        /// </summary>
        [ForeignKey(nameof(PurchaseRequestItemId))]
        public virtual PurchaseRequestItem PurchaseRequestItem { get; set; }

        #endregion
    }
} 