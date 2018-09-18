using System;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;
using Dualog.eCatch.Shared.Models;

namespace Dualog.eCatch.Shared.Messages
{
    /// <summary>
    /// Base class for all messages
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// RN - Record number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TM - Message Type
        /// </summary>
        public MessageType MessageType { get; }

        /// <summary>
        /// DA and TI - Date time the message was sent
        /// </summary>
        public DateTime Sent { get; }
        /// <summary>
        /// MA - Master name
        /// </summary>
        public string SkipperName { get; }

        /// <summary>
        /// Ship with RC (radio call signal), NA (name of ship) and XR (external registration)
        /// </summary>
        public Ship Ship { get; }

        /// <summary>
        /// RE - Return error number. When sent from a boat, it acts as a cancelation code or correction code. Value 521 if a message needs to be canceled or value 511 if it needs to be corrected
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// MV - Message version. If a message has been corrected, this value needs to be the +1 increment of corrected message
        /// </summary>
        public int MessageVersion { get; }

        /// <summary>
        /// FT - ForwardTo. Needs to be included if a message is sent in another zone than norway
        /// </summary>
        public string ForwardTo { get; set; }

        /// <summary>
        /// SQ - Sequence number. A sequence number between this boat and and zone that the message is being forwarded to. 
        /// </summary>
        public int SequenceNumber { get; set; }

		protected Message(MessageType messageType, DateTime sent, string skipperName, Ship ship, string errorCode = "", int messageVersion = 0)
		{
		    Sent = sent;
			MessageType = messageType;
		    SkipperName = skipperName;
		    Ship = ship;
		    ErrorCode = errorCode;
		    MessageVersion = messageVersion;
		}


        private void WriteHeader(StringBuilder sb)
        {
            sb.Append("//SR");
            sb.Append($"//TM/{MessageType}");
			sb.Append($"//RN/{Id}");
            if (!ForwardTo.IsNullOrEmpty())
            {
                sb.Append($"//FT/{ForwardTo}");
            }
            if (SequenceNumber > 0)
            {
                sb.Append($"//SQ/{SequenceNumber}");
            }
            if (MessageVersion > 0)
            {
                sb.Append($"//MV/{MessageVersion}");
            }
            if (!ErrorCode.IsNullOrEmpty())
            {
                sb.Append($"//RE/{ErrorCode}");
            }
            sb.Append("//AD/NOR");
            sb.Append($"//RC/{Ship.RadioCallSignal}");
            sb.Append($"//NA/{Ship.Name}");
            sb.Append($"//XR/{Ship.RegistrationNumber}");
            sb.Append($"//MA/{SkipperName}");
            sb.Append($"//DA/{Sent.ToFormattedDate()}");
            sb.Append($"//TI/{Sent.ToFormattedTime()}");
        }

        public override int GetHashCode()
        {
            return new { Id, Sent.Date, Sent.Hour, Sent.Minute, ErrorCode }.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as Message;
            if (rhs == null) return false;
            var rhsHash = rhs.GetHashCode();
            var thisHash = GetHashCode();
            return rhsHash == thisHash;
        }

        protected abstract void WriteBody(StringBuilder sb);

        private static void WriteFooter(StringBuilder sb)
        {
            sb.Append("//ER//");
        }

        /// <summary>
        /// Generates NAF format for message
        /// </summary>
        /// <returns>NAF message</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            WriteHeader(sb);
            WriteBody(sb);
            WriteFooter(sb);
            return sb.ToString();
        }
    }
}
