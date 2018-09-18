using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Messages;
using Dualog.eCatch.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests
{
    public class ReportServiceTest
    {
        private readonly DCAMessage[] messages;

        public ReportServiceTest()
        {
            var dcaTemplate = "//SR//TM/DCA//RN/24//MV/1//AD/NOR//RC/ZXXZ//NA/MyShip//XR/B-01-KV//MA/Skipper Joe//DA/{0}//TI/0930//QI/1//AC/FIS//TS//BD/{0}//BT/0929//ZO/NOR//LT/57//LG/52//GE/PT//GP/2//XT/57//XG/52//DU/1//CA/{1}//ER//";

            var today = DateTime.Today.ToFormattedDate();
            var yesterday = DateTime.Today.AddDays(-1).ToFormattedDate();

            var dca1 = MessageFactory.Parse<DCAMessage>(string.Format(dcaTemplate, today, "FISHA 10 FISHB 20"));
            var dca2 = MessageFactory.Parse<DCAMessage>(string.Format(dcaTemplate, today, "FISHB 10 FISHC 30"));
            var dca3 = MessageFactory.Parse<DCAMessage>(string.Format(dcaTemplate, today, "FISHD 15 FISHE 5"));
            var dca4 = MessageFactory.Parse<DCAMessage>(string.Format(dcaTemplate, yesterday, "FISHA 30 FISHF 5"));

            messages = new[] { dca1, dca2, dca3, dca4 };
        }
        [Fact]
        public void Can_calculate_catch_report_based_on_DCA_messages()
        {
            var result = CatchReportService.CreateReport(messages);
            var todayResult = result.Lines.ElementAt(1);
            var yesterdayResult = result.Lines.ElementAt(0);

            Assert.Equal("MyShip", result.Ship.Name);

            Assert.Equal(40, result.Totals.ElementAt(0).Weight);
            Assert.Equal(30, result.Totals.ElementAt(1).Weight);

            Assert.Equal(90, todayResult.TotalWeight);
            Assert.Equal(10, todayResult.Catch.ElementAt(0).Weight);
            Assert.Equal(30, todayResult.Catch.ElementAt(1).Weight);
            Assert.Equal(30, todayResult.Catch.ElementAt(2).Weight);
            Assert.Equal(15, todayResult.Catch.ElementAt(3).Weight);
            Assert.Equal(5, todayResult.Catch.ElementAt(4).Weight);
            Assert.Equal(0, todayResult.Catch.ElementAt(5).Weight);

            Assert.Equal(35, yesterdayResult.TotalWeight);
            Assert.Equal(30, yesterdayResult.Catch.ElementAt(0).Weight);
            Assert.Equal(0, yesterdayResult.Catch.ElementAt(1).Weight);
            Assert.Equal(0, yesterdayResult.Catch.ElementAt(2).Weight);
            Assert.Equal(0, yesterdayResult.Catch.ElementAt(3).Weight);
            Assert.Equal(0, yesterdayResult.Catch.ElementAt(4).Weight);
            Assert.Equal(5, yesterdayResult.Catch.ElementAt(5).Weight);
        }

        [Fact]
        public void Can_calculate_catch_report_based_on_DCA_messages_for_a_given_date_intervall()
        {
            var result = CatchReportService.CreateReport(messages, DateTime.Today, DateTime.Today);
            Assert.Single(result.Lines);
        }

        [Fact]
        public void Can_transform_catch_report_to_HTML()
        {
            var html = CatchReportService.CreateReport(messages, DateTime.Today, DateTime.Today).ToHtml(EcatchLangauge.Norwegian);
            Assert.StartsWith("<!doctype html>", html);
        }

        [Fact]
        public void Can_calculate_cast_report_based_on_DCA_messages()
        {
            var result = CastReportService.CreateReport(messages);
            Assert.NotNull(result);
            Assert.Equal("MyShip", result.Ship.Name);

            Assert.Equal(40, result.Totals.ElementAt(0).Weight);
            Assert.Equal(30, result.Totals.ElementAt(1).Weight);

            result.CastPrDay.ElementAt(0).Lines.Count().Should().Be(1);
            result.CastPrDay.ElementAt(1).Lines.Count().Should().Be(3);
        }

        [Fact]
        public void Can_calculate_cast_report_based_on_DCA_messages_for_a_given_date_intervall()
        {
            var result = CastReportService.CreateReport(messages, DateTime.Today, DateTime.Today);
            Assert.Single(result.CastPrDay);
        }

        [Fact]
        public void Can_transform_cast_report_to_HTML()
        {
            var html = CastReportService.CreateReport(messages, DateTime.Today, DateTime.Today).ToHtml(EcatchLangauge.Norwegian);
            Assert.StartsWith("<!doctype html>", html);
        }

        [Fact]
        public void Can_keep_fish_and_weight_in_sorted_set()
        {
            var set = new SortedSet<FishFAOAndWeight>();

            var fish1 = new FishFAOAndWeight("B", 20);
            var fish2 = new FishFAOAndWeight("A", 10);
            var fish3 = new FishFAOAndWeight("C", 40);

            set.Add(fish1);
            set.Add(fish2);
            set.Add(fish3);

            Assert.Equal("A", set.ElementAt(0).FAOCode);
            Assert.Equal("B", set.ElementAt(1).FAOCode);
            Assert.Equal("C", set.ElementAt(2).FAOCode);
        }
    }
}
