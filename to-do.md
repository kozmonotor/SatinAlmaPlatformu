# Satın Alma Platformu - Proje Yapılacaklar Listesi (TODO)

**Not:** Bu liste kapsamlıdır ve projenin karmaşıklığına göre bazı adımlar birleştirilebilir veya önceliklendirilebilir.

## Aşama 0: Planlama, Tasarım ve Kurulum

### 0.1. Proje Yönetimi ve Planlama
- [x] Proje yönetim aracını (Jira, Trello, Azure DevOps Boards vb.) belirle ve kur. (GitHub Projects seçildi, temel kurulum yapıldı)
- [x] Detaylı iş kırılım yapısını (WBS) oluştur.
- [x] Sprint planlaması veya görev atamalarını yap.
- [ ] İletişim planını oluştur (toplantı sıklığı, raporlama vb.).
- [ ] Risk analizini yap ve risk yönetimi planını oluştur.

### 0.2. Gereksinim Detaylandırma ve Analiz
- [ ] Her bir temel özellik için detaylı fonksiyonel gereksinimleri yaz.
- [ ] Rol bazlı yetkilendirme matrisini detaylandır.
- [ ] Onay süreçleri için farklı senaryoları (kategori, bütçe vb.) ve akışları detaylandır.
- [ ] Raporlama ihtiyaçlarını (hangi raporlar, hangi filtreler, hangi metrikler) detaylandır.
- [ ] Kapsam dışı konuların sınırlarını netleştir.

### 0.3. Mimari ve Teknik Tasarım
- [ ] N-Tier katmanlarının (Presentation, BLL, DAL, Entities/Models, Core/Common) sorumluluklarını netleştir.
- [x] Veritabanı şemasını tasarla (Tablolar, ilişkiler, veri tipleri, indexler).
- [ ] API endpoint'lerinin listesini ve temel request/response yapılarını tasarla (Swagger/OpenAPI taslağı).
- [ ] Kullanılacak temel kütüphaneleri ve NuGet paketlerini belirle (.NET Identity, EF Core, Automapper vb.).
- [ ] Loglama, hata yönetimi ve monitoring stratejisini belirle.
- [ ] Bildirim sistemi mimarisini tasarla (E-posta şablonları, uygulama içi bildirim yapısı).
- [ ] Dosya yönetimi stratejisini belirle (depolama yeri, isimlendirme kuralları, güvenlik).
- [ ] Güvenlik mekanizmalarını detaylandır (JWT ayarları, şifre politikaları, rol/claim yönetimi).

### 0.4. UI/UX Tasarımı
- [ ] Kullanıcı akışlarını (user flows) çiz.
- [ ] Wireframe'leri oluştur (tüm ana ekranlar için).
- [ ] Mockup'ları ve prototipleri tasarla (renk paleti, tipografi, bileşenler).
- [ ] Responsive tasarım prensiplerini belirle.
- [ ] Kullanılabilirlik testleri için plan yap (opsiyonel ama önerilir).

### 0.5. Geliştirme Ortamı Kurulumu
- [ ] Kod repozitörünü (Git - örn: GitHub, Azure Repos, GitLab) oluştur ve yapılandır (branch stratejisi vb.).
- [ ] Geliştirme ortamlarını (local) ve araçlarını (IDE, DB istemcisi vb.) standartlaştır.
- [ ] Temel CI/CD pipeline'ını kur (Build, temel testler).
- [ ] Veritabanı bağlantılarını ve konfigürasyon yönetimini ayarla (appsettings.json, User Secrets vb.).

## Aşama 1: Temel Altyapı ve Çekirdek Backend Geliştirmeleri

### 1.1. Proje Yapısı ve Temel Katmanlar
- [x] .NET 8 Solution ve Projeleri oluştur (API, BLL, DAL, Entities, Core/Common).
- [x] Temel Entity sınıflarını oluştur (BaseEntity vb.).
- [x] Veritabanı projesini veya EF Core DbContext'i ve konfigürasyonlarını ayarla.
- [x] EF Core Migrations mekanizmasını kur ve ilk migration'ı oluştur.
- [x] Dependency Injection (DI) konteynerini yapılandır.
- [ ] Temel loglama mekanizmasını entegre et (Serilog, NLog vb.).
- [ ] Temel hata yönetimi middleware'ini ekle.
- [ ] Automapper veya benzeri bir mapping kütüphanesini yapılandır.

### 1.2. Kullanıcı Yönetimi ve Güvenlik (Backend)
- [x] User, Role, Permission, Department entity'lerini ve ilişkilerini oluştur/güncelle.
- [ ] .NET Identity veya özel kimlik doğrulama mekanizmasını entegre et.
- [ ] Kullanıcı kayıt (opsiyonel) ve giriş işlemlerini implemente et.
- [ ] JWT (veya seçilen token) oluşturma ve doğrulama mekanizmasını implemente et.
- [ ] Rol tabanlı yetkilendirme (RBAC) altyapısını kur (Claims, Policy veya Role-based attributes).
- [ ] Kullanıcı CRUD operasyonları için API endpoint'lerini ve BLL/DAL katmanlarını oluştur (Admin).
- [ ] Rol ve Yetki yönetimi için API endpoint'lerini ve BLL/DAL katmanlarını oluştur (Admin).
- [ ] Kullanıcıya rol/departman atama işlemlerini implemente et (Admin).
- [ ] Kullanıcının kendi profil bilgilerini görme/güncelleme API'lerini oluştur.

## Aşama 2: Ana Özelliklerin Backend Geliştirmeleri

### 2.1. Satın Alma Talebi Yönetimi (Backend)
- [x] PurchaseRequest (Satın Alma Talebi) entity'sini ve ilgili alt tabloları (örn: Talep Kalemleri) oluştur.
- [x] Talep oluşturma API endpoint'ini ve BLL/DAL katmanlarını implemente et (Normal Kullanıcı, Satın Alma Per.).
- [ ] Talep detaylarını getirme API endpoint'ini implemente et.
- [ ] Kullanıcının kendi taleplerini listeleme API endpoint'ini implemente et.
- [ ] Tüm/Filtreli talepleri listeleme API endpoint'ini implemente et (Satın Alma Per., Yönetici, Admin).
- [ ] Talep güncelleme (statü, detay vb.) API endpoint'ini implemente et.
- [ ] Talep silme/iptal etme API endpoint'ini implemente et.
- [ ] Talep oluşturma/güncelleme için gerekli validasyonları ekle (BLL).
- [ ] Talep durumu (enum/lookup table) yönetimini yap.

### 2.2. Onay Süreçleri (Backend)
- [x] ApprovalFlow / ApprovalStep entity'lerini tasarla ve oluştur.
- [x] Onay akışı konfigürasyon mekanizmasını tasarla (DB'de veya config dosyasında).
- [x] Bir talep oluşturulduğunda ilgili onay akışını başlatma BLL mantığını yaz.
- [x] Yöneticinin onayına düşen talepleri listeleme API endpoint'ini oluştur.
- [x] Talep onaylama/reddetme API endpoint'ini oluştur (Yönetici).
- [x] Onay/Red sonrası talep statüsünü güncelleme ve gerekirse sonraki adıma geçirme BLL mantığını yaz.
- [x] Geri bildirim/revizyon isteme mekanizmasını tasarla ve API'lerini oluştur.
- [x] Onay/Red/Revizyon durumlarında ilgili bildirimleri tetikleme mekanizmasını (BLL) oluştur.

### 2.3. Tedarikçi Teklif Yönetimi (Backend)
- [x] Supplier (Tedarikçi) entity'sini oluştur.
- [x] BidRequest (Teklif İsteği) ve Bid (Teklif) entity'lerini oluştur (Taleplerle ve Tedarikçilerle ilişkili).
- [ ] Tedarikçi yönetimi (CRUD) API endpoint'lerini oluştur (Satın Alma Per./Admin).
- [ ] Talebe bağlı teklif isteği oluşturma ve tedarikçilere gönderme BLL mantığını yaz.
- [ ] Tedarikçinin kendisine gönderilen teklif isteklerini listeleme API'sini oluştur.
- [ ] Tedarikçinin teklif gönderme API endpoint'ini oluştur.
- [ ] Bir talep için gelen teklifleri listeleme/karşılaştırma API endpoint'ini oluştur (Satın Alma Per.).
- [ ] Teklif kabul etme/reddetme API endpoint'ini oluştur (Satın Alma Per.).
- [ ] Teklif kabul edildiğinde ilgili talep ve sipariş süreçlerini tetikleme BLL mantığını yaz.
- [x] Tedarikçi performans değerlendirmesi için temel veri yapısını oluştur (opsiyonel).

### 2.4. Sipariş ve Takip (Backend)
- [x] Order (Sipariş) entity'sini ve ilgili alt tabloları (örn: Sipariş Kalemleri) oluştur.
- [ ] Onaylanan tekliften/talepten otomatik sipariş oluşturma BLL mantığını yaz.
- [ ] Manuel sipariş oluşturma API endpoint'ini oluştur (Satın Alma Per.).
- [ ] Sipariş listeleme API endpoint'lerini oluştur (Rol bazlı filtreleme ile).
- [ ] Sipariş detaylarını getirme API endpoint'ini oluştur.
- [ ] Sipariş durumunu (Hazırlanıyor, Kargoda, Teslim Edildi vb.) güncelleme API endpoint'ini oluştur.
- [ ] Fatura ve ödeme durumu takibi için temel API alanlarını/endpoint'lerini oluştur (Detaylı muhasebe değil!).
- [ ] Sipariş tamamlama ve değerlendirme (opsiyonel) API'lerini oluştur.

### 2.5. Bildirim Sistemi (Backend)
- [x] INotificationService ve NotificationService sınıflarını oluştur ve temel metotları tanımla
- [x] Bildirimler için veri modelini ve repositoryi tanımla (Notification entity)
- [x] E-posta şablonlarını oluştur ve yönetimini sağla
- [x] Gerçek zamanlı bildirim mekanizması için SignalR entegrasyonu
- [ ] Bildirim tercihleri için kullanıcı ayarları mekanizması geliştir (opsiyonel)

### 2.6. Dosya Yönetimi (Backend)
- [x] Dosya yükleme için Attachment entity'sini oluştur (Talep ekleri, teklif dokümanları vb.).
- [ ] Dosya yükleme API endpoint'ini oluştur.
- [ ] Yüklenen dosyaları belirlenen stratejiye göre (disk, cloud storage) kaydetme BLL mantığını yaz.
- [ ] Dosya indirme API endpoint'ini oluştur (Yetkilendirme kontrolü ile).
- [ ] İlişkili kayıtlara (Talep, Teklif vb.) ait dosyaları listeleme API'sini oluştur.

### 2.7. Raporlama ve Analiz (Backend)
- [ ] Raporlama için gerekli verileri çekecek API endpoint'lerini oluştur:
    - [ ] Departman/Kategori bazlı harcama toplamları.
    - [ ] Ortalama onay süreleri.
    - [ ] Talep/Sipariş sayıları (zaman bazlı).
    - [ ] Maliyet tasarrufu metrikleri için temel veri sağlama (örn: Tahmini Bütçe vs Sipariş Tutarı).
- [ ] Bu endpoint'ler için optimize edilmiş DAL sorgularını yaz (View veya Stored Procedure kullanımı düşünülebilir).

## Aşama 3: Frontend (Kullanıcı Arayüzü) Geliştirmeleri

### 3.1. Frontend Proje Kurulumu
- [ ] Frontend framework/library seçimi (React, Angular, Vue, Blazor WASM vb.) ve proje kurulumu.
- [ ] Temel UI kütüphanesi/component setini (Material UI, Bootstrap, Tailwind CSS vb.) seç ve entegre et.
- [ ] Proje yapısını (klasörler, bileşenler) oluştur.
- [ ] API istemcisini (axios, fetch) ve state management çözümünü (Redux, Zustand, NgRx, Context API vb.) yapılandır.
- [ ] Routing mekanizmasını kur.

### 3.2. Temel UI Bileşenleri ve Layout
- [ ] Login / Kullanıcı Giriş Sayfası.
- [ ] Ana Layout (Sidebar/Navbar, Header, Footer, Content Area).
- [ ] Rol bazlı menü/navigasyon oluşturma.
- [ ] Bildirim gösterim alanı (örn: Header'da bir ikon).
- [ ] Genel Hata Sayfaları (404, 500 vb.).

### 3.3. Rol Bazlı Ekranlar
- [ ] **Normal Kullanıcı Ekranları:**
    - [ ] Dashboard (Özet bilgiler, son talepler vb.).
    - [ ] Yeni Talep Oluşturma Formu.
    - [ ] Taleplerim Listesi (Arama, filtreleme, sıralama).
    - [ ] Talep Detay Sayfası.
    - [ ] Profilim Sayfası.
- [ ] **Satın Alma Personeli Ekranları:**
    - [ ] Dashboard (Özetler, bekleyen işler).
    - [ ] Tüm Talepler Listesi (Gelişmiş filtreleme).
    - [ ] Talep İnceleme/Değerlendirme Ekranı.
    - [ ] Tedarikçi Yönetimi (Listeleme, Ekleme/Düzenleme).
    - [ ] Teklif İstekleri Yönetimi.
    - [ ] Teklif Karşılaştırma Ekranı.
    - [ ] Sipariş Oluşturma Ekranı.
    - [ ] Siparişler Listesi (Filtreleme).
    - [ ] Sipariş Detay/Takip Ekranı.
- [ ] **Yönetici Ekranları:**
    - [ ] Dashboard (Onay bekleyen talep sayısı vb.).
    - [ ] Onay Bekleyen Talepler Listesi.
    - [ ] Talep Detay ve Onay/Red Ekranı.
- [ ] **Tedarikçi Ekranları:**
    - [ ] Dashboard (Yeni teklif istekleri vb.).
    - [ ] Teklif İstekleri Listesi.
    - [ ] Teklif Gönderme Formu.
    - [ ] Gönderilen Teklifler Listesi.
    - [ ] Siparişlerim Listesi (Kendilerine verilen).
    - [ ] Sipariş Detay Sayfası.
- [ ] **Sistem Yöneticisi Ekranları:**
    - [ ] Kullanıcı Yönetimi (Listeleme, Ekleme/Düzenleme, Rol Atama).
    - [ ] Rol/Yetki Yönetimi.
    - [ ] Departman Yönetimi.
    - [ ] Sistem Ayarları Ekranı (Onay akışı yapılandırma vb.).
- [ ] **Raporlama Ekranları:**
    - [ ] Rapor seçimi/filtreleme ekranı.
    - [ ] Rapor sonuçlarını gösterme (Tablolar, grafikler - Chart.js, ApexCharts vb.).
    - [ ] Raporları dışa aktarma (Excel/CSV) butonu (opsiyonel).

### 3.4. Genel Frontend İşlevleri
- [ ] Form validasyonlarını implemente et (Frontend tarafı).
- [ ] API çağrılarını ilgili ekranlara entegre et.
- [ ] Yükleniyor (Loading) göstergelerini ekle.
- [ ] Hata mesajlarını kullanıcı dostu şekilde göster.
- [ ] Uygulama içi bildirimleri gösterme mekanizmasını entegre et.
- [ ] Dosya yükleme bileşenini entegre et.
- [ ] Responsive tasarımı tüm ekranlarda uygula ve test et.

## Aşama 4: Test, Entegrasyon ve İyileştirme

### 4.1. Testler
- [ ] Backend Unit Testlerini yaz (BLL, kritik yardımcı sınıflar).
- [ ] Backend Entegrasyon Testlerini yaz (API endpoint'leri, DB etkileşimleri).
- [ ] Frontend Unit/Component Testlerini yaz (kritik bileşenler).
- [ ] End-to-End (E2E) test senaryolarını oluştur ve otomatikleştir (Cypress, Selenium, Playwright vb.).
- [ ] Manuel test senaryolarını oluştur ve farklı rollerle testleri gerçekleştir.
- [ ] Kullanıcı Kabul Testlerini (UAT) organize et ve geri bildirimleri topla.
- [ ] Güvenlik testleri yap (OWASP Top 10 kontrolü, yetkilendirme testleri).
- [ ] Performans ve yük testleri yap (API'lar için - K6, JMeter vb.).
- [ ] Farklı tarayıcılar ve cihazlarda (responsive) testleri yap.

### 4.2. Hata Düzeltme ve İyileştirme
- [ ] Testler sırasında bulunan hataları düzelt.
- [ ] UAT geri bildirimlerini değerlendir ve gerekli iyileştirmeleri yap.
- [ ] Kod kalitesini gözden geçir (code review) ve refactoring yap.
- [ ] Performans darboğazlarını tespit et ve optimize et (Veritabanı sorguları, API yanıt süreleri).
- [ ] Güvenlik açıklarını kapat.

### 4.3. Dokümantasyon
- [ ] API dokümantasyonunu güncelle/tamamla (Swagger/OpenAPI).
- [ ] Teknik dokümantasyonu yaz (Mimari, kurulum, bağımlılıklar).
- [ ] Kullanıcı kılavuzlarını (rol bazlı) hazırla.
- [ ] Test senaryolarını ve sonuçlarını dokümante et.

## Aşama 5: Dağıtım (Deployment) ve Lansman

### 5.1. Hazırlık
- [ ] Staging (Test) ve Production (Canlı) ortamlarını hazırla (Sunucular, Veritabanı, Ağ ayarları, SSL sertifikaları).
- [ ] Ortam değişkenlerini (veritabanı bağlantıları, API key'leri vb.) yapılandır.
- [ ] Deployment (Yayınlama) script'lerini veya CI/CD pipeline'larını sonlandır.
- [ ] Veritabanı yedekleme ve geri yükleme planını oluştur ve test et.
- [ ] Monitoring ve loglama araçlarını (Application Insights, ELK Stack, Datadog vb.) production ortamı için yapılandır.

### 5.2. Dağıtım
- [ ] Staging ortamına deploy et ve son kontrolleri yap.
- [ ] Production veritabanını hazırla (gerekirse başlangıç verilerini yükle).
- [ ] Production ortamına deploy et (seçilen stratejiye göre: blue/green, canary vb.).
- [ ] Lansman sonrası ilk kontrolleri yap (temel işlevler çalışıyor mu?).

## Aşama 6: Lansman Sonrası ve Bakım

- [ ] Sistemi sürekli izle (Performans, hatalar, güvenlik).
- [ ] Kullanıcı geri bildirimlerini topla ve değerlendir.
- [ ] Periyodik yedeklemeleri kontrol et.
- [ ] İlk hata düzeltme (hotfix) ve bakım sürümlerini planla.
- [ ] Proje başarı kriterlerini (hedeflere ulaşıldı mı?) ölçümle ve raporla.
- [ ] Gelecek geliştirmeler için backlog oluştur.