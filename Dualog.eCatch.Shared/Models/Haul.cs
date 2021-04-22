using System;
using System.Collections.Generic;

namespace Dualog.eCatch.Shared.Models
{
    public class Haul
    {
        public DateTime StartTime { get; }
        public DateTime StopTime { get; }
        public double StartLatitude { get; }
        public double StartLongitude { get; }
        public double StopLatitude { get; }
        public double StopLongitude { get; }
        public string RouteNumber { get; }
        public string Tool { get; }
        public string Problem { get; }
        public IReadOnlyList<FishFAOAndWeight> FishDistribution { get; }
        public int MaskWidth { get; }
        public int NumberOfTrawls { get; }
        public int ExtraToolInfo { get; }
        public string Zone { get; }
        public string HerringType { get; }
        public IReadOnlyList<AnimalAndCount> AnimalCount { get; } 

        public Haul(DateTime startTime, DateTime stopTime, double startLatitude, double startLongitude, double stopLatitude, double stopLongitude, string tool, string problem, IReadOnlyList<FishFAOAndWeight> fishDistribution, int maskWidth, int numberOfTrawls, int extraToolInfo, string zone, string herringType, IReadOnlyList<AnimalAndCount> animalCount)
        {
            StartTime = startTime;
            StopTime = stopTime;
            StartLatitude = startLatitude;
            StartLongitude = startLongitude;
            StopLatitude = stopLatitude;
            StopLongitude = stopLongitude;
            Tool = tool;
            Problem = problem;
            FishDistribution = fishDistribution;
            MaskWidth = maskWidth;
            NumberOfTrawls = numberOfTrawls;
            ExtraToolInfo = extraToolInfo;
            Zone = zone;
            HerringType = herringType;
            AnimalCount = animalCount;
        }

        public int GetDuration()
        {
            var startTimeWithoutSeconds = StartTime.Date + new TimeSpan(StartTime.Hour, StartTime.Minute, 0);
            var stopTimeWithoutSeconds = StopTime.Date + new TimeSpan(StopTime.Hour, StopTime.Minute, 0);
            return Convert.ToInt32((stopTimeWithoutSeconds - startTimeWithoutSeconds).TotalMinutes);
        }
    }
}
