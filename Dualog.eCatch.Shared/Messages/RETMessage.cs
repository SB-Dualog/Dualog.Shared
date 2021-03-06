﻿using System;
using System.Collections.Generic;
using System.Text;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;

namespace Dualog.eCatch.Shared.Messages
{
    public class RETMessage
    {
        public MessageType MessageType => MessageType.RET;
        public int Id { get; }
        public DateTime Sent { get; }
        public string RadioCallSignal { get; }
        public int MessageVersion { get; }
        public string ErrorCode { get; }
        public string MessageStatus { get; }
        public int SequenceNumber { get; }
        public string SentFrom { get; }
        public string FishingLicense { get; }
        public string Message { get; }

        public RETMessage(
            int id,
            DateTime sent,
            string radioCallSignal,
            string messageStatus,
            string errorCode = "",
            int sequenceNumber = 0,
            int messageVersion = 0,
            string from = "NOR",
            string fishingLicense = "",
            string message = "")
        {
            Id = id;
            Sent = sent;
            RadioCallSignal = radioCallSignal;
            ErrorCode = errorCode;
            MessageStatus = messageStatus;
            SequenceNumber = sequenceNumber;
            MessageVersion = messageVersion;
            SentFrom = from;
            FishingLicense = fishingLicense;
            Message = message;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("//SR");
            sb.Append($"//FR/{SentFrom}");
            sb.Append($"//TM/{MessageType}");
            sb.Append($"//RN/{Id}");
            sb.Append($"//RC/{RadioCallSignal}");
            sb.Append($"//RS/{MessageStatus}");

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

            sb.Append($"//DA/{Sent.ToFormattedDate()}");
            sb.Append($"//TI/{Sent.ToFormattedTime()}");

            if (!FishingLicense.IsNullOrEmpty())
            {
                sb.Append($"//FL/{FishingLicense}");
            }
            if (!Message.IsNullOrEmpty())
            {
                sb.Append($"//MS/{Message}");
            }

            sb.Append("//ER//");

            return sb.ToString();
        }


        public override int GetHashCode()
        {
            return new { Id, SentFrom, Sent, RadioCallSignal, ErrorCode, SequenceNumber, MessageVersion, MessageStatus }.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as RETMessage;
            if (rhs == null) return false;
            return rhs.GetHashCode() == GetHashCode();
        }

        public static RETMessage ParseNAFFormat(int id, DateTime sent, IReadOnlyDictionary<string, string> values)
        {
            return new RETMessage(id, sent,
                values["RC"],
                values["RS"],
                values.ContainsKey("RE") ? values["RE"] : string.Empty,
                values.ContainsKey("SQ") ? int.Parse(values["SQ"]) : 0,
                values.ContainsKey("MV") ? int.Parse(values["MV"]) : 0,
                values.ContainsKey("FR") ? values["FR"] : "NOR",
                fishingLicense: values.ContainsKey("FL") ? values["FL"] : string.Empty,
                message: values.ContainsKey("MS") ? values["MS"] : string.Empty
                );
        }
    }
}
