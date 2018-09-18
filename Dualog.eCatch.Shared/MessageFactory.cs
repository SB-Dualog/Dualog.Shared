using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Messages;

namespace Dualog.eCatch.Shared
{
    public static class MessageFactory
    {
        private static readonly Regex NAFRegex = new Regex("([A-Z]{2})\\/([^/]*)");
        /// <summary>
        /// Regex for a TS field in a message. Since there may be multiple TS elements in a message, we need a list containing dictionaries for them.
        /// </summary>
        private static readonly Regex TSRegex = new Regex(@"(\bTS\b).*?(?=(\bTS\b|\bER\b))");

        public static T Parse<T>(string naf) where T : Message
        {
            return (T) Parse(naf);
        }


        public static RETMessage ParseReturnMessage(string naf)
        {
            var values = ParseNAFToDictionary(naf);
            var messageType = EnumHelper.Parse<MessageType>(values["TM"]);
            if(messageType != MessageType.RET) throw new Exception($"Cannot parse message of type {messageType} to RETMessage");
            if (!values.ContainsKey("RN"))
            {
                throw new Exception("Unable to determine message id, RN-field missing.");
            }

            if (!values.ContainsKey("DA") && !values.ContainsKey("TI"))
            {
                throw new Exception("Unable to determine date and time message was sent, DA and TI-fields missing.");
            }

            var id = Convert.ToInt32(values["RN"]);
            var sent = (values["DA"] + values["TI"]).FromFormattedDateTime();
            return RETMessage.ParseNAFFormat(id, sent, values);
        }
        public static Message Parse(string naf)
        {
            var values = ParseNAFToDictionary(naf);
            if (!values.ContainsKey("TM"))
            {
                throw new Exception("Unable to determine message type, TM-field missing.");
            }

			if (!values.ContainsKey("RN"))
			{
				throw new Exception ("Unable to determine message id, RN-field missing.");
			}

            if (!values.ContainsKey("DA") && !values.ContainsKey("TI"))
            {
                throw new Exception("Unable to determine date and time message was sent, DA and TI-fields missing.");
            }

			var id = Convert.ToInt32(values ["RN"]);
            var messageType = EnumHelper.Parse<MessageType>(values["TM"]);
            var sent = (values["DA"] + values["TI"]).FromFormattedDateTime();

            switch (messageType)
            {
                case MessageType.DEP:
                    return DEPMessage.ParseNAFFormat(id, sent, values);
                case MessageType.COE:
					return COEMessage.ParseNAFFormat(id, sent, values);
                case MessageType.COX:
                    return COXMessage.ParseNAFFormat(id, sent, values);
                case MessageType.POR:
                    return PORMessage.ParseNAFFormat(id, sent, values);
                case MessageType.DCA:
                    var casts = ParseTSCollectionToDictionaryList(naf);
                    return DCAMessage.ParseNAFFormat(id, sent, values, casts);
                case MessageType.AUD:
                    return AUDMessage.ParseNAFFormat(id, sent, values);
                case MessageType.TRA:
                    return TRAMessage.ParseNAFFormat(id, sent, values);
                case MessageType.CAT:
                    return CATMessage.ParseNAFFormat(id, sent, values);
                case MessageType.CON:
                    return CONMessage.ParseNAFFormat(id, sent, values);
                case MessageType.LAN:
                    var fishLandings = ParseTSCollectionToDictionaryList(naf);
                    return LANMessage.ParseNAFFormat(id, sent, values, fishLandings);
                default:
                    throw new NotImplementedException($"Parsing of {messageType}-messages not implemented.");
            }
        }

        public static IReadOnlyDictionary<string, string> ParseNAFToDictionary(string nafFormat)
        {
            var matches = NAFRegex.Matches(nafFormat);
            var values = new Dictionary<string, string>();

            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value;
                var value = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;

                //If it is a DCA Message and we've reached the TS key, end the loop since casts will be read in a separate method
                if (key == "TS" && values["TM"] == "DCA")
                {
                    values.Add("ER", "");
                    break;
                }
                if (values.ContainsKey(key))
                {
                    //TODO Determine what to do with duplicate keys
                }
                else
                {
                    values.Add(key, value);
                }
            }

            return new ReadOnlyDictionary<string, string>(values);
        }

        public static List<IReadOnlyDictionary<string, string>> ParseTSCollectionToDictionaryList(string naf)
        {
            var list = new List<IReadOnlyDictionary<string, string>>();
            var castMatches = TSRegex.Matches(naf);
            foreach (Match castMatch in castMatches)
            {
                var values = new Dictionary<string, string>();
                var matches = NAFRegex.Matches(castMatch.Value);
                foreach (Match match in matches)
                {
                    var key = match.Groups[1].Value;
                    var value = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;
                    values.Add(key, value);
                }
                list.Add(values);
            }
            return list;
        }
    }
}
