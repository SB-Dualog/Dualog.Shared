using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    /// <summary>
    /// Port report message
    /// </summary>
    public class PORMessage : Message
    {
        /// <summary>
        /// PO - Port code
        /// </summary>
        public string ArrivalHarbourCode { get; }

        /// <summary>
        /// OB - Quantity on board
        /// </summary>
        public IReadOnlyList<FishFAOAndWeight> FishOnBoard { get; }

        /// <summary>
        /// KG - Quantity to deliver
        /// </summary>
        public IReadOnlyList<FishFAOAndWeight> FishToDeliver { get; }

        /// <summary>
        /// LS - Facility where catch is delivered
        /// </summary>
        public string DeliveryFacility { get; }

        /// <summary>
        /// PD and PT - Date and time for arrival, in UTC
        /// </summary>
        public DateTime ArrivalDateTime { get; }

        public PORMessage(             
            DateTime sent,
            string arrivalHarbourCode,
            DateTime arrivalDateTime,
            IReadOnlyList<FishFAOAndWeight> fishOnBoard,
            IReadOnlyList<FishFAOAndWeight> fishToDeliver,
            string deliveryFacility, 
            string skipperName, 
            Ship ship,
            string cancelCode = "") : base(MessageType.POR, sent, skipperName, ship, errorCode: cancelCode)
        {
            ArrivalHarbourCode = arrivalHarbourCode;
            FishOnBoard = fishOnBoard;
            FishToDeliver = fishToDeliver;
            DeliveryFacility = deliveryFacility;
            ArrivalDateTime = arrivalDateTime;

            if (FishToDeliver.Count > 0)
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
            sb.Append($"//OB/{FishOnBoard.ToNAF()}");

            if (FishToDeliver.Count > 0)
            {
                sb.Append($"//LS/{DeliveryFacility}");
                sb.Append($"//KG/{FishToDeliver.ToNAF()}");
            }            
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);

            result.Add("ArrivalAt".Translate(lang), $"{ArrivalHarbourCode.ToHarbourName()}, {ArrivalDateTime:dd.MM.yyyy HH:mm} UTC");
            result.Add("FishOnBoard".Translate(lang), FishOnBoard.ToDetailedWeightAndFishNameSummary(lang));

            if (FishToDeliver.Count > 0)
            {
                result.Add("DeliveringTo".Translate(lang), $"{DeliveryFacility}, {FishToDeliver.ToDetailedWeightAndFishNameSummary(lang)}");
            }

            return result;
        }

        public static PORMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new PORMessage(
                sent,
                values["PO"],
                (values["PD"] + values["PT"]).FromFormattedDateTime(),
                MessageParsing.ParseFishWeights(values.ContainsKey("OB") ? values["OB"] : string.Empty),
                MessageParsing.ParseFishWeights(values.ContainsKey("KG") ? values["KG"] : string.Empty),
                values.ContainsKey("LS") ? values["LS"] : string.Empty,
                values["MA"], 
                new Ship(values["NA"], values["RC"], values["XR"]),
                values.ContainsKey("RE") ? values["RE"] : string.Empty)
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
