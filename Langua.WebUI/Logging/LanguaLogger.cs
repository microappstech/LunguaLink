using System.Management;
using System.Text;

namespace Langua.WebUI.Logging
{
    public class LanguaLoggerProvider: ILoggerProvider
    {
        private readonly IConfiguration _configuration;
        private readonly Func<string, bool> _logLevelFun;
        public LanguaLoggerProvider(IConfiguration configuration, Func<string,bool> logLevel)
        {
            this._configuration = configuration;
            this._logLevelFun = logLevel;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        ILogger ILoggerProvider.CreateLogger(string categoryName)
        {
            return new LanguaFileLogger(_configuration,_logLevelFun, categoryName);
        }
    }




    public class LanguaFileLogger : ILogger
    {
        readonly string _logPath;
        readonly int maxSize;
        long currentFileSize;
        Func<string, bool> LogLevelFunc;
        private readonly LogLevel _logLevel;
        string currentFilePath;
        public LanguaFileLogger(IConfiguration configuration, Func<string, bool> LogLevelFunc, string category)
        {
            _logPath = configuration["Logging:File:Path"];
            maxSize = configuration.GetValue<int>("Logging:File:MaxSizeFile");
            _ = Enum.TryParse<LogLevel>(configuration["Logging:LogLevel:Default"], out _logLevel);
            this.LogLevelFunc = LogLevelFunc;
            InitializeLogger(); 

        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (logLevel >= _logLevel);
        }   

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            var message = formatter(state, exception);
            message = $"[{DateTime.Now:dd/MM/yyyy  HH:mm:ss}] - {message}";
            await WriteLogToFile(message);

        }
        SemaphoreSlim SemaphoreSlimFile = new SemaphoreSlim(1, 1);
        private async Task WriteLogToFile(string Message)
        {
            try
            {
                await SemaphoreSlimFile.WaitAsync();
                if (currentFileSize >= maxSize)
                    _ = true;
                byte[] encodedText = Encoding.UTF8.GetBytes(Message+Environment.NewLine);

                string query = $"ASSOCIATORS OF {{CIM_DataFile.Name='{currentFilePath.Replace("\\", "\\\\")}'}} WHERE ResultClass=CIM_Process";

                using FileStream sourceStream = new FileStream(currentFilePath, FileMode.Append,FileAccess.Write,FileShare.ReadWrite, bufferSize: 4096, useAsync: true);
                await sourceStream.WriteAsync(encodedText);
                currentFileSize += encodedText.Length;

            }
            finally
            {
                SemaphoreSlimFile.Release();
            }


        }
        private void InitializeLogger()
        {
            string directory = Path.GetDirectoryName(_logPath)!;
            if(!Directory.Exists(directory)) 
                Directory.CreateDirectory(directory);
            currentFilePath = _logPath;
            currentFileSize = File.Exists(_logPath) ? new FileInfo(_logPath).Length : 0;


        }
        public string GetNextFilePath()
        {
            string baseFileName = Path.GetFileNameWithoutExtension(_logPath);
            string directory = Path.GetDirectoryName(_logPath);
            string extension = Path.GetExtension(_logPath);
            for(int index=0; ;index++)
            {
                string filePath = Path.Combine(directory, $"{baseFileName}_{index}{extension}");
                if (!File.Exists(filePath))
                {
                    return filePath;
                }
            }
        }
    }
}
