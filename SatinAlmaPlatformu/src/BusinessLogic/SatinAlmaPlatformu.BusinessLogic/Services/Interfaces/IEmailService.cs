using System.Collections.Generic;
using System.Threading.Tasks;
using SatinAlmaPlatformu.Core.Results;

namespace SatinAlmaPlatformu.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// E-posta gönderme işlemlerini yöneten servis arayüzü
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Tek bir alıcıya e-posta gönderir
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi</param>
        /// <param name="subject">E-posta konusu</param>
        /// <param name="body">E-posta içeriği (HTML)</param>
        /// <param name="isHtml">İçerik HTML mi</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        
        /// <summary>
        /// Birden fazla alıcıya aynı e-postayı gönderir
        /// </summary>
        /// <param name="toAddresses">Alıcı e-posta adresleri</param>
        /// <param name="subject">E-posta konusu</param>
        /// <param name="body">E-posta içeriği (HTML)</param>
        /// <param name="isHtml">İçerik HTML mi</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendEmailToMultipleRecipientsAsync(List<string> toAddresses, string subject, string body, bool isHtml = true);
        
        /// <summary>
        /// Ekli dosya ile e-posta gönderir
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi</param>
        /// <param name="subject">E-posta konusu</param>
        /// <param name="body">E-posta içeriği (HTML)</param>
        /// <param name="attachmentPaths">Eklenecek dosya yolları</param>
        /// <param name="isHtml">İçerik HTML mi</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendEmailWithAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths, bool isHtml = true);
        
        /// <summary>
        /// Şablonlu e-posta gönderir
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi</param>
        /// <param name="subject">E-posta konusu</param>
        /// <param name="templateName">Kullanılacak şablon adı</param>
        /// <param name="templateData">Şablonda kullanılacak veriler</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendTemplatedEmailAsync(string to, string subject, string templateName, Dictionary<string, string> templateData);
        
        /// <summary>
        /// Onay talebi e-postası gönderir
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi</param>
        /// <param name="userName">Alıcının adı</param>
        /// <param name="requestNumber">Talep numarası</param>
        /// <param name="requestTitle">Talep başlığı</param>
        /// <param name="requesterName">Talep sahibinin adı</param>
        /// <param name="approvalLink">Onay sayfası linki</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendApprovalRequestEmailAsync(string to, string userName, string requestNumber, string requestTitle, string requesterName, string approvalLink);
        
        /// <summary>
        /// Talep onaylandı e-postası gönderir
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi</param>
        /// <param name="userName">Alıcının adı</param>
        /// <param name="requestNumber">Talep numarası</param>
        /// <param name="requestTitle">Talep başlığı</param>
        /// <param name="approverName">Onaylayan kişinin adı</param>
        /// <param name="requestDetailsLink">Talep detay sayfası linki</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendRequestApprovedEmailAsync(string to, string userName, string requestNumber, string requestTitle, string approverName, string requestDetailsLink);
        
        /// <summary>
        /// Talep reddedildi e-postası gönderir
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi</param>
        /// <param name="userName">Alıcının adı</param>
        /// <param name="requestNumber">Talep numarası</param>
        /// <param name="requestTitle">Talep başlığı</param>
        /// <param name="rejectionReason">Red nedeni</param>
        /// <param name="rejectedBy">Reddeden kişinin adı</param>
        /// <param name="requestDetailsLink">Talep detay sayfası linki</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SendRequestRejectedEmailAsync(string to, string userName, string requestNumber, string requestTitle, string rejectionReason, string rejectedBy, string requestDetailsLink);
    }
} 