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
// using Microsoft.WindowsAzure.Storage;
// using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure.Storage; // ID05052020.n
using Microsoft.Azure.Cosmos.Table; // ID05052020.n
using System;
using System.Collections.Generic;
using System.Text;

/**
 * ID05052020 : gradhakr : Updated code to use Azure Cosmos v1.0.x .NET API
 */

namespace backup.core.Interfaces
{
    /// <summary>
    /// IEventData
    /// </summary>
    public class IEventData : TableEntity
    {
    }
}
