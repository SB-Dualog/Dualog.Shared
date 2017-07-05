using System;
using System.Collections.Generic;
using System.Text;
using Dualog.Shared.Enums;
using Dualog.Shared.Extensions;
using Dualog.Shared.Models;

namespace Dualog.Shared.Messages
{
    public class CONMessage : Message
    {
        public string ControlPoint { get; }
        public DateTime ControlTime { get; }
        public string Latitude { get; }
        public string Longitude { get; }
        public CONMessage(DateTime sent, string controlPoint, DateTime controlTime, string latitude, string longitude, string skipperName, Ship ship, string errorCode = "") : base(MessageType.CON, sent, skipperName, ship, errorCode)
        {
            ControlPoint = controlPoint;
            ControlTime = controlTime;
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//PD/{ControlTime.ToFormattedDate()}");
            sb.Append($"//PT/{ControlTime.ToFormattedTime()}");
            sb.Append($"//CP/{ControlPoint}");
            if (ForwardTo != Constants.Zones.Russia)
            {
                sb.Append($"//LT/{Latitude}");
                sb.Append($"//LG/{Longitude}");
            }
        }

        public static CONMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new CONMessage(
                sent,
                values["CP"],
                (values["PD"] + values["PT"]).FromFormattedDateTime(),
                values.ContainsKey("LT") ? values["LT"] : string.Empty,
                values.ContainsKey("LG") ? values["LG"] : string.Empty,
                values["MA"],
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty
                )
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
