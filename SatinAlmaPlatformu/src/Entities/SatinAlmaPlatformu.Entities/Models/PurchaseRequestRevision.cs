using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Satın alma talebi için istenen revizyon bilgilerini tutar
    /// </summary>
    public class PurchaseRequestRevision : BaseEntity
    {
        /// <summary>
        /// İlgili satın alma talebi ID'si
        /// </summary>
        [Required]
        public int PurchaseRequestId { get; set; }
        
        /// <summary>
        /// Revizyonu isteyen kullanıcı ID'si
        /// </summary>
        [Required]
        public string RequestedById { get; set; }
        
        /// <summary>
        /// Revizyon isteği tarihi
        /// </summary>
        [Required]
        public DateTime RequestedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Revizyon için son tarih
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Revizyon açıklaması/yorumu
        /// </summary>
        [StringLength(500)]
        public string Comments { get; set; }
        
        /// <summary>
        /// Revizyon tamamlandı mı?
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; } = false;
        
        /// <summary>
        /// Revizyonu tamamlayan kullanıcı ID'si
        /// </summary>
        public string CompletedById { get; set; }
        
        /// <summary>
        /// Revizyon tamamlama tarihi
        /// </summary>
        public DateTime? CompletedDate { get; set; }
        
        /// <summary>
        /// Revizyon tamamlandığında eklenen not
        /// </summary>
        [StringLength(500)]
        public string RevisionNote { get; set; }
        
        /// <summary>
        /// Revizyon öncesi talep durumu
        /// </summary>
        public PurchaseRequestStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// Revizyon öncesi onay adımı ID'si (revizyon sonrası dönüş için)
        /// </summary>
        public int? PreviousApprovalStepId { get; set; }
        
        /// <summary>
        /// Revizyon tamamlandığında hangi adıma dönüleceği
        /// </summary>
        public int? ReturnToStepId { get; set; }
        
        #region Navigation Properties
        
        /// <summary>
        /// İlgili satın alma talebi
        /// </summary>
        [ForeignKey(nameof(PurchaseRequestId))]
        public virtual PurchaseRequest PurchaseRequest { get; set; }
        
        /// <summary>
        /// Revizyonu isteyen kullanıcı
        /// </summary>
        [ForeignKey(nameof(RequestedById))]
        public virtual User RequestedBy { get; set; }
        
        /// <summary>
        /// Revizyonu tamamlayan kullanıcı
        /// </summary>
        [ForeignKey(nameof(CompletedById))]
        public virtual User CompletedBy { get; set; }
        
        /// <summary>
        /// Revizyon öncesi onay adımı
        /// </summary>
        [ForeignKey(nameof(PreviousApprovalStepId))]
        public virtual ApprovalFlowStep PreviousApprovalStep { get; set; }
        
        /// <summary>
        /// Revizyon tamamlandığında dönülecek adım
        /// </summary>
        [ForeignKey(nameof(ReturnToStepId))]
        public virtual ApprovalFlowStep ReturnToStep { get; set; }
        
        #endregion
    }
} 