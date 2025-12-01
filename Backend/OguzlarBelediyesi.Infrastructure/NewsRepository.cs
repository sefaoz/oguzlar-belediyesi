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
        new NewsItem(
            "https://picsum.photos/800/600?random=1",
            "25 Kasım 2025",
            "Oğuzlar Belediyesi Yeni Hizmet Binası Açıldı",
            "Modern kütüphane, zabıta birimi ve halkla ilişkiler noktasında hizmet kalitemizi artırıyoruz.",
            "yeni-hizmet-binasi",
            new[]
            {
                "https://picsum.photos/1200/800?random=1",
                "https://picsum.photos/1200/800?random=2",
                "https://picsum.photos/1200/800?random=3"
            }),
        new NewsItem(
            "https://picsum.photos/800/600?random=4",
            "20 Kasım 2025",
            "Ceviz Festivali Hazırlıkları Başladı",
            "Yerli ve yabancı misafirlerimizin buluşacağı festival alanı için sahne kurulumu tamamlanmak üzere.",
            "ceviz-festivali-hazirliklari",
            new[]
            {
                "https://picsum.photos/1200/800?random=4",
                "https://picsum.photos/1200/800?random=5"
            }),
        new NewsItem(
            "https://picsum.photos/800/600?random=6",
            "15 Kasım 2025",
            "Obruk Barajı Çevresi Düzenleniyor",
            "Çevre düzenlemeleri, yürüyüş yolları ve peyzaj çalışmaları halkımızın kullanımına hazırlanıyor.",
            "obruk-baraji-duzenleniyor",
            new[]
            {
                "https://picsum.photos/1200/800?random=6",
                "https://picsum.photos/1200/800?random=7",
                "https://picsum.photos/1200/800?random=8"
            }),
        new NewsItem(
            "https://picsum.photos/800/600?random=9",
            "10 Kasım 2025",
            "Kış Spor Okulları Kayıtları Açıldı",
            "Gençlerimizin sporla buluşması için kayak, masa tenisi ve yüzme kurs kayıtları başladı.",
            "kis-spor-okullari"),
        new NewsItem(
            "https://picsum.photos/800/600?random=10",
            "05 Kasım 2025",
            "Yol Bakım Onarım Çalışmaları Sürüyor",
            "Fen işleri ekiplerimiz ilçe merkezinde ve kırsal mahallelerde yol bakım seferberliğini sürdürüyor.",
            "yol-bakim-onarimlari"),
        new NewsItem(
            "https://picsum.photos/800/600?random=11",
            "29 Ekim 2025",
            "29 Ekim Cumhuriyet Bayramı Coşkulu Kutlandı",
            "Cumhuriyetimizin 102. yılı tüm birimlerin ortak kutlamasıyla gerçekleştirildi.",
            "cumhuriyet-bayrami")
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
