using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application;

public interface IMunicipalUnitRepository
{
    Task<IReadOnlyList<MunicipalUnit>> GetAllAsync();
}
