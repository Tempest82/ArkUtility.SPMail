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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Net.Mail;
using Microsoft.SharePoint;
using System.Web;

namespace ArkUtility.SPMail.Infrastructure
{
    [Serializable]
    [DataContract]
    public class EmailMessage
    {
        public EmailMessage()
        {
            ShouldEmbedImages = true;
            ShouldWrapBodyWithHtml = true;
            LinkedResources = new List<LinkedResource>();
            ImageLibraryUrls = new string[0];
        }
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Cc { get; set; }
        [DataMember]
        public string Bcc { get; set; }
        [DataMember]
        public bool ShouldEmbedImages { get; set; }
        [DataMember]
        public bool ShouldWrapBodyWithHtml { get; set; }
        [DataMember]
        public string MessageSubject { get; set; }
        [DataMember]
        public string MessageBody { get; set; }
        [DataMember]
        public string[] ImageLibraryUrls { get; set; }
        [IgnoreDataMember]
        public List<LinkedResource> LinkedResources { get; set; }
        [DataMember]
        public string LogListUrl { get; set; }
        public string GetMessageBody()
        {
            if (ShouldWrapBodyWithHtml)
            {
                return $"<html><body>{GetMessageBody(ShouldEmbedImages)}\r\n</body></html>";
            }
            else
            {
                return GetMessageBody(ShouldEmbedImages);
            }

        }
        public string GetMessageSubject()
        {
            return MessageSubject;
        }
        public void SetMessageFrom()
        {

            if (string.IsNullOrWhiteSpace(From))
                From = SPContext.Current.Web.Site.WebApplication.OutboundMailReplyToAddress;
            From = From?.Trim().TrimEnd(',');
        }
        public void SetMessageTo()
        {
            To = To?.Trim().TrimEnd(',');
        }
        public void SetMessageCc()
        {
            Cc = Cc?.Trim().TrimEnd(',');
        }
        public void SetMessageBcc()
        {
            Bcc = Bcc?.Trim().TrimEnd(',');
        }
        public string GetMessageBody(bool embedImages)
        {
            if (!embedImages)
                return MessageBody;

            return ImageHelper.GetHtmlWithEmbeddedImages(MessageBody, this);
        }
        public SendResponse Send()
        {
            SendResponse result = new SendResponse();
            var smtpServer = SPContext.Current.Web.Site.WebApplication.OutboundMailServiceInstance.Server.Address;
            SetMessageFrom();
            var localServerName = HttpContext.Current?.Server?.MachineName;
            var userName = HttpContext.Current?.User?.Identity?.Name;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SetMessageFrom();
                    SetMessageTo();
                    SetMessageCc();
                    SetMessageBcc();

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(From);
                    if (!string.IsNullOrWhiteSpace(To))
                        mail.To.Add(To);
                    if (!string.IsNullOrWhiteSpace(Cc))
                        mail.CC.Add(Cc);
                    if (!string.IsNullOrWhiteSpace(Bcc))
                        mail.Bcc.Add(Bcc);
                    mail.Subject = GetMessageSubject();
                    mail.IsBodyHtml = true;
                    mail.Body = GetMessageBody();
                    var htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                    if (LinkedResources != null && LinkedResources.Any())
                    {
                        foreach (var res in LinkedResources)
                        {
                            htmlView.LinkedResources.Add(res);
                        }
                    }
                    mail.AlternateViews.Add(htmlView);
                    //var smtpServer = SPContext.Current.Web.Site.WebApplication.OutboundMailServiceInstance.Server.Address;
                    SmtpClient smtp = new SmtpClient(smtpServer);
                    smtp.Send(mail);
                    result.Successful = true;
                    result.Status = "Success";
                    result.Message = "Success";
                });
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Status = "Failed";
                result.Message = $"{ex.Message} \r\n{ex.StackTrace}\r\n{ex.InnerException?.Message}\r\n{ex.InnerException?.StackTrace} - {ex.InnerException?.InnerException?.Message} - {ex.InnerException?.InnerException?.StackTrace} ";
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(LogListUrl))
                {
                    if (result.Successful)
                        UnifiedLogging.WriteLog(UnifiedLogging.Category.Information, "ArkUtility.SPMail", "ArkUtility.SPMail - EmailMessage.Send() Success");
                    else
                        UnifiedLogging.WriteLog(UnifiedLogging.Category.Unexpected, "ArkUtility.SPMail", $"ArkUtility.SPMail - EmailMessage.Send() Failure: {result.Message} \r\nFrom: {From}\r\n To:{To}\r\n CC:{Cc}\r\nSubject: { GetMessageSubject()}\r\nRequested by User: {userName}");
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(LogListUrl))
                        {
                            using (SPWeb web = site.OpenWeb(LogListUrl))
                            {
                                SPList list = web.GetList(LogListUrl);
                                web.AllowUnsafeUpdates = true;
                                if (!list.Fields.ContainsField("EmailResult"))
                                {
                                    list.Fields.Add("EmailResult", SPFieldType.Note, false);
                                    list.Update();
                                }
                                var listItems = list.Items;
                                var logentry = listItems.Add();
                                if (list.Fields.ContainsField("Title"))
                                    logentry["Title"] = $"Email Sent {DateTime.UtcNow.ToString("u")} - { (result.Successful ? "Successful" : "Failed")}";
                                logentry["EmailResult"] = $"{ (result.Successful ? "Successful" : "Failed")}\r\n Status: {result.Status}\r\n{result.Message}\r\nSubject: { GetMessageSubject()}\r\nFrom: {From}\r\n To:{To}\r\n CC:{Cc}\r\nRequested by User: {userName}\r\nLogged:{DateTime.UtcNow.ToString("u")} from {localServerName}";
                                logentry.Update();
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    });

                }
            }
            catch (Exception ex)
            {
                string msg = $"ArkUtility.SPMail has encountered an error during logging. {ex.Message} \r\nInner Error:{ex.InnerException?.Message} \r\n\r\n{ex.InnerException?.InnerException?.Message}";
                System.Diagnostics.Trace.WriteLine(msg, "Diagnostics");
                UnifiedLogging.WriteLog(UnifiedLogging.Category.Unexpected, "ArkUtility.SPMail", msg);

            }

            return result;
        }

    }
}
