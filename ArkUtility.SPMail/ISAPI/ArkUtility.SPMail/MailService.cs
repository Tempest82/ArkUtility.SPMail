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
using System.Collections.Generic;
using System.ServiceModel.Activation;
using ArkUtility.SPMail.Infrastructure;


namespace ArkUtility.SPMail
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MailService : IMailService
    {
        public string GetInstalledFullName()
        {
            UnifiedLogging.WriteLog(UnifiedLogging.Category.Information, "ArkUtility.SPMail",
                $"ArkUtility.SPMail Installation Verified via GetInstalledFullName() .");
            return System.Reflection.Assembly.GetExecutingAssembly().FullName;
        }

        public SendResponse Send(EmailMessage emailMessage)
        {
            if (emailMessage == null)
            {
                UnifiedLogging.WriteLog(UnifiedLogging.Category.Unexpected, "ArkUtility.SPMail",
                    $"ArkUtility.SPMail - Failure: Unrecognized JSON or No Message provided.");
                return new SendResponse()
                {
                    Successful = false,
                    Message = "Unrecognized JSON or No Message provided.",
                    Status = "Failed"
                };
            }
            if (emailMessage.LinkedResources == null)
                emailMessage.LinkedResources = new List<System.Net.Mail.LinkedResource>();
            return emailMessage.Send();
        }

        public string License()
        {
            UnifiedLogging.WriteLog(UnifiedLogging.Category.Information, "ArkUtility.SPMail",
                $"ArkUtility.SPMail License retrieved via License() .");
            return Infrastructure.License.GetFullLicenseNotice();
        }
    }
}
