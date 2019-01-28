using Microsoft.EntityFrameworkCore;
using Sisc.Api.Common;

namespace Sisc.Api.Data.Common
{
    public interface ISimpleContext : ISiscContext
    {
        DbSet<Airline> Airlines { get; set; }
    }
}