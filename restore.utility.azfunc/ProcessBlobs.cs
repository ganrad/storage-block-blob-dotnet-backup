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

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

/**
 * Description:
 * This Azure Function Wrapper restores Azure Storage Block blobs from backup storage account into destination storage
 * account.
 *
 * Author: Ganesh Radhakrishnan @Microsoft
 * Dated: 05-07-2020
 *
 * NOTES: Capture updates to the code below.
 */

namespace restore.utility.azfunc
{
    /// <summary>
    /// This class restores blobs from the backup SA to the source/original SA.
    /// </summary>
    class ProcessBlobs
    {
        private readonly IRestoreBackup _restoreBackup;

	public ProcessBlobs(IRestoreBackup restoreBackup)
	{
	   _restoreBackup = restoreBackup;
	}

	/// <summary>
	/// This function exposes an HTTP end-point
	/// </summary>
	[FunctionName("PerformRestore")]
        public async Task<IActionResult> Run(
		[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "restore/blobs")]
		HttpRequest req, Microsoft.Extensions.Logging.ILogger log)
        {
	   log.LogInformation($"PerformRestore: Invoked at: {DateTime.Now}");

	   string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
	   dynamic reqData = JsonConvert.DeserializeObject(requestBody);
	   string args0 = reqData?.startDate;
	   string args1 = reqData?.endDate;

           if ( (args0 == null) || (args1 == null) )
	      return new BadRequestObjectResult("Request is missing JSON payload containing start and end dates!");

           DateTime startDate = DateTime.MinValue;
           DateTime endDate = DateTime.MinValue;

           bool startDateParsed = false;
           bool endDateParsed = false;

           startDateParsed = DateTime.TryParse(args0, out startDate);
           endDateParsed = DateTime.TryParse(args1, out endDate);

           if (!startDateParsed || !endDateParsed)
              return new BadRequestObjectResult($"Unable to parse start and end dates. Provide dates in mm/dd/yyyy format. Start date value {args0} End date value {args1}. ");


           if (startDate > endDate)
              return new BadRequestObjectResult("Start date cannot be greater than end date.");

           log.LogInformation($"PerformRestore: Start date : {startDate.ToString("MM/dd/yyyy")}, End date {endDate.ToString("MM/dd/yyyy")}. Proceeding with restore process ...");

	   try
	   {
                // Run the restore process
                await _restoreBackup.Run(startDate, endDate);
            }
            catch(Exception ex)
            {
                log.LogError($"PerformRestore: Exception occurred. Exception: {@ex.ToString()}");
            }
	    log.LogInformation($"PerformRestore: Completed execution at: {DateTime.Now}");

	    return (ActionResult) new OkObjectResult("Restore process finished OK");
        }
    }
}
