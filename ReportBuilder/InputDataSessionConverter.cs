using ReportBuilder.Exceptions;
using ReportBuilder.Interfaces;
using ReportBuilder.Types;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ReportBuilder;

/// <summary>
/// Преобразователь входных данных в сессию.
/// </summary>
public class InputDataSessionConverter : IInputDataSessionConverter
{
    private readonly bool _switchLogOn;
    private readonly HashSet<string> _stateTypes = State.States.ToHashSet();

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="switchLogOn">Включить логирование.</param>
    public InputDataSessionConverter(bool switchLogOn)
    {
        _switchLogOn = switchLogOn;
    }

    /// <inheritdoc />
    public Session? Convert(string data)
    {
        var parts = data.Split(";");
        if (parts.Length == 0)
        {
            Console.WriteLine(SessionConversationException.EmptyInputData);
            return null;
        }

        if (!DateTime.TryParseExact(
                parts[(int)SessionDataIndex.DateStart],
                "dd.MM.yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out var dateStart))
        {
            if (_switchLogOn)
            {
                Console.WriteLine(
                    SessionConversationException.InvalidDateStart(parts[(int)SessionDataIndex.DateStart]));
            }

            return null;
        }

        if (!DateTime.TryParseExact(
                parts[(int)SessionDataIndex.DateEnd],
                "dd.MM.yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out var dateEnd))
        {
            if (_switchLogOn)
            {
                Console.WriteLine(
                    SessionConversationException.InvalidDateEnd(parts[(int)SessionDataIndex.DateEnd]));
            }
            
            return null;
        }

        var project = parts[(int)SessionDataIndex.Project];

        var operatorName = parts[(int)SessionDataIndex.Operator];
        const string operatorNameMathPattern = @"^[^\d]*$";
        if (string.IsNullOrEmpty(operatorName) || !Regex.IsMatch(operatorName, operatorNameMathPattern))
        {
            if (_switchLogOn)
            {
                Console.WriteLine(
                    SessionConversationException.InvalidOperator(operatorName));
            }
            
            return null;
        }

        var state = parts[(int)SessionDataIndex.State];
        if (!_stateTypes.Contains(state))
        {
            if (_switchLogOn)
            {
                Console.WriteLine(
                    SessionConversationException.InvalidState(state));
            }
            
            return null;
        }

        if (!int.TryParse(parts[(int)SessionDataIndex.Duration], out var duration))
        {
            if (_switchLogOn)
            {
                Console.WriteLine(
                    SessionConversationException.InvalidDuration(duration));
            }
            
            return null;
        }

        return new Session(dateStart,
            dateEnd,
            project,
            operatorName,
            state,
            duration);
    }

    private enum SessionDataIndex
    {
        DateStart,
        DateEnd,
        Project,
        Operator,
        State,
        Duration
    }
}