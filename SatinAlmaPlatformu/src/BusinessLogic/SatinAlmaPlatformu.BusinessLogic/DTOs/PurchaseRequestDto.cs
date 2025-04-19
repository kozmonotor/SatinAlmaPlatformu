using System;
using System.Collections.Generic;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs
{
    /// <summary>
    /// Satın alma talebi DTO'su
    /// </summary>
    public class PurchaseRequestDto
    {
        /// <summary>
        /// Talep ID'si
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Talep numarası (PR-2023-00001 vb.)
        /// </summary>
        public string RequestNumber { get; set; }
        
        /// <summary>
        /// Talebi oluşturan kullanıcı ID'si
        /// </summary>
        public int RequestedById { get; set; }
        
        /// <summary>
        /// Talebi oluşturan kullanıcı adı
        /// </summary>
        public string RequestedBy { get; set; }
        
        /// <summary>
        /// Departman ID'si
        /// </summary>
        public int DepartmentId { get; set; }
        
        /// <summary>
        /// Departman adı
        /// </summary>
        public string Department { get; set; }
        
        /// <summary>
        /// Talep başlığı
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Talep açıklaması
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Talep nedeni/gerekçesi
        /// </summary>
        public string Justification { get; set; }
        
        /// <summary>
        /// Kategori ID'si
        /// </summary>
        public int? CategoryId { get; set; }
        
        /// <summary>
        /// Kategori adı
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Alt kategori ID'si
        /// </summary>
        public int? SubCategoryId { get; set; }
        
        /// <summary>
        /// Alt kategori adı
        /// </summary>
        public string SubCategory { get; set; }
        
        /// <summary>
        /// Talebin durumu
        /// </summary>
        public PurchaseRequestStatus Status { get; set; }
        
        /// <summary>
        /// Talebin durumu (metin)
        /// </summary>
        public string StatusText { get; set; }
        
        /// <summary>
        /// Talep oluşturma tarihi
        /// </summary>
        public DateTime RequestDate { get; set; }
        
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
        /// Toplam tahmini maliyet
        /// </summary>
        public decimal EstimatedTotalCost { get; set; }
        
        /// <summary>
        /// Para birimi
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// Uygulanan onay akışı ID'si
        /// </summary>
        public int? ApprovalFlowId { get; set; }
        
        /// <summary>
        /// Mevcut onay adımı ID'si
        /// </summary>
        public int? CurrentApprovalStepId { get; set; }
        
        /// <summary>
        /// Mevcut onay adımı adı
        /// </summary>
        public string CurrentApprovalStep { get; set; }
        
        /// <summary>
        /// Öncelik düzeyi
        /// </summary>
        public PriorityLevel Priority { get; set; }
        
        /// <summary>
        /// Öncelik düzeyi (metin)
        /// </summary>
        public string PriorityText { get; set; }
        
        /// <summary>
        /// Talebin iptal edilip edilmediği
        /// </summary>
        public bool IsCancelled { get; set; }
        
        /// <summary>
        /// İptal edildiyse iptal nedeni
        /// </summary>
        public string CancellationReason { get; set; }
        
        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Son güncelleme notu
        /// </summary>
        public string LastUpdateNote { get; set; }
        
        /// <summary>
        /// Talep kalemleri
        /// </summary>
        public List<PurchaseRequestItemDto> Items { get; set; }
    }
    
    /// <summary>
    /// Satın alma talebi kalemi DTO'su
    /// </summary>
    public class PurchaseRequestItemDto
    {
        /// <summary>
        /// Kalem ID'si
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Kalem adı
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Kalem açıklaması
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Miktar
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Ölçü birimi
        /// </summary>
        public string UnitOfMeasure { get; set; }
        
        /// <summary>
        /// Tahmini birim fiyat
        /// </summary>
        public decimal? EstimatedUnitPrice { get; set; }
        
        /// <summary>
        /// Tahmini toplam fiyat (miktar * birim fiyat)
        /// </summary>
        public decimal? EstimatedTotalPrice { get; set; }
        
        /// <summary>
        /// Stok kodu (SKU)
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// Teknik özellikler
        /// </summary>
        public string Specifications { get; set; }
    }
} 