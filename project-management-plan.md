# Satın Alma Platformu - Proje Yönetimi Planı

## Proje Kanban Düzeni

GitHub Projects'te oluşturulacak kanban tahtası için önerilen sütunlar:

- **Backlog**: Henüz planlanmamış tüm görevler
- **To Do**: Sprint için seçilmiş, çalışmaya hazır görevler
- **In Progress**: Üzerinde çalışılan görevler
- **Review/Test**: Kod incelemesi veya test bekleyen görevler
- **Done**: Tamamlanmış görevler

## Etiketler (Labels)

Görev takibi ve filtreleme için kullanılacak etiketler:

- **Özellikler (Mavi Tonları)**
  - `feature:kullanici-yonetimi`
  - `feature:talep-yonetimi`
  - `feature:onay-surecleri`
  - `feature:tedarikci-yonetimi`
  - `feature:siparis-takip`
  - `feature:dosya-yonetimi`
  - `feature:raporlama`
  - `feature:bildirim-sistemi`

- **Katmanlar (Yeşil Tonları)**
  - `layer:api`
  - `layer:bll`
  - `layer:dal`
  - `layer:frontend`
  - `layer:db`

- **Öncelik (Kırmızı Tonları)**
  - `priority:high`
  - `priority:medium`
  - `priority:low`

- **Görev Türü (Turuncu Tonları)**
  - `type:bug`
  - `type:enhancement`
  - `type:documentation`

## Sprint Planlaması

Her sprint için:

1. Sprint planlaması toplantısı düzenle
2. Backlog'dan "To Do" sütununa görevleri taşı
3. Haftalık durum değerlendirme toplantıları yap
4. Sprint sonunda retrospektif toplantı yap

## İlk Sprint Görevleri

İlk sprint için öncelikli görevler (to-do.md dosyasından alınmıştır):

1. Temel loglama mekanizmasını entegre et
2. Temel hata yönetimi middleware'ini ekle
3. Automapper veya benzeri bir mapping kütüphanesini yapılandır
4. .NET Identity veya özel kimlik doğrulama mekanizmasını entegre et
5. Kullanıcı kayıt ve giriş işlemlerini implemente et
6. JWT oluşturma ve doğrulama mekanizmasını implemente et

## Rol ve Sorumluluklar

- **Proje Yöneticisi**: Sprint planlaması, görev takibi, engellerin kaldırılması
- **Geliştirici(ler)**: Kod geliştirme, birim testleri yazma, kod incelemeleri
- **Test Uzmanı**: Manuel ve otomatik testlerin planlanması ve yürütülmesi
- **Ürün Sahibi**: Önceliklendirme, iş gereksinimlerinin netleştirilmesi

## Akış Diyagramları ve Şemalar

GitHub içinde ilgili wiki sayfalarında veya ayrı belgelerde şunlar bulunacak:

- Veritabanı şeması
- Onay akışları
- API endpoint listesi
- UI wireframe'leri

## GitHub Entegrasyonu İçin Sonraki Adımlar

1. **GitHub Repository Oluştur**: 
   - Repository adı: SatinAlmaPlatformu
   - Açıklama: Kurumsal satın alma süreçlerini dijitalleştiren ve optimize eden bir yazılım çözümü

2. **GitHub Projects Kur**:
   - Yeni bir proje oluştur (Table veya Board view)
   - Yukarıdaki kanban düzenini ayarla
   - Etiketleri oluştur
   
3. **İş Takibi**:
   - to-do.md dosyasındaki görevleri GitHub Issues olarak aktar
   - Her issue'ya uygun etiketleri ekle
   - İlk sprint için görevleri milestone ile ilişkilendir 