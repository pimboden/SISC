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
    public class CountriesManager : BaseManager<Country>, ICountriesManager
    {
        private readonly ICountriesRepository _countriesRepository;
        public CountriesManager(ICountriesRepository countriesRepository, ICacheHandler cacheHandler) : base(countriesRepository, cacheHandler.GetCacheInfo(CacheName.AirlineCache))
        {
            _countriesRepository = countriesRepository;

        }

      
        public async Task<Airline> GetByIataCodeAsync(string iataCode, CancellationToken cancellationToken)
        {
            var findTask= await _airlineRepository.FindAsync(x => x.IataCode.StartsWith(iataCode, StringComparison.InvariantCultureIgnoreCase),0,1, cancellationToken);
            return findTask.FirstOrDefault();
        }
    }
}
