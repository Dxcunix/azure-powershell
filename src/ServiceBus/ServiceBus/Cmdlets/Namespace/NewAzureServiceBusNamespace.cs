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

using System;
using Microsoft.Azure.Commands.ServiceBus.Models;
using Microsoft.WindowsAzure.Commands.Common.CustomAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.ResourceManager.Common.Tags;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.Azure.Commands.ServiceBus.Commands.Namespace
{
    /// <summary>
    /// 'New-AzServiceBusNamespace' cmdlet creates a new Servicebus NameSpace
    /// </summary>
    [CmdletOutputBreakingChange(typeof(PSNamespaceAttributes), DeprecatedOutputProperties = new string[] { "ResourceGroup" }, NewOutputProperties = new string[] { "ResourceGroupName" })]
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ServiceBusNamespace", SupportsShouldProcess = true), OutputType(typeof(PSNamespaceAttributes))]
    public class NewAzureRmServiceBusNamespace : AzureServiceBusCmdletBase
    {
        /// <summary>
        /// Name of the resource group.
        /// </summary>
        [Parameter( Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0, HelpMessage = "Resource Group Name")]
        [ResourceGroupCompleter]
        [Alias("ResourceGroup")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// EventHub Namespace Location.
        /// </summary>
        [Parameter( Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1, HelpMessage = "ServiceBus Namespace Location")]
        [LocationCompleter("Microsoft.ServiceBus/namespaces")]
        [ValidateNotNullOrEmpty]
        public string Location { get; set; }

        /// <summary>
        /// EventHub Namespace Name.
        /// </summary>
        [Parameter( Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 2, HelpMessage = "ServiceBus Namespace Name")]
        [Alias(AliasNamespaceName)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        
        /// <summary>
        /// Namespace Sku Name.
        /// </summary>
        [Parameter( Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Namespace Sku Name")]
        [ValidateSet(SKU.Basic, SKU.Standard, SKU.Premium, IgnoreCase = true)]
        public string SkuName { get; set; }

        /// <summary>
        /// The Service Bus Premium namespace throughput units.
        /// </summary>        
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The Service Bus premium namespace throughput units, allowed values 1 or 2 or 4")]
        [PSArgumentCompleter("1", "2", "4")]
        public int? SkuCapacity { get; set; }

        /// <summary>
        /// Hashtables which represents resource Tags.
        /// </summary>
        [Parameter( Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Hashtables which represents resource Tags")]
        public Hashtable Tag { get; set; }


        /// <summary>
        /// Indicates whether ZoneRedundant is enabled.
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "enabling or disabling Zone Redundant for namespace")]
        public SwitchParameter ZoneRedundant { get; set; }

        /// <summary>
        /// Indicates whether DisableLocalAuth is enabled.
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "enabling or disabling SAS authentication for the Service Bus namespace")]
        public SwitchParameter DisableLocalAuth { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage ="Identity Type" )]
        [ValidateSet("SystemAssigned", "UserAssigned", "SystemAssigned, UserAssigned", "None", IgnoreCase = true)]
        public string IdentityType { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "List of user assigned Identity Ids")]
        public string[] IdentityId { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Key Property")]
        public PSEncryptionConfigAttributes[] EncryptionConfig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override void ExecuteCmdlet()
        {
            try
            {
                // Create a new ServiceBus namespace
                Dictionary<string, string> tagDictionary = TagsConversionHelper.CreateTagDictionary(Tag, validate: true);

                if (ShouldProcess(target: Name, action: string.Format(Resources.CreateNamesapce, Name, ResourceGroupName)))
                {
                    try
                    {
                        PSNamespaceAttributes createresponse = Client.BeginCreateNamespace(resourceGroupName: ResourceGroupName,
                                                                                           namespaceName: Name,
                                                                                           location: Location, 
                                                                                           skuName: SkuName, 
                                                                                           tags: tagDictionary, 
                                                                                           isZoneRedundant: ZoneRedundant.IsPresent,
                                                                                           isDisableLocalAuth: DisableLocalAuth.IsPresent,
                                                                                           identityType: IdentityType, 
                                                                                           identityIds: IdentityId, 
                                                                                           encryptionconfigs: EncryptionConfig, 
                                                                                           skuCapacity: SkuCapacity);
                        WriteObject(createresponse);
                    }
                    catch (ErrorResponseException ex)
                    {
                        WriteError(ServiceBusClient.WriteErrorforBadrequest(ex));
                    }
                }
            }
            catch(Exception ex)
            {
                WriteError(new ErrorRecord(ex, ex.Message, ErrorCategory.OpenError, ex));
            }
        }
    }
}
