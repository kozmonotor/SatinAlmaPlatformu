# Satın Alma Platformu - Risk Analizi ve Risk Yönetimi Planı

## 1. Risk Analizi Metodolojisi

Bu belge, Satın Alma Platformu projesi için tanımlanmış riskleri, bunların değerlendirmesini ve yönetim stratejilerini içerir. Risk analizi aşağıdaki metodoloji kullanılarak gerçekleştirilmiştir:

1. **Risk Tanımlama**: Beyin fırtınası, geçmiş proje deneyimleri ve uzman görüşleri ile riskler tanımlanmıştır.
2. **Risk Değerlendirme**: Her bir risk, olasılık (1-5) ve etki (1-5) açısından değerlendirilmiştir.
3. **Risk Puanı Hesaplama**: Olasılık × Etki = Risk Puanı
4. **Risk Kategorilendirme**:
   - Düşük Risk: 1-6 puan
   - Orta Risk: 7-14 puan
   - Yüksek Risk: 15-25 puan
5. **Risk Yanıt Stratejileri Belirleme**: Her risk için uygun yanıt stratejileri (kaçınma, azaltma, transfer, kabul) belirlenmiştir.

## 2. Tanımlanmış Riskler ve Değerlendirme

### 2.1. Proje Yönetimi Riskleri

| ID | Risk Tanımı | Olasılık (1-5) | Etki (1-5) | Risk Puanı | Kategori | Risk Sahibi |
|----|-------------|---------------|------------|------------|-----------|-------------|
| PM-01 | Proje kapsamının netleştirilmemesi veya sürekli değişmesi | 3 | 5 | 15 | Yüksek | Proje Yöneticisi |
| PM-02 | Gerçekçi olmayan proje takvimi | 3 | 4 | 12 | Orta | Proje Yöneticisi |
| PM-03 | İletişim problemleri veya bilgi akışı eksikliği | 2 | 4 | 8 | Orta | Proje Yöneticisi |
| PM-04 | Takım üyelerinin diğer projeler ile aşırı yüklenmiş olması | 4 | 3 | 12 | Orta | Proje Yöneticisi |

### 2.2. Teknik Riskler

| ID | Risk Tanımı | Olasılık (1-5) | Etki (1-5) | Risk Puanı | Kategori | Risk Sahibi |
|----|-------------|---------------|------------|------------|-----------|-------------|
| TR-01 | Seçilen teknolojilerin veya mimari yaklaşımın uygun olmaması | 2 | 5 | 10 | Orta | Teknik Lider |
| TR-02 | Veri göçü veya entegrasyon sorunları | 3 | 4 | 12 | Orta | Teknik Lider |
| TR-03 | Performans veya ölçeklenebilirlik sorunları | 3 | 4 | 12 | Orta | Teknik Lider |
| TR-04 | Güvenlik açıkları veya veri sızıntısı | 2 | 5 | 10 | Orta | Teknik Lider |
| TR-05 | Üçüncü taraf bileşenler veya API'ler ile uyumluluk sorunları | 3 | 3 | 9 | Orta | Teknik Lider |

### 2.3. Kaynaklar ve Yetenekler

| ID | Risk Tanımı | Olasılık (1-5) | Etki (1-5) | Risk Puanı | Kategori | Risk Sahibi |
|----|-------------|---------------|------------|------------|-----------|-------------|
| RS-01 | Gerekli teknik becerilere sahip personel eksikliği | 3 | 4 | 12 | Orta | Proje Yöneticisi & İK |
| RS-02 | Takım üyelerinin projeden ayrılması | 2 | 4 | 8 | Orta | Proje Yöneticisi & İK |
| RS-03 | Yetersiz altyapı veya donanım kaynakları | 2 | 3 | 6 | Düşük | BT Yöneticisi |

### 2.4. Dış Faktörler

| ID | Risk Tanımı | Olasılık (1-5) | Etki (1-5) | Risk Puanı | Kategori | Risk Sahibi |
|----|-------------|---------------|------------|------------|-----------|-------------|
| EF-01 | Gereksinimlerin değişmesi veya net olmaması | 4 | 4 | 16 | Yüksek | Ürün Sahibi |
| EF-02 | Yasal düzenlemeler veya standartlardaki değişiklikler | 2 | 4 | 8 | Orta | Ürün Sahibi |
| EF-03 | Tedarikçi veya üçüncü taraf hizmet sağlayıcıların gecikmesi | 3 | 3 | 9 | Orta | Proje Yöneticisi |

## 3. Risk Yanıt Stratejileri ve Azaltma Planları

### Yüksek Riskler

#### PM-01: Proje kapsamının netleştirilmemesi veya sürekli değişmesi
- **Strateji**: Azaltma
- **Azaltma Planı**:
  - Proje başlangıcında detaylı bir kapsam belirleme çalıştayı düzenlenecek
  - Kapsam Değişiklik Yönetimi süreci tanımlanacak ve uygulanacak
  - Sprint bazlı çalışarak küçük parçalar halinde onay alınacak
  - Düzenli paydaş gözden geçirme toplantıları düzenlenecek
- **Acil Durum Planı**: Etki analizine dayalı olarak, değişiklik değerlendirme komitesi tarafından onaylanan kritik değişiklikler için ek kaynak ve zaman ayrılacak

#### EF-01: Gereksinimlerin değişmesi veya net olmaması
- **Strateji**: Azaltma
- **Azaltma Planı**:
  - Gereksinim analizi sürecinde tüm paydaşların katılımını sağlamak
  - Her gereksinim için açık kabul kriterleri oluşturmak
  - Prototipleme ve erken demo'lar ile gereksinimlerin doğruluğunu kontrol etmek
  - Değişiklik yönetimi sürecini disiplinli bir şekilde uygulamak
- **Acil Durum Planı**: Kritik değişiklikler için etki analizi yapıp, proje planını ve bütçeyi gözden geçirmek

### Orta Riskler (Öne Çıkanlar)

#### PM-02: Gerçekçi olmayan proje takvimi
- **Strateji**: Azaltma
- **Azaltma Planı**:
  - Takım tabanlı tahminleme yapmak (Planning Poker vb.)
  - Belirsizlik faktörlerini içeren tampon süreleri plan içerisine dahil etmek
  - Periyodik plan gözden geçirmeleri yapmak
  - İlerlemeyi ölçmek ve plan sapmaları için erken uyarı mekanizmaları oluşturmak

#### PM-04: Takım üyelerinin diğer projeler ile aşırı yüklenmiş olması
- **Strateji**: Azaltma ve Transfer
- **Azaltma Planı**:
  - Proje başlangıcında net kaynak tahsisi yapmak ve üst yönetimden onay almak
  - Diğer projelerle kaynak paylaşımı konusunda anlaşmalar yapmak
  - Takımın kapasitesini ve performansını düzenli olarak izlemek
  - Kritik görevler için yedekleme stratejisi belirlemek
- **Transfer Planı**: Kritik durumlarda dışarıdan kaynak temin etmek için önceden anlaşmalar yapmak

#### TR-02: Veri göçü veya entegrasyon sorunları
- **Strateji**: Azaltma
- **Azaltma Planı**:
  - Detaylı bir veri göçü ve entegrasyon stratejisi oluşturmak
  - Test ortamında kapsamlı entegrasyon testleri yapmak
  - Pilot uygulama ile reel verilerle test etmek
  - Geri dönüş (rollback) planları oluşturmak

#### TR-03: Performans veya ölçeklenebilirlik sorunları
- **Strateji**: Azaltma ve Kaçınma
- **Azaltma Planı**:
  - Performans ve stres testleri planlamak
  - Uygulama mimarisinde ölçeklenebilirliğe dikkat etmek (mikro servisler, önbellek stratejisi vb.)
  - Yük altında düzenli testler yapmak
  - Performans izleme araçlarını entegre etmek
- **Kaçınma Planı**: Mimari tasarımda, yüksek yük için öncelikli olarak bulut tabanlı ölçeklenebilir çözümler kullanmak

## 4. Risk İzleme ve Kontrol

### 4.1. Risk İzleme Süreci
- Haftalık risk gözden geçirme toplantıları düzenlenecek
- Risk kayıt defteri düzenli olarak güncellenecek
- Her sprint başlangıcında o sprint ile ilgili riskler gözden geçirilecek
- Her sprint sonunda risk durumu değerlendirilecek ve gerekli güncellemeler yapılacak

### 4.2. Gösterge ve Tetikleyiciler
Her risk için aşağıdaki tetikleyiciler tanımlanmıştır:
- Proje takviminde 10% veya daha fazla gecikme
- Test sürecinde kritik hataların tespit edilmesi
- Takım performansının %20'den fazla düşmesi
- Maliyet aşımı %10'u geçmesi

### 4.3. Raporlama
- Aylık durum raporlarında risk güncellemeleri yer alacak
- Kritik risk tetikleyicileri gerçekleştiğinde acil durum bildirimleri yapılacak
- Çeyrek dönemlik risk değerlendirme raporları hazırlanacak

## 5. Rol ve Sorumluluklar

### 5.1. Risk Yönetimi Ekibi
- **Proje Yöneticisi**: Risk yönetimi sürecinin genel koordinasyonu, risk kaydının tutulması
- **Teknik Lider**: Teknik risklerin değerlendirilmesi ve yönetimi
- **Ürün Sahibi**: İş ve gereksinim risklerinin değerlendirilmesi
- **Takım Üyeleri**: Risklerin tanımlanması ve azaltma faaliyetlerine katılım

### 5.2. Karar Alma Süreci
- Düşük Riskler: Risk sahibi tarafından yönetilir
- Orta Riskler: Risk yönetimi ekibi tarafından değerlendirilir ve onaylanır
- Yüksek Riskler: Proje yönlendirme komitesi tarafından değerlendirilir ve onaylanır

## 6. Risk Yönetimi Eğitim ve İletişim

### 6.1. Eğitim
- Proje başlangıcında tüm ekip üyelerine risk yönetimi eğitimi verilecek
- Risk izleme ve raporlama konusunda sorumlulara özel eğitimler sağlanacak

### 6.2. İletişim
- Risk kaydı tüm ekip ile GitHub üzerinden paylaşılacak
- Yüksek riskler haftalık proje durum raporlarında yer alacak
- Risk durumu değişiklikleri e-posta ile tüm paydaşlara iletilecek

## 7. Sürekli İyileştirme

Risk yönetimi süreci, proje boyunca aşağıdaki mekanizmalarla sürekli olarak iyileştirilecektir:
- Her sprint retrospektifinde risk yönetimi değerlendirilecek
- Gerçekleşen risklerden öğrenilen dersler dokümante edilecek
- Üç ayda bir risk yönetimi süreç değerlendirmesi yapılacak 