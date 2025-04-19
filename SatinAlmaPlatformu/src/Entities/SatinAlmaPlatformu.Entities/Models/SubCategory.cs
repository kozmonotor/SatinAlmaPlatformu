using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Satın alma kategorisinin alt kategorisi
    /// </summary>
    public class SubCategory : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Alt kategori adı
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Alt kategori açıklaması
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Üst kategori ID'si
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Sıralama değeri
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Alt kategori aktif mi
        /// </summary>
        public bool IsActive { get; set; } = true;

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Üst kategori
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Bu alt kategoriye ait satın alma talepleri
        /// </summary>
        public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }

        #endregion
    }
} 