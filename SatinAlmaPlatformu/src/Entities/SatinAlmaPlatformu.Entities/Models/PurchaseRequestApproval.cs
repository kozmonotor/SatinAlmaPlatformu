using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.Entities.Models
{
    /// <summary>
    /// Satın alma taleplerinin onay adımlarını ve durumlarını takip eder
    /// </summary>
    public class PurchaseRequestApproval : BaseEntity
    {
        /// <summary>
        /// İlgili satın alma talebi ID'si
        /// </summary>
        [Required]
        public int PurchaseRequestId { get; set; }
        
        /// <summary>
        /// İlgili onay akışı adımı ID'si
        /// </summary>
        [Required]
        public int ApprovalFlowStepId { get; set; }
        
        /// <summary>
        /// Onaylayıcı kullanıcı ID'si
        /// </summary>
        public int? ApproverId { get; set; }
        
        /// <summary>
        /// Onaylayıcı türü (Kullanıcı, Rol veya Departman)
        /// </summary>
        [StringLength(100)]
        public string ApproverType { get; set; }
        
        /// <summary>
        /// Onaylayıcı referans değeri (Rol ID veya Departman ID)
        /// </summary>
        [StringLength(100)]
        public string ApproverReferenceId { get; set; }
        
        /// <summary>
        /// Onay durumu
        /// </summary>
        [Required]
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
        
        /// <summary>
        /// İşlem tarihi (onay veya red)
        /// </summary>
        public DateTime? ActionDate { get; set; }
        
        /// <summary>
        /// Onay/red açıklaması
        /// </summary>
        [StringLength(500)]
        public string Comments { get; set; }
        
        /// <summary>
        /// Onay aksiyonu nasıl gerçekleşti (Manuel, E-posta, Mobil vb.)
        /// </summary>
        [StringLength(50)]
        public string ActionMethod { get; set; }
        
        /// <summary>
        /// Otomatik onay/red olup olmadığı
        /// </summary>
        public bool IsAutomaticAction { get; set; } = false;
        
        /// <summary>
        /// Onay devredildiyse, kimden devredildi
        /// </summary>
        public int? DelegatedFromId { get; set; }
        
        /// <summary>
        /// Birden fazla paralel onay adımı varsa, sıralama
        /// </summary>
        public int ApprovalOrder { get; set; } = 1;
        
        /// <summary>
        /// Son onay tarihi (varsa)
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Gönderilen hatırlatma sayısı
        /// </summary>
        public int ReminderCount { get; set; } = 0;
        
        /// <summary>
        /// Son hatırlatma ne zaman gönderildi
        /// </summary>
        public DateTime? LastReminderSent { get; set; }
        
        #region Navigation Properties
        
        /// <summary>
        /// İlgili satın alma talebi
        /// </summary>
        [ForeignKey(nameof(PurchaseRequestId))]
        public virtual PurchaseRequest PurchaseRequest { get; set; }
        
        /// <summary>
        /// İlgili onay akışı adımı
        /// </summary>
        [ForeignKey(nameof(ApprovalFlowStepId))]
        public virtual ApprovalFlowStep ApprovalFlowStep { get; set; }
        
        /// <summary>
        /// Onaylayıcı kullanıcı
        /// </summary>
        [ForeignKey(nameof(ApproverId))]
        public virtual User Approver { get; set; }
        
        /// <summary>
        /// Onay devredildiyse, asıl onaylayıcı
        /// </summary>
        [ForeignKey(nameof(DelegatedFromId))]
        public virtual User DelegatedFrom { get; set; }
        
        #endregion
    }
} 