# Satın Alma Platformu - Sprint Planı

## Sprint 1: Temel Altyapı Çalışmaları (2 hafta)

**Sprint Amacı:** Sistemin temel altyapısını oluşturmak ve geliştirmeye hazır hale getirmek.

**Başlangıç Tarihi:** _[Başlangıç tarihi eklenecek]_  
**Bitiş Tarihi:** _[Bitiş tarihi eklenecek]_

### Sprint Backlog

#### Proje Hazırlığı
- [x] Proje yönetim aracı seçilmesi ve kurulumu (GitHub Projects)
- [x] İş kırılım yapısının (WBS) oluşturulması
- [ ] İletişim planının oluşturulması
- [ ] Temel risk analizinin yapılması
  
#### Teknik Altyapı Geliştirmeleri
- [ ] Temel loglama mekanizmasının entegrasyonu (Serilog)
  - Görevli: _[Atanacak]_
  - Tahmin: 4 saat
  - Açıklama: Serilog kütüphanesinin projeye eklenmesi, dosya ve konsol loglamanın yapılandırılması
  
- [ ] Hata yönetimi middleware'inin eklenmesi 
  - Görevli: _[Atanacak]_
  - Tahmin: 4 saat
  - Açıklama: Global exception handler eklenmesi, hata loglama mekanizmasının kurulması

- [ ] AutoMapper veya benzeri bir mapping kütüphanesinin yapılandırılması
  - Görevli: _[Atanacak]_
  - Tahmin: 3 saat
  - Açıklama: AutoMapper kütüphanesinin projeye eklenmesi, temel mapping profillerin oluşturulması
  
#### Kimlik Doğrulama ve Yetkilendirme
- [ ] .NET Identity veya özel kimlik doğrulama mekanizmasının entegre edilmesi
  - Görevli: _[Atanacak]_
  - Tahmin: 8 saat
  - Açıklama: ASP.NET Core Identity eklenmesi, DbContext'e entegrasyonu, rol yapılandırması
  
- [ ] Kullanıcı kayıt ve giriş işlemlerinin implemente edilmesi
  - Görevli: _[Atanacak]_
  - Tahmin: 6 saat
  - Açıklama: Kayıt ve giriş API'lerinin yazılması, validasyonların eklenmesi
  
- [ ] JWT oluşturma ve doğrulama mekanizmasının implemente edilmesi
  - Görevli: _[Atanacak]_
  - Tahmin: 5 saat
  - Açıklama: JWT token üretimi ve doğrulama konfigürasyonu, refresh token mekanizması
  
#### Talep Yönetimi Temelleri
- [ ] Talep oluşturma API endpoint'inin implementasyonu
  - Görevli: _[Atanacak]_
  - Tahmin: 6 saat
  - Açıklama: Talep oluşturma API'sinin yazılması, validasyonların eklenmesi
  
- [ ] Talep listeleme API endpoint'inin implementasyonu
  - Görevli: _[Atanacak]_
  - Tahmin: 4 saat
  - Açıklama: Talepleri listeleme (kişisel ve tümü) API'lerinin yazılması

### Sprint Toplantı Zamanları
- Sprint Planlama: _[Tarih/saat eklenecek]_
- Günlük Stand-up: Her gün saat _[saat eklenecek]_
- Sprint Review: _[Tarih/saat eklenecek]_
- Sprint Retrospektif: _[Tarih/saat eklenecek]_

### Tanımlanmış Riskler
1. Kimlik doğrulama ve yetkilendirme kurulumunda yaşanabilecek gecikmeler
   - Azaltma Stratejisi: Gerekirse daha basit bir kimlik doğrulama mekanizması ile başlayıp, sonraki sprintlerde genişletme
   
2. Takım üyelerinin diğer projelerle aynı anda çalışma riski
   - Azaltma Stratejisi: Net görev atamaları ve günlük durum takibi

### Sprint Başarı Kriterleri
1. Temel altyapı bileşenlerinin çalışır durumda olması
2. Kimlik doğrulama mekanizmasının (giriş, kayıt, JWT) çalışır durumda olması
3. Talep oluşturma ve listeleme API'lerinin çalışır durumda olması

### Bir Sonraki Sprint İçin Ön Hazırlık
- Kullanıcı yönetimi API'leri için gereksinim detaylandırma
- Onay süreçleri API'leri için gereksinim detaylandırma
- Frontend geliştirme için teknoloji seçimi 