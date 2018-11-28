using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Messages;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared
{
    public static class HaulReportService
    {

        public static HaulReport CreateReportForShip(IEnumerable<DCAMessage> messages, Ship ship)
        {
            return CreateReport(messages, DateTime.MinValue, DateTime.MaxValue, ship);
        }
        public static HaulReport CreateReport(IEnumerable<DCAMessage> messages)
        {
            return CreateReport(messages, DateTime.MinValue, DateTime.MaxValue);
        }

        public static HaulReport CreateReport(IEnumerable<DCAMessage> messages, DateTime from, DateTime to, Ship ship = null)
        {
            if (messages.IsEmpty())
            {
                throw new ArgumentException("No DCA messages. You need minimum one to generate haul report.");
            }

            var hauls = messages.SelectMany(m => m.Hauls).Where(c => c.StopTime.Date >= from && c.StopTime.Date <= to);

            var species = new HashSet<string>(from haul in hauls
                from fish in haul.FishDistribution
                select fish.FAOCode);

            var groupedHauls = 
                from haul in hauls
                group haul by haul.StopTime.Date into g
                select new {Date = g.Key, Hauls = g};

            var haulReportDays = new List<HaulReportDay>();

            foreach (var group in groupedHauls)
            {
                var haulNumber = 1;
                var haulLines = new List<HaulReportLine>();
                foreach (var haul in group.Hauls.OrderBy(x => x.StartTime))
                {
                    var dict = haul.FishDistribution.ToDictionary(fish => fish.FAOCode, fish => fish.Weight);
                    foreach (var s in species.Where(s => !dict.ContainsKey(s)))
                    {
                        dict.Add(s, 0);
                    }
                    haulLines.Add(new HaulReportLine(haulNumber, haul, dict));
                    haulNumber++;
                }

                haulReportDays.Add(new HaulReportDay(group.Date, haulLines));
            }
                      
            return new HaulReport(ship ?? messages.First().Ship, haulReportDays.OrderBy(x => x.Date));
        }
    }
}
