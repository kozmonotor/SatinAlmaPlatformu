using System;
using System.Collections.Generic;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs
{
    /// <summary>
    /// Talep için revizyon isteme DTO'su
    /// </summary>
    public class RevisionRequestDto
    {
        /// <summary>
        /// Revizyon isteyen kullanıcının yorumu/açıklaması
        /// </summary>
        public string Comments { get; set; }
        
        /// <summary>
        /// Revizyon için son tarih (opsiyonel)
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Revizyon sonrası hangi onay adımına dönüleceği (opsiyonel)
        /// </summary>
        public int? ReturnToStepId { get; set; }
    }
    
    /// <summary>
    /// Revizyonu yanıtlama DTO'su
    /// </summary>
    public class RevisionResponseDto
    {
        /// <summary>
        /// Güncellenen bilgilere ilişkin açıklama
        /// </summary>
        public string RevisionNote { get; set; }
        
        /// <summary>
        /// Güncellenmiş talep bilgileri
        /// </summary>
        public UpdatePurchaseRequestDto UpdatedRequest { get; set; }
        
        /// <summary>
        /// Talep tekrar onaya gönderilsin mi?
        /// </summary>
        public bool SubmitForApproval { get; set; } = true;
    }
    
    /// <summary>
    /// Satın alma talebini güncellemek için DTO
    /// </summary>
    public class UpdatePurchaseRequestDto
    {
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
        /// Kategori ID
        /// </summary>
        public int? CategoryId { get; set; }
        
        /// <summary>
        /// Alt kategori ID
        /// </summary>
        public int? SubCategoryId { get; set; }
        
        /// <summary>
        /// İhtiyaç/teslimat tarihi
        /// </summary>
        public DateTime? RequiredDate { get; set; }
        
        /// <summary>
        /// Toplam tahmini bütçe
        /// </summary>
        public decimal EstimatedTotalCost { get; set; }
        
        /// <summary>
        /// Para birimi
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// Öncelik düzeyi
        /// </summary>
        public int Priority { get; set; }
        
        /// <summary>
        /// Bütçe kodları (Muhasebe entegrasyonu için)
        /// </summary>
        public string BudgetCode { get; set; }
        
        /// <summary>
        /// Güncellemeye dair açıklama/not
        /// </summary>
        public string UpdateNote { get; set; }
        
        /// <summary>
        /// Güncellenen talep kalemleri
        /// </summary>
        public List<UpdatePurchaseRequestItemDto> Items { get; set; }
    }
    
    /// <summary>
    /// Satın alma talebi kalemini güncellemek için DTO
    /// </summary>
    public class UpdatePurchaseRequestItemDto
    {
        /// <summary>
        /// Kalem ID (mevcut bir kalem güncelleniyorsa)
        /// </summary>
        public int? Id { get; set; }
        
        /// <summary>
        /// Kalem adı/başlığı
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Kalem açıklaması
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Ölçü birimi
        /// </summary>
        public string UnitOfMeasure { get; set; }
        
        /// <summary>
        /// Tahmini birim fiyat
        /// </summary>
        public decimal EstimatedUnitPrice { get; set; }
        
        /// <summary>
        /// Ürün kodu (SKU)
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// Teknik özellikler
        /// </summary>
        public string Specifications { get; set; }
        
        /// <summary>
        /// Bu kalem silinecek mi? (mevcut bir kalem için)
        /// </summary>
        public bool IsDeleted { get; set; }
    }
} 