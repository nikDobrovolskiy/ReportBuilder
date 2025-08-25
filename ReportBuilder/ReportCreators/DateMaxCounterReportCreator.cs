using ReportBuilder.Interfaces;
using ReportBuilder.Types;

namespace ReportBuilder.ReportCreators;

/// <summary>
/// Создаёт отчёт о максимальном количестве
/// одновременно существовавших сессий в каждый отдельный день.
/// </summary>
public class DateMaxCounterReportCreator : ISessionReportCreator
{
    private readonly Dictionary<DateTime, int> _dateCounter = new();
    private readonly IReportPrinter _reportPrinter;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="reportPrinter">Вывод отчёта.</param>
    public DateMaxCounterReportCreator(IReportPrinter reportPrinter)
    {
        _reportPrinter = reportPrinter;
    }

    /// <inheritdoc />
    public void Add(Session session)
    {
        var date = session.DateStart.Date;
        var dateEnd = session.DateEnd.Date;
        while (date <= dateEnd)
        {
            if (!_dateCounter.TryAdd(date, 1))
            {
                _dateCounter[date]++;
            }

            date = date.AddDays(1);
        }
    }

    /// <inheritdoc />
    public void Print()
    {
        _reportPrinter.PrintLine("\nДень\tКоличество сессий");
        foreach (var data in GetAll())
        {
            _reportPrinter.PrintLine(data);
        }
    }

    /// <inheritdoc />
    public IEnumerable<string> GetAll()
    {
        return _dateCounter
            .OrderBy(e => e.Key)
            .Select(date => $"{date.Key.ToShortDateString()}\t{date.Value}");
    }
}