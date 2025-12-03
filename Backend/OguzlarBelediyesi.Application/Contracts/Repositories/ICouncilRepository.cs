using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ICouncilRepository
{
    Task<IReadOnlyList<CouncilDocument>> GetAllAsync();
}
