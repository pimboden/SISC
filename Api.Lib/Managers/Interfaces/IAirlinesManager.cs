using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common;

namespace Sisc.Api.Lib.Managers
{
    public interface IAirlinesManager : IBaseManager<Airline>
    {
        Task<Airline> GetByIataCodeAsync(string iataCode, CancellationToken cancellationToken);
    }
}
