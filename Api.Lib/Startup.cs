
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sisc.Api.Data.Common;
using Sisc.Api.Lib.Managers;

namespace Sisc.Api.Lib
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, List<string> connectionStrings)
        {
            var dataStart = new Data.StartUp();
            dataStart.ConfigureServices(services, connectionStrings);
            services.AddTransient<ICountriesManager, CountriesManager>();
            services.AddTransient<ISimpleContext, SimpleContext>();
        }
    }
}
