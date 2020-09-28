using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="lineNo">Ignore, used by compiler services.</param>
        /// <param name="memberName">Ignore, used by compiler services.</param>
        /// <param name="filePath">Ignore, used by compiler services.</param>
        public void Log(
            string message,
            LogEntryType type = LogEntryType.Log)
        {
            Log(new LogEntry(message, type));
        }

        public void Log(
            LogEntry entry)
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

    public class DiagnosticLogEntry : LogEntry
    {
        public int CallingLineNo { get; set; }
        public string CallingMemberName { get; set; }
        public string CallingFilePath { get; set; }

        public override string ToString() => $"[{Timestamp}][{Type}][{CallingFilePath}:{CallingLineNo} ({CallingMemberName})]: {RawMessage}";

        public DiagnosticLogEntry(
            string rawMessage,
            LogEntryType type = LogEntryType.Error,
            [CallerLineNumber] int lineNo = 0,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null)
            : base(rawMessage, type)
        {
            CallingLineNo = lineNo;
            CallingMemberName = memberName;
            CallingFilePath = filePath;
        }

        public DiagnosticLogEntry(
            LogEntry entry,
            [CallerLineNumber] int lineNo = 0,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null)
            : base(entry.RawMessage, entry.Type)
        {
            Timestamp = entry.Timestamp;
            CallingLineNo = lineNo;
            CallingMemberName = memberName;
            CallingFilePath = filePath;
        }

        public DiagnosticLogEntry(
            Exception ex,
            [CallerLineNumber] int lineNo = 0,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null)
            : base(ex.ToString(), LogEntryType.Error)
        {
            CallingLineNo = lineNo;
            CallingMemberName = memberName;
            CallingFilePath = filePath;
        }

        public DiagnosticLogEntry()
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