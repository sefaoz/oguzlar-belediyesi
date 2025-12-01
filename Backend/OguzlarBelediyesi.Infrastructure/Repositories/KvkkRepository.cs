using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure.Repositories;

public sealed class KvkkRepository : IKvkkRepository
{
    private static readonly IReadOnlyList<KvkkDocument> Documents = new[]
    {
        new KvkkDocument(
            Id: 1,
            Title: "Oğuzlar Belediyesi KVKK Politikası",
            Type: "Politika",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/oguzlar-belediyesi-kvkk-politikasi_17.pdf"
        ),
        new KvkkDocument(
            Id: 2,
            Title: "Oğuzlar Belediyesi KVKK Başvuru Formu",
            Type: "Form",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/kvkk-basvuru-formu_03.pdf"
        ),
        new KvkkDocument(
            Id: 3,
            Title: "Oğuzlar Belediye Sosyal Medya Aydınlatma Metni",
            Type: "Aydınlatma Metni",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/sosyal-medya-aydinlatma-metni_21.pdf"
        ),
        new KvkkDocument(
            Id: 4,
            Title: "Oğuzlar Belediyesi Kişisel Veri Saklama ve İmha Politikası",
            Type: "Politika",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/kisisel-veri-saklama-ve-imha-politikasi_01.pdf"
        ),
        new KvkkDocument(
            Id: 5,
            Title: "Oğuzlar Belediyesi Aydınlatma Metni",
            Type: "Aydınlatma Metni",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/aydinlatma-metni_52.pdf"
        ),
        new KvkkDocument(
            Id: 6,
            Title: "Oğuzlar Belediyesi Çerez Politikası",
            Type: "Politika",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/cerez-politikasi_55.pdf"
        ),
        new KvkkDocument(
            Id: 7,
            Title: "Oğuzlar Belediyesi İzin Formu",
            Type: "Form",
            FileUrl: "http://www.oguzlar.bel.tr/Upload/files/izin-formu_58.pdf"
        )
    };

    public Task<IReadOnlyList<KvkkDocument>> GetAllAsync()
    {
        return Task.FromResult(Documents);
    }
}
