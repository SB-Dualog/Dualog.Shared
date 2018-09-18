using System;

namespace Dualog.eCatch.Shared.Models
{
    public class FishLanding
    {
        public string FishSpecies { get; }
        public int Weight { get; }
        public string Conservation { get; }
        public string Condition { get; }
        public string UnitType { get; }
        public string Zone { get; }
        public string CatchArea { get; }
        public int NumberOfUnits { get; }
        public int UnitAverageWeight { get; }

        public FishLanding(
            string fishSpecies,
            int weight,
            int numberOfUnits,
            string conservation,
            string condition,
            string unitType,
            string zone,
            string catchArea)
        {
            FishSpecies = fishSpecies;
            Weight = weight;
            NumberOfUnits = numberOfUnits;
            Conservation = conservation;
            Condition = condition;
            UnitType = unitType;
            Zone = zone;
            CatchArea = catchArea;

            UnitAverageWeight = NumberOfUnits > 0 ? (int)Math.Round((double)Weight/NumberOfUnits) : 0;
        }
    }
}
