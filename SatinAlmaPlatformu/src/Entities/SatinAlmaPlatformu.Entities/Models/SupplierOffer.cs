using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Tedarikçiden alınan teklif bilgisi
    /// </summary>
    public class SupplierOffer : BaseEntity
    {
        #region Properties

        /// <summary>
        /// İlgili satın alma talebi ID'si
        /// </summary>
        [Required]
        public int PurchaseRequestId { get; set; }

        /// <summary>
        /// Teklif veren tedarikçi ID'si
        /// </summary>
        [Required]
        public int SupplierId { get; set; }

        /// <summary>
        /// Teklif referans numarası
        /// </summary>
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Teklif tarihi
        /// </summary>
        [Required]
        public DateTime OfferDate { get; set; }

        /// <summary>
        /// Teklif geçerlilik süresi
        /// </summary>
        public DateTime? ValidUntil { get; set; }

        /// <summary>
        /// Toplam teklif tutarı
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Teklif para birimi
        /// </summary>
        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = "TRY";

        /// <summary>
        /// Teslimat süresi (gün)
        /// </summary>
        public int? DeliveryTimeInDays { get; set; }

        /// <summary>
        /// Ödeme koşulları
        /// </summary>
        [StringLength(200)]
        public string PaymentTerms { get; set; }

        /// <summary>
        /// Teklif notu
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        /// <summary>
        /// Teklif durumu
        /// </summary>
        [Required]
        public OfferStatus Status { get; set; } = OfferStatus.Received;

        /// <summary>
        /// Tedarikçi teklifi değerlendirme puanı (1-5)
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Değerlendirme açıklaması
        /// </summary>
        [StringLength(500)]
        public string EvaluationNotes { get; set; }

        /// <summary>
        /// Teklifin kabul/red tarihi
        /// </summary>
        public DateTime? EvaluationDate { get; set; }

        /// <summary>
        /// Değerlendiren/kabul eden kullanıcı ID'si
        /// </summary>
        public int? EvaluatedById { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// İlgili satın alma talebi
        /// </summary>
        [ForeignKey(nameof(PurchaseRequestId))]
        public virtual PurchaseRequest PurchaseRequest { get; set; }

        /// <summary>
        /// Tedarikçi bilgisi
        /// </summary>
        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// Değerlendiren kullanıcı
        /// </summary>
        [ForeignKey(nameof(EvaluatedById))]
        public virtual User EvaluatedBy { get; set; }

        /// <summary>
        /// Teklif kalemleri
        /// </summary>
        public virtual ICollection<SupplierOfferItem> Items { get; set; }

        /// <summary>
        /// Teklife ait dosyalar/ekler
        /// </summary>
        public virtual ICollection<SupplierOfferAttachment> Attachments { get; set; }

        #endregion
    }

    /// <summary>
    /// Tedarikçi teklifi durumu
    /// </summary>
    public enum OfferStatus
    {
        /// <summary>
        /// Alındı
        /// </summary>
        Received = 0,

        /// <summary>
        /// Değerlendiriliyor
        /// </summary>
        UnderEvaluation = 1,

        /// <summary>
        /// Kabul edildi
        /// </summary>
        Accepted = 2,

        /// <summary>
        /// Reddedildi
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// Süresi doldu
        /// </summary>
        Expired = 4,

        /// <summary>
        /// İptal edildi
        /// </summary>
        Cancelled = 5
    }
} 