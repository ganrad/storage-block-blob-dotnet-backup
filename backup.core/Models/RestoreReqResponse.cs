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

using System;
using Newtonsoft.Json;

namespace backup.core.Models
{
   /// <summary>
   /// A request and response container for the 'Restore' process
   /// </summary>
   [JsonObject(MemberSerialization.OptIn)]
   public class RestoreReqResponse
   {
      /// <summary>
      /// Start date
      /// </summary>
      [JsonProperty]
      public string StartDate { get; set; }

      /// <summary>
      /// End date
      /// </summary>
      [JsonProperty]
      public string EndDate { get; set; }

      /// <summary>
      /// Internal start date
      /// </summary>
      public DateTime StDate { get; set; }

      /// <summary>
      /// Internal end date
      /// </summary>
      public DateTime EnDate { get; set; }

      /// <summary>
      /// Blob container name
      /// </summary>
      [JsonProperty]
      public string ContainerName { get; set; }

      /// <summary>
      /// File name
      /// </summary>
      [JsonProperty]
      public string BlobName { get; set; } = "";

      /// <summary>
      /// Request type : Synchronous or Asynchronous
      /// </summary>
      [JsonProperty]
      public string ReqType { get; set; } = "Sync";

      /// <summary>
      /// Start time
      /// </summary>
      [JsonProperty]
      public DateTime? StartTime { get; set; } = null;

      /// <summary>
      /// End time
      /// </summary>
      [JsonProperty]
      public DateTime? EndTime { get; set; } = null;

      /// <summary>
      /// No. of blobs restored successfully
      /// </summary>
      [JsonProperty]
      public int TotalSuccessCount { get; set; }

      /// <summary>
      /// No. of blobs which failed, could not be restored
      /// </summary>
      [JsonProperty]
      public int TotalFailureCount { get; set; }

      /// <summary>
      /// Exceptions
      /// </summary>
      [JsonProperty]
      public string ExceptionMessage { get; set; } = "";

      /// <summary>
      /// Execution time
      /// </summary>
      [JsonProperty]
      public string ExecutionTime { get; set; } = "";

      /// <summary>
      /// Skip blob deletes ?
      /// </summary>
      [JsonProperty]
      public string SkipDeletes { get; set; } = "No";

      /// <summary>
      /// URI to check the status of restore process
      /// </summary>
      [JsonProperty]
      public string StatusLocationUri { get; set; } = "";
   }
}
