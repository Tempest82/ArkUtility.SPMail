/*This file/service/feature is part of ArkUtility SPMail.

ArkUtility SPMail is free software: you can redistribute it and / or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ArkUtility SPMail is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with ArkUtility SPMail.If not, see<http://www.gnu.org/licenses/>.
 */
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ArkUtility.SPMail.Infrastructure
{
    public class CustomMultipleBaseAddressWebServiceHostFactory : Microsoft.SharePoint.Client.Services.MultipleBaseAddressWebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new CustomMultipleBaseServiceHost(serviceType, baseAddresses);
        }
    }
    public class CustomMultipleBaseServiceHost : Microsoft.SharePoint.Client.Services.MultipleBaseAddressWebServiceHost
    {
        public CustomMultipleBaseServiceHost(Type serviceType, params Uri[] baseAddresses) 
            : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            foreach(ServiceEndpoint endpoint in this.Description.Endpoints)
            {
                EnableHelpPageOnWebHttpBehavior(endpoint);
                IncreaseFileUploadSize(endpoint);
            }
        }

        private void EnableHelpPageOnWebHttpBehavior(ServiceEndpoint endpoint)
        {
            foreach(var webHttpBehavior in endpoint.EndpointBehaviors.OfType<WebHttpBehavior>())
            {
                webHttpBehavior.HelpEnabled = true;
            }
            foreach (var webHttpBehavior in endpoint.EndpointBehaviors.OfType<WebHttpBehavior>())
            {
                webHttpBehavior.HelpEnabled = true;
            }
        }

        private void IncreaseFileUploadSize(ServiceEndpoint endpoint)
        {
            var customBinding = endpoint.Binding as WebHttpBinding;
            if (customBinding != null)
            {
                customBinding.MaxBufferSize = Int32.MaxValue;
                customBinding.MaxReceivedMessageSize = Int32.MaxValue;
            }
        }
    }
}
