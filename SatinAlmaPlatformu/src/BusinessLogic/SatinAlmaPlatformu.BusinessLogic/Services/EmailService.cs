using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.BusinessLogic.Settings;
using SatinAlmaPlatformu.Core.Results;

namespace SatinAlmaPlatformu.BusinessLogic.Services
{
    /// <summary>
    /// SMTP protokolü üzerinden e-posta gönderme işlemlerini yönetir
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        /// <inheritdoc />
        public async Task<Result> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                
                using var client = CreateSmtpClient();
                await client.SendMailAsync(message);
                
                _logger.LogInformation("E-posta gönderildi. Alıcı: {Recipient}, Konu: {Subject}", to, subject);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "E-posta gönderilirken hata oluştu. Alıcı: {Recipient}, Konu: {Subject}", to, subject);
                return Result.Failure($"E-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }
        
        /// <inheritdoc />
        public async Task<Result> SendEmailToMultipleRecipientsAsync(List<string> toAddresses, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                
                foreach (var address in toAddresses)
                {
                    message.To.Add(new MailAddress(address));
                }
                
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                
                using var client = CreateSmtpClient();
                await client.SendMailAsync(message);
                
                _logger.LogInformation("Toplu e-posta gönderildi. Alıcı sayısı: {RecipientCount}, Konu: {Subject}", toAddresses.Count, subject);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Toplu e-posta gönderilirken hata oluştu. Alıcı sayısı: {RecipientCount}, Konu: {Subject}", toAddresses.Count, subject);
                return Result.Failure($"Toplu e-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }
        
        /// <inheritdoc />
        public async Task<Result> SendEmailWithAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths, bool isHtml = true)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                
                // Ekler
                foreach (var attachmentPath in attachmentPaths)
                {
                    if (File.Exists(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        message.Attachments.Add(attachment);
                    }
                    else
                    {
                        _logger.LogWarning("Eklenmek istenen dosya bulunamadı: {AttachmentPath}", attachmentPath);
                    }
                }
                
                using var client = CreateSmtpClient();
                await client.SendMailAsync(message);
                
                _logger.LogInformation("Ekli e-posta gönderildi. Alıcı: {Recipient}, Konu: {Subject}, Ek sayısı: {AttachmentCount}", 
                    to, subject, message.Attachments.Count);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ekli e-posta gönderilirken hata oluştu. Alıcı: {Recipient}, Konu: {Subject}", to, subject);
                return Result.Failure($"Ekli e-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }
        
        /// <inheritdoc />
        public async Task<Result> SendTemplatedEmailAsync(string to, string subject, string templateName, Dictionary<string, string> templateData)
        {
            try
            {
                // Şablon dosyasını oku
                string templatePath = Path.Combine(_emailSettings.TemplatesPath, $"{templateName}.html");
                
                if (!File.Exists(templatePath))
                {
                    _logger.LogError("E-posta şablonu bulunamadı: {TemplateName}", templateName);
                    return Result.Failure($"E-posta şablonu bulunamadı: {templateName}");
                }
                
                string templateContent = await File.ReadAllTextAsync(templatePath);
                
                // Şablondaki değişkenleri değiştir
                foreach (var item in templateData)
                {
                    templateContent = templateContent.Replace($"{{{{{item.Key}}}}}", item.Value);
                }
                
                // E-postayı gönder
                return await SendEmailAsync(to, subject, templateContent, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şablonlu e-posta gönderilirken hata oluştu. Alıcı: {Recipient}, Şablon: {TemplateName}", to, templateName);
                return Result.Failure($"Şablonlu e-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }
        
        /// <inheritdoc />
        public async Task<Result> SendApprovalRequestEmailAsync(string to, string userName, string requestNumber, string requestTitle, string requesterName, string approvalLink)
        {
            var templateData = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "RequestNumber", requestNumber },
                { "RequestTitle", requestTitle },
                { "RequesterName", requesterName },
                { "ApprovalLink", approvalLink },
                { "CurrentDate", DateTime.Now.ToString("dd.MM.yyyy") }
            };
            
            return await SendTemplatedEmailAsync(
                to,
                $"Satın Alma Talebi Onay İsteği: {requestNumber}",
                "ApprovalRequest",
                templateData);
        }
        
        /// <inheritdoc />
        public async Task<Result> SendRequestApprovedEmailAsync(string to, string userName, string requestNumber, string requestTitle, string approverName, string requestDetailsLink)
        {
            var templateData = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "RequestNumber", requestNumber },
                { "RequestTitle", requestTitle },
                { "ApproverName", approverName },
                { "RequestDetailsLink", requestDetailsLink },
                { "CurrentDate", DateTime.Now.ToString("dd.MM.yyyy") }
            };
            
            return await SendTemplatedEmailAsync(
                to,
                $"Satın Alma Talebiniz Onaylandı: {requestNumber}",
                "RequestApproved",
                templateData);
        }
        
        /// <inheritdoc />
        public async Task<Result> SendRequestRejectedEmailAsync(string to, string userName, string requestNumber, string requestTitle, string rejectionReason, string rejectedBy, string requestDetailsLink)
        {
            var templateData = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "RequestNumber", requestNumber },
                { "RequestTitle", requestTitle },
                { "RejectionReason", rejectionReason },
                { "RejectedBy", rejectedBy },
                { "RequestDetailsLink", requestDetailsLink },
                { "CurrentDate", DateTime.Now.ToString("dd.MM.yyyy") }
            };
            
            return await SendTemplatedEmailAsync(
                to,
                $"Satın Alma Talebiniz Reddedildi: {requestNumber}",
                "RequestRejected",
                templateData);
        }
        
        /// <summary>
        /// SMTP client oluşturur
        /// </summary>
        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            
            return client;
        }
    }
} 