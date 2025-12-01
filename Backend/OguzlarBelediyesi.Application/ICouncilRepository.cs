using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application;

public interface ICouncilRepository
{
    Task<IReadOnlyList<CouncilDocument>> GetAllAsync();
}
