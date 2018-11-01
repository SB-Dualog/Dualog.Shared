using System.Globalization;

namespace Dualog.eCatch.Shared
{
    public static class Constants
    {
        public static readonly NumberFormatInfo IntNumberFormat = new NumberFormatInfo {NumberGroupSeparator = " ", NumberDecimalDigits = 0};
        public static class Headers
        {
            public const string ErrorCode = "ErrorCode";
        }

        public static class MessageErrorCodes
        {
            public const string CorrectionCode = "511";
            public const string CancelationCode = "521";
            /// <summary>
            /// RET message error codes that will tell if the message has been cancelled/corrected as NAK or ACK
            /// When getting RET messages, we need to pair them with the messages that we set as cancelled or corrected in order to distinguish them from the original message with the same record number.
            /// </summary>
            public static readonly string[] CodesForCorrectedOrCancelledMessage = {"100", "501", "505", "512", "513", "522", "523"};
        }

        public static class HiReturnCodes
        {
            public const string TripSelectedForSampling = "631";
            public const string TripNotSelectedForSampling = "632";
            public const string CatchSelectedForSampling = "641";
            public const string CatchNotSelectedForSampling = "642";
            public const string CatchSamplingIsOverForTrip = "643";
        }

        public static class Zones
        {
            public const string Norway = "NOR";
            public const string Svalbard = "XSV";
            public const string EU = "XEU";
            public const string Russia = "RUS";
            public const string NEAFC = "XNE";
            public const string Island = "ISL";
            public const string FaroeIslands = "FRO";
            public const string SvalbardTerritorialWaters = "XSI";
            public const string JanMayenFishingZone = "XJM";
            public const string Skagerrak = "XSK";
            public const string Havforskningsinstituttet = "ZZH";
        }

        public static class SpecifiedToolNeeds
        {
            /// <summary>
            /// ME - Mask width given in MM. This array contains all tool codes that needs to set the ME field.
            /// </summary>
            public static readonly string[] MaskWidth = {"OTB", "TBS", "SSC", "GEN", "OTM", "PS1"};
            /// <summary>
            /// FO - Number of hooks. This array contains all tool codes that needs to set the FO field
            /// </summary>
            public static readonly string[] Hooks = {"LL"};
            /// <summary>
            /// FO - Number of traps. This array contains all tool codes that needs to set the FO field
            /// </summary>
            public static readonly string[] Traps = {"FPO"};
            /// <summary>
            /// FO - Yarn in meters. This array contains all tool codes that needs to set the FO field
            /// </summary>
            public static readonly string[] Yarn = {"GEN"};
            /// <summary>
            /// GS - Number of trawls, 1 to 3. This array contains all tool codes that need to set the GS field
            /// </summary>
            public static readonly string[] Trawl = {"OTB", "TBS", "OTM"};
        }

        public static class ZoneRequires
        {
            /// <summary>
            /// Zones requiring the LT and LG fields. LT using the format +/- DD.ddd (WGS-84), and LG using the format +/- DDD.ddd (WGS-84)
            /// </summary>
            public static readonly string[] LtLg = {Zones.EU, Zones.FaroeIslands, Zones.Island, Zones.Russia};

            /// <summary>
            /// Zones requiring the LA and LO fields. LA using the format N/SGGDD (WGS-84), and LO using the format E/WGGGDD (WGS-84)
            /// </summary>
            public static readonly string[] LaLo = {Zones.Norway, Zones.Svalbard, Zones.NEAFC, Zones.SvalbardTerritorialWaters};
        }
    }
}