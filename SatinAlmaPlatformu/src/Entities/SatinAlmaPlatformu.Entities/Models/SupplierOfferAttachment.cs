using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Tedarikçi teklifine eklenen dosyalar/ekler
    /// </summary>
    public class SupplierOfferAttachment : BaseEntity
    {
        #region Properties

        /// <summary>
        /// İlgili tedarikçi teklifi ID'si
        /// </summary>
        [Required]
        public int SupplierOfferId { get; set; }

        /// <summary>
        /// Dosya adı
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        /// <summary>
        /// Dosya türü/uzantısı
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FileType { get; set; }

        /// <summary>
        /// Dosya boyutu (bytes)
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Dosya açıklaması
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Fiziksel dosya yolu veya bulut depolama referansı
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string FilePath { get; set; }

        /// <summary>
        /// Dosya yükleme tarihi
        /// </summary>
        [Required]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Yükleyen kullanıcı ID'si
        /// </summary>
        [Required]
        public int UploadedById { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// İlgili tedarikçi teklifi
        /// </summary>
        [ForeignKey(nameof(SupplierOfferId))]
        public virtual SupplierOffer SupplierOffer { get; set; }

        /// <summary>
        /// Yükleyen kullanıcı
        /// </summary>
        [ForeignKey(nameof(UploadedById))]
        public virtual User UploadedBy { get; set; }

        #endregion
    }
} 