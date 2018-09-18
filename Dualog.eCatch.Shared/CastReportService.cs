using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Messages;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared
{
    public static class CastReportService
    {

        public static CastReport CreateReportForShip(IEnumerable<DCAMessage> messages, Ship ship)
        {
            return CreateReport(messages, DateTime.MinValue, DateTime.MaxValue, ship);
        }
        public static CastReport CreateReport(IEnumerable<DCAMessage> messages)
        {
            return CreateReport(messages, DateTime.MinValue, DateTime.MaxValue);
        }

        public static CastReport CreateReport(IEnumerable<DCAMessage> messages, DateTime from, DateTime to, Ship ship = null)
        {
            if (messages.IsEmpty())
            {
                throw new ArgumentException("No DCA messages. You need minimum one to generate cast report.");
            }

            var casts = messages.SelectMany(m => m.Casts).Where(c => c.StopTime.Date >= from && c.StopTime.Date <= to);

            var species = new HashSet<string>(from cast in casts
                from fish in cast.FishDistribution
                select fish.FAOCode);

            var groupedCasts = 
                from cast in casts
                group cast by cast.StopTime.Date into g
                select new {Date = g.Key, Casts = g};

            var castReportDays = new List<CastReportDay>();

            foreach (var group in groupedCasts)
            {
                var castNumber = 1;
                var castLines = new List<CastReportLine>();
                foreach (var cast in group.Casts.OrderBy(x => x.StartTime))
                {
                    var dict = cast.FishDistribution.ToDictionary(fish => fish.FAOCode, fish => fish.Weight);
                    foreach (var s in species.Where(s => !dict.ContainsKey(s)))
                    {
                        dict.Add(s, 0);
                    }
                    castLines.Add(new CastReportLine(castNumber, cast, dict));
                    castNumber++;
                }

                castReportDays.Add(new CastReportDay(group.Date, castLines));
            }
                      
            return new CastReport(ship ?? messages.First().Ship, castReportDays.OrderBy(x => x.Date));
        }
    }
}
