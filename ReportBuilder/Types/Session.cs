namespace ReportBuilder.Types;

/// <summary>
/// Сессия.
/// </summary>
/// <param name="DateStart">Дата и время начала сессии.</param>
/// <param name="DateEnd">Дата и время окончания сессии.</param>
/// <param name="Project">Проект, на котором работает оператор.</param>
/// <param name="Operator">ФИО.</param>
/// <param name="State">Состояние.</param>
/// <param name="Duration">Длительность.</param>
public record Session(
    DateTime DateStart,
    DateTime DateEnd,
    string? Project,
    string Operator,
    string State,
    int Duration);

