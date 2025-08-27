using ReportBuilder.Interfaces;
using ReportBuilder.Types;
using System.Text;

namespace ReportBuilder.ReportCreators;

/// <summary>
/// Создаёт отчёт о времени, проведённом каждым оператором в каждом состоянии. 
/// </summary>
public class OperatorStatesCounterReportCreator : ISessionReportCreator
{
    private readonly Dictionary<string, Dictionary<string, int>> _nameCounter = new ();
    private readonly Dictionary<string, int> _stateTypes = 
        State.States.ToDictionary(state => state, _ => 0);
    private readonly IReportPrinter _reportPrinter;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="reportPrinter">Вывод отчёта.</param>
    public OperatorStatesCounterReportCreator(IReportPrinter reportPrinter)
    {
        _reportPrinter = reportPrinter;
    }

    /// <inheritdoc />
    public void Add(Session session)
    {
        var operatorName = session.Operator;
        var state = session.State;

        _nameCounter.TryAdd(operatorName, _stateTypes.ToDictionary());

        if (!_nameCounter[operatorName].TryAdd(state, session.Duration))
        {
            _nameCounter[operatorName][state] += session.Duration;
        }
    }

    /// <inheritdoc />
    public void Print()
    {
        var sb = new StringBuilder("\nФИО");
        _stateTypes.Select(e => e.Key)
            .ToList()
            .ForEach(x => sb.Append("\t" + x));
        _reportPrinter.PrintLine(sb.ToString());
        foreach (var data in GetAll())
        {
            _reportPrinter.PrintLine(data);
        }
    }

    /// <inheritdoc />
    public IEnumerable<string> GetAll()
    {
        return _nameCounter
            .OrderBy(e => e.Key)
            .Select(nameCount =>
            {
                var sb = new StringBuilder(nameCount.Key);
                var stateCounts = nameCount.Value
                    .Select(e => e.Value);
                foreach (var stateCount in stateCounts)
                {
                    sb.Append($"\t{stateCount}");
                }

                return sb.ToString();
            });
    }
}

