using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Satın alma taleplerini temsil eden entity sınıfı
/// </summary>
public class PurchaseRequest : BaseEntity
{
    /// <summary>
    /// Talep numarası (sistemde otomatik oluşur, PR-2023-00001 vb.)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string RequestNumber { get; set; }
    
    /// <summary>
    /// Talebi oluşturan kullanıcı ID'si
    /// </summary>
    [Required]
    public int RequestedById { get; set; }
    
    /// <summary>
    /// Talebin ait olduğu departman ID'si
    /// </summary>
    [Required]
    public int DepartmentId { get; set; }
    
    /// <summary>
    /// Talep başlığı
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; }
    
    /// <summary>
    /// Talep açıklaması
    /// </summary>
    [StringLength(2000)]
    public string Description { get; set; }
    
    /// <summary>
    /// Talep nedeni/gerekçesi
    /// </summary>
    [StringLength(1000)]
    public string Justification { get; set; }
    
    /// <summary>
    /// Talebin kategori ID'si
    /// </summary>
    public int? CategoryId { get; set; }
    
    /// <summary>
    /// Talebin alt kategori ID'si
    /// </summary>
    public int? SubCategoryId { get; set; }
    
    /// <summary>
    /// Talep durumu
    /// </summary>
    [Required]
    public PurchaseRequestStatus Status { get; set; } = PurchaseRequestStatus.Draft;
    
    /// <summary>
    /// Talep oluşturma tarihi
    /// </summary>
    [Required]
    public DateTime RequestDate { get; set; } = DateTime.Now;
    
    /// <summary>
    /// İhtiyaç/teslimat tarihi
    /// </summary>
    public DateTime? RequiredDate { get; set; }
    
    /// <summary>
    /// Onay için gönderilme tarihi
    /// </summary>
    public DateTime? SubmittedDate { get; set; }
    
    /// <summary>
    /// Tamamlanma tarihi (son onay veya red tarihi)
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    
    /// <summary>
    /// Toplam tahmini bütçe (talep oluşturulurken belirtilen)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedTotalCost { get; set; }
    
    /// <summary>
    /// Para birimi
    /// </summary>
    [StringLength(3)]
    public string Currency { get; set; } = "TRY";
    
    /// <summary>
    /// Uygulanan onay akışı ID'si
    /// </summary>
    public int? ApprovalFlowId { get; set; }
    
    /// <summary>
    /// Mevcut onay adımı ID'si
    /// </summary>
    public int? CurrentApprovalStepId { get; set; }
    
    /// <summary>
    /// Seçilen tedarikçi teklifi ID'si (kabul edilen teklif)
    /// </summary>
    public int? AcceptedSupplierOfferId { get; set; }
    
    /// <summary>
    /// Düzenleyen kullanıcının son güncelleme notu
    /// </summary>
    [StringLength(500)]
    public string LastUpdateNote { get; set; }
    
    /// <summary>
    /// Talep reddedildiyse, red nedeni
    /// </summary>
    [StringLength(500)]
    public string RejectionReason { get; set; }
    
    /// <summary>
    /// Öncelik düzeyi
    /// </summary>
    public PriorityLevel Priority { get; set; } = PriorityLevel.Normal;
    
    /// <summary>
    /// Talebin iptal edilip edilmediği
    /// </summary>
    public bool IsCancelled { get; set; } = false;
    
    /// <summary>
    /// İptal edildiyse iptal nedeni
    /// </summary>
    [StringLength(500)]
    public string CancellationReason { get; set; }
    
    /// <summary>
    /// İptal eden kullanıcı ID'si
    /// </summary>
    public int? CancelledById { get; set; }
    
    /// <summary>
    /// İptal tarihi
    /// </summary>
    public DateTime? CancellationDate { get; set; }
    
    /// <summary>
    /// Bütçe kodları (Muhasebe entegrasyonu için)
    /// </summary>
    [StringLength(100)]
    public string BudgetCode { get; set; }
    
    #region Navigation Properties
    
    /// <summary>
    /// Talebi oluşturan kullanıcı
    /// </summary>
    [ForeignKey(nameof(RequestedById))]
    public virtual User RequestedBy { get; set; }
    
    /// <summary>
    /// Talebin ait olduğu departman
    /// </summary>
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    
    /// <summary>
    /// Talebin kategori bilgisi
    /// </summary>
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    
    /// <summary>
    /// Talebin alt kategori bilgisi
    /// </summary>
    [ForeignKey(nameof(SubCategoryId))]
    public virtual SubCategory SubCategory { get; set; }
    
    /// <summary>
    /// Talebe uygulanan onay akışı
    /// </summary>
    [ForeignKey(nameof(ApprovalFlowId))]
    public virtual ApprovalFlow ApprovalFlow { get; set; }
    
    /// <summary>
    /// Mevcut onay adımı
    /// </summary>
    [ForeignKey(nameof(CurrentApprovalStepId))]
    public virtual ApprovalFlowStep CurrentApprovalStep { get; set; }
    
    /// <summary>
    /// Kabul edilen tedarikçi teklifi
    /// </summary>
    [ForeignKey(nameof(AcceptedSupplierOfferId))]
    public virtual SupplierOffer AcceptedSupplierOffer { get; set; }
    
    /// <summary>
    /// İptal eden kullanıcı
    /// </summary>
    [ForeignKey(nameof(CancelledById))]
    public virtual User CancelledBy { get; set; }
    
    /// <summary>
    /// Talep kalemleri
    /// </summary>
    public virtual ICollection<PurchaseRequestItem> Items { get; set; }
    
    /// <summary>
    /// Talep onay adımları
    /// </summary>
    public virtual ICollection<PurchaseRequestApproval> Approvals { get; set; }
    
    /// <summary>
    /// Tedarikçi teklifleri
    /// </summary>
    public virtual ICollection<SupplierOffer> SupplierOffers { get; set; }
    
    /// <summary>
    /// Talep güncellemeleri (geçmiş)
    /// </summary>
    public virtual ICollection<PurchaseRequestHistory> History { get; set; }
    
    /// <summary>
    /// Talep ekleri
    /// </summary>
    public virtual ICollection<PurchaseRequestAttachment> Attachments { get; set; }
    
    /// <summary>
    /// Talep notları
    /// </summary>
    public virtual ICollection<PurchaseRequestNote> Notes { get; set; }
    
    /// <summary>
    /// Revizyon istekleri
    /// </summary>
    public virtual ICollection<PurchaseRequestRevision> Revisions { get; set; }
    
    #endregion
}

/// <summary>
/// Satın alma talebi durumunu belirten enum
/// </summary>
public enum PurchaseRequestStatus
{
    /// <summary>
    /// Taslak (henüz gönderilmemiş)
    /// </summary>
    Draft = 0,
    
    /// <summary>
    /// Onay sürecinde (onaya gönderilmiş, değerlendiriliyor)
    /// </summary>
    InReview = 1,
    
    /// <summary>
    /// Onaylandı (tüm onaylar tamamlandı)
    /// </summary>
    Approved = 2,
    
    /// <summary>
    /// Reddedildi (herhangi bir onay adımında reddedildi)
    /// </summary>
    Rejected = 3,
    
    /// <summary>
    /// Revizyon istendi (düzeltme/ek bilgi talep edildi)
    /// </summary>
    RevisionRequested = 4,
    
    /// <summary>
    /// Teklif toplama aşamasında
    /// </summary>
    CollectingOffers = 5,
    
    /// <summary>
    /// Teklif değerlendirme aşamasında
    /// </summary>
    EvaluatingOffers = 6,
    
    /// <summary>
    /// Sipariş oluşturuldu
    /// </summary>
    OrderCreated = 7,
    
    /// <summary>
    /// Sipariş tamamlandı (teslim alındı)
    /// </summary>
    Completed = 8,
    
    /// <summary>
    /// İptal edildi
    /// </summary>
    Cancelled = 9
}

/// <summary>
/// Öncelik düzeyi
/// </summary>
public enum PriorityLevel
{
    /// <summary>
    /// Düşük
    /// </summary>
    Low = 0,
    
    /// <summary>
    /// Normal
    /// </summary>
    Normal = 1,
    
    /// <summary>
    /// Yüksek
    /// </summary>
    High = 2,
    
    /// <summary>
    /// Acil
    /// </summary>
    Urgent = 3,
    
    /// <summary>
    /// Kritik
    /// </summary>
    Critical = 4
} 