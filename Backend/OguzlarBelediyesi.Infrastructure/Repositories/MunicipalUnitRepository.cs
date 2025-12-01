using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure.Repositories;

public sealed class MunicipalUnitRepository : IMunicipalUnitRepository
{
    private static readonly IReadOnlyList<MunicipalUnit> Units = new[]
    {
        new MunicipalUnit(
            Id: "ozel-kalem",
            Title: "Özel Kalem, Basın ve Halkla İlişkiler",
            Content: "Özel Kalem, Basın ve Halkla İlişkiler birimi, başkanlık makamının programlarını düzenler ve belediyenin basınla ilişkilerini yürütür.",
            Icon: "fa-pen-fancy",
            Staff: new[]
            {
                new UnitStaff("Ahmet YILMAZ", "Özel Kalem Müdürü", "assets/images/placeholder-person.jpg")
            }
        ),
        new MunicipalUnit(
            Id: "zabita",
            Title: "Zabıta Amirliği",
            Content: "Zabıta Amirliği, belde halkının esenlik, huzur, sağlık ve düzenini sağlamakla görevli birimdir.",
            Icon: "fa-shield-alt",
            Staff: new[]
            {
                new UnitStaff("Ali KÜRENCİ", "Zabıta Amiri", "assets/images/zabita-amiri.jpg")
            }
        ),
        new MunicipalUnit(
            Id: "mali-hizmetler",
            Title: "Mali Hizmetler Müdürlüğü",
            Content: "Mali Hizmetler Müdürlüğü, belediyenin mali kaynaklarını yönetir.",
            Icon: "fa-coins",
            Staff: new[]
            {
                new UnitStaff("Mehmet DEMİR", "Mali Hizmetler Müdürü", "assets/images/placeholder-person.jpg")
            }
        ),
        new MunicipalUnit(
            Id: "tahsilat-emlak",
            Title: "Tahsilat ve Emlak Birimi",
            Content: "Tahsilat ve Emlak Birimi, belediye gelirlerinin tahsilatını ve emlak işlemlerini yürütür.",
            Icon: "fa-file-invoice-dollar",
            Staff: new[]
            {
                new UnitStaff("Ayşe KAYA", "Birim Sorumlusu", "assets/images/placeholder-person.jpg")
            }
        ),
        new MunicipalUnit(
            Id: "yazi-isleri",
            Title: "Yazı İşleri Müdürlüğü",
            Content: "Yazı İşleri Müdürlüğü, belediyenin resmi yazışmalarını ve meclis kararlarını takip eder.",
            Icon: "fa-file-signature",
            Staff: new[]
            {
                new UnitStaff("Fatma ÇELİK", "Yazı İşleri Müdürü", "assets/images/placeholder-person.jpg")
            }
        ),
        new MunicipalUnit(
            Id: "fen-isleri",
            Title: "Fen İşleri Müdürlüğü",
            Content: "Fen İşleri Müdürlüğü, ilçenin altyapı ve üstyapı çalışmalarını yürütür.",
            Icon: "fa-hard-hat",
            Staff: new[]
            {
                new UnitStaff("Mustafa ÇAHİN", "Fen İşleri Müdürü", "assets/images/placeholder-person.jpg")
            }
        )
    };

    public Task<IReadOnlyList<MunicipalUnit>> GetAllAsync()
    {
        return Task.FromResult(Units);
    }
}
