using ReportBuilder;
using ReportBuilder.Interfaces;
using ReportBuilder.ReportCreators;

#region Add arguments
if (args.Length == 0)
{
    Console.WriteLine("При вызове программы в аргументах должен быть указан путь до файла");
    return -1;
}

var file = args[0];
if (!File.Exists(file))
{
    Console.WriteLine("Не найден указанный файл: " + file);
    return -1;
}

var logOn = false;
if (args.Length > 1)
{
    bool.TryParse(args[1], out logOn);
}
#endregion

#region DI
IReportPrinter reportPrinter = new ConsoleReportPrinter();
var reportCreators = new List<ISessionReportCreator>
{
    new DateMaxCounterReportCreator(reportPrinter),
    new OperatorStatesCounterReportCreator(reportPrinter)
};
var reportController = new ReportController(
    file,
    new InputDataSessionConverter(logOn),
    reportCreators);
#endregion

#region Report
reportController.Create();
reportController.Print();
#endregion

return 0;