using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common;
using Sisc.Api.Common.Helpers;
using Sisc.Api.Common.Runtime.Caching;
using Sisc.Api.Data.Repositories;
using Sisc.Api.Lib.Managers.Base;

namespace Sisc.Api.Lib.Managers
{
    public class CountriesManager : BaseManager<Country>, ICountriesManager
    {
        private readonly ICountriesRepository _countriesRepository;
        private ICacheHandler _cacheHandler;
        private CacheInfo _countriesCacheInfo;
        const string cacheKeyPrefix = "Countries";

        public CountriesManager(ICountriesRepository countriesRepository, ICacheHandler cacheHandler) : base(countriesRepository)
        {
            _countriesRepository = countriesRepository;
            _cacheHandler = cacheHandler;
            _countriesCacheInfo = cacheHandler.GetCacheInfo(CacheName.CountriesCache);
            
        }

        private bool TryGetAllOrderedByNameFromCache(string cacheKey, out List<Country> countries)
        {
            try
            {
                countries = _countriesCacheInfo.ObjectCache.Get<List<Country>>(cacheKey);
                return countries != null;
            }
            catch
            {
                countries = null;
                return false;
            }
        }

        public async Task<List<Country>> LoadAllOrderedByNameAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => !cancellationToken.IsCancellationRequested ? LoadAllOrderedByName() : null);
        }

        public List<Country> LoadAllOrderedByName()
        {
            string cacheKey = _cacheHandler.GetCacheKey(cacheKeyPrefix, "AllOrderedByName");

            if (TryGetAllOrderedByNameFromCache(cacheKey, out var countries))
            {
                return countries;
            }
            var loadLock = _countriesCacheInfo.CacheLocks.GetOrAdd(_cacheHandler.GetCacheKey(cacheKeyPrefix, "AllOrderedByName", "Load"), new object());

            lock (loadLock)
            {
                //Was it added to the cache while wating for lock release?
                if (TryGetAllOrderedByNameFromCache(cacheKey, out countries))
                {
                    return countries;
                }
                var queryParams = new BaseQueryParams
                {
                    
                    PageIndex = 0,
                    PageSize = 10000,
                    OrderByColumns = new List<OrderByColumn> { new OrderByColumn { ColumnName = "Name" } }
                };
                countries = base.GetAll(queryParams);
                if (countries != null)
                {
                    _countriesCacheInfo.AddToCache(countries, cacheKey);
                }
                return countries;
            }
        }

        public async Task<Country> GetByIsoCodeAsync(string iataCode, CancellationToken cancellationToken)
        {
            var findTask= await _countriesRepository.FindAsync(x => x.IsoCode.StartsWith(iataCode, StringComparison.InvariantCultureIgnoreCase),0,1, cancellationToken);
            return findTask.FirstOrDefault();
        }


        public virtual int Patch(Country country)
        {
            var countryToUpdate = _countriesRepository.Get(new object[] {country.Id});

            if (!string.IsNullOrEmpty(country.IsoCode))
            {
                countryToUpdate.IsoCode = country.IsoCode;
            }
            if (!string.IsNullOrEmpty(country.Name))
            {
                countryToUpdate.Name = country.Name;
            }
            _countriesRepository.Update(countryToUpdate);
            return _countriesRepository.Complete();

        }

        public virtual async Task<int> PatchAsync(Country country, CancellationToken cancellationToken)
        {
            var countryToUpdate = await _countriesRepository.GetAsync(new object[] { country.Id }, cancellationToken);

            if (!string.IsNullOrEmpty(country.IsoCode))
            {
                countryToUpdate.IsoCode = country.IsoCode;
            }
            if (!string.IsNullOrEmpty(country.Name))
            {
                countryToUpdate.Name = country.Name;
            }
            _countriesRepository.Update(countryToUpdate);
            return await _countriesRepository.CompleteAsync(cancellationToken);
        }

    }
}
