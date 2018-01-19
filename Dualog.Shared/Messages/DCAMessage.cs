using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Dualog.Shared.Enums;
using Dualog.Shared.Extensions;
using Dualog.Shared.Models;

namespace Dualog.Shared.Messages
{
    public class DCAMessage : Message
    {
        public IReadOnlyCollection<Cast> Casts { get; }
        public string FishingActivity { get; }
        public string FishingPermission { get; }
        public string ArrivalHarbour { get; }

        public DCAMessage(
            string fishingPermission, 
            string fishingActivity, 
            string arrivalHarbour, 
            IReadOnlyCollection<Cast> casts, 
            DateTime sent, 
            string skipperName, 
            Ship ship, 
            int messageVersion = 1,
            string correctionCode = "") : base(MessageType.DCA, sent, skipperName, ship, errorCode: correctionCode, messageVersion: messageVersion)
        {
            this.FishingPermission = fishingPermission;
            this.FishingActivity = fishingActivity;
            ArrivalHarbour = arrivalHarbour;
            Casts = casts;
        }

		public IReadOnlyList<FishFAOAndWeight> GetFishAndWeights()
		{
			return Casts.SelectMany(m => m.FishDistribution).ToList();
		}

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//QI/{FishingPermission}");
            sb.Append($"//AC/{FishingActivity}");
            if (!ArrivalHarbour.IsNullOrEmpty())
            {
                sb.Append($"//PO/{ArrivalHarbour}");
            }
            Casts.ForEach(c => WriteCast(sb, c));           
        }

        private void WriteCast(StringBuilder sb, Cast cast)
        {
            var startLat = cast.StartLatitude.ToWgs84Format(CoordinateType.Latitude);
            var startLon = cast.StartLongitude.ToWgs84Format(CoordinateType.Longitude);
            var stopLat = cast.StopLatitude.ToWgs84Format(CoordinateType.Latitude);
            var stopLon = cast.StopLongitude.ToWgs84Format(CoordinateType.Longitude);
            sb.Append("//TS");
            sb.Append($"//BD/{cast.StartTime.ToFormattedDate()}");
            sb.Append($"//BT/{cast.StartTime.ToFormattedTime()}");
            sb.Append($"//ZO/{cast.Zone}");
            sb.Append($"//LT/{startLat}");
            sb.Append($"//LG/{startLon}");
            sb.Append($"//GE/{cast.Tool}");
            sb.Append($"//GP/{cast.Problem}");
            sb.Append($"//XT/{stopLat}");
            sb.Append($"//XG/{stopLon}");
            sb.Append($"//DU/{cast.GetDuration()}");


            if (!cast.HerringType.IsNullOrEmpty())
            {
                sb.Append($"//SS/{cast.HerringType}");
            }

            sb.Append($"//CA/{cast.FishDistribution.ToNAF()}");
            if (cast.AnimalCount.Any())
            {
                if (cast.FishDistribution.Any())
                {
                    sb.Append(" " + cast.AnimalCount.ToNAF());
                }
                else
                {
                    sb.Append(cast.AnimalCount.ToNAF());
                }
            }

            if (cast.MaskWidth > 0)
            {
                sb.Append($"//ME/{cast.MaskWidth}");
            }
            if (cast.NumberOfTrawls > 0)
            {
                sb.Append($"//GS/{cast.NumberOfTrawls}");
            }
            if (cast.ExtraToolInfo > 0)
            {
                sb.Append($"//FO/{cast.ExtraToolInfo}");
            }
            if (FishingActivity.Equals("REL"))
            {
                sb.Append("//TF/RADIOFROM");
            }

            if (cast.Tool.Equals("HAR") && !cast.Problem.Equals("7"))
            {
                sb.Append("//IN/12");
            }

            if (cast.Tool.Equals("HAR") && 
                (!cast.Problem.Equals("7") && !cast.Problem.Equals("8")))
            {
                sb.Append("//SE/1");
                sb.Append("//LE/46");
                sb.Append("//CI/32");
                sb.Append("//BM/21");
            }

            if (cast.Tool.Equals("HAR"))
            {
                sb.Append("//GN/17");
                sb.Append("//LF/32");
            }
        }

        public static DCAMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values, List<IReadOnlyDictionary<string, string>> haulValues)
        {
            return new DCAMessage(
                values["QI"],
                values["AC"],
                values.ContainsKey("PO") ? values["PO"] : string.Empty,
                haulValues.Select(haul => 
                    new Cast(
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
                new Ship(values["NA"], values["RC"], values["XR"]),
                Convert.ToInt32(values["MV"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty)
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
