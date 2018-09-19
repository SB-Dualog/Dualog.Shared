using System;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Messages;
using Dualog.eCatch.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests.MessageTests
{
    public class DcaMessageTests
    {
        private Ship _ship => new Ship("Ship1", "RC1", "REG1");

        [Fact]
        public void Calculate_duration_should_only_care_about_minutes_case1()
        {
            var startTime = new DateTime(2017, 1, 1, 20, 48, 50);
            var endTime = new DateTime(2017, 1, 2, 0, 0, 5);

            //This is a 191 minute and 15 second duration
            (endTime - startTime).TotalMinutes.Should().Be(191.25);

            var haul = new Cast(startTime, endTime, 0, 0, 0, 0, "", "", new FishFAOAndWeight[0], 0, 0, 0, "", "",
                new AnimalAndCount[0]);

            var duration = haul.GetDuration();

            duration.Should().Be(192);
        }

        [Fact]
        public void Calculate_duration_should_only_care_about_minutes_case2()
        {
            var startTime = new DateTime(2017, 1, 1, 20, 48, 50);
            var endTime = new DateTime(2017, 1, 1, 20, 49, 5);

            //This is a 15 second duration
            (endTime - startTime).TotalMinutes.Should().Be(0.25);
            (endTime - startTime).TotalSeconds.Should().Be(15);

            var haul = new Cast(startTime, endTime, 0, 0, 0, 0, "", "", new FishFAOAndWeight[0], 0, 0, 0, "", "",
                new AnimalAndCount[0]);

            var duration = haul.GetDuration();

            duration.Should().Be(1); //We expect 1 minute since the time started at minute 48 and ended at minute 49
        }

        [Fact]
        public void Calculate_duration_should_only_care_about_minutes_case3()
        {
            var startTime = new DateTime(2017, 1, 1, 20, 48, 0);
            var endTime = new DateTime(2017, 1, 1, 20, 48, 59);

            //This is a 59 second duration
            (endTime - startTime).TotalSeconds.Should().Be(59);

            var haul = new Cast(startTime, endTime, 0, 0, 0, 0, "", "", new FishFAOAndWeight[0], 0, 0, 0, "", "",
                new AnimalAndCount[0]);

            var duration = haul.GetDuration();

            duration.Should().Be(0); //We expect 0 minutes since the time started at minute 48 and ended at minute 48
        }

        [Fact]
        public void GetSummaryShouldNotContainHaulsIfThereAreNoHauls()
        {
            var dca = new DCAMessage("", "", "NOTOS", new Cast[]{}, DateTime.Now, "Skipper", _ship);

            var summaryDict = dca.GetSummaryDictionary(EcatchLangauge.English);

            summaryDict.Should().NotContainKey("Haul 1");
        }

        [Fact]
        public void GetSummaryShouldContainHaul()
        {
            var haul1 = new Cast(DateTime.Now, DateTime.Now, 0, 0, 0, 0, "", "", new FishFAOAndWeight[0], 0, 0, 0, "", "",
                new AnimalAndCount[0]);
            var dca = new DCAMessage("", "", "NOTOS", new Cast[] {haul1 }, DateTime.Now, "Skipper", _ship);

            var summaryDict = dca.GetSummaryDictionary(EcatchLangauge.English);
            summaryDict.Should().ContainKey("Haul 1");

            var haul2 = new Cast(DateTime.Now, DateTime.Now, 0, 0, 0, 0, "", "", new FishFAOAndWeight[0], 0, 0, 0, "", "",
                new AnimalAndCount[0]);

            var dca2 = new DCAMessage("", "", "NOTOS", new Cast[] { haul1, haul2 }, DateTime.Now, "Skipper", _ship);

            var dca2SummaryDict = dca2.GetSummaryDictionary(EcatchLangauge.English);

            dca2SummaryDict.Should().ContainKey("Haul 1");
            dca2SummaryDict.Should().ContainKey("Haul 2");

        }
    }
}
