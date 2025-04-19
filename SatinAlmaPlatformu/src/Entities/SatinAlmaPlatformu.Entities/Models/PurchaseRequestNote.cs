using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Satın alma talebiyle ilgili notlar
    /// </summary>
    public class PurchaseRequestNote : BaseEntity
    {
        #region Properties

        /// <summary>
        /// İlgili satın alma talebi ID'si
        /// </summary>
        [Required]
        public int PurchaseRequestId { get; set; }

        /// <summary>
        /// Not içeriği
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        /// <summary>
        /// Not tipi (iç not, tedarikçi notu vb.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NoteType { get; set; } = "Internal";

        /// <summary>
        /// Not tarihi
        /// </summary>
        [Required]
        public DateTime NoteDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Notu ekleyen kullanıcı ID'si
        /// </summary>
        [Required]
        public int CreatedById { get; set; }

        /// <summary>
        /// İşlem yapılan IP adresi
        /// </summary>
        [StringLength(50)]
        public string IpAddress { get; set; }

        /// <summary>
        /// Not özel mi (belirli kullanıcılara mı gösterilecek)
        /// </summary>
        public bool IsPrivate { get; set; } = false;

        #endregion

        #region Navigation Properties

        /// <summary>
        /// İlgili satın alma talebi
        /// </summary>
        [ForeignKey(nameof(PurchaseRequestId))]
        public virtual PurchaseRequest PurchaseRequest { get; set; }

        /// <summary>
        /// Notu ekleyen kullanıcı
        /// </summary>
        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; }

        #endregion
    }
} 