namespace MyRetailApi
{
    public static class LoggingExtensions
    {
        public static void LogErrorMessageAndStack(this ILogger logger, Exception ex)
        {
            logger.LogError("{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
