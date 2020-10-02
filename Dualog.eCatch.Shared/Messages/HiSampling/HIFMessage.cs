using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages.HiSampling
{
    public class HIFMessage : Message
    {
        public IReadOnlyCollection<Haul> Hauls { get; }
        public string FishingActivity { get; }
        public string FishingPermission { get; }
        public string ArrivalHarbour { get; }
        public string PumpingFromBoat { get; }

        public HIFMessage(
            string fishingPermission,
            string fishingActivity,
            string arrivalHarbour,
            IReadOnlyCollection<Haul> hauls,
            DateTime sent,
            string skipperName,
            Ship ship,
            int messageVersion = 1,
            string correctionCode = "",
            string pumpingFromBoat = "") : base(MessageType.HIF, sent, skipperName, ship, errorCode: correctionCode, messageVersion: messageVersion)
        {
            this.FishingPermission = fishingPermission;
            this.FishingActivity = fishingActivity;
            ArrivalHarbour = arrivalHarbour;
            Hauls = hauls;
            PumpingFromBoat = pumpingFromBoat;
            ForwardTo = Constants.Zones.Havforskningsinstituttet;
        }

        public IReadOnlyList<FishFAOAndWeight> GetFishAndWeights()
        {
            return Hauls.SelectMany(m => m.FishDistribution).ToList();
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//QI/{FishingPermission}");
            sb.Append($"//AC/{FishingActivity}");
            if (!ArrivalHarbour.IsNullOrEmpty())
            {
                sb.Append($"//PO/{ArrivalHarbour}");
            }
            Hauls.ForEach(c => WriteHaul(sb, c));
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);
            var i = 1;
            foreach (var haul in Hauls)
            {
                result.Add($"{"Haul".Translate(lang)} {i}", $"{haul.StartTime:dd.MM.yyyy HH:mm} - {haul.StopTime:dd.MM.yyyy HH:mm} ({haul.GetDuration()} {"Minutes".Translate(lang).ToLowerInvariant()})");
                result.Add($"{"Catch".Translate(lang)} for {"Haul".Translate(lang).ToLowerInvariant()} {i}", haul.FishDistribution.ToDetailedWeightAndFishNameSummary(lang));
                i++;
            }
            return result;
        }

        private void WriteHaul(StringBuilder sb, Haul haul)
        {
            var startLat = haul.StartLatitude.ToWgs84Format(CoordinateType.Latitude);
            var startLon = haul.StartLongitude.ToWgs84Format(CoordinateType.Longitude);
            var stopLat = haul.StopLatitude.ToWgs84Format(CoordinateType.Latitude);
            var stopLon = haul.StopLongitude.ToWgs84Format(CoordinateType.Longitude);
            sb.Append("//TS");
            sb.Append($"//BD/{haul.StartTime.ToFormattedDate()}");
            sb.Append($"//BT/{haul.StartTime.ToFormattedTime()}");
            sb.Append($"//ZO/{haul.Zone}");
            sb.Append($"//LT/{startLat}");
            sb.Append($"//LG/{startLon}");
            sb.Append($"//GE/{haul.Tool}");
            sb.Append($"//GP/{haul.Problem}");
            sb.Append($"//XT/{stopLat}");
            sb.Append($"//XG/{stopLon}");
            sb.Append($"//DU/{haul.GetDuration()}");

            if (!haul.HerringType.IsNullOrEmpty())
            {
                sb.Append($"//SS/{haul.HerringType}");
            }

            sb.Append($"//CA/{haul.FishDistribution.ToNAF()}");
            if (haul.AnimalCount.Any())
            {
                if (haul.FishDistribution.Any())
                {
                    sb.Append(" " + haul.AnimalCount.ToNAF());
                }
                else
                {
                    sb.Append(haul.AnimalCount.ToNAF());
                }
            }

            if (haul.MaskWidth > 0)
            {
                sb.Append($"//ME/{haul.MaskWidth}");
            }

            if (haul.NumberOfTrawls > 0)
            {
                sb.Append($"//GS/{haul.NumberOfTrawls}");
            }

            if (haul.ExtraToolInfo > 0)
            {
                sb.Append($"//FO/{haul.ExtraToolInfo}");
            }

            if (FishingActivity.Equals("REL") && !PumpingFromBoat.IsNullOrEmpty())
            {
                sb.Append($"//TF/{PumpingFromBoat}");
            }
        }

        public static HIFMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values, List<IReadOnlyDictionary<string, string>> haulValues)
        {
            var pumpingFrom = string.Empty;
            if (haulValues.Any(x => x.ContainsKey("TF")))
            {
                var firstHaulWithPumping = haulValues.FirstOrDefault(x => x.ContainsKey("TF"));
                firstHaulWithPumping?.TryGetValue("TF", out pumpingFrom);
            }

            return new HIFMessage(
                values["QI"],
                values["AC"],
                values.ContainsKey("PO") ? values["PO"] : string.Empty,
                haulValues.Select(haul =>
                    new Haul(
                        (haul["BD"] + haul["BT"]).FromFormattedDateTime(),
                        (haul["BD"] + haul["BT"]).FromFormattedDateTime().AddMinutes(Convert.ToDouble(haul["DU"], CultureInfo.InvariantCulture)),
                        Convert.ToDouble(haul["LT"], CultureInfo.InvariantCulture),
                        Convert.ToDouble(haul["LG"], CultureInfo.InvariantCulture),
                        Convert.ToDouble(haul["XT"], CultureInfo.InvariantCulture),
                        Convert.ToDouble(haul["XG"], CultureInfo.InvariantCulture),
                        haul["GE"],
                        haul["GP"],
                        MessageParsing.ParseFishWeights(haul["CA"]),
                        haul.ContainsKey("ME") ? Convert.ToInt32(haul["ME"]) : 0,
                        haul.ContainsKey("GS") ? Convert.ToInt32(haul["GS"]) : 0,
                        haul.ContainsKey("FO") ? Convert.ToInt32(haul["FO"]) : 0,
                        haul["ZO"],
                        haul.ContainsKey("SS") ? haul["SS"] : string.Empty,
                        MessageParsing.ParseAnimalCount(haul["CA"])
                        )
                    ).ToList(),
                sent,
                values["MA"],
                new Ship(
                    values.ContainsKey("NA") ? values["NA"] : string.Empty,
                    values["RC"],
                    values.ContainsKey("XR") ? values["XR"] : string.Empty),
                Convert.ToInt32(values["MV"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty,
                pumpingFrom)
            {
                Id = id,
                ForwardTo = values["FT"],
                SequenceNumber = Convert.ToInt32(values["SQ"])
            };
        }
    }
}