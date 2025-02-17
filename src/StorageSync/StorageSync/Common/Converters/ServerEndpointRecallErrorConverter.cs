﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.StorageSync.Common.Converters
{
    using Microsoft.Azure.Commands.StorageSync.Models;
    using System;
    using StorageSyncModels = Microsoft.Azure.Management.StorageSync.Models;

    /// <summary>
    /// Class ServerEndpointRecallErrorConverter.
    /// Implements the <see cref="Converters.ConverterBase{PSServerEndpointRecallError, StorageSyncModels.ServerEndpointRecallError}" />
    /// </summary>
    /// <seealso cref="Converters.ConverterBase{PSServerEndpointRecallError, StorageSyncModels.ServerEndpointRecallError}" />
    public class ServerEndpointRecallErrorConverter : ConverterBase<PSServerEndpointRecallError, StorageSyncModels.ServerEndpointRecallError>
    {
        /// <summary>
        /// Transforms the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>StorageSyncModels.ServerEndpointRecallError.</returns>
        protected override StorageSyncModels.ServerEndpointRecallError Transform(PSServerEndpointRecallError source)
        {
            // Recall status properties are readonly from the RP
            throw new NotSupportedException();
        }

        /// <summary>
        /// Transforms the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>PSServerEndpointRecallError.</returns>
        protected override PSServerEndpointRecallError Transform(StorageSyncModels.ServerEndpointRecallError source)
        {
            return new PSServerEndpointRecallError()
            {
                ErrorCode = source.ErrorCode,
                Count = source.Count
            };
        }
    }
}
