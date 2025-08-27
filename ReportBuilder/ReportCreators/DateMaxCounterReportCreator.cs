using ReportBuilder.Interfaces;
using ReportBuilder.Types;

namespace ReportBuilder.ReportCreators;

/// <summary>
/// Создаёт отчёт о максимальном количестве
/// одновременно существовавших сессий в каждый отдельный день.
/// </summary>
public class DateMaxCounterReportCreator : ISessionReportCreator
{
    private const int SessionDatesListCount = 20000;
    private readonly Dictionary<DateTime, List<SessionDates>> _sessionsByDay = new();
    private readonly Dictionary<DateTime, int> _daySessionCount = new();
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
        var start = session.DateStart;
        var end = session.DateEnd;

        var date = start;
        while (date.Date <= end.Date)
        {
            if (_sessionsByDay.TryGetValue(date.Date, out var dayWithSession))
            {
                dayWithSession.Add(session);
            }
            else
            {
                _sessionsByDay.Add(date.Date, new List<SessionDates>(SessionDatesListCount) { session });
            }

            date = date.AddDays(1);
        }
    }

    /// <inheritdoc />
    public void Print()
    {
        var daysWithCounter = GetAll().ToList();
        _reportPrinter.PrintLine("\nДень\tКоличество сессий");
        foreach (var dayWithCounter in daysWithCounter)
        {
            _reportPrinter.PrintLine(dayWithCounter);
        }
    }

    /// <inheritdoc />
    public IEnumerable<string> GetAll()
    {
        _sessionsByDay.AsParallel().ForAll(dayWithSessions =>
        {
            var sessions = dayWithSessions.Value;
            var dayStart = dayWithSessions.Key;
            var dayEnd = dayStart.AddDays(1).AddTicks(-1);
            var measurePoints = sessions
                .Where(e =>
                {
                    var s = e.DateStart;
                    var eEnd = e.DateEnd;
                    return (s >= dayStart && s <= dayEnd) || (eEnd >= dayStart && eEnd <= dayEnd);
                })
                .Select(e =>
                {
                    var s = e.DateStart;
                    var eEnd = e.DateEnd;
                    return (s >= dayStart && s <= dayEnd) ? s : eEnd;
                })
                .ToList();

            if (measurePoints.Count == 0)
            {
                // если все интервали начинаются и заканчиваются не в этот день
                measurePoints.Add(dayStart);
            }

            var maxIntersectionCount = measurePoints
                .AsParallel()
                .Select(point => sessions.Count(session => Intersect(point, session)))
                .Max();

            if (!_daySessionCount.TryAdd(dayStart, maxIntersectionCount))
            {
                if (maxIntersectionCount > _daySessionCount[dayStart])
                {
                    _daySessionCount[dayStart] = maxIntersectionCount;
                }
            }
        });
        
        return _daySessionCount
            .OrderBy(e => e.Key)
            .Select(date => $"{date.Key.ToShortDateString()}\t{date.Value}");
    }

    private static bool Intersect(DateTime dateTime, SessionDates sessions)
    {
        return dateTime >= sessions.DateStart && dateTime <= sessions.DateEnd;
    }
}