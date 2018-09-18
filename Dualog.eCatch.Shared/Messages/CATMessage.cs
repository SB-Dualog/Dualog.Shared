using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    public class CATMessage : Message
    {
        public string CatchArea { get; }
        public IReadOnlyList<FishFAOAndWeight> CatchSummarized { get; }
        public string Zone { get; }
        public int FishingDaysTotal { get; }
        public CATMessage(DateTime sent, string catchArea, IReadOnlyList<FishFAOAndWeight> catchSummarized, int fishingDaysTotal, string zone, string skipperName, Ship ship) : base(MessageType.CAT, sent, skipperName, ship)
        {
            CatchArea = catchArea;
            CatchSummarized = catchSummarized;
            Zone = zone;
            FishingDaysTotal = fishingDaysTotal;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//CA/{CatchSummarized.ToNAF()}");
            sb.Append($"//RA/{CatchArea}");
            sb.Append($"//DF/{FishingDaysTotal}");
        }

        public static CATMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new CATMessage(
                sent,
                values["RA"],
                MessageParsing.ParseFishWeights(values["CA"]),
                Convert.ToInt32(values["DF"]),
                values.ContainsKey("ZO") ? values["ZO"] : string.Empty, 
                values["MA"],
                new Ship(values["NA"], values["RC"], values["XR"]))
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
