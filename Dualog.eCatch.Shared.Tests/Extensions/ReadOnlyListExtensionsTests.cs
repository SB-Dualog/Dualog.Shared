using System.Collections.Generic;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests.Extensions
{
    public class ReadOnlyListExtensionsTests
    {

        [Fact]
        public void ToNAF_should_return_correct_sample_format()
        {
            IReadOnlyList<HiSample> samples = new List<HiSample>()
            {
                new HiSample("RADIO50", 40, 1, "N"),
                new HiSample("RADIO50", 45, 5, "Y"),
                new HiSample("RADIO50", 47, 8, "Y"),
            };

            samples.ToNAF().ShouldBeEquivalentTo("RADIO50-40-1 N RADIO50-45-5 Y RADIO50-47-8 Y");
        }
    }
}
