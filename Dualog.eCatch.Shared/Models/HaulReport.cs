using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;

namespace Dualog.eCatch.Shared.Models
{
    public class HaulReport
    {
        public Ship Ship { get; }
        public IEnumerable<HaulReportDay> HaulPrDay { get; }
        public SortedSet<FishFAOAndWeight> Totals { get; }
        public int TotalWeight => Totals.Select(c => c.Weight).Sum();

        public HaulReport(Ship ship, IEnumerable<HaulReportDay> haulPrDay)
        {
            Ship = ship;
            HaulPrDay = haulPrDay;

            var dict = new Dictionary<string, int>();
            foreach (var haulDay in haulPrDay)
            {
                foreach (var line in haulDay.Lines)
                {
                    foreach (var @catch in line.Catch)
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
            }

            Totals = new SortedSet<FishFAOAndWeight>(
                from keyValue in dict
                select new FishFAOAndWeight(keyValue.Key, keyValue.Value));
        }


        public string ToHtml(EcatchLangauge lang, bool darkTheme = false, bool excelFormat = false)
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
            sb.AppendFormat("<th>{0}</th>", "Date".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "Haul".Translate(lang));
            sb.AppendLine("<th>Sum</th>");
            foreach (var fish in Totals)
            {
                sb.AppendFormat("<th>{0}</th>", fish.FAOCode.ToFishName(lang));
            }
            sb.AppendFormat("<th>{0}</th>", "StartTime".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "EndTime".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "Duration".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "StartPosition".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "EndPosition".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "Gear".Translate(lang));
            sb.AppendFormat("<th>{0}</th>", "Zone".Translate(lang));
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");

            sb.AppendLine("<tbody>");
            foreach (var day in HaulPrDay.OrderByDescending(x => x.Date))
            {
                var i = 0;
                foreach (var line in day.Lines)
                {
                    sb.AppendLine("<tr>");
                    if (i == 0)
                    {
                        sb.AppendFormat("<td rowspan=\"{0}\">{1:dd.MM.yyyy}</td>", day.Lines.Count(), day.Date);
                    }else if (excelFormat) // We need to include a cell here since our html to excel parser does not understand rowspan
                    {
                        sb.AppendFormat("<td>{0:dd.MM.yyyy}</td>", day.Date);
                    }
                    sb.AppendFormat("<td>{0}</td>", line.Number);
                    sb.AppendFormat("<td class='one-line'>{0}</td>", excelFormat ? line.TotalWeight.ToString() : line.TotalWeight.WithThousandSeparator());
                    foreach (var fish in line.Catch)
                    {
                        sb.AppendFormat("<td class='one-line'>{0}</td>", excelFormat ? fish.Weight.ToString() : fish.Weight.WithThousandSeparator());
                    }
                    sb.AppendFormat("<td>{0:dd.MM.yyyy HH:mm}</td>", line.Haul.StartTime);
                    sb.AppendFormat("<td>{0:dd.MM.yyyy HH:mm}</td>", line.Haul.StopTime);
                    var duration = line.Haul.StopTime - line.Haul.StartTime;
                    var hoursString = Math.Floor(duration.TotalHours);
                    sb.AppendFormat("<td>{0}</td>", hoursString + duration.ToString(@"\:mm"));
                    sb.AppendFormat("<td>{0} {1}</td>", line.Haul.StartLatitude.ToWgs84Format(CoordinateType.Latitude),
                        line.Haul.StartLongitude.ToWgs84Format(CoordinateType.Longitude));
                    sb.AppendFormat("<td>{0} {1}</td>", line.Haul.StopLatitude.ToWgs84Format(CoordinateType.Latitude),
                        line.Haul.StopLongitude.ToWgs84Format(CoordinateType.Longitude));
                    sb.AppendFormat("<td>{0}</td>", line.Haul.Tool.ToToolName(lang));
                    sb.AppendFormat("<td>{0}</td>", line.Haul.Zone.ToZoneName(lang));
                    sb.AppendLine("</tr>");
                    ++i;
                }
            }
            sb.AppendLine("</tbody>");

            sb.AppendLine("<tfoot>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td></td>");
            sb.AppendLine("<td></td>");
            sb.AppendFormat("<td class='one-line'>{0}</td>", excelFormat ? TotalWeight.ToString() : TotalWeight.WithThousandSeparator());
            foreach (var fish in Totals)
            {
                sb.AppendFormat("<td class='one-line'>{0}</td>", excelFormat ? fish.Weight.ToString() : fish.Weight.WithThousandSeparator());
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</tfoot>");

            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }

    public class HaulReportDay
    {
        public DateTime Date { get; }
        public IEnumerable<HaulReportLine> Lines { get; }

        public HaulReportDay(DateTime date, IEnumerable<HaulReportLine> lines)
        {
            Date = date;
            Lines = lines;
        } 
    }        
}
