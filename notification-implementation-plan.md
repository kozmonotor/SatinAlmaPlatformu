# Bildirim Sistemi Tamamlama Planı

## Mevcut Durum
- ✅ `Notification` veri modeli oluşturuldu
- ✅ `INotificationService` arayüzü ve temel `NotificationService` implementasyonu geliştirildi
- ✅ Temel bildirim oluşturma ve listeleme işlevleri implemente edildi
- ✅ Onay süreçlerine bağlı bildirimler entegre edildi (onay isteği, onay, red, revizyon)
- ✅ Kullanıcı bildirimleri için veritabanı katmanı hazır

## Tamamlanacak İşler

### 1. E-posta Bildirim Sistemi
#### 1.1. E-posta Servis Entegrasyonu
- [ ] E-posta gönderimi için servis arayüzü oluşturma (`IEmailService`)
```csharp
public interface IEmailService
{
    Task<Result> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task<Result> SendTemplatedEmailAsync(string to, string templateName, Dictionary<string, string> parameters);
}
```
- [ ] SMTP tabanlı e-posta servisi gerçekleştirimi
```csharp
public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<SmtpEmailService> _logger;
    
    // Constructor ve metot implementasyonları...
}
```
- [ ] Alternatif olarak SendGrid veya Mailgun gibi servis entegrasyonu için implementasyon (opsiyonel)
- [ ] E-posta servisini Dependency Injection konteynerine kaydetme

#### 1.2. E-posta Şablonları
- [ ] Şablon mekanizması tasarımı (Razor Views veya String Interpolation)
- [ ] Temel e-posta şablonu (markasız versiyon)
- [ ] Onay istekleri için e-posta şablonu
- [ ] Onay/red bildirimleri için e-posta şablonu
- [ ] Revizyon istekleri için e-posta şablonu

#### 1.3. NotificationService ile Entegrasyon
- [ ] `NotificationService`'e `IEmailService` enjekte etme
- [ ] Bildirim oluşturma metotlarında e-posta gönderimi için gerekli adımları ekleme
- [ ] Kullanıcıların e-posta tercihlerini kontrol etme mekanizması

### 2. Gerçek Zamanlı Bildirim Sistemi (SignalR)
#### 2.1. SignalR Hub Oluşturma
- [ ] `NotificationHub` sınıfı oluşturma:
```csharp
public class NotificationHub : Hub
{
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }
    
    public async Task LeaveUserGroup(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
    }
}
```
- [ ] SignalR hizmetini `Startup.cs`'de yapılandırma ve endpoint'leri ekleme

#### 2.2. Gerçek Zamanlı Bildirim Gönderimi için Servis
- [ ] `IRealTimeNotificationService` arayüzü oluşturma:
```csharp
public interface IRealTimeNotificationService
{
    Task SendNotificationToUserAsync(string userId, NotificationDto notification);
    Task SendNotificationToRoleAsync(string role, NotificationDto notification);
    Task SendNotificationToAllAsync(NotificationDto notification);
}
```
- [ ] SignalR tabanlı implementasyon:
```csharp
public class SignalRNotificationService : IRealTimeNotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    
    // Constructor ve metot implementasyonları...
}
```
- [ ] Servisi Dependency Injection konteynerine kaydetme

#### 2.3. NotificationService ile Entegrasyon
- [ ] `NotificationService`'e `IRealTimeNotificationService` enjekte etme
- [ ] Bildirim oluşturma metotlarında gerçek zamanlı bildirim gönderimi için gerekli adımları ekleme

### 3. API Katmanı Geliştirmeleri
#### 3.1. Bildirim API Endpoint'leri
- [ ] `NotificationController` oluşturma veya genişletme:
```csharp
[Route("api/notifications")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    
    // Constructor ve action metotları...
    
    [HttpGet("unread")]
    public async Task<ActionResult<Result<List<NotificationDto>>>> GetUnreadNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _notificationService.GetUnreadNotificationsAsync(userId);
    }
    
    // Diğer action metotları...
}
```
- [ ] Bildirimleri listeleme, okundu olarak işaretleme vb. API endpoint'leri ekleme
- [ ] Swagger dokümantasyonunu güncelleme

### 4. Frontend Entegrasyonu
#### 4.1. Bildirim Bileşenleri
- [ ] Bildirim listesi bileşeni (Header içerisinde dropdown olarak görünen)
- [ ] Bildirim detayları görüntüleme
- [ ] Bildirim okundu/okunmadı durumu görsel gösterimi
- [ ] Bildirim redirection (ilgili sayfaya yönlendirme)

#### 4.2. SignalR İstemci Entegrasyonu
- [ ] SignalR client kütüphanesini entegre etme
- [ ] Hub bağlantısını yönetme (bağlanma, bağlantıyı kesme, yeniden bağlanma)
- [ ] Gelen bildirimleri yakalama ve UI'da gösterme (toast/snackbar)
- [ ] Bildirim sayısını gerçek zamanlı güncelleme

## Öncelik Sıralaması
1. E-posta entegrasyonu (1.1 ve 1.2)
2. API katmanı geliştirmeleri (3)
3. SignalR Hub ve servis (2.1 ve 2.2)
4. NotificationService entegrasyonları (1.3 ve 2.3)
5. Frontend entegrasyonu (4)

## Test Senaryoları
1. E-posta gönderimi tests (birim ve entegrasyon)
2. SignalR hub ve bildirim servisi testleri
3. Farklı senaryo ve rollerde bildirim akışları için uçtan uca testler

## Notlar
- E-posta şablonları için kaynak klasörü: `EmailTemplates`
- SignalR hub endpoint: `/hubs/notification`
- Frontend işlerini frontend ekibi ile koordine etme gerekecek
- Kullanıcı tercihleri için sonradan ayarlar ekranı geliştirilebilir 