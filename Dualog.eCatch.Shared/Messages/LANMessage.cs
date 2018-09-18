using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    public class LANMessage : Message
    {
        public IReadOnlyCollection<FishLanding> FishLandings { get; } 
        public string Harbour { get; }
        public DateTime LandingTime { get; }
        public LANMessage(DateTime sent, string harbour, DateTime landingTime, IReadOnlyCollection<FishLanding> fishLandings, string skipperName, Ship ship) : base(MessageType.LAN, sent, skipperName, ship)
        {
            Harbour = harbour;
            LandingTime = landingTime;
            FishLandings = fishLandings;
        }

        private void WriteFishLanding(StringBuilder sb, FishLanding fishLanding)
        {
            sb.Append("//TS");
            sb.Append($"//SN/{fishLanding.FishSpecies}");
            sb.Append($"//EZ/{fishLanding.Zone}");
            sb.Append($"//RA/{fishLanding.CatchArea}");
            sb.Append($"//NE/{fishLanding.Weight}");
            sb.Append($"//PS/{fishLanding.Conservation}");
            sb.Append($"//PR/{fishLanding.Condition}");
            sb.Append($"//TY/{fishLanding.UnitType}");
            sb.Append($"//NU/{fishLanding.NumberOfUnits}");
            //var formattedAverageWeight = $"{string.Format("{0:0.#}",fishLanding.UnitAverageWeight)}".Replace(".", ",");
            sb.Append($"//AW/{fishLanding.UnitAverageWeight}");
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//DL/{LandingTime.ToFormattedDate()}");
            sb.Append($"//HL/{LandingTime.ToFormattedTime()}");
            sb.Append($"//PO/{Harbour}");
            FishLandings.ForEach(x => WriteFishLanding(sb, x));
        }

        public static LANMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values,
            List<IReadOnlyDictionary<string, string>> fishLandingValues)
        {
            return new LANMessage(
                sent,
                values["PO"],
                (values["DL"] + values["HL"]).FromFormattedDateTime(),
                fishLandingValues.Select(f =>
                    new FishLanding(
                        f["SN"],
                        Convert.ToInt32((string) f["NE"]),
                        Convert.ToInt32((string) f["NU"]),
                        f["PS"],
                        f["PR"],
                        f["TY"],
                        f["EZ"],
                        f["RA"]
                        )).ToList(),
                values["MA"],
                new Ship(values["NA"], values["RC"], values["XR"])
                )
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
