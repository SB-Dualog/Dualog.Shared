using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dualog.Shared.Extensions;

namespace Dualog.Shared.Models
{
    public class CatchReport
    {
        public Ship Ship { get; }
        public IEnumerable<CatchReportLine> Lines { get; }
        public SortedSet<FishFAOAndWeight> Totals { get; }
        public int TotalWeight => Totals.Select(c => c.Weight).Sum();

        public CatchReport(Ship ship, IEnumerable<CatchReportLine> lines)
        {
            Ship = ship;
            Lines = lines;

            var dict = new Dictionary<string, int>();
            foreach(var line in lines)
            {
                foreach(var @catch in line.Catch)
                {
                    if (dict.ContainsKey(@catch.FAOCode))
                    {
                        dict[@catch.FAOCode] += @catch.Weight;
                    }
                    else
                    {
                        dict.Add(@catch.FAOCode, @catch.Weight);
                    }
                }
            }

            Totals = new SortedSet<FishFAOAndWeight>(
                from keyValue in dict
                select new FishFAOAndWeight(keyValue.Key, keyValue.Value));
        }

        public string ToHtml()
        {
            return ToHtml(s => s);
        }

        public string ToHtml(Func<string, string> translate, bool darkTheme = false, bool excelFormat = false, int languageIndex = 1)
        {
            var theme = darkTheme ? "themeDark" : "themeLight";
            var sb = new StringBuilder();
            sb.AppendLine("<!doctype html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style type='text/css'>");
            sb.AppendLine(".themeLight{background: #E8E8E8}.themeDark{background: #3A3A3A; color: white;} td, th{text-align: left; padding:5px; vertical-align:top;} ");
            sb.AppendLine(@"
                th, td {
                    padding: 0.6vh 0.8vw;
                    text-align: center;
                    border: 1px solid rgba(0, 0, 0, 0.04);
                    outline: 1px solid rgba(0, 0, 0, 0.02);
                }

                th {
                    background-color: #009688;
                    color: #FFF;
                    text-transform: uppercase;
                    font-weight: normal;
                }

                tbody tr:nth-of-type(odd) {
                    background-color: rgba(255, 255, 255, 0.8);
                }

                tfoot {
                    background-color: fade(#009688, 10%);
                    color: #424242;
                    font-weight: bold;
                }

                .themeDark tfoot{
                    color: #FFF;
                }

                .one-line{
                    white-space: nowrap;
                    overflow: hidden;
                    text-align: right;
                }

            ");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine($"<body class='{theme}'>");
            sb.AppendLine("<table>");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendFormat("<th>{0}</th>", translate("Date"));
            foreach (var fish in Totals)
            {
                sb.AppendFormat("<th>{0}</th>", fish.FAOCode.ToFishName(languageIndex));
            }
            sb.AppendLine("<th>Sum</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");

            sb.AppendLine("<tbody>");
            foreach (var line in Lines.OrderByDescending(x => x.Date))
            {
                sb.AppendLine("<tr>");
                sb.AppendFormat("<td>{0:dd.MM.yyyy}</td>", line.Date);
                foreach (var fish in line.Catch)
                {
                    sb.AppendFormat("<td class='one-line'>{0}</td>", excelFormat ? fish.Weight.ToString() : fish.Weight.WithThousandSeparator());
                }
                sb.AppendFormat("<td  class='one-line'>{0}</td>", excelFormat ? line.TotalWeight.ToString() : line.TotalWeight.WithThousandSeparator());
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");

            sb.AppendLine("<tfoot>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td></td>");
            foreach (var fish in Totals)
            {
                sb.AppendFormat("<td  class='one-line'>{0}</td>", excelFormat ? fish.Weight.ToString() : fish.Weight.WithThousandSeparator());
            }
            sb.AppendFormat("<td  class='one-line'>{0}</td>", excelFormat ? TotalWeight.ToString() : TotalWeight.WithThousandSeparator());
            sb.AppendLine("</tr>");
            sb.AppendLine("</tfoot>");
            
            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
