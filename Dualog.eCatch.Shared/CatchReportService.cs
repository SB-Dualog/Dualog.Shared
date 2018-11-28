using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Messages;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared
{
    public static class CatchReportService
    {
        public static CatchReport CreateReportForShip(IEnumerable<DCAMessage> messages, Ship ship)
        {
            return CreateReport(messages, DateTime.MinValue, DateTime.MaxValue, ship);
        }
        public static CatchReport CreateReport(IEnumerable<DCAMessage> messages)
        {
            return CreateReport(messages, DateTime.MinValue, DateTime.MaxValue);
        }

        public static CatchReport CreateReport(IEnumerable<DCAMessage> messages, DateTime from, DateTime to, Ship ship = null)
        {
            if (messages.IsEmpty())
            {
                throw new ArgumentException("No DCA messages. You need minimum one to generate catch report.");
            }

            var hauls = messages.SelectMany(m => m.Hauls).Where(c => c.StopTime.Date >= from && c.StopTime.Date <= to);

            var species = new HashSet<string>(from haul in hauls
                                              from fish in haul.FishDistribution
                                              select fish.FAOCode);

            var groupedHauls =
                from haul in hauls
                group haul by haul.StopTime.Date into g
                select new { Date = g.Key, Hauls = g };

            var catchReportLines = new List<CatchReportLine>();
            foreach (var haulGroup in groupedHauls)
            {
                var dict = new Dictionary<string, int>();
                foreach (var haul in haulGroup.Hauls)
                {
                    foreach (var fish in haul.FishDistribution)
                    {
                        if (dict.ContainsKey(fish.FAOCode))
                        {
                            dict[fish.FAOCode] += fish.Weight;
                        }
                        else
                        {
                            dict.Add(fish.FAOCode, fish.Weight);
                        }
                    }

                    foreach (var s in species.Where(s => !dict.ContainsKey(s)))
                    {
                        dict.Add(s, 0);
                    }                    
                }
                catchReportLines.Add(new CatchReportLine(haulGroup.Date, dict));
            }

            return new CatchReport(ship ?? messages.First().Ship, catchReportLines.OrderBy(x => x.Date));
        }
    }
}
