# Satın Alma Platformu Projesi - Yapı Dokümanı

## Genel Bakış
Satın Alma Platformu, şirketlerin satın alma süreçlerini yönetmek, talepleri oluşturmak, onaylamak ve tedarikçilerden teklif almak için kullanılan bir web uygulamasıdır.

## Mimari
Proje, N-tier (çok katmanlı) mimariye sahip bir .NET 8.0 uygulamasıdır. Temel 5 katmandan oluşur:

1. **Core**: Veri modelleri, DTOlar, arayüzler ve yardımcı sınıfların bulunduğu temel katman
2. **Infrastructure**: Veritabanı işlemleri, harici servis entegrasyonları ve altyapı kodları
3. **Application**: İş mantığı, servisler ve komutların bulunduğu katman
4. **API**: REST API endpointlerinin bulunduğu, istemcilerle iletişim kuran katman
5. **Frontend**: React tabanlı kullanıcı arayüzü

## Klasör Yapısı

```
SatinAlmaPlatformu-new/
└── src/
    ├── Core/                          # Temel veri yapıları ve arayüzler
    ├── Infrastructure/                # Altyapı ve veritabanı işlemleri
    ├── Application/                   # İş mantığı ve servis katmanı
    ├── Api/                          
    │   └── SatinAlmaPlatformu.Api/    # API Projesi
    │       ├── Program.cs             # API başlangıç noktası ve konfigürasyon
    │       ├── Controllers/           # API controller'ları
    │       │   ├── AuthController.cs  # Kimlik doğrulama işlemleri
    │       │   ├── TalepController.cs # Talep yönetimi işlemleri
    │       │   └── TedarikciController.cs # Tedarikçi yönetimi işlemleri
    │       ├── Properties/            # API özellikleri ve ayarları
    │       │   └── launchSettings.json # API başlatma ayarları
    │       └── SatinAlmaPlatformu.Api.csproj # Proje bağımlılıkları (Swagger/OpenAPI dahil)
    └── satinalma-frontend/            # Frontend projesi
        ├── src/
        │   ├── components/           # React bileşenleri
        │   │   ├── common/           # Ortak bileşenler
        │   │   │   └── ProtectedRoute.tsx # Rol tabanlı erişim kontrolü
        │   │   ├── layout/           # Düzen bileşenleri
        │   │   └── pages/            # Sayfa bileşenleri  
        │   │       ├── LoginPage.tsx # Giriş sayfası
        │   │       ├── satinalma/    # Satın alma süreçleriyle ilgili sayfalar
        │   │       │   ├── TeklifIstegiDuzenlePage.tsx
        │   │       │   ├── TumTaleplerPage.tsx
        │   │       │   ├── TeklifIstegiListePage.tsx
        │   │       │   ├── SiparisDetayPage.tsx
        │   │       │   ├── SiparislerPage.tsx
        │   │       │   ├── TedarikciListelePage.tsx
        │   │       │   ├── TedarikciDetayPage.tsx
        │   │       │   ├── SiparisOlusturPage.tsx
        │   │       │   ├── TeklifIstegiDetayPage.tsx
        │   │       │   ├── TeklifIstegiOlusturPage.tsx
        │   │       │   ├── TeklifKarsilastirmaPage.tsx
        │   │       │   ├── TalepDegerlendirmePage.tsx
        │   │       │   └── TedarikciDuzenleEklePage.tsx
        │   │       └── LoginPage.tsx # Giriş sayfası
        │   ├── context/              # React context'leri
        │   │   └── AuthContext.tsx   # Kimlik doğrulama context'i
        │   ├── services/             # API servisleri
        │   │   ├── api.ts            # API istemcisi konfigürasyonu
        │   │   └── authService.ts    # Kimlik doğrulama servisi
        │   └── constants/            # Sabitler
        │       └── menuItems.ts      # Rol tabanlı menü öğeleri
        ├── package.json              # NPM paket konfigürasyonu
        └── public/                   # Statik dosyalar
```

## Yapılan Değişiklikler

1. **Proje yapısı oluşturulması**: SatinAlmaPlatformu-new altında N-tier mimari ile organize edilmiş bir yapı oluşturuldu.
2. **Temel API endpointleri eklenmesi**: AuthController, TalepController ve TedarikciController eklendi.
3. **CORS konfigürasyonu**: Frontend'in API'ye erişebilmesi için CORS ayarları yapıldı.
4. **JWT tabanlı kimlik doğrulama**: Access token kullanılarak kullanıcı kimlik doğrulaması eklendi.
5. **Frontend API bağlantısı**: Frontend'in API ile iletişim kurması için gerekli servisler eklendi.
6. **Null atanamaz özellikleri düzeltme**: TalepController ve TedarikciController'da null reference uyarıları düzeltildi.
7. **Tip dönüşüm hatası düzeltme**: TedarikciController'da List<dynamic> to List<string> dönüşüm hatası giderildi.
8. **AuthController null referans uyarısı düzeltme**: Kullanıcı adı ve şifre kontrolü eklendi.
9. **Tüm derleme hataları düzeltildi**: API sorunsuz şekilde derleniyor.
10. **Test endpointleri eklendi**: AuthController'a kimlik doğrulama testleri için endpoint'ler eklendi.
11. **Backend ve Frontend başlatılması**: Backend ve frontend servisleri başlatılarak aralarındaki bağlantı test edildi.
12. **Frontend build derleme hataları düzeltildi**: API bağlantısı ve kimlik doğrulama ile ilgili tip dönüşüm ve referans hataları giderildi.
13. **Frontend API URL'i backend port'u ile eşleşecek şekilde güncellendi**: `config/constants.ts` dosyasında `http://localhost:3000` -> `http://localhost:5000`
14. **Login işlemindeki API bağlantı sorunları düzeltildi**:
    - Frontend'deki API URL konfigürasyon sorunu düzeltildi
    - LoginResponse ve ApiResponse tiplemesi düzeltildi
    - Token localStorage'a doğru şekilde kaydedilmesi sağlandı
    - Demo kullanıcılar backend'deki demo kullanıcılarla eşleştirildi
    - API hata ayıklama logları geliştirildi
15. **LoginRequest yapısı backend ile uyumlu hale getirildi**:
    - Frontend'de `username` ve `password` alanları yerine backend'in beklediği `kullaniciAdi` ve `sifre` alanları kullanıldı
    - API isteklerinde `/auth/login` yerine doğru path olan `/api/auth/login` kullanıldı
    - API yanıt ve istek logları geliştirildi
16. **API endpoint hataları düzeltildi**:
    - Backend API yolları önünde eksik olan `/api` prefix'i eklendi
    - Login yanıtındaki isSuccess/success uyumsuzluğu giderildi
    - Talep servisindeki tüm API çağrıları düzeltildi
    - Bildirim servisindeki tüm API çağrıları düzeltildi
17. **JWT token çözümleme geliştirmeleri**:
    - JWT token içinde alternatif rol claim formatlarını destekleme eklendi
    - Rol alanı çözümleme geliştirildi (`role`, `roles` ve standart claim formatı)
    - Token ve kullanıcı bilgilerini kontrol için ek debug logları eklendi
    - Talep servisi hata işleme ve loglama geliştirildi
18. **formatCurrency fonksiyonu güncellenecek**: `formatUtils.ts` dosyasında, varsayılan para birimi kodu parametresi desteği eklenecek. Böylece farklı para birimleriyle de kullanılabilecek.
19. **formatUtils.ts dosyasındaki formatCurrency fonksiyonu ve ilişkili fonksiyonlar incelendi**: Fonksiyonun mevcut hali ve diğer yardımcı fonksiyonlarla ilişkisi analiz edildi. Kodda değişiklik yapılmadı, sadece analiz ve dokümantasyon amacıyla incelendi.
4. **Swagger ve Backend Hataları Çözüldü**:
   - Ortak bir ApiResponse<T> modeli oluşturuldu: src/Api/SatinAlmaPlatformu.Api/Common/ApiResponse.cs
   - Tüm controller'lardaki (AuthController, TalepController, TedarikciController) kendi içindeki ApiResponse<T> tanımları kaldırıldı ve ortak model kullanıldı.
   - Program.cs dosyasında Swagger config'e `c.CustomSchemaIds(type => type.FullName);` eklendi.
   - Swagger ve backend başlatıldığında hata alınmıyor, dokümantasyon düzgün çalışıyor.
6. **Swagger Model Referans Hataları Çözüldü**:
   - Tüm request/response modelleri (LoginRequest, LoginResponse, Talep, TalepRequest, TalepKalemi, TalepKalemiRequest, Tedarikci, TedarikciRequest) ayrı dosyalara ve Models klasörüne taşındı.
   - Controller içindeki nested class'lar kaldırıldı ve ilgili modeller kullanıldı.
   - Swagger/OpenAPI referans hataları tamamen giderildi, şema eksiksiz ve hatasız gösteriliyor.
7. **Frontend Menü-Sayfa Eşleşme ve Eksik Sayfa Analizi Başlatıldı**:
   - Menüdeki tüm linkler ve roller, pages klasöründeki dosyalar ve route tanımları ile otomatik olarak karşılaştırılacak.
   - Eksik/hatalı olanlar raporlanacak ve eksik sayfa dosyaları otomatik template ile oluşturulacak.
   - Çalışmayan ekle/düzenle/detay butonları da test edilip raporlanacak.
8. **Frontend pages/satinalma klasörü ve altındaki tüm sayfa dosyaları detaylı olarak eklendi.**
9. **Menüdeki /taleplerim ve /yeni-talep linkleri, route ile uyumlu olacak şekilde /kullanici/taleplerim ve /kullanici/yeni-talep olarak güncellendi.**
10. **TumTaleplerPage.tsx dosyasında formatCurrency fonksiyonu kullanılırken currency parametresi eksikse varsayılan olarak 'TRY' atanacak şekilde güncellendi.**

## Çalıştırma Talimatları

### Backend (API)
API servisi şu şekilde çalıştırılabilir:
```
cd SatinAlmaPlatformu-new/src/Api/SatinAlmaPlatformu.Api
dotnet run
```
API http://localhost:5000 adresinde çalışacaktır.

Swagger/OpenAPI dokümantasyonu için: http://localhost:5000/swagger

### Frontend
Frontend servisi şu şekilde çalıştırılabilir:
```
cd SatinAlmaPlatformu-new/src/satinalma-frontend
npm start
```
Frontend http://localhost:3000 adresinde çalışacaktır.

### Test Kullanıcıları
Sistem şu demo kullanıcılarıyla test edilebilir:
- **Normal Kullanıcı**: normal@ornek.com / 123456 (Rol: NormalKullanici)
- **Satın Alma Personeli**: satinalma@ornek.com / 123456 (Rol: SatinAlmaPersoneli)
- **Yönetici**: yonetici@ornek.com / 123456 (Rol: Yonetici)
- **Tedarikçi**: tedarikci@ornek.com / 123456 (Rol: Tedarikci)
- **Admin**: admin@ornek.com / 123456 (Rol: Admin)
- **Admin (Özel)**: kozmi@msn.com / 123456 (Rol: Admin)

## Kimlik Doğrulama Sistemi

Backend'de JWT token tabanlı kimlik doğrulama sistemi kullanılmaktadır. Kullanıcı rollerine göre yetkilendirme yapılmaktadır. Frontend'de AuthContext ile kullanıcı kimlik bilgileri yönetilmekte ve ProtectedRoute bileşeni ile rol tabanlı erişim kontrolü sağlanmaktadır.

### Backend (API) Tarafı
- JWT token kullanılmaktadır
- AuthController.cs giriş işlemlerini yönetir ve JWT token üretir
- Kullanıcı rolleri JWT token içinde ClaimTypes.Role olarak saklanır
- Program.cs dosyasında JWT Bearer kimlik doğrulama ve yetkilendirme yapılandırılmıştır
- Token süresi 1 saat olarak ayarlanmıştır
- Demo kullanıcılar ve rolleri: NormalKullanici, SatinAlmaPersoneli, Yonetici, Tedarikci, Admin

### Frontend Tarafı
- React Context API kullanılarak AuthContext ile kimlik doğrulama durumu yönetilir
- authService.ts API isteklerini yönetir ve JWT tokenları localStorage'da saklar
- getCurrentUser() metodu JWT tokenden kullanıcı bilgilerini çıkarır
- hasRole() metodu kullanıcı rollerini kontrol eder
- ProtectedRoute bileşeni, rollere göre erişim kontrolü sağlar
- Menü yapısı, kullanıcı rollerine göre dinamik olarak getMenuItems() fonksiyonu ile oluşturulur

## Yapılan Son Değişiklikler

1. **TedarikciController yeniden yapılandırıldı**: 
   - Tedarikçi model sınıfı güncellendi
   - ApiResponse tiplemesi standartlaştırıldı
   - Kategoriler alanı için tür dönüşüm hatası düzeltildi
   - Demo veri yeniden düzenlendi

2. **Frontend servis katmanı iyileştirildi**:
   - api.ts dosyasında apiInstance dışa aktarıldı
   - authService.ts'de JWT token işlemleri düzeltildi
   - Axios hata işleme mekanizması geliştirildi
   - Tip güvenliği sağlandı

3. **Kimlik doğrulama tamamlandı**:
   - Login akışı tam olarak çalışır hale getirildi
   - Token doğrulama ve çözümleme işlemleri düzeltildi
   - API bağlantıları için hata işleme geliştirildi

4. **Build süreçleri kontrol edildi**:
   - API projesi başarıyla derlendi
   - Frontend build sürecinde tip hatası düzeltildi
   - API ve Frontend entegrasyonu tamamlandı

# Proje Dosya ve Klasör Yapısı

- cypress/
  - e2e/
    - dynamic-page-access.cy.js
- cypress.config.js

## Yapılan Değişiklikler
- `cypress/e2e/dynamic-page-access.cy.js` dosyasında login selectorları (`input[name="username"]`, `input[name="password"]`) olarak güncellendi.
- TumTaleplerPage.tsx dosyasında formatCurrency fonksiyonu kullanılırken currency parametresi eksikse varsayılan olarak 'TRY' atanacak şekilde güncellendi.
- Tüm işlemler sadece `D:\selcuk\SatinAlmaPlatformu-new\src\satinalma-frontend` dizininde yapıldı.
- Yanlışlıkla oluşturulan `D:\selcuk\cypress\e2e\dynamic-page-access.cy.js` dosyası silindi. Artık sadece ana proje dizininde işlem yapılacak.
- `cypress/e2e/dynamic-page-access.cy.js` dosyasında login sonrası menü veya ana layout'un varlığını kontrol eden assertion eklendi: `cy.get('nav, .MuiDrawer-root, .sidebar', { timeout: 10000 }).should('exist');`

# Proje Yapısı

## SatinAlmaPlatformu-new/src/satinalma-frontend/
- src/
  - components/
    - layout/
      - MainLayout.tsx (Ana düzen bileşeni, tüm sayfalarda kullanılıyor)
      - AdminMenu.tsx
    - common/
    - rapor/
  - context/
    - AuthContext.tsx
    - AppContext.tsx
    - NotificationContext.tsx
  - pages/
    - LoginPage.tsx
    - HomePage.tsx
    - ProfilePage.tsx
    - AccountSettingsPage.tsx
    - kullanici/
      - YeniTalepPage.tsx
      - TaleplerimPage.tsx
      - TalepDetayPage.tsx
    - satinalma/
    - yonetici/
    - tedarikci/
  - constants/
    - menuItems.ts (Navigasyon menüsü öğeleri)
  - App.tsx (Ana uygulama ve yönlendirme yapılandırması)
- cypress/
  - e2e/
    - dynamic-page-access.cy.js
  - screenshots/

## Ana Düzen (MainLayout) Bileşeni
`MainLayout.tsx` dosyası, uygulamanın ana düzenini oluşturan bileşeni içerir. Bu bileşen şunları içerir:
- Üst çubuk (AppBar) - Uygulama başlığı, bildirimler ve kullanıcı profil menüsü
- Yan menü (Drawer) - Navigasyon için kullanılan sol taraftaki kenar çubuğu
- Duyarlı tasarım - Mobil ve masaüstü görünümlerinde farklı davranış
- Oturum yönetimi - Oturum zaman aşımı kontrolü

## Yönlendirme (Routing) Yapısı
Uygulama, React Router kullanarak yönlendirme yapısını kurmuştur. Tüm korumalı sayfalar `MainLayout` içinde görüntülenir.

## Yapılan Değişiklikler
- Proje yapısı dosyası oluşturuldu.
- Ana düzen bileşeni ve yönlendirme yapısı hakkında bilgiler eklendi.
