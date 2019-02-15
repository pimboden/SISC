using Microsoft.EntityFrameworkCore;
using Sisc.Api.Common;
using Sisc.Api.Data.Common;

namespace Sisc.Api.Data.Repositories
{
    public class CountriesRepository : Repository<Country>, ICountriesRepository
    {
        public CountriesRepository(ISimpleContext context) : base((DbContext)context)
        {
        }
    }
}