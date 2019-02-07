using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sisc.Api.Data.Common;
using Sisc.Api.Data.Repositories;

namespace Sisc.Api.Data
{
    public class StartUp
    {
        public void ConfigureServices(IServiceCollection services, List<string> sqlConnectionStrings)
        {
            services.AddTransient<ISimpleContext, SimpleContext>();
            services.AddDbContext<SimpleContext>(options =>
                options.UseNpgsql(
                    sqlConnectionStrings[0]
                )
            );
            services.AddTransient<IAirlineRepository, AirlineRepository>();
        }
    }
}
