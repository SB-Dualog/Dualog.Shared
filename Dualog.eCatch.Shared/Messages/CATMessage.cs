using System;
using System.Collections.Generic;
using System.Linq;
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
        public string FishingLicense { get; }
        public CATMessage(
            DateTime sent, 
            string catchArea, 
            IReadOnlyList<FishFAOAndWeight> catchSummarized, 
            int fishingDaysTotal, 
            string zone, 
            string skipperName, 
            Ship ship,
            string fishingLicense = "") : base(MessageType.CAT, sent, skipperName, ship)
        {
            CatchArea = catchArea;
            CatchSummarized = catchSummarized;
            Zone = zone;
            FishingDaysTotal = fishingDaysTotal;
            FishingLicense = fishingLicense;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//CA/{CatchSummarized.ToNAF()}");
            sb.Append($"//RA/{CatchArea}");
            sb.Append($"//DF/{FishingDaysTotal}");
            if (!FishingLicense.IsNullOrEmpty())
            {
                sb.Append($"//FL/{FishingLicense}");
            }
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);

            result.Add("FishingDays".Translate(lang), FishingDaysTotal.ToString());
            result.Add("CatchArea".Translate(lang), CatchArea);
            result.Add("DailyCatch".Translate(lang), string.Join(", ", CatchSummarized.Select(x => x.ToReadableFormat(lang))));
            if (!FishingLicense.IsNullOrEmpty())
            {
                result.Add("FishingLicense".Translate(lang), FishingLicense);
            }

            return result;
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
                new Ship(values["NA"], values["RC"], values["XR"]),
                fishingLicense: values.ContainsKey("FL") ? values["FL"] : string.Empty)
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
