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
using ArkUtility.SPMail.Infrastructure;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ArkUtility.SPMail
{
    [ServiceContract]
    interface IMailService
    {
        /// <summary>
        /// Accepts An Email Message attempts to send and returns a response.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Send",
            RequestFormat =WebMessageFormat.Json,
            ResponseFormat =WebMessageFormat.Json,
            Method ="POST")]
        SendResponse Send(EmailMessage emailMessage);
        /// <summary>
        /// Gets Installed Version
        /// </summary>
        /// <returns>
        /// JSON Object with version
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetInstalledFullName", ResponseFormat = WebMessageFormat.Json)]
        string GetInstalledFullName();
        /// <summary>
        /// Gets License Notice
        /// </summary>
        /// <returns>
        /// JSON Object with Gets License Notice
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "License", ResponseFormat = WebMessageFormat.Json)]
        string License();
    }

}
