using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages.HiSampling
{
    public class HILMessage : Message
    {
        public string ArrivalHarbourCode { get; }
        public IReadOnlyList<HISample> SamplesToDeliver { get; }
        public string DeliveryFacility { get; }
        public DateTime ArrivalDateTime { get; }

        public HILMessage(             
            DateTime sent,
            string arrivalHarbourCode,
            DateTime arrivalDateTime,
            IReadOnlyList<HISample> samplesToDeliver,
            string deliveryFacility, 
            string skipperName, 
            Ship ship,
            string cancelCode = "") : base(MessageType.HIL, sent, skipperName, ship, errorCode: cancelCode)
        {
            ArrivalHarbourCode = arrivalHarbourCode;
            SamplesToDeliver = samplesToDeliver;
            DeliveryFacility = deliveryFacility;
            ArrivalDateTime = arrivalDateTime;
            ForwardTo = Constants.Zones.Havforskningsinstituttet;

            if (SamplesToDeliver.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(deliveryFacility))
                {
                    throw new ArgumentException("Delivery facility is required when delivering fish", nameof(deliveryFacility));
                }
                else if (deliveryFacility.Length > 60)
                {
                    throw new ArgumentException("Delivery facility has a max length of 60 characters", nameof(deliveryFacility));
                }
            }
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//PO/{ArrivalHarbourCode}");
            sb.Append($"//PD/{ArrivalDateTime.ToFormattedDate()}");
            sb.Append($"//PT/{ArrivalDateTime.ToFormattedTime()}");

            if (SamplesToDeliver.Count > 0)
            {
                sb.Append($"//LS/{DeliveryFacility}");
                sb.Append($"//MS/{SamplesToDeliver.ToNAF()}");
            }            
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);
            return result;
        }

        public static HILMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new HILMessage(
                sent,
                values["PO"],
                (values["PD"] + values["PT"]).FromFormattedDateTime(),
                MessageParsing.ParseHISamples(values["MS"]),
                values.ContainsKey("LS") ? values["LS"] : string.Empty,
                values["MA"], 
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty)
            {
                Id = id,
                ForwardTo = values["FT"], // Should fail if missing?
                SequenceNumber = Convert.ToInt32(values["SQ"]) // // Should fail if missing?
            };
        }
    }
}
