using System.Collections.Generic;
using System.Threading.Tasks;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.Core.Results;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Bildirim işlemlerini yöneten servis arayüzü
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Kullanıcıya bildirim gönderir
        /// </summary>
        /// <param name="userId">Bildirim gönderilecek kullanıcı ID'si</param>
        /// <param name="title">Bildirim başlığı</param>
        /// <param name="message">Bildirim mesajı</param>
        /// <param name="type">Bildirim tipi</param>
        /// <param name="referenceType">Bildirim referans tipi</param>
        /// <param name="referenceId">Bildirim referans ID'si</param>
        /// <param name="redirectUrl">Yönlendirme URL'i (opsiyonel)</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendNotificationAsync(string userId, string title, string message, 
            NotificationType type, NotificationReferenceType referenceType, int referenceId, string redirectUrl = null);
        
        /// <summary>
        /// Birden fazla kullanıcıya aynı bildirimi gönderir
        /// </summary>
        /// <param name="userIds">Bildirim gönderilecek kullanıcı ID'leri</param>
        /// <param name="title">Bildirim başlığı</param>
        /// <param name="message">Bildirim mesajı</param>
        /// <param name="type">Bildirim tipi</param>
        /// <param name="referenceType">Bildirim referans tipi</param>
        /// <param name="referenceId">Bildirim referans ID'si</param>
        /// <param name="redirectUrl">Yönlendirme URL'i (opsiyonel)</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendBulkNotificationsAsync(List<string> userIds, string title, string message,
            NotificationType type, NotificationReferenceType referenceType, int referenceId, string redirectUrl = null);
        
        /// <summary>
        /// Onay isteği bildirimi gönderir
        /// </summary>
        /// <param name="userId">Onayı istenen kullanıcı ID'si</param>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="approvalStepId">Onay adımı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendApprovalRequestNotificationAsync(string userId, int purchaseRequestId, int approvalStepId);
        
        /// <summary>
        /// Talep onaylandığında talep sahibine bildirim gönderir
        /// </summary>
        /// <param name="userId">Talep sahibi kullanıcı ID'si</param>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendRequestApprovedNotificationAsync(string userId, int purchaseRequestId);
        
        /// <summary>
        /// Talep reddedildiğinde talep sahibine bildirim gönderir
        /// </summary>
        /// <param name="userId">Talep sahibi kullanıcı ID'si</param>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="rejectionReason">Red nedeni</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendRequestRejectedNotificationAsync(string userId, int purchaseRequestId, string rejectionReason);
        
        /// <summary>
        /// Revizyon istendiğinde talep sahibine bildirim gönderir
        /// </summary>
        /// <param name="userId">Talep sahibi kullanıcı ID'si</param>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="revisionComment">Revizyon açıklaması</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendRevisionRequestNotificationAsync(string userId, int purchaseRequestId, string revisionComment);
        
        /// <summary>
        /// Revizyon tamamlandığında revizyon isteyen kullanıcıya bildirim gönderir
        /// </summary>
        /// <param name="userId">Revizyon isteyen kullanıcı ID'si</param>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="revisionNote">Revizyon notu</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendRevisionCompletedNotificationAsync(string userId, int purchaseRequestId, string revisionNote);
        
        /// <summary>
        /// Bildirim okundu olarak işaretler
        /// </summary>
        /// <param name="notificationId">Bildirim ID'si</param>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> MarkAsReadAsync(int notificationId, string userId);
        
        /// <summary>
        /// Birden fazla bildirimi okundu olarak işaretler
        /// </summary>
        /// <param name="notificationIds">Bildirim ID'leri</param>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> MarkMultipleAsReadAsync(List<int> notificationIds, string userId);
        
        /// <summary>
        /// Kullanıcının okunmamış bildirimlerini getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Bildirim listesini içeren Result nesnesi</returns>
        Task<Result<List<NotificationDto>>> GetUnreadNotificationsAsync(string userId);
        
        /// <summary>
        /// Kullanıcının tüm bildirimlerini getirir (sayfalama destekli)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa büyüklüğü</param>
        /// <returns>Bildirim listesini içeren Result nesnesi</returns>
        Task<Result<PagedResult<NotificationDto>>> GetNotificationsAsync(string userId, int pageNumber = 1, int pageSize = 10);
    }
} 