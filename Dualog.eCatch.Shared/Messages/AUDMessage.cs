using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    /// <summary>
    /// AUD Message - Used to test communication.
    /// </summary>
    public class AUDMessage : Message
    {
        /// <summary>
        /// MS - Optional test message
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public AUDMessage(string text, DateTime sent, string skipperName, Ship ship) : base(MessageType.AUD, sent, skipperName, ship)
        {
            Text = text;
        }

        protected override void WriteBody(StringBuilder sb)
        {
            sb.Append($"//MS/{Text}");
        }

        public override Dictionary<string, string> GetSummaryDictionary(EcatchLangauge lang)
        {
            var result = CreateBaseSummaryDictionary(lang);
            result.Add("Content".Translate(lang), Text);
            return result;
        }

        public static AUDMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new AUDMessage(
                values.ContainsKey("MS") ? values["MS"] : string.Empty, 
                sent,
                values.ContainsKey("MA") ? values["MA"] : string.Empty,
                new Ship(
                    values.ContainsKey("NA") ? values["NA"] : string.Empty, 
                    values["RC"],
                    values.ContainsKey("XR") ? values["XR"] : string.Empty))
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
