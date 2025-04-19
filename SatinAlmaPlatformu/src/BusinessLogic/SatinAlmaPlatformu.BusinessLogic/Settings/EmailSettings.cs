namespace SatinAlmaPlatformu.BusinessLogic.Settings
{
    /// <summary>
    /// E-posta gönderimi için gerekli ayarlar
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// SMTP sunucu adresi
        /// </summary>
        public string SmtpServer { get; set; }
        
        /// <summary>
        /// SMTP port numarası
        /// </summary>
        public int SmtpPort { get; set; }
        
        /// <summary>
        /// SMTP kullanıcı adı
        /// </summary>
        public string SmtpUsername { get; set; }
        
        /// <summary>
        /// SMTP şifresi
        /// </summary>
        public string SmtpPassword { get; set; }
        
        /// <summary>
        /// SSL kullanılsın mı
        /// </summary>
        public bool EnableSsl { get; set; }
        
        /// <summary>
        /// Gönderen e-posta adresi
        /// </summary>
        public string SenderEmail { get; set; }
        
        /// <summary>
        /// Gönderen adı
        /// </summary>
        public string SenderName { get; set; }
        
        /// <summary>
        /// E-posta şablonlarının bulunduğu klasör yolu
        /// </summary>
        public string TemplatesPath { get; set; }
    }
} 