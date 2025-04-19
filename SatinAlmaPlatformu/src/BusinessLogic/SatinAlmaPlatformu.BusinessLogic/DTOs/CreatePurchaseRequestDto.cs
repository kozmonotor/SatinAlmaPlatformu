using System;
using System.Collections.Generic;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs
{
    /// <summary>
    /// Yeni satın alma talebi oluşturmak için DTO
    /// </summary>
    public class CreatePurchaseRequestDto
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
        /// Departman ID
        /// </summary>
        public int DepartmentId { get; set; }
        
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
        public PriorityLevel Priority { get; set; } = PriorityLevel.Normal;
        
        /// <summary>
        /// Bütçe kodları (Muhasebe entegrasyonu için)
        /// </summary>
        public string BudgetCode { get; set; }
        
        /// <summary>
        /// Talep kalemleri
        /// </summary>
        public List<CreatePurchaseRequestItemDto> Items { get; set; }
        
        /// <summary>
        /// Talep oluşturulduktan sonra doğrudan onaya gönderilsin mi?
        /// </summary>
        public bool SubmitForApproval { get; set; }
    }
    
    /// <summary>
    /// Satın alma talebi kalemi oluşturmak için DTO
    /// </summary>
    public class CreatePurchaseRequestItemDto
    {
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
        /// Stok kodu (SKU)
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// Teknik özellikler
        /// </summary>
        public string Specifications { get; set; }
    }
} 