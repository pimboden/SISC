using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common;

namespace Sisc.Api.Lib.Managers
{
    public interface ICountriesManager : IBaseManager<Country>
    {
        List<Country> LoadAllOrderedByName();
        Task<List<Country>> LoadAllOrderedByNameAsync(CancellationToken cancellationToken);

        Task<Country> GetByIsoCodeAsync(string isoCode, CancellationToken cancellationToken);

        int Patch(Country country);
        Task<int> PatchAsync(Country country, CancellationToken cancellationToken);
        
    }
}
