using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using MySqlConnector;
using Serilog.Core;
using Serilog.Events;

namespace OguzlarBelediyesi.Infrastructure.Logging;

public sealed class MySqlLogSink : ILogEventSink, IDisposable
{
    private readonly string _connectionString;
    private readonly string _tableName;
    private readonly IFormatProvider _formatProvider;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public MySqlLogSink(string connectionString, string tableName = "SerilogLogs", IFormatProvider? formatProvider = null)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _tableName = tableName;
        _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent is null)
        {
            return;
        }

        var result = new ConcurrentDictionary<string, string>();
        foreach (var property in logEvent.Properties)
        {
            result[property.Key] = property.Value.ToString(format: null, formatProvider: _formatProvider);
        }

        var serializedProperties = JsonSerializer.Serialize(result, _jsonOptions);

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $@"
INSERT INTO `{_tableName}` (
    `Timestamp`, `Level`, `MessageTemplate`, `RenderedMessage`, `Exception`, `Properties`, `Endpoint`, `DurationMs`, `Username`
) VALUES (
    @Timestamp, @Level, @MessageTemplate, @RenderedMessage, @Exception, @Properties, @Endpoint, @DurationMs, @Username
)";
        var endpoint = ExtractStringProperty(logEvent, "Endpoint");
        var username = ExtractStringProperty(logEvent, "Username");
        var durationMs = ExtractDurationMilliseconds(logEvent);

        command.Parameters.AddWithValue("@Timestamp", logEvent.Timestamp.UtcDateTime);
        command.Parameters.AddWithValue("@Level", logEvent.Level.ToString());
        command.Parameters.AddWithValue("@MessageTemplate", logEvent.MessageTemplate.Text);
        command.Parameters.AddWithValue("@RenderedMessage", logEvent.RenderMessage(_formatProvider));
        command.Parameters.AddWithValue("@Exception", logEvent.Exception?.ToString() ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Properties", serializedProperties);
        command.Parameters.AddWithValue("@Endpoint", endpoint ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@DurationMs", durationMs.HasValue ? durationMs.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@Username", username ?? (object)DBNull.Value);
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
    }

    private string? ExtractStringProperty(LogEvent logEvent, string propertyName)
    {
        if (logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            return value.ToString(format: null, formatProvider: _formatProvider);
        }

        return null;
    }

    private static long? ExtractDurationMilliseconds(LogEvent logEvent)
    {
        if (logEvent.Properties.TryGetValue("DurationMs", out var durationValue))
        {
            return TryConvertToLong(durationValue);
        }

        if (logEvent.Properties.TryGetValue("ElapsedMilliseconds", out var elapsedValue))
        {
            return TryConvertToLong(elapsedValue);
        }

        return null;
    }

    private static long? TryConvertToLong(LogEventPropertyValue? value)
    {
        if (value is ScalarValue scalar && scalar.Value is IConvertible convertible)
        {
            try
            {
                return convertible.ToInt64(CultureInfo.InvariantCulture);
            }
            catch
            {
            }
        }

        return null;
    }
}
