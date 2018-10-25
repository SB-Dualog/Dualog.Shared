using Dualog.eCatch.Shared.Messages;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class MessageExtensions
    {
        /// <summary>
        /// When sending a HIA message, if the return message returns the code "631" it means that the current trip needs
        /// to send HIF and HIL messages.
        /// </summary>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static bool TripSelectedForSampling(this RETMessage retMessage)
        {
            return retMessage.ErrorCode == "631";
        }

        /// <summary>
        /// If the RET message for a HIF message returns the code "641" it means that this catch needs to have a sample delivered
        /// </summary>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static bool CatchSelectedForSampling(this RETMessage retMessage)
        {
            return retMessage.ErrorCode == "641";
        }

        /// <summary>
        /// If the RET message for a HIF message returns the code "642" it means that this catch does not need to create a sample
        /// </summary>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static bool CatchNotSelectedForSampling(this RETMessage retMessage)
        {
            return retMessage.ErrorCode == "642";
        }

        /// <summary>
        /// If the RET message for a HIF message returns the code "643", there is no need to send in any more HIF messages
        /// for current trip
        /// </summary>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static bool CatchSamplingIsOverForTrip(this RETMessage retMessage)
        {
            return retMessage.ErrorCode == "643";
        }
    }
}
