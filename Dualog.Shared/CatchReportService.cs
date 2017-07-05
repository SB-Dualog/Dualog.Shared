using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.Shared.Extensions;
using Dualog.Shared.Messages;
using Dualog.Shared.Models;

namespace Dualog.Shared
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

            var casts = messages.SelectMany(m => m.Casts).Where(c => c.StopTime.Date >= from && c.StopTime.Date <= to);

            var species = new HashSet<string>(from cast in casts
                                              from fish in cast.FishDistribution
                                              select fish.FAOCode);

            var groupedCasts =
                from cast in casts
                group cast by cast.StopTime.Date into g
                select new { Date = g.Key, Casts = g };

            var catchReportLines = new List<CatchReportLine>();
            foreach (var castGroup in groupedCasts)
            {
                var dict = new Dictionary<string, int>();
                foreach (var cast in castGroup.Casts)
                {
                    foreach (var fish in cast.FishDistribution)
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
                catchReportLines.Add(new CatchReportLine(castGroup.Date, dict));
            }

            return new CatchReport(ship ?? messages.First().Ship, catchReportLines.OrderBy(x => x.Date));
        }
    }
}
