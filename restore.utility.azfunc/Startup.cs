// 
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using backup.core.Implementations;
using backup.core.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using System;
using System.IO;

/**
 * Description:
 * This class configures and loads the Function's service dependencies into the IoC container.
 * 
 * Author: Ganesh Radhakrishnan @Microsoft
 * Dated: 05-06-2020
 *
 * NOTES: Capture updates to the code below.
 */

[assembly: FunctionsStartup(typeof(restore.utility.azfunc.Startup))]

namespace restore.utility.azfunc
{
    public class Startup : FunctionsStartup
    {
       public override void Configure(IFunctionsHostBuilder builder)
       {
	  var configuration = new ConfigurationBuilder()
            // .SetBasePath(Directory.GetCurrentDirectory())
	    .SetBasePath("/home/site/wwwroot/")
            .AddJsonFile("appsettings.json", false)
            .Build();

          var logger = new LoggerConfiguration()
	    .WriteTo.Console()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

          builder.Services.AddLogging(opt => opt.AddSerilog(logger));
	  builder.Services.AddLogging();
	    
          // Add access to generic IConfigurationRoot
	  // builder.Services.AddSingleton<Serilog.ILogger>(logger);
          builder.Services.AddSingleton(configuration);

          // Add services
          builder.Services.AddTransient<IStorageRepository, TableRepository>();
          builder.Services.AddTransient<IBlobRepository, BlobRepository>();
          builder.Services.AddTransient<IRestoreBackup, RestoreBackupWorker>();
       }
    }
}
