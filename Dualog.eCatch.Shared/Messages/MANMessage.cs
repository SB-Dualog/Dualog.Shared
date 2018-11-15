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
        public MANMessage(DateTime sent, string latitude, string longitude, string skipperName, Ship ship) 
            : base(MessageType.MAN, sent, skipperName, ship)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//LA/{Latitude}");
            sb.Append($"//LO/{Longitude}");
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);
            result.Add("Latitude".Translate(lang), Latitude);
            result.Add("Longitude".Translate(lang), Longitude);
            return result;
        }

        public static MANMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            var lat = "";
            var lon = "";
            if (values.ContainsKey("LA"))
            {
                lat = values["LA"];
            }
            if (values.ContainsKey("LO"))
            {
                lon = values["LO"];
            }
            
            return new MANMessage(
                sent,
                lat,
                lon,
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
