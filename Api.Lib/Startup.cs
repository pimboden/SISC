using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Sisc.Api.Lib
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, List<string> connectionStrings)
        {
            var dataStart = new Sisc.Api.Data.Startup();
            dataStart.ConfigureServices(services, connectionStrings);
        }
    }
}
