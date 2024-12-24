// See https://aka.ms/new-console-template for more information
public static class DataHelper
{
    public static string fileName = "data.csv";
    public static string FilePath = Path.Combine("C:\\Users\\robin\\Documents\\GitHub\\PreAna", fileName);
    public static void Exe()
    {

        var data = DataHelper.GenerateData().ToList();
        File.WriteAllLines(FilePath, new[] { data.First().CsvHeader });
        File.AppendAllLines(FilePath, data.Select(x => x.ToCsv()));

        Console.WriteLine("Data written to file");
    }

    public static IEnumerable<VisitData> ReadData()
    {
        return File.ReadAllLines(FilePath)
            .Skip(1)
            .Select(VisitData.FromCsv);
    }

    public static IEnumerable<VisitData> GenerateData()
    {
        var startDate = new DateTime(2022, 1, 1);
        var endDate = new DateTime(2023, 1, 1);

        var a = CreatePeriodData(startDate, endDate, 70);
        foreach (var item in a)
        {
            yield return item;
        }

        startDate = new DateTime(2023, 1, 1);
        endDate = new DateTime(2024, 1, 1);
        var b = CreatePeriodData(startDate, endDate, 90);
        foreach (var item in b)
        {
            yield return item;
        }
    }

    public static IEnumerable<VisitData> CreatePeriodData(DateTime startDate, DateTime endDate, float baseVisitors)
    {
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var min = baseVisitors * 0.9f;
            var max = baseVisitors * 1.1f;
            var rnd = new Random();
            var visitors = rnd.Next((int)min, (int)max);
            var baseToUse = 0f;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    baseToUse = visitors * 0.9f;
                    break;
                case DayOfWeek.Tuesday:
                    baseToUse  = visitors * 0.8f;
                    break;
                case DayOfWeek.Wednesday:
                    baseToUse = visitors * 0.5f;
                    break;
                case DayOfWeek.Thursday:
                    baseToUse = visitors * 1.1f;
                    break;
                case DayOfWeek.Friday:
                    baseToUse = visitors * 1.3f;
                    break;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                default:
                    break;
            }
            yield return new VisitData
            {
                NumberOfVisitors = baseToUse,
                Year = date.Year,
                Month = date.Month,
                Day = date.Day
            };
        }
    }

}
