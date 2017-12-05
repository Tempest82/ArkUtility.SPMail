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
using Microsoft.SharePoint;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace ArkUtility.SPMail.Infrastructure
{
    public static class ImageHelper
    {
        /// <summary>
        /// Returns Html with CID named references and updates email message attachments.
        /// </summary>
        /// <param name="html">Html to parse for images</param>
        /// <param name="emailMessage">Adds Attachments to email messages.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static string GetHtmlWithEmbeddedImages(string html, EmailMessage emailMessage)
        {

            try
            {
                List<string> imgTags = HtmlHelper.GetImagesInHtml(html);
                if (imgTags == null || !imgTags.Any())
                    return html;

                List<string> sources = new List<string>();
                foreach (var tag in imgTags)
                {
                    if (string.IsNullOrWhiteSpace(tag))
                        continue;
                    var imgSrc = HtmlHelper.GetSrcFromImage(tag);
                    if (string.IsNullOrWhiteSpace(imgSrc) || imgSrc.StartsWith("data:") || imgSrc.StartsWith("cid:"))
                        continue;
                    sources.Add(imgSrc);
                }

                sources.RemoveAll(x => string.IsNullOrWhiteSpace(x));
               
                if (emailMessage?.ImageLibraryUrls?.Any() == true)
                {
                    foreach (var additionalImageLibrary in emailMessage.ImageLibraryUrls)
                    {
                        using (SPSite site = new SPSite(additionalImageLibrary))
                        {
                            using (SPWeb web = site.OpenWeb(additionalImageLibrary))
                            {
                                SPList docs = web.GetList(additionalImageLibrary);
                                if (docs?.Items == null || docs.Items.Count == 0)
                                    return html;
                                foreach (SPListItem item in docs.Items)
                                {
                                    if (item.File == null)
                                        continue;
                                    SPFile file = item.File;
                                    var urlEncodedName = Uri.EscapeUriString(file.Name).Replace("'", "%27");
                                    var contentIdCheckName = file.Name.Replace(" ", "%20").Replace("'", "%27");
                                    if (file != null &&
                                        emailMessage.LinkedResources.All(x => contentIdCheckName != x.ContentId)
                                        && sources.Any(x => Path.GetFileName(x)
                                                                .Equals(file.Name,
                                                                    StringComparison.InvariantCultureIgnoreCase)
                                                            || Path.GetFileName(x)
                                                                .Equals(urlEncodedName,
                                                                    StringComparison.InvariantCultureIgnoreCase)))
                                    {
                                        byte[] filecontents = file.OpenBinary();
                                        MemoryStream ms = new MemoryStream(filecontents);
                                        string contentType = MediaTypeNames.Text.Plain;
                                        if (file.Name.EndsWith("jpg") || file.Name.EndsWith("jpeg"))
                                            contentType = MediaTypeNames.Image.Jpeg;
                                        else if (file.Name.EndsWith("png"))
                                            contentType = "image/png";
                                        else if (file.Name.EndsWith("gif"))
                                            contentType = MediaTypeNames.Image.Gif;
                                        else if (file.Name.EndsWith("pdf"))
                                            contentType = MediaTypeNames.Application.Pdf;
                                        var imgRes =
                                            new LinkedResource(ms,
                                                contentType); //Using Linked resource so images do not display as attachments.
                                        imgRes.ContentId = file.Name;
                                        imgRes.ContentId = imgRes.ContentId.Replace(" ", "%20").Replace("'", "%27");
                                        emailMessage.LinkedResources
                                            .Add(imgRes); //Add Linked Item (to be used as CID reference)
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var linkedRes in emailMessage.LinkedResources)
                {
                    List<string> modifiedSources = new List<string>();
                    foreach (var source in sources.Where(x => Path.GetFileName(x).Equals(linkedRes.ContentId, StringComparison.InvariantCultureIgnoreCase)
                                                           || Uri.EscapeUriString(Path.GetFileName(x)).Replace("'", "%27").Equals(linkedRes.ContentId, StringComparison.InvariantCultureIgnoreCase)
                                                           || Path.GetFileName(x).Equals(Uri.EscapeUriString(linkedRes.ContentId).Replace("'", "%27"), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        if (modifiedSources.Any(x => x == source))
                            continue;//don't do the same image more than once
                        html = html.Replace(source, "cid:" + linkedRes.ContentId);//Add CID Reference
                        modifiedSources.Add(source);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Exception occured in ArkUtility.SPMail processing. Specific error: {ex.Message}. Stack: {ex.StackTrace}", ex);
            }
            return html;
        }
    }
}

