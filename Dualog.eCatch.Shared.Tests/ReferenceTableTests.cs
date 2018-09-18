using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests
{
    public class ReferenceTableTests
    {
        [Fact]
        public void Can_get_fish_name_from_code()
        {
            Assert.Equal("Akkar", "SQE".ToFishName(EcatchLangauge.Norwegian));
            Assert.Equal("Albakor", "ALB".ToFishName(EcatchLangauge.Norwegian));
        }

        [Fact]
        public void Can_get_tool_name_from_code()
        {
            Assert.Equal("Teiner", "FPO".ToToolName(EcatchLangauge.Norwegian));
        }


        [Fact]
        public void Can_get_zone_name_from_code()
        {
            Assert.Equal("Norway", "NOR".ToZoneName(EcatchLangauge.English));
            Assert.Equal("Norge", "NOR".ToZoneName(EcatchLangauge.Norwegian));
        }
    }
}
