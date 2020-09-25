using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Webshot
{
    public class Logger : ILogProcessor
    {
        public event EventHandler<LoggerEventArgs> Logged;

        private readonly ILogProcessor[] _processors;

        public Logger(params ILogProcessor[] processors)
            : this(
                LogEntryType.Log | LogEntryType.Warning | LogEntryType.Error,
                processors)
        {
        }

        public Logger(LogEntryType typeFilter, params ILogProcessor[] processors)
        {
            _processors = processors;
            TypeFilter = typeFilter;
        }

        public readonly static Logger Default = new Logger(new DebugLogger());

        public virtual LogEntryType TypeFilter { get; set; } =
            LogEntryType.Log | LogEntryType.Warning | LogEntryType.Error;

        public void Log(string message, LogEntryType type = LogEntryType.Log)
        {
            Log(new LogEntry(message, type));
        }

        public void Log(LogEntry entry)
        {
            if (!TypeFilter.HasFlag(entry.Type)) return;

            _processors.ForEach(TryProcessEntry);

            var args = new LoggerEventArgs(entry);
            Logged?.Invoke(this, args);

            void TryProcessEntry(ILogProcessor processor)
            {
                try
                {
                    processor.Log(entry);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to log entry {entry}. Error: {ex.Message}");
                }
            }
        }
    }

    public interface ILogProcessor
    {
        void Log(LogEntry entry);
    }

    [Flags]
    public enum LogEntryType { Log = 0b1, Warning = 0b10, Error = 0b100 }

    public class LoggerEventArgs : EventArgs
    {
        public LogEntry Entry { get; }

        public LoggerEventArgs(LogEntry entry)
        {
            Entry = entry;
        }
    }

    public class LogEntry
    {
        public LogEntryType Type { get; set; }
        public string RawMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public override string ToString() => $"[{Timestamp}][{Type}]: {RawMessage}";

        public LogEntry(string rawMessage, LogEntryType type = LogEntryType.Log)
        {
            RawMessage = rawMessage;
            Type = type;
        }

        public LogEntry()
        {
        }
    }

    public class FileLogger : ILogProcessor
    {
        public string FilePath { get; }

        public FileLogger(string filePath)
        {
            FilePath = filePath;
        }

        public FileLogger(FileProjectStore projectStore)
        {
            FilePath = Path.Combine(projectStore.ProjectDir, $"{nameof(Webshot)}.log");
        }

        public void Log(LogEntry entry)
        {
            string entryText = Environment.NewLine + entry.ToString();
            File.AppendAllText(FilePath, entryText);
        }
    }

    public class ActionLogger : ILogProcessor
    {
        private readonly Action<LogEntry> _action;

        public ActionLogger(Action<LogEntry> action)
        {
            _action = action;
        }

        public void Log(LogEntry entry)
        {
            _action?.Invoke(entry);
        }
    }

    public class DebugLogger : ILogProcessor
    {
        public void Log(LogEntry entry)
        {
            System.Diagnostics.Debug.WriteLine(entry.ToString());
        }
    }
}