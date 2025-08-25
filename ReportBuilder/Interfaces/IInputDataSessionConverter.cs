using ReportBuilder.Types;

namespace ReportBuilder.Interfaces;

/// <summary>
/// Интерфейс преобразования входных данных сессии.
/// </summary>
public interface IInputDataSessionConverter
{
    /// <summary>
    /// Преобразует входную строку в сессию.
    /// </summary>
    /// <param name="data">Строка сессии.</param>
    Session? Convert(string data);
}