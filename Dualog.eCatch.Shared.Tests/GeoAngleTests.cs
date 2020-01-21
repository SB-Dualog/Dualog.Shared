using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Models;
using Xunit;

namespace Dualog.eCatch.Shared.Tests
{
    public class GeoAngleTests
    {
        [Theory]
        [InlineData(1.01, 1, 0)]
        [InlineData(1.3, 1, 18)]
        [InlineData(45.15, 45, 9)]
        [InlineData(45.167, 45, 10)]
        [InlineData(89.9999833333333, 89, 59)]
        public void FromDouble_should_display_correct_values(double angleInDegrees, int expectedDegrees, int expectedMinutes)
        {
            var geoAngle = GeoAngle.FromDouble(angleInDegrees);
            Assert.Equal(expectedDegrees, geoAngle.Degrees);
            Assert.Equal(expectedMinutes, geoAngle.Minutes);
        }
    }
}
