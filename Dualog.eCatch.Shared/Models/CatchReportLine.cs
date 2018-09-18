using System;
using System.Collections.Generic;
using System.Linq;

namespace Dualog.eCatch.Shared.Models
{
    public class CatchReportLine
    {
        public DateTime Date { get; }

        public SortedSet<FishFAOAndWeight> Catch { get; }

        public int TotalWeight => Catch.Select(c => c.Weight).Sum();

        public CatchReportLine(DateTime date, Dictionary<string, int> fishWeight)
        {
            Date = date;

            var query = 
                from pair in fishWeight
                select new FishFAOAndWeight(pair.Key, pair.Value);

            Catch = new SortedSet<FishFAOAndWeight>(query);
        } 
    }
}
