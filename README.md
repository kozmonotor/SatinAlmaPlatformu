# Satın Alma Platformu

Kurumsal satın alma süreçlerini dijitalleştiren ve optimize eden bir yazılım çözümü.

## Proje Tanımı

Bu platform şunları sağlar:
- Kullanıcıların satın alma taleplerini dijital ortamda oluşturması
- Yöneticilerin talepleri onaylaması/reddetmesi
- Tedarikçilerin tekliflerini sisteme yüklemesi
- Satın alma süreçlerinin izlenmesi ve analizi

## Teknik Altyapı

- Backend: .NET 8.0 (N-tier Architecture)
- Database: SQL Server
- API: RESTful API
- Authentication: JWT tabanlı kimlik doğrulama
- Bildirim sistemi: E-posta ve uygulama içi bildirimler

## Geliştirme Ortamı Gereksinimleri

- Visual Studio 2022 veya daha yeni versiyon
- .NET 8.0 SDK
- SQL Server 2019 veya daha yeni (Express edition yeterli)
- Git

## Kurulum

1. Repository'yi klonlayın:
```
git clone https://github.com/kozmonotor/SatinAlmaPlatformu.git
```

2. Veritabanını oluşturun (Migration'ları çalıştırın):
```
dotnet ef database update
```

3. Uygulamayı çalıştırın:
```
dotnet run
```

## Proje Yönetimi

Bu proje GitHub Projects ile yönetilmektedir. Kanban tahtasına erişmek için:
- [GitHub Projects Linki (Eklenecek)]()

## Katkıda Bulunmak

1. Bir özellik (feature) branch oluşturun: `git checkout -b feature/yeni-ozellik`
2. Değişikliklerinizi commit edin: `git commit -m 'Yeni özellik: Açıklama'`
3. Branch'inizi push edin: `git push origin feature/yeni-ozellik`
4. Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## Bildirim Sistemi

Satın Alma Platformu, iki tür bildirim mekanizması içerir:

1. **E-posta Bildirimleri**: Kritik işlemler için kullanıcılara e-posta bildirimleri gönderilir (onay istekleri, talep durumu değişiklikleri vb.)

2. **Gerçek Zamanlı Bildirimler**: SignalR kullanılarak uygulamada anlık bildirimler sağlanır:
   - Kullanıcılar için bildirim çanı ve sayacı
   - Bildirim popup'ları (toasts)
   - Okundu/okunmadı durumu takibi

### Bildirim Entegrasyonu

Frontend'de bildirim sistemi ile entegrasyon için aşağıdaki adımlar izlenmelidir:

```javascript
// Bildirim istemcisi başlatma
notificationClient.initialize('kullanıcı-id');

// Bildirim arayüzünü başlatma
notificationUI.initialize({
    countBadgeSelector: '#notificationBadge',
    notificationContainerSelector: '#notificationDropdown',
    notificationListSelector: '#notificationList',
    toastContainerSelector: '#toastContainer'
});

// Bildirim alındığında özel işlem yapmak için
notificationClient.onNotificationReceived(function(notification) {
    console.log('Yeni bildirim:', notification);
    // Özel işlemler...
});
```
