using System.Collections.Generic;
using System.Linq;

namespace Dualog.eCatch.Shared.Models
{
    public class HaulReportLine
    {
        public int Number { get; }
        public Haul Haul { get; }
        public SortedSet<FishFAOAndWeight> Catch { get; }

        public int TotalWeight => Catch.Select(c => c.Weight).Sum();

        public HaulReportLine(int number, Haul haul, Dictionary<string, int> fishWeight)
        {
            Number = number;
            Haul = haul;
            
            var query =
                from pair in fishWeight
                select new FishFAOAndWeight(pair.Key, pair.Value);

            Catch = new SortedSet<FishFAOAndWeight>(query);
        }
    }
}
