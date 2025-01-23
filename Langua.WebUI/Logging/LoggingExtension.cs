namespace Langua.WebUI.Logging
{
    public static class LoggingExtension
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new LanguaLoggerProvider(configuration, (cate) => true));
            return builder;
        }
    }
}
