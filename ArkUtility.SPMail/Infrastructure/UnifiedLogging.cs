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
using Microsoft.SharePoint.Administration;

namespace ArkUtility.SPMail.Infrastructure
{
    /// <summary>
    /// This is a utility class to write to the ULS logs
    /// </summary>
    /// <example>UnifiedLogging.WriteLog(UnifiedLogging.Category.Unexpected, "Place of error", "Error message");</example>
    public class UnifiedLogging : SPDiagnosticsServiceBase
    {
        public static string DiagnosticAreaName = "ArkUtility.SPMail";
        private static UnifiedLogging _Current;
        public static UnifiedLogging Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new UnifiedLogging();
                }

                return _Current;
            }
        }

        public UnifiedLogging() : base("ArkUtility.SPMail Logging Service", SPFarm.Local)
        {

        }

        public enum Category
        {
            Unexpected,
            High,
            Medium,
            Information
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>
        {
            new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
            {
                new SPDiagnosticsCategory("Unexpected", TraceSeverity.Unexpected, EventSeverity.Error),
                new SPDiagnosticsCategory("High", TraceSeverity.High, EventSeverity.Warning),
                new SPDiagnosticsCategory("Medium", TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Information", TraceSeverity.Verbose, EventSeverity.Information)
            })
        };

            return areas;
        }

        public static void WriteLog(Category categoryName, string source, string errorMessage)
        {
            SPDiagnosticsCategory category = UnifiedLogging.Current.Areas[DiagnosticAreaName].Categories[categoryName.ToString()];
            UnifiedLogging.Current.WriteTrace(0, category, category.TraceSeverity, string.Concat(source, ": ", errorMessage));
        }
    }
}
