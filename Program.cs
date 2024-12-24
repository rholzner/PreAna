
Console.WriteLine("Hello, World!");

//DataHelper.Exe();

var date = new DateTime(2025, 1, 8);
var data = DataHelper.ReadData();
var sameDayData = data.Where(x => x.Date.DayOfWeek == date.DayOfWeek);

var last5Days = sameDayData.OrderByDescending(x => x.Date).Take(5).OrderBy(x => x.Date);
foreach (var item in last5Days)
{
    Console.WriteLine($"Last 5 days: {item.Date.ToShortDateString()} - {item.Date.DayOfWeek}: {item.NumberOfVisitors}");
}

Console.WriteLine();
//RunPredictionFastTree(date, data);
RunPredictionFastTree(date, sameDayData);
//RunPredictionLightGbm(date, data);
RunPredictionLightGbm(date, sameDayData);

static void RunPredictionFastTree(DateTime date, IEnumerable<VisitData> data)
{
    float predictedVisitors = VisitData.PredictVisitorsFastTree(date.Year, date.Month, date.Day, data);
    Console.WriteLine($"Fasttree Predicted number of visitors on {date.ToShortDateString()} - {date.DayOfWeek}: {predictedVisitors}");
}

static void RunPredictionLightGbm(DateTime date, IEnumerable<VisitData> data)
{
    float predictedVisitors = VisitData.PredictVisitorsLightGbm(date.Year, date.Month, date.Day, data);
    Console.WriteLine($"LightGbm Predicted number of visitors on {date.ToShortDateString()} - {date.DayOfWeek}: {predictedVisitors}");
}