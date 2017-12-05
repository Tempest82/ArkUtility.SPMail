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
using System.Text.RegularExpressions;

namespace ArkUtility.SPMail.Infrastructure
{
    public static class HtmlHelper
    {
        public static List<string> GetImagesInHtml(string htmlString)
        {
            List<string> imgs = new List<string>();
            string imgPattern = @"<(img)\b[^>]*>";
            Regex rgx = new Regex(imgPattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);
            for (int i = 0; i < matches.Count; i++)
            {
                imgs.Add(matches[i].Value);
            }
            return imgs;
        }
        public static string GetSrcFromImage(string imgTag)
        {
            string srcPattern = @"(?<=src="")[^""]*(?="")";
            Match match = Regex.Match(imgTag, srcPattern, RegexOptions.IgnoreCase);
            return match.Value;
        }
    }
}
