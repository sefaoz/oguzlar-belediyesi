# Haber ve Sayfa Verisi Planı

Bu belge, `baskan-hakkinda`, `baskan-mesaji`, `baskana-mesaj`, `cografi-yapi`, `iletisim` ve `tarihimiz` sayfalarının veri katmanını hizmetlerden besleyerek backend üzerinden çalıştırılmasını ve kısa vadede mock veri yapısıyla ilerlerken orta vadede gerçek veritabanına geçişi takip etmek için hazırlanmıştır.

## Amaçlar

1. Tüm sayfa içeriklerinin API aracılığıyla alınmasını sağlamak.
2. API’nin şu anda mock ("sahte") veri üzerinden çalışmasına olanak tanımak.
3. Veritabanı temelli gerçek veri altyapısı için gerekli bir geçiş planı yaratmak.

## Plan

1. **İçerik İncelemesi ve Veri Modelleme**  
   - Her sayfa için gösterilen metin/başlık/iletisim verilerini kataloglayıp tek bir veya sınıflı modellerle temsil etmek. (Örn. `ContentBlock`, `ContactInfo`, `BreadcrumbSet` gibi.)
   - Mevcut metinleri backend tarafında saklayacak basit DTO/record yapıları oluşturmak ve ortak repository ile erişilebilir halde tutmak.
2. **Mock API ve Servis Katmanı**  
   - Backend’de `PageContentRepository` benzeri bir servis oluşturup sayfa bazında mock veriyi saklayacak metotlar (`GetAboutPage`, `GetGeographyPage`, `GetContactInfo`, `GetHistoryPage`, `GetMessagePage`, `GetPresidentMessage`) tanımlamak.
   - Bu repository’yi Web API’de bağımlılık olarak kaydedip `/api/pages/{key}` veya ayrı endpoint’ler açarak HTTP yoluyla servislemek.
   - Angular tarafında `PageContentService` oluşturup environment seçimleri üzerinden API URL’ine yönlendirmek (halihazırda `environment.newsApiUrl` var, benzer şekilde `environment.pageContentUrl` de eklenebilir).
3. **Front-end Tüketimi**  
   - Her sayfa bileşeni (başkan, coğrafi yapı, iletişim, tarihimiz vb.) için mevcut statik içerikleri service çağrıları ile dolduracak şekilde minimal state logic eklemek.
   - Zorunlu olmayan içeriklerde skeleton/placeholder kullanarak veri yüklenene kadar kullanıcıya bilgi vermek.
4. **Mock’tan Veritabanına Geçiş Planı**  
   - Backend katmanına bir `IDataSeeder` ekleyerek mock veri girişini kolaylaştırmak; ileride `DbContext` bazlı repository’ler bu seeder’dan beslenebilir.
   - Real DB ile entegrasyon: `OguzlarBelediyesi.Infrastructure` içinde `NewsContext` ve `PageContentContext` oluşturup migration’lar hazırlamak; `INewsRepository` gibi arayüzlere Entity Framework implementasyonu eklemek.
   - UI tarafında caching/hatırlama stratejisi olmadan API’den gelen içerikleri göstermeye devam etmek; ileriki aşamada `NgRx` vb. eklenecekse yeni katman planlanabilir.

## Takip

- Bu dosya üzerinden hangi adımda olduğumuzu, çalışmalar sırasında yapılan değişiklikleri ve gelecekteki görevleri takip edeceğiz.  
- Yeni görev eklendiğinde buraya kısa not düşülecek; örneğin "[Tarihimiz page] içerik mock veri olarak backend’e eklenecek" gibi.  
- Geliştirme ilerledikçe bu planı güncel tutup sonrasında güncellenmiş halleri burada saklayacağız.
