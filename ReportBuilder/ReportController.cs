using ReportBuilder.Interfaces;
using ReportBuilder.Types;

namespace ReportBuilder;

/// <summary>
/// Контроллер работы с отчётами.
/// </summary>
public class ReportController
{
    private readonly string _connectionString;
    private readonly IInputDataSessionConverter _inputDataConverter;
    private readonly List<ISessionReportCreator> _reportCreators;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="connectionString">Строка источника данных.</param>
    /// <param name="inputDataConverter">Конвертор входных данных.</param>
    /// <param name="reportCreators">Создатели отчётов.</param>
    public ReportController(
        string connectionString,
        IInputDataSessionConverter inputDataConverter,
        List<ISessionReportCreator> reportCreators)
    {
        _connectionString = connectionString;
        _inputDataConverter = inputDataConverter;
        _reportCreators = reportCreators;
    }

    /// <summary>
    /// Создаёт отчёты.
    /// </summary>
    public void Create()
    {
        using ISessionSource sourceData = new SessionSource(_connectionString);
        Session? session;
        foreach (var data in sourceData.GetItem())
        {
            session = _inputDataConverter.Convert(data);
            if (session == null)
            {
                continue;
            }

            foreach (var sessionReportCreator in _reportCreators)
            {
                sessionReportCreator.Add(session);
            }
        }
    }
    
    /// <summary>
    /// Выводит отчёты.
    /// </summary>
    public void Print()
    {
        foreach (var sessionReportCreator in _reportCreators)
        {
            sessionReportCreator.Print();
        }
    }
}