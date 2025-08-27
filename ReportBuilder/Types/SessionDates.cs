namespace ReportBuilder.Types;

/// <summary>
/// Даты сессии.
/// </summary>
/// <param name="DateStart">Дата и время начала сессии.</param>
/// <param name="DateEnd">Дата и время окончания сессии.</param>
public record SessionDates(
    DateTime DateStart,
    DateTime DateEnd);

