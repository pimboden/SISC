using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common;

namespace Sisc.Api.Lib.Managers
{
    public interface ICountriesManager : IBaseManager<Country>
    {
        Task<Country> GetByIsoCodeAsync(string isoCode, CancellationToken cancellationToken);
    }
}
