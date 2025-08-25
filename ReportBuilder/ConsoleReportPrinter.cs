using ReportBuilder.Interfaces;

namespace ReportBuilder;

/// <summary>
/// Создаёт.
/// </summary>
public class ConsoleReportPrinter : IReportPrinter
{
    /// <inheritdoc />
    public void PrintLine(string line)
    {
        Console.WriteLine(line);
    }
}