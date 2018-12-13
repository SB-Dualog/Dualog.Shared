using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    /// <summary>
    /// MAN Message - Used to send position to FMC.
    /// </summary>
    public class MANMessage : Message
    {
        public string Latitude { get; }
        public string Longitude { get; }
        /// <summary>
        /// Course from 0 to 360 degrees
        /// </summary>
        public int Course { get; }
        /// <summary>
        /// Speed in knots. Set as Knots * 10 (10.5 knots * 10 = 105) 
        /// </summary>
        public int Speed { get; }
        public MANMessage(DateTime sent, string latitude, string longitude, string skipperName, Ship ship, int course, int speed) 
            : base(MessageType.MAN, sent, skipperName, ship)
        {
            Latitude = latitude;
            Longitude = longitude;
            Course = course;
            Speed = speed;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//LA/{Latitude}");
            sb.Append($"//LO/{Longitude}");
            sb.Append($"//CO/{Course}");
            sb.Append($"//SP/{Speed}");
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);
            result.Add("Latitude".Translate(lang), Latitude);
            result.Add("Longitude".Translate(lang), Longitude);
            result.Add("Course".Translate(lang), $"{Course}°");
            result.Add("Speed".Translate(lang), $"{Speed / 10} kn");
            return result;
        }

        public static MANMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new MANMessage(
                sent,
                values["LA"],
                values["LO"],
                values["MA"],
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("CO") ? int.Parse(values["CO"]) : 0,
                values.ContainsKey("SP") ? int.Parse(values["SP"]) : 0
            )
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
