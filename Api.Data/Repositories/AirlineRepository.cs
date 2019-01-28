using Microsoft.EntityFrameworkCore;
using Sisc.Api.Common;
using Sisc.Api.Data.Common;

namespace Sisc.Api.Data.Repositories
{
    public class AirlineRepository : Repository<Airline>, IAirlineRepository
    {
        public AirlineRepository(ISimpleContext context) : base((DbContext)context)
        {
        }
    }
}