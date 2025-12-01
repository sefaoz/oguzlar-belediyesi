using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure;

public sealed class CouncilRepository : ICouncilRepository
{
    private static readonly IReadOnlyList<CouncilDocument> Documents = new[]
    {
        new CouncilDocument(
            Id: 1,
            Title: "2023 Mali Yılı Faaliyet Raporu",
            Type: "Rapor",
            Date: new DateTime(2024, 1, 15),
            Description: "2023 bütçesinin kapanışı, gelir-gider tabloları ve birim bazlı analizleri sunulmaktadır.",
            FileUrl: "https://documents.oguzlar.bel.tr/faaliyet-2023.pdf"
        ),
        new CouncilDocument(
            Id: 2,
            Title: "2022 Faaliyet Raporu",
            Type: "Rapor",
            Date: new DateTime(2023, 1, 20),
            Description: "2022 yılı boyunca yürütülen projelerin detaylı raporu."
        ),
        new CouncilDocument(
            Id: 3,
            Title: "Meclis Üyeleri ve Görev Dağılımı",
            Type: "Liste",
            Date: new DateTime(2024, 4, 1),
            Description: "Güncel meclis üyeleri ve komisyon görevleri listesi.",
            FileUrl: "https://documents.oguzlar.bel.tr/meclis-uyeleri.pdf"
        )
    };

    public Task<IReadOnlyList<CouncilDocument>> GetAllAsync()
    {
        return Task.FromResult(Documents);
    }
}
