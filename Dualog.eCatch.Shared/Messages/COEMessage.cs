using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    public class COEMessage : Message
    {
        public string CatchArea { get; }
        public DateTime FishStart { get; }
        public string TargetSpecies { get; }
        public string CurrentLatitude { get; }
        public string CurrentLongitude { get; }
        public string FishStartLatitude { get; }
        public string FishStartLongitude { get; }
        public IReadOnlyList<FishFAOAndWeight> FishOnBoard { get; }

        public DateTime CrossBorderTime { get; private set; }
        public string CrossBorderLatitude { get; private set; }
        public string CrossBorderLongitude { get; private set; }

        public COEMessage(
                        DateTime sent,
                        string catchArea, 
                        DateTime fishStart, 
                        string targetSpecies, 
                        string currentLatitude, 
                        string currentLongitude, 
                        string fishStartLatitude,
                        string fishStartLongitude,
                        IReadOnlyList<FishFAOAndWeight> fishOnBoard,
                        string skipperName,
                        Ship ship,
                        string cancelCode = "") : base(MessageType.COE, sent, skipperName, ship, errorCode: cancelCode)
        {
            CatchArea = catchArea;
            FishStart = fishStart;
            TargetSpecies = targetSpecies;
		    CurrentLatitude = currentLatitude;
		    CurrentLongitude = currentLongitude;
		    FishOnBoard = fishOnBoard;
            FishStartLatitude = fishStartLatitude;
            FishStartLongitude = fishStartLongitude;
        }

        public void SetCrossBorderInfo(DateTime crossBorderTime, string crossBorderLat, string crossBorderLon)
        {
            CrossBorderTime = crossBorderTime;
            CrossBorderLatitude = crossBorderLat;
            CrossBorderLongitude = crossBorderLon;
        }
        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//OB/{FishOnBoard.ToNAF()}");
            if (MessageFieldChecker.Coe.XtXg(ForwardTo))
            {
                sb.Append($"//XT/{CurrentLatitude}");
                sb.Append($"//XG/{CurrentLongitude}");
            }
            if (MessageFieldChecker.Coe.Ra(ForwardTo))
            {
                sb.Append($"//RA/{CatchArea}");
            }
            sb.Append($"//PD/{FishStart.ToFormattedDate()}");
            sb.Append($"//PT/{FishStart.ToFormattedTime()}");
            if (MessageFieldChecker.ZoneUsesFormat.LtLg(ForwardTo))
            {
                sb.Append($"//LT/{FishStartLatitude}");
                sb.Append($"//LG/{FishStartLongitude}");
            }
            else if (MessageFieldChecker.ZoneUsesFormat.LaLo(ForwardTo))
            {
                //NEAFC Uses the LA/LO as current position and not fish start...
                if (ForwardTo == Constants.Zones.NEAFC)
                {
                    sb.Append($"//LA/{CurrentLatitude}");
                    sb.Append($"//LO/{CurrentLongitude}");
                }
                else
                {
                    sb.Append($"//LA/{FishStartLatitude}");
                    sb.Append($"//LO/{FishStartLongitude}");

                }
            }
            if (ForwardTo == Constants.Zones.Russia)
            {
                sb.Append($"//ZD/{CrossBorderTime.ToFormattedDate()}");
                sb.Append($"//ZT/{CrossBorderTime.ToFormattedTime()}");
                sb.Append($"//ZA/{CrossBorderLatitude}");
                sb.Append($"//ZG/{CrossBorderLongitude}");
            }

            if (MessageFieldChecker.Coe.Ds(ForwardTo))
            {
                sb.Append($"//DS/{TargetSpecies}");
            }

        }

        public static COEMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            var forwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty;
            var fishStartLat = "";
            var fishStartLon = "";
            var currentLat = "";
            var currentLon = "";
            if (MessageFieldChecker.Coe.XtXg(forwardTo) && values.ContainsKey("XT") && values.ContainsKey("XG"))
            {
                currentLat = values["XT"];
                currentLon = values["XG"];
            }

            if (MessageFieldChecker.ZoneUsesFormat.LtLg(forwardTo) && values.ContainsKey("LT") && values.ContainsKey("LG"))
            {
                fishStartLat = values["LT"];
                fishStartLon = values["LG"];
            }
            else if (MessageFieldChecker.ZoneUsesFormat.LaLo(forwardTo) && values.ContainsKey("LA") && values.ContainsKey("LO"))
            {
                //NEAFC Uses the LA/LO as current position and not fish start...
                if (forwardTo == Constants.Zones.NEAFC)
                {
                    currentLon = values["LA"];
                    currentLat = values["LO"];
                }
                else
                {
                    fishStartLat = values["LA"];
                    fishStartLon = values["LO"];
                }
            }
            var coeMessage = new COEMessage(
                sent,
                values.ContainsKey("RA") ? values["RA"] : string.Empty,
                (values["PD"] + values["PT"]).FromFormattedDateTime(),
                values.ContainsKey("DS") ? values["DS"] : string.Empty,
                currentLat,
                currentLon,
                fishStartLat,
                fishStartLon,
                MessageParsing.ParseFishWeights(values["OB"]),
                values["MA"],
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty)
            {
                Id = id,
                ForwardTo = forwardTo,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
            if (forwardTo == Constants.Zones.Russia)
            {
                var time = (values["ZD"] + values["ZT"]).FromFormattedDateTime();
                var borderLat = values["ZA"];
                var borderLon = values["ZG"];
                coeMessage.SetCrossBorderInfo(time, borderLat, borderLon);
            }
            return coeMessage;
        }
    }
}
