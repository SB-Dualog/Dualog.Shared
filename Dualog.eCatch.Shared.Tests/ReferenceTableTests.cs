using Dualog.eCatch.Shared.Extensions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests
{
    public class ReferenceTableTests
    {
        [Fact]
        public void Can_get_fish_name_from_code()
        {
            Assert.Equal("Akkar", "SQE".ToFishName());
            Assert.Equal("Albakor", "ALB".ToFishName());
        }

        [Fact]
        public void Can_get_tool_name_from_code()
        {
            Assert.Equal("Teiner", "FPO".ToToolName());
        }


        [Fact]
        public void Can_get_zone_name_from_code()
        {
            Assert.Equal("Norway", "NOR".ToZoneName());
        }
    }
}
