using System;
using System.Collections.Generic;
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
            message.SamplesToDeliver.Should().BeEquivalentTo(new List<HiSample>()
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
            message.SamplesToDeliver.Should().BeEquivalentTo(new List<HiSample>()
            {
                sample1, sample2, sample3
            });
        }

        [Fact]
        public void ParseHiaFromeFangst()
        {
            var message = MessageFactory.Parse("//SR//TM/HIA//AD/NOR//DA/20200924//TI/1757//RC/RCSIG72//RN/192//FT/ZZH//SQ/74//NA/HI-Grim//MA/Torgrim Bakke//PO/DKASN//ZD/20200924//ZT/1757//PD/20200924//PT/1757//LA/N6943//LO/E01859//AC/FIS//DS/HER//OB/HER 1900000//ER//");
            Assert.True(message.MessageType == Enums.MessageType.HIA);
        }

        [Fact]
        public void ParseHilFromeFangstWithEmptyPOandLS()
        {
            var message = MessageFactory.Parse("//SR//TM/HIL//AD/NOR//DA/20201106//TI/1334//RC/MCFC01//XR/F-0055-BD//RN/418//FT/ZZH//SQ/170//NA/Jonasguten//MA/Janthima Karlsen//PO///PD/20201106//PT/1334//LS///SH/MCFC01-390-162 N//ER//");
            Assert.True(message.MessageType == Enums.MessageType.HIL);
        }
    }
}