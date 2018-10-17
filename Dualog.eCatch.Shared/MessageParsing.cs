using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared
{
    public static class MessageParsing
    {
        private static readonly string[] MammalsAndBirdCodes = { "BPZ", "BQV", "BQZ", "BVZ", "BWC", "BWE", "BWF", "CVV", "DAZ", "DBK", "DIM", "DKI", "DKJ", "EGF", "EGJ", "FNO", "FPA", "GCZ", "GHD", "GHH", "GHI", "GWA", "GWC", "GCG", "HBB", "HBH", "HBO", "HBW", "HFZ", "HHZ", "HVE", "HWA", "ISW", "ISY", "ITV", "LCW", "LHZ", "LOW", "LVU", "LVW", "LVY", "LVZ", "MVB", "PUG", "PFG", "QDT", "SJD", "UQT", "TBW", "TFH", "TVF", "TVH", "TZC", "TZE", "TZF", "TZH", "UIP", "UQU", "WOS", "WOT", "WOW", "WOY", "WYC", "BEL", "BEW", "BLW", "BOW", "BRW", "BWD", "BWW", "CPM", "DCO", "FIW", "KIW", "MAM", "MYS", "ODN", "PIW", "SEB", "SEC", "SEG", "SEH", "SER", "SEZ", "SIW", "SPW", "SXX", "WAL", "SHW", "PHR", "MIW", "TVA"};

        public static IReadOnlyList<FishFAOAndWeight> ParseFishWeights(string fishWeights)
        {
            var result = new List<FishFAOAndWeight>();
            if (string.IsNullOrEmpty(fishWeights))
            {
                return new ReadOnlyCollection<FishFAOAndWeight>(result);
            }
            fishWeights = fishWeights.TrimStart();
            var catchOnBoard = fishWeights.Split(' ');
            for (var i = 0; i < catchOnBoard.Length; i = i + 2)
            {
                if (i + 1 < catchOnBoard.Length)
                {
                    var species = catchOnBoard[i];
                    if(!MammalsAndBirdCodes.Contains(species))
                    result.Add(
                    new FishFAOAndWeight(
                        species,
                        Convert.ToInt32(catchOnBoard[i + 1])));
                }
            }
            return new ReadOnlyCollection<FishFAOAndWeight>(result);
        }

        public static IReadOnlyList<HISample> ParseHISamples(string hiSamples)
        {
            var result = new List<HISample>();
            if (string.IsNullOrEmpty(hiSamples))
            {
                return new ReadOnlyCollection<HISample>(result);
            }
            hiSamples = hiSamples.TrimStart();
            var samples = hiSamples.Split(',');
            foreach (var sample in samples)
            {
                var values = sample.Split('-');
                var radioCallSignal = values[0];
                var recordNumber = values[1];
                var sequenceNumber = values[2];
                result.Add(
                    new HISample(
                        Convert.ToInt32(radioCallSignal),
                        Convert.ToInt32(recordNumber),
                        Convert.ToInt32(sequenceNumber)
                    ));
            }
          
            return new ReadOnlyCollection<HISample>(result);
        }

        public static IReadOnlyList<AnimalAndCount> ParseAnimalCount(string animalCount)
        {
            var result = new List<AnimalAndCount>();
            if (string.IsNullOrEmpty(animalCount))
            {
                return new ReadOnlyCollection<AnimalAndCount>(result);
            }
            animalCount = animalCount.TrimStart();
            var catchOnBoard = animalCount.Split(' ');
            for (var i = 0; i < catchOnBoard.Length; i = i + 2)
            {
                if (i + 1 < catchOnBoard.Length)
                {
                    var animal = catchOnBoard[i];
                    if (MammalsAndBirdCodes.Contains(animal))
                    {
                        result.Add(new AnimalAndCount(animal,
                         Convert.ToInt32(catchOnBoard[i + 1])));
                    }
                    
                }
            }
            return new ReadOnlyCollection<AnimalAndCount>(result);
        } 
    }
}