using System.Collections.Generic;
using System.Linq;

namespace Dualog.eCatch.Shared.Models
{
    public class CastReportLine
    {
        public int Number { get; }
        public Cast Cast { get; }
        public SortedSet<FishFAOAndWeight> Catch { get; }

        public int TotalWeight => Catch.Select(c => c.Weight).Sum();

        public CastReportLine(int number, Cast cast, Dictionary<string, int> fishWeight)
        {
            Number = number;
            Cast = cast;
            
            var query =
                from pair in fishWeight
                select new FishFAOAndWeight(pair.Key, pair.Value);

            Catch = new SortedSet<FishFAOAndWeight>(query);
        }
    }
}
