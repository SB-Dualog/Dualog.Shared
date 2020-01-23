using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests
{
    public class GeoAngleTests
    {
        [Theory]
        [InlineData(1.01, 1, 0, 36)]
        [InlineData(1.3, 1, 18, 0)]
        [InlineData(45.15, 45, 9, 0)]
        [InlineData(45.167, 45, 10, 1.2)]
        [InlineData(89.9999833333333, 89, 59, 59.94)]
        [InlineData(70.051326, 70, 3, 4.774)]
        [InlineData(24.971782, 24, 58, 18.415)]

        public void FromDouble_should_display_correct_minute_value(double angleInDegrees, int expectedDegrees, int expectedMinutes, double expectedSeconds)
        {
            var geoAngle = GeoAngle.FromDouble(angleInDegrees);
            Assert.Equal(expectedDegrees, geoAngle.Degrees);
            Assert.Equal(expectedMinutes, geoAngle.Minutes);
            Assert.Equal(expectedSeconds, geoAngle.Seconds);
        }

        [Theory]
        [InlineData(70.051326, 24.971782, "70° 03' 04.774\" N", "24° 58' 18.415\" E") ]
        public void ToString_should_display_correct_value(double latInDegrees, double lonInDegrees, string expectedLat, string expectedLon)
        {
            var geoAngleLatString = GeoAngle.FromDouble(latInDegrees).ToString("NS");
            var geoAngleLonString = GeoAngle.FromDouble(lonInDegrees).ToString("WE");
            
            geoAngleLatString.Should().Be(expectedLat);
            geoAngleLonString.Should().Be(expectedLon);
        }
    }
}
