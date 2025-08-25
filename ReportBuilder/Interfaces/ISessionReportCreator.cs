using ReportBuilder.Types;

namespace ReportBuilder.Interfaces;

/// <summary>
/// Интерфейс создателя отчёта.
/// </summary>
public interface ISessionReportCreator
{
    /// <summary>
    /// Обрабатывает сессию для построения отчёта.
    /// </summary>
    /// <param name="session">Сессия.</param>
    void Add(Session session);

    /// <summary>
    /// Запрос всех данных в виде строки.
    /// </summary>
    IEnumerable<string> GetAll();

    /// <summary>
    /// Выводит отчёт.
    /// </summary>
    void Print();
}

