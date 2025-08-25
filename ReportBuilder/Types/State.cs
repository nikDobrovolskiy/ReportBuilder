namespace ReportBuilder.Types;

/// <summary>
/// Состояния сессии.
/// </summary>
public static class State
{
    private static readonly string[] _states =
    [
        "Пауза", 
        "Готов",
        "Разговор",
        "Обработка",
        "Перезвон"
    ];

    public static string[] States => _states.ToArray();
}