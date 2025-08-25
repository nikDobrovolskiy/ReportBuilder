namespace ReportBuilder;

/// <summary>
/// Источник входных данных
/// </summary>
public class SessionSource : ISessionSource
{
    private readonly StreamReader _reader;

    public SessionSource(string connectionString)
    {
        if (!File.Exists(connectionString))
        {
            throw new ArgumentException(nameof(connectionString));
        }

        _reader = new StreamReader(connectionString);
    }

    /// <inheritdoc />
    public IEnumerable<string> GetItem()
    {
        string line;
        while ((line = _reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    public void Dispose()
    {
        _reader.Dispose();
    }
}

public interface ISessionSource : IDisposable
{
    IEnumerable<string> GetItem();
}

