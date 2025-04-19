using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Onay akışındaki her bir adımı temsil eder
/// </summary>
public class ApprovalFlowStep : BaseEntity
{
    /// <summary>
    /// Bu adımın bağlı olduğu onay akışı ID'si
    /// </summary>
    [Required]
    public int ApprovalFlowId { get; set; }
    
    /// <summary>
    /// Adımın sıra numarası
    /// </summary>
    [Required]
    public int StepOrder { get; set; }
    
    /// <summary>
    /// Adımın başlığı/adı
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    /// <summary>
    /// Adımın açıklaması
    /// </summary>
    [StringLength(500)]
    public string Description { get; set; }
    
    /// <summary>
    /// Adımın türü (Bireysel onay, grup onayı, etc.)
    /// </summary>
    public StepType Type { get; set; }
    
    /// <summary>
    /// Bu adımı onaylayacak olan kullanıcı ID'si (Type = IndividualApproval ise)
    /// </summary>
    public int? ApproverId { get; set; }
    
    /// <summary>
    /// Bu adımı onaylayacak olan rol ID'si (Type = RoleApproval ise)
    /// </summary>
    public int? ApproverRoleId { get; set; }
    
    /// <summary>
    /// Bu adımı onaylayacak olan departman ID'si (Type = DepartmentApproval ise)
    /// </summary>
    public int? ApproverDepartmentId { get; set; }
    
    /// <summary>
    /// Bu adım için gerekli olan minimum onay sayısı (Type = MultipleApproval ise)
    /// </summary>
    public int? RequiredApprovalCount { get; set; }
    
    /// <summary>
    /// Onay süre limiti (saat cinsinden). Belirtilen süre içinde onaylanmazsa ne yapılacağını TimeoutAction belirler.
    /// </summary>
    public int? TimeoutHours { get; set; }
    
    /// <summary>
    /// Zaman aşımında yapılacak işlem
    /// </summary>
    public TimeoutAction? TimeoutAction { get; set; }
    
    /// <summary>
    /// Adımda gerçekleştirilecek otomatik işlem (varsa)
    /// </summary>
    public AutomatedAction? AutomatedAction { get; set; }
    
    /// <summary>
    /// Adımın aktif olup olmadığı (false ise, akış bu adımı atlayacaktır)
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Adımda reddetme durumunda iptal mi edilecek yoksa geri mi dönülecek
    /// </summary>
    public RejectAction RejectAction { get; set; } = RejectAction.Cancel;
    
    /// <summary>
    /// Reddedildiğinde dönülecek adım numarası (RejectAction = ReturnToStep ise)
    /// </summary>
    public int? ReturnToStepOrder { get; set; }
    
    /// <summary>
    /// İlişkili onay akışı
    /// </summary>
    [ForeignKey("ApprovalFlowId")]
    public virtual ApprovalFlow ApprovalFlow { get; set; } = null!;
    
    /// <summary>
    /// İlişkili onaylayıcı kullanıcı
    /// </summary>
    [ForeignKey("ApproverId")]
    public virtual User? Approver { get; set; }
    
    /// <summary>
    /// İlişkili onaylayıcı rol
    /// </summary>
    [ForeignKey("ApproverRoleId")]
    public virtual Role? ApproverRole { get; set; }
    
    /// <summary>
    /// İlişkili onaylayıcı departman
    /// </summary>
    [ForeignKey("ApproverDepartmentId")]
    public virtual Department? ApproverDepartment { get; set; }
    
    /// <summary>
    /// Bu adımın gerçekleştirilmiş örnekleri (taleplerin onay adımı kayıtları)
    /// </summary>
    public virtual ICollection<PurchaseRequestApproval> RequestApprovals { get; set; } = new List<PurchaseRequestApproval>();
}

/// <summary>
/// Onay adımının türünü belirtir
/// </summary>
public enum StepType
{
    /// <summary>
    /// Tek bir kişi tarafından onaylanır
    /// </summary>
    IndividualApproval = 0,
    
    /// <summary>
    /// Belirli bir roldeki herhangi bir kişi tarafından onaylanır
    /// </summary>
    RoleApproval = 1,
    
    /// <summary>
    /// Belirli bir departmandaki yönetici tarafından onaylanır
    /// </summary>
    DepartmentApproval = 2,
    
    /// <summary>
    /// Belirli sayıda kişinin onayı gerekir
    /// </summary>
    MultipleApproval = 3,
    
    /// <summary>
    /// Otomatik olarak onaylanır (sistem tarafından)
    /// </summary>
    AutomaticApproval = 4
}

/// <summary>
/// Onay zaman aşımında yapılacak işlem
/// </summary>
public enum TimeoutAction
{
    /// <summary>
    /// Hiçbir şey yapma (bekletmeye devam et)
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Otomatik olarak onayla
    /// </summary>
    AutoApprove = 1,
    
    /// <summary>
    /// Otomatik olarak reddet
    /// </summary>
    AutoReject = 2,
    
    /// <summary>
    /// Bir üst onaylayıcıya yükselt
    /// </summary>
    Escalate = 3,
    
    /// <summary>
    /// Hatırlatma bildirimi gönder
    /// </summary>
    SendReminder = 4
}

/// <summary>
/// Otomatik adım eylemleri
/// </summary>
public enum AutomatedAction
{
    /// <summary>
    /// Otomatik eylem yok
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Tedarikçilere teklif isteği gönder
    /// </summary>
    SendRfqToSuppliers = 1,
    
    /// <summary>
    /// Bütçe kontrolü yap
    /// </summary>
    BudgetCheck = 2,
    
    /// <summary>
    /// Stok kontrolü yap
    /// </summary>
    InventoryCheck = 3,
    
    /// <summary>
    /// E-posta bildirimi gönder
    /// </summary>
    SendNotification = 4
}

/// <summary>
/// Reddetme durumunda yapılacak işlem
/// </summary>
public enum RejectAction
{
    /// <summary>
    /// Talebi tamamen iptal et
    /// </summary>
    Cancel = 0,
    
    /// <summary>
    /// Talebi talep sahibine geri gönder
    /// </summary>
    ReturnToRequester = 1,
    
    /// <summary>
    /// Belirli bir adıma geri dön
    /// </summary>
    ReturnToStep = 2
} 