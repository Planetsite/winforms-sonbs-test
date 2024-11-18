using Microsoft.Extensions.Logging;

namespace SonbsTest;

public sealed class StatusStripLogger : ILogger
{
    private readonly ToolStripStatusLabel _ssLog;

    public StatusStripLogger(ToolStripStatusLabel ssLabel)
        => _ssLog = ssLabel;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => null;

    public bool IsEnabled(LogLevel logLevel)
        => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var msg = formatter(state, exception);
        if (logLevel > LogLevel.Information) msg = $"[{logLevel}] {msg}";
        _ssLog.Text = msg;
    }
}

public sealed class StatusStripLogger2<T> : ILogger<T>
{
    private readonly StatusStripLogger _logger;

    public StatusStripLogger2(StatusStripLogger logger)
        => _logger = logger;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => null;

    public bool IsEnabled(LogLevel logLevel)
        => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);
}
