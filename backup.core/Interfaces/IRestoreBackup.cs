﻿// 
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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/**
 * NOTES:
 * ID05192020: gradhakr : Updated code to allow users to restore blobs for a single container
 */

namespace backup.core.Interfaces
{
    /// <summary>
    /// IRestoreBackup
    /// </summary>
    public interface IRestoreBackup
    {
        /// <summary>
        /// Run
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
	/// <param name="contName"></param>
        /// <returns></returns>
        // Task Run(DateTime startDate, DateTime endDate); ID05192020.o
        Task Run(DateTime startDate, DateTime endDate, String contName); // ID05192020.n
    }
}
