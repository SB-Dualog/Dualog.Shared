using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static IReadOnlyList<HiSample> ParseHISamples(string hiSamples)
        {
            var result = new List<HiSample>();
            if (string.IsNullOrEmpty(hiSamples))
            {
                return new ReadOnlyCollection<HiSample>(result);
            }
            hiSamples = hiSamples.TrimStart();

            // parses RADIO50-40-1 N to (RADIO50)-(40)-(1) (N)
            Regex rx = new Regex(@"([A-Z0-9]{4,7})-(\d{1,4})-(\d{1,4})[ ]*([Y|N])[ ]?", 
                RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(hiSamples);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                var radioCallSignal = groups[1].Value;
                var recordNumber = groups[2].Value;
                var sequenceNumber = groups[3].Value;
                var status = groups[4].Value;
                result.Add(
                    new HiSample(
                        radioCallSignal,
                        Convert.ToInt32(recordNumber),
                        Convert.ToInt32(sequenceNumber)
                    )
                    {
                        Status = status
                    });
            }
          
            return new ReadOnlyCollection<HiSample>(result);
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