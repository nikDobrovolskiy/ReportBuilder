namespace ReportBuilder.Interfaces;

/// <summary>
/// Интерфейс вывода отчёта.
/// </summary>
public interface IReportPrinter
{
    /// <summary>
    /// Выводить строку отчёта.
    /// </summary>
    /// <param name="line">Строка отчёта.</param>
    void PrintLine(string line);
}