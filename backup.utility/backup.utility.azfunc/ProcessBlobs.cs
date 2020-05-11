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
using backup.core.Models;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

/**
 * Description:
 * This function runs the backup process and is triggered by messages arriving in an Azure Storage Queue. For each
 * message, the function then copies a blob from the source storage container to the target storage container.
 * 
 * Author: Ganesh Radhakrishnan @Microsoft
 * Dated: 05-06-2020
 *
 * NOTES: Capture updates to the code below.
 */

namespace backup.utility.azfunc
{
    /// <summary>
    /// This class copies (backs up) blobs from source SA to an backup SA.
    /// </summary>
    class ProcessBlobs
    {
	private readonly IStorageBackup _storageBackup;
        
	public ProcessBlobs(IStorageBackup storageBackup)
	{
	    _storageBackup = storageBackup;
	}

	/// <summary>
	/// This function is invoked by a Timer Trigger
	/// </summary>
	[FunctionName("PerformBackup")]
	public async Task Run(
		[TimerTrigger("*/10 * * * * *")] TimerInfo callTimer,  // Fire every 10 seconds ...
		 Microsoft.Extensions.Logging.ILogger log)
	{
	    if ( callTimer.IsPastDue )
	    {
		log.LogInformation("PerformBackup: Timer is running late!");
	    }

	    log.LogInformation($"PerformBackup: Invoked at: {DateTime.Now}");
            try
            {
                // Run the storage process
                await _storageBackup.Run();
            }
            catch(Exception ex)
            {
                log.LogError($"PerformBackup: Exception occurred while processing message. Exception : {@ex.ToString()}");
            }
	    log.LogInformation($"PerformBackup: Completed execution at: {DateTime.Now}");
	}
    }
}
