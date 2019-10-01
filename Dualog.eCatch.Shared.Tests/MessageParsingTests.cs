using System.Collections.Generic;
using Dualog.eCatch.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests
{
    public class MessageParsingTests
    {
        [Fact]
        public void ParseHISamples_should_parse_example_correctly()
        {
            var samples = MessageParsing.ParseHISamples("RADIO50-40-1 N RADIO50-45-5 Y RADIO50-47-8 Y");
            samples.Should().BeEquivalentTo(new List<HiSample>()
            {
                new HiSample("RADIO50", 40, 1, "N"),
                new HiSample("RADIO50", 45, 5, "Y"),
                new HiSample("RADIO50", 47, 8, "Y")
            });
        }
    }
}
