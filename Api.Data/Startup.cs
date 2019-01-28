using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sisc.Api.Data.Common;
using Sisc.Api.Data.Repositories;

namespace Sisc.Api.Data
{
    public class Startup
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

            //            services.AddTransient<ICountriesRepository, CountriesRepository>();
        }
    }
}
