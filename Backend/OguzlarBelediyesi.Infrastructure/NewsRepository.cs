using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure;

public sealed class NewsRepository : INewsRepository
{
    private static readonly IReadOnlyList<NewsItem> News = new[]
    {
        new NewsItem("https://picsum.photos/400/250?image=20", "12 Kasım 2025", "Yeni Park Alanı Halkın Hizmetine Açıldı",
            "Çocuklar ve aileler için tasarlanan modern oyun ve dinlenme alanları büyük beğeni topladı.",
            "yeni-park-alani-halkin-hizmetine-acildi"),
        new NewsItem("https://picsum.photos/400/250?image=30", "11 Kasım 2025", "Altyapı Yenileme Çalışmaları Tamamlandı",
            "İlçemizin ana caddelerindeki su ve kanalizasyon hatları tamamen yenilendi.",
            "altyapi-yenileme-calismalari-tamamlandi"),
        new NewsItem("https://picsum.photos/400/250?image=40", "10 Kasım 2025", "Sokak Hayvanları İçin Yeni Barınak",
            "Modern ve hijyenik koşullara sahip yeni hayvan barınağımız hizmete girdi.",
            "sokak-hayvanlari-icin-yeni-barinak"),
        new NewsItem("https://picsum.photos/400/250?image=50", "08 Kasım 2025", "Kültür Merkezi İnşaatı Hızla Devam Ediyor",
            "İlçemize değer katacak yeni kültür merkezinin kaba inşaatı tamamlandı.",
            "kultur-merkezi-insaati-hizla-devam-ediyor"),
        new NewsItem("https://picsum.photos/400/250?image=60", "05 Kasım 2025", "Geleneksel Oğuzlar Festivali Başlıyor",
            "Bu yıl 15.si düzenlenecek olan festivalimiz renkli görüntülere sahne olacak.",
            "geleneksel-oguzlar-festivali-basliyor"),
        new NewsItem("https://picsum.photos/400/250?image=70", "01 Kasım 2025", "Belediyemizden Öğrencilere Kırtasiye Desteği",
            "İlçemizdeki ihtiyaç sahibi öğrencilere kırtasiye seti dağıtımı yapıldı.",
            "belediyemizden-ogrencilere-kirtasiye-destegi")
    };

    public Task<IEnumerable<NewsItem>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<NewsItem>>(News);
    }

    public Task<NewsItem?> GetBySlugAsync(string slug)
    {
        var story = News.FirstOrDefault(item =>
            string.Equals(item.Slug, slug, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(story);
    }
}
