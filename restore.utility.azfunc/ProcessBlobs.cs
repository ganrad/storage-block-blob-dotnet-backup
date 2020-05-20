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
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using System;
using System.Diagnostics;
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
 * ID05192020: gradhakr: Allow users to restore blobs for a single container (feature request)
 * ID05202020: gradhakr: Wrap the 'Restore' process request and response in a JSON object
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
        public async Task<ActionResult<RestoreReqResponse>> Run(
		[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "restore/blobs")]
		HttpRequest req, Microsoft.Extensions.Logging.ILogger log)
        {
	   log.LogInformation($"PerformRestore: Invoked at: {DateTime.Now}");
	   Stopwatch stopWatch = new Stopwatch();
	   stopWatch.Start();

	   string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
	   RestoreReqResponse reqRespData = JsonConvert.DeserializeObject<RestoreReqResponse>(requestBody);

           if ( String.IsNullOrEmpty(reqRespData.StartDate) || String.IsNullOrEmpty(reqRespData.EndDate) ) {
	      reqRespData.ExceptionMessage = "Request is missing JSON payload containing start and end dates!";
	      stopWatch.Stop();
	      reqRespData.ExecutionTime = getTimeString(stopWatch.Elapsed);

	      return reqRespData;
	   };

           DateTime startDate = DateTime.MinValue;
           DateTime endDate = DateTime.MinValue;

           bool startDateParsed = false;
           bool endDateParsed = false;

           startDateParsed = DateTime.TryParse(reqRespData.StartDate, out startDate);
           endDateParsed = DateTime.TryParse(reqRespData.EndDate, out endDate);

           if (!startDateParsed || !endDateParsed) {
	      reqRespData.ExceptionMessage = $"Unable to parse start and end dates. Provide dates in mm/dd/yyyy format. Start date value {reqRespData.StartDate} End date value {reqRespData.EndDate}. ";
	      stopWatch.Stop();
	      reqRespData.ExecutionTime = getTimeString(stopWatch.Elapsed);

	      return reqRespData;
	   };


           if (startDate > endDate) {
              reqRespData.ExceptionMessage = "Start date cannot be greater than end date.";
	      stopWatch.Stop();
	      reqRespData.ExecutionTime = getTimeString(stopWatch.Elapsed);

	      return reqRespData;
	   };

           log.LogInformation($"PerformRestore: Start date : {startDate.ToString("MM/dd/yyyy")}, End date {endDate.ToString("MM/dd/yyyy")}. Proceeding with restore process ...");

	   reqRespData.StDate = startDate;
	   reqRespData.EnDate = endDate;

	   // ID05192020.sn
	   if ( ! String.IsNullOrEmpty(reqRespData.ContainerName) )
              log.LogInformation($"PerformRestore: Container Name : {reqRespData.ContainerName}");

	   if ( ! String.IsNullOrEmpty(reqRespData.BlobName) ) {
	      if ( String.IsNullOrEmpty(reqRespData.ContainerName) ) {
	         reqRespData.ExceptionMessage = $"To restore File : {reqRespData.BlobName}, container name is required!";
		 stopWatch.Stop();
		 reqRespData.ExecutionTime = getTimeString(stopWatch.Elapsed);

		 return reqRespData;
	      };
	   };
	   // ID05192020.en 
	   
	   try
	   {
              // Run the restore process
              await _restoreBackup.Run(reqRespData);
           }
           catch(Exception ex)
           {
              log.LogError($"PerformRestore: Exception occurred. Exception: {@ex.ToString()}");
	      reqRespData.ExceptionMessage = $"Encountered exception : {@ex.ToString()}";
	      stopWatch.Stop();
	      reqRespData.ExecutionTime = getTimeString(stopWatch.Elapsed);

	      return reqRespData;
           }
	   log.LogInformation($"PerformRestore: Completed execution at: {DateTime.Now}");

	   stopWatch.Stop();
	   reqRespData.ExecutionTime = getTimeString(stopWatch.Elapsed);
	   // return (ActionResult) new OkObjectResult("Restore process finished OK"); ID05202020.o
	   return reqRespData; // ID05202020.n
        }

	private string getTimeString(TimeSpan ts) {
	  return ( String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,
	    ts.Milliseconds / 10) );
	}
    }
}
