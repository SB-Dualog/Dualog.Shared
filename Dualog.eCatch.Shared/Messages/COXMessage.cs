using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    public class COXMessage : Message
    {
        public string Zone { get; }
        public string DeliveryHarbour { get; }
        public string CatchArea { get; }
        public IReadOnlyList<FishFAOAndWeight> CatchSummarized { get;}
        public PositionAndTime PositionAndTime { get;}
        public IReadOnlyList<FishFAOAndWeight> CatchOnBoard { get;} 
        public int DaysFishing { get; }

        public COXMessage(DateTime sent, string zone, string skipperName, Ship ship, IReadOnlyList<FishFAOAndWeight> catchSummarized, IReadOnlyList<FishFAOAndWeight> catchOnBoard, PositionAndTime positionAndTime = null, int daysFishing = 0, string deliveryHarbour = "", string catchArea = "", string errorCode = "") : base(MessageType.COX, sent, skipperName, ship, errorCode)
        {
            Zone = zone;
            DeliveryHarbour = deliveryHarbour;
            CatchArea = catchArea;
            CatchSummarized = catchSummarized;
            CatchOnBoard = catchOnBoard;
            PositionAndTime = positionAndTime;
            DaysFishing = daysFishing;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            if (!DeliveryHarbour.IsNullOrEmpty())
            {
                sb.Append($"//PO/{DeliveryHarbour}");
            }
            if (Zone == Constants.Zones.NEAFC)
            {
                sb.Append($"//RA/{CatchArea}");
                sb.Append($"//DF/{DaysFishing}");
                sb.Append($"//CA/{CatchSummarized.ToNAF()}");
            }
            if (Zone == Constants.Zones.Russia)
            {
                sb.Append($"//ZD/{PositionAndTime.DateTime.ToFormattedDate()}");
                sb.Append($"//ZT/{PositionAndTime.DateTime.ToFormattedTime()}");
                sb.Append($"//ZA/{PositionAndTime.Latitude.ToWgs84Format(CoordinateType.Latitude)}");
                sb.Append($"//ZG/{PositionAndTime.Longitude.ToWgs84Format(CoordinateType.Longitude)}");
            }
            if (Zone == Constants.Zones.Russia || Zone == Constants.Zones.Island || Zone == Constants.Zones.FaroeIslands)
            {
                sb.Append($"//OB/{CatchOnBoard.ToNAF()}");
            }
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);

            if (Zone == Constants.Zones.NEAFC)
            {
                result.Add("CatchArea".Translate(lang), CatchArea);
                result.Add("FishingDays".Translate(lang), DaysFishing.ToString());
                result.Add("Catch".Translate(lang), CatchSummarized.ToDetailedWeightAndFishNameSummary(lang));
            }

            if (!string.IsNullOrEmpty(DeliveryHarbour))
            {
                result.Add("DeliveryHarbour".Translate(lang), DeliveryHarbour.ToHarbourName());
            }

            if (Zone == Constants.Zones.Russia || Zone == Constants.Zones.Island || Zone == Constants.Zones.FaroeIslands)
            {
                result.Add("FishOnBoard".Translate(lang), CatchOnBoard.ToDetailedWeightAndFishNameSummary(lang));
            }

            return result;
        }

        public static COXMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new COXMessage(
                sent,
                values.ContainsKey("FT") ? values["FT"] : string.Empty,
                values["MA"],
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("CA") ? MessageParsing.ParseFishWeights(values["CA"]) : new List<FishFAOAndWeight>(),
                values.ContainsKey("OB") ? MessageParsing.ParseFishWeights(values["OB"]) : new List<FishFAOAndWeight>(),
                values.ContainsKey("ZD") && values.ContainsKey("ZT") ? new PositionAndTime((values["ZD"] + values["ZT"]).FromFormattedDateTime(), Convert.ToDouble(values["ZA"], CultureInfo.InvariantCulture), Convert.ToDouble(values["ZG"], CultureInfo.InvariantCulture)) : null, 
                values.ContainsKey("DF") ? Convert.ToInt32(values["DF"]) : 0,
                values.ContainsKey("PO") ? values["PO"] : string.Empty,
                values.ContainsKey("RA") ? values["RA"] : string.Empty,
                values.ContainsKey("RE") ? values["RE"] : string.Empty)
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
