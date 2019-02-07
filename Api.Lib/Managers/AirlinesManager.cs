using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common;
using Sisc.Api.Common.Runtime.Caching;
using Sisc.Api.Data.Repositories;
using Sisc.Api.Lib.Managers.Base;

namespace Sisc.Api.Lib.Managers
{
    public class AirlinesManager: BaseManager<Airline>, IAirlinesManager
    {
        private readonly IAirlineRepository _airlineRepository;
        public AirlinesManager(IAirlineRepository airlineRepository, ICacheHandler cacheHandler) : base(airlineRepository, cacheHandler.GetCacheInfo(CacheName.AirlineCache))
        {
            _airlineRepository = airlineRepository;

        }

        public async Task<Airline> GetByIataCodeAsync(string iataCode, CancellationToken cancellationToken)
        {
            var findTask= await _airlineRepository.FindAsync(x => x.IataCode.StartsWith(iataCode, StringComparison.InvariantCultureIgnoreCase),0,1, cancellationToken);
            return findTask.FirstOrDefault();
        }
    }
}
