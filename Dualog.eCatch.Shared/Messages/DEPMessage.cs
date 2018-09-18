using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    public class DEPMessage : Message
    {
		public AC_FishingActivity FishingActivity { get; }
		public string TargetFishSpeciesCode { get; }
		public string DepartureHarbourCode { get; }
		public DateTime DepartureDateTime { get; }
		public DateTime ArrivalDateTime { get; }
		public string Latitude { get; }
		public string Longitude { get; }
		public IReadOnlyList<FishFAOAndWeight> FishOnBoard { get; }
        public string Tool { get; }

		public DEPMessage(
                        DateTime sent,
                        AC_FishingActivity activity, 
						string targetFishSpeciesCode, 
						string harbourCode, 
						DateTime departurDateTime, 
						DateTime arrivalDateTime, 
						string latitude, 
						string longitude, 
			            IReadOnlyList<FishFAOAndWeight> fishOnBoard,
                        string skipperName,
                        Ship ship,
                        string cancelCode = "",
                        string tool = "") : base(MessageType.DEP, sent, skipperName, ship, errorCode: cancelCode)
        {
            FishingActivity = activity;
            TargetFishSpeciesCode = targetFishSpeciesCode;
            DepartureHarbourCode = harbourCode;
            DepartureDateTime = departurDateTime;
            ArrivalDateTime = arrivalDateTime;
            Latitude = latitude;
            Longitude = longitude;
			FishOnBoard = fishOnBoard;
		    Tool = tool;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//PO/{DepartureHarbourCode}");
            sb.Append($"//ZD/{DepartureDateTime.ToFormattedDate()}");
            sb.Append($"//ZT/{DepartureDateTime.ToFormattedTime()}");
            sb.Append($"//PD/{ArrivalDateTime.ToFormattedDate()}");
            sb.Append($"//PT/{ArrivalDateTime.ToFormattedTime()}");
            sb.Append($"//LA/{Latitude}");
            sb.Append($"//LO/{Longitude}");
            sb.Append($"//AC/{FishingActivity}");
            sb.Append($"//DS/{TargetFishSpeciesCode}");
            sb.Append($"//OB/{FishOnBoard.ToNAF()}");
            if (!string.IsNullOrEmpty(Tool))
            {
                sb.Append($"//GE/{Tool}");
            }
        }

        public Dictionary<string, string> GetSummaryForDictionary(EcatchLangauge lang, string arrivalInfo)
        {
            var result = CreateBaseSummaryDictionary(lang);

            result.Add("DepartureFrom".Translate(lang), $"{DepartureHarbourCode.ToHarbourName()} {DepartureDateTime:dd.MM.yyyy HH:mm} UTC");
            result.Add("Arrival".Translate(lang), $"{arrivalInfo} {ArrivalDateTime:dd.MM.yyyy HH:mm} UTC");
            result.Add("PlannedActivity".Translate(lang), $"{FishingActivity.ToString().ToFishingActivityName(lang)}");
            result.Add("TargetSpecies".Translate(lang), $"{TargetFishSpeciesCode.ToFishName(lang)}");
            result.Add("EstimatedWeightAtDeparture".Translate(lang), FishOnBoard.ToDetailedWeightAndFishNameSummary(lang));

            return result;
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            return GetSummaryForDictionary(lang, $"Lat: {Latitude}, Lon: {Longitude}");
        }

        public static DEPMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new DEPMessage(
                sent,
                EnumHelper.Parse<AC_FishingActivity>(values["AC"]),
                values["DS"],
                values["PO"],
                (values["ZD"] + values["ZT"]).FromFormattedDateTime(),
                (values["PD"] + values["PT"]).FromFormattedDateTime(),
                values["LA"],
                values["LO"],
                MessageParsing.ParseFishWeights(values["OB"]),
                values["MA"], 
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty,
                values.ContainsKey("GE") ? values["GE"] : string.Empty)
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
