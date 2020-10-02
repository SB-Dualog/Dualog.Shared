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
        public HILMessage(
            DateTime sent,
            string arrivalHarbourCode,
            DateTime arrivalDateTime,
            IReadOnlyList<HiSample> samplesToDeliver,
            string deliveryFacility,
            string skipperName,
            Ship ship,
            string cancelCode = "") : base(MessageType.HIL, sent, skipperName, ship, cancelCode)
        {
            ArrivalHarbourCode = arrivalHarbourCode;
            SamplesToDeliver = samplesToDeliver;
            DeliveryFacility = deliveryFacility;
            ArrivalDateTime = arrivalDateTime;
            ForwardTo = Constants.Zones.Havforskningsinstituttet;

            if (SamplesToDeliver.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(deliveryFacility))
                    throw new ArgumentException("Delivery facility is required when delivering samples",
                        nameof(deliveryFacility));

                if (deliveryFacility.Length > 60)
                    throw new ArgumentException("Delivery facility has a max length of 60 characters",
                        nameof(deliveryFacility));
            }
        }

        public string ArrivalHarbourCode { get; }
        public IReadOnlyList<HiSample> SamplesToDeliver { get; }
        public string DeliveryFacility { get; }
        public DateTime ArrivalDateTime { get; }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//PO/{ArrivalHarbourCode}");
            sb.Append($"//PD/{ArrivalDateTime.ToFormattedDate()}");
            sb.Append($"//PT/{ArrivalDateTime.ToFormattedTime()}");

            if (SamplesToDeliver.Count > 0)
            {
                sb.Append($"//LS/{DeliveryFacility}");
                sb.Append($"//SH/{SamplesToDeliver.ToNAF()}");
            }
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);

            result.Add("ArrivalAt".Translate(lang), $"{ArrivalHarbourCode.ToHarbourName()}, {ArrivalDateTime:dd.MM.yyyy HH:mm} UTC");
            result.Add("DeliveringTo".Translate(lang), $"{DeliveryFacility}, {"Reporting".Translate(lang)} {SamplesToDeliver.Count} {"Samples".Translate(lang).ToLowerInvariant()}");
            var samplesBeingDelivered = new StringBuilder();
            foreach (var sample in SamplesToDeliver)
            {
                samplesBeingDelivered.Append(sample.Taken ? sample.Name + ", " : "");
            }
            result.Add("Delivering".Translate(lang), samplesBeingDelivered.ToString());

            return result;
        }

        public static HILMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new HILMessage(
                sent,
                values["PO"],
                (values["PD"] + values["PT"]).FromFormattedDateTime(),
                values.ContainsKey("SH") ? MessageParsing.ParseHISamples(values["SH"]) : new List<HiSample>(),
                values.ContainsKey("LS") ? values["LS"] : string.Empty,
                values["MA"],
                new Ship(
                    values.ContainsKey("NA") ? values["NA"] : string.Empty,
                    values["RC"],
                    values.ContainsKey("XR") ? values["XR"] : string.Empty),
                values.ContainsKey("RE") ? values["RE"] : string.Empty)
            {
                Id = id,
                ForwardTo = values["FT"],
                SequenceNumber = Convert.ToInt32(values["SQ"])
            };
        }
    }
}