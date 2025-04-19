using System;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs
{
    /// <summary>
    /// Bildirim verilerini taşıyan DTO sınıfı
    /// </summary>
    public class NotificationDto
    {
        /// <summary>
        /// Bildirim ID'si
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Bildirim başlığı
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Bildirim mesajı
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Bildirim tipi
        /// </summary>
        public NotificationType Type { get; set; }
        
        /// <summary>
        /// Bildirim tipi (metin olarak)
        /// </summary>
        public string TypeText => Type.ToString();
        
        /// <summary>
        /// Bildirim referans tipi
        /// </summary>
        public NotificationReferenceType ReferenceType { get; set; }
        
        /// <summary>
        /// Bildirim referans tipi (metin olarak)
        /// </summary>
        public string ReferenceTypeText => ReferenceType.ToString();
        
        /// <summary>
        /// Bildirim referans ID'si
        /// </summary>
        public int ReferenceId { get; set; }
        
        /// <summary>
        /// Bildirimin okunma durumu
        /// </summary>
        public bool IsRead { get; set; }
        
        /// <summary>
        /// Bildirimin okunma tarihi
        /// </summary>
        public DateTime? ReadAt { get; set; }
        
        /// <summary>
        /// Bildirime tıklandığında yönlendirilecek URL
        /// </summary>
        public string RedirectUrl { get; set; }
        
        /// <summary>
        /// Bildirimin oluşturulma tarihi
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Bildirimin göreceli zamanı (örn: "5 dakika önce", "2 saat önce")
        /// </summary>
        public string RelativeTime { get; set; }
    }
    
    /// <summary>
    /// Sayfalanmış sonuçlar için DTO sınıfı
    /// </summary>
    /// <typeparam name="T">Sayfalanmış veri tipi</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Sayfalanmış veriler
        /// </summary>
        public List<T> Items { get; set; }
        
        /// <summary>
        /// Toplam veri sayısı
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Toplam sayfa sayısı
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Mevcut sayfa numarası
        /// </summary>
        public int CurrentPage { get; set; }
        
        /// <summary>
        /// Sayfa büyüklüğü
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Önceki sayfa var mı?
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;
        
        /// <summary>
        /// Sonraki sayfa var mı?
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
