namespace Tekton.ProductAPI.Infrastructure.Logger
{
    public interface IAPILogger
    {
        /// <summary>
        /// Write debug information to log storage with current datetime stamp.
        /// </summary>
        /// <param name="AMessage"></param>
        void LogDebug(string AMessage);

        /// <summary>
        /// Write error message to log storage with current datetime stamp.
        /// </summary>
        /// <param name="AMessage"></param>
        void LogError(string AMessage);

        /// <summary>
        /// Write information message to log storage with current datetime stamp.
        /// </summary>
        /// <param name="AMessage"></param>
        void LogInfo(string AMessage);

        /// <summary>
        /// Write warning message to log storage with current datetime stamp.
        /// </summary>
        /// <param name="AMessage"></param>
        void LogWarn(string AMessage);

        /// <summary>
        /// Write fatal error information to log storage with current datetime stamp.
        /// </summary>
        /// <param name="AMessage"></param>
        void LogFatal(string AMessage);
    }
}
