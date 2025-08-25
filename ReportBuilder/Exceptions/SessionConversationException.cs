namespace ReportBuilder.Exceptions;

/// <summary>
/// Ошибки преобразования входных данных сессии.
/// </summary>
public static class SessionConversationException
{
    private static readonly string InvalidExceptionPrefix = "Ошибка конвертации входной строки.";

    public static string EmptyInputData =>
        "Постая строка входных данных";

    public static string InvalidDateStart(string input) =>
        InvalidExceptionPrefix + " Неверный формат даты начала сессии: " + "\"" + input + "\"";

    public static string InvalidDateEnd(string input) =>
        InvalidExceptionPrefix + " Неверный формат даты конца сессии: " + "\"" + input + "\"";

    public static string InvalidProject(string input) =>
        InvalidExceptionPrefix + " Неверный формат проекта: " + "\"" + input + "\"";

    public static string InvalidOperator(string input) =>
        InvalidExceptionPrefix + " Неверный формат оператора: " + "\"" + input + "\"";

    public static string InvalidState(string input) =>
        InvalidExceptionPrefix + " Неверный формат состояния: " + "\"" + input + "\"";

    public static string InvalidDuration(int input) =>
        InvalidExceptionPrefix + " Неверный формат длительности: " + "\"" + input + "\"";
}
