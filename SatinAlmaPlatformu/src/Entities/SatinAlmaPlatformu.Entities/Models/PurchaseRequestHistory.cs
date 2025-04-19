using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Satın alma talebindeki değişiklikleri kaydeder
    /// </summary>
    public class PurchaseRequestHistory : BaseEntity
    {
        #region Properties

        /// <summary>
        /// İlgili satın alma talebi ID'si
        /// </summary>
        [Required]
        public int PurchaseRequestId { get; set; }

        /// <summary>
        /// Değişiklik yapan kullanıcı ID'si
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Değişiklik tarihi
        /// </summary>
        [Required]
        public DateTime ChangeDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Değişiklik türü
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ChangeType { get; set; }

        /// <summary>
        /// Değişiklik açıklaması
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Değişiklikten önceki değer
        /// </summary>
        [StringLength(2000)]
        public string OldValue { get; set; }

        /// <summary>
        /// Değişiklikten sonraki değer
        /// </summary>
        [StringLength(2000)]
        public string NewValue { get; set; }

        /// <summary>
        /// Değişikliğin kaynağı (web, mobil vb.)
        /// </summary>
        [StringLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// İşlem yapılan IP adresi
        /// </summary>
        [StringLength(50)]
        public string IpAddress { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// İlgili satın alma talebi
        /// </summary>
        [ForeignKey(nameof(PurchaseRequestId))]
        public virtual PurchaseRequest PurchaseRequest { get; set; }

        /// <summary>
        /// Değişiklik yapan kullanıcı
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        #endregion
    }
} 