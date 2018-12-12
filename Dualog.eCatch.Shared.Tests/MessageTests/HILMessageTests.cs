using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Messages.HiSampling;
using Dualog.eCatch.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Dualog.eCatch.Shared.Tests.MessageTests
{
    public class HILMessageTests
    {
        [Fact]
        public void ParseNAFFormat_should_support_old_format()
        {
            var old_format =
                "//SR//TM/HIL//RN/281//FT/ZZH//SQ/69//AD/NOR//RC/mcb8//NA/boaty mcboatface//XR/FF3333KK//MA/Dag Fisk//DA/20181128//TI/1453//PO/NOAAA//PD/20181128//PT/1553//LS/iiiii//MS/mcb8-279-68,mcb8-279-68,mcb8-279-68//ER//";
            var dict = MessageFactory.ParseNAFToDictionary(old_format);
            var message = HILMessage.ParseNAFFormat(1, new DateTime(), dict);
            message.SamplesToDeliver.ShouldBeEquivalentTo(new List<HiSample>()
            {

            });
        }       

        [Fact]
        public void ParseNAFFormat_should_support_current_format()
        {
            var old_format =
                "//SR//TM/HIL//RN/281//FT/ZZH//SQ/69//AD/NOR//RC/mcb8//NA/boaty mcboatface//XR/FF3333KK//MA/Dag Fisk//DA/20181128//TI/1453//PO/NOAAA//PD/20181128//PT/1553//LS/iiiii//MS//" +
                "/SH/mcb8-279-68 N mcb8-280-69 Y mcb8-281-70 Y//ER//";
            var dict = MessageFactory.ParseNAFToDictionary(old_format);
            var message = HILMessage.ParseNAFFormat(1, new DateTime(), dict);

            var sample1 = new HiSample("mcb8", 279, 68);
            sample1.SetNotTaken();
            var sample2 = new HiSample("mcb8", 280, 69);
            sample2.SetTaken();
            var sample3 = new HiSample("mcb8", 281, 70);
            sample3.SetTaken();
            message.SamplesToDeliver.ShouldBeEquivalentTo(new List<HiSample>()
            {
                sample1, sample2, sample3
            });
        }
    }
}
