using ReportBuilder.Interfaces;

namespace ReportBuilder;

/// <summary>
/// Выводит данные в консоль.
/// </summary>
public class ConsoleReportPrinter : IReportPrinter
{
    /// <inheritdoc />
    public void PrintLine(string line)
    {
        Console.WriteLine(line);
    }
}