using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
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

        protected override List<string> GetSummaryList(EcatchLangauge lang)
        {
            throw new NotImplementedException();
        }

        public static AUDMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new AUDMessage(values["MS"], sent, values["MA"], new Ship(values["NA"], values["RC"], values["XR"]))
            {
                Id = id,
                ForwardTo = values.ContainsKey("FT") ? values["FT"] : string.Empty,
                SequenceNumber = values.ContainsKey("SQ") ? Convert.ToInt32(values["SQ"]) : 0
            };
        }
    }
}
