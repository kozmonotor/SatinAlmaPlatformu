using System;
using System.ComponentModel.DataAnnotations;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Tüm entity sınıfları için temel sınıf
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Entity ID'si
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Entity'nin oluşturulma tarihi
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Entity'yi oluşturan kullanıcı ID'si
        /// </summary>
        public int? CreatedById { get; set; }
        
        /// <summary>
        /// Entity'nin son güncellenme tarihi
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        
        /// <summary>
        /// Entity'yi güncelleyen kullanıcı ID'si
        /// </summary>
        public int? UpdatedById { get; set; }
        
        /// <summary>
        /// Entity'nin aktif olup olmadığı
        /// Silme işlemleri için soft delete yönteminde kullanılabilir
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 