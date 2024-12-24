using Microsoft.ML;
using Microsoft.ML.Data;

// See https://aka.ms/new-console-template for more information
public class VisitData
{
    public float NumberOfVisitors { get; set; }

    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }

    public DateTime Date => new DateTime(Year, Month, Day);

    public string ToCsv()
    {
        return $"{NumberOfVisitors};{Year};{Month};{Day}";

    }
    public static VisitData FromCsv(string csvLine)
    {
        var parts = csvLine.Split(';');
        return new VisitData
        {
            NumberOfVisitors = float.Parse(parts[0]),
            Year = int.Parse(parts[1]),
            Month = int.Parse(parts[2]),
            Day = int.Parse(parts[3])
        };
    }

    public string CsvHeader => "NumberOfVisitors;Year;Month;Day";

    public class VisitDataPrediction
    {
        [ColumnName("Score")]
        public float PredictedVisitors { get; set; }
    }

    public static float PredictVisitorsFastTree(int year, int month, int day, IEnumerable<VisitData> historicalData)
    {
        var context = new MLContext();
        var data = context.Data.LoadFromEnumerable(historicalData);

        var pipeline = context.Transforms.Conversion.ConvertType(new[]
            {
                new InputOutputColumnPair("Year", "Year"),
                new InputOutputColumnPair("Month", "Month"),
                new InputOutputColumnPair("Day", "Day")
            }, DataKind.Single)
            .Append(context.Transforms.Concatenate("Features", "Year", "Month", "Day"))
            .Append(context.Transforms.CopyColumns("Label", "NumberOfVisitors"))
            .Append(context.Regression.Trainers.FastTree());

        var model = pipeline.Fit(data);

        var predictionEngine = context.Model.CreatePredictionEngine<VisitData, VisitDataPrediction>(model);

        var newVisit = new VisitData { Year = year, Month = month, Day = day };
        var prediction = predictionEngine.Predict(newVisit);

        return prediction.PredictedVisitors;
    }

    public static float PredictVisitorsLightGbm(int year, int month, int day, IEnumerable<VisitData> historicalData)
    {
        var context = new MLContext();
        var data = context.Data.LoadFromEnumerable(historicalData);

        var pipeline = context.Transforms.Conversion.ConvertType(new[]
            {
                new InputOutputColumnPair("Year", "Year"),
                new InputOutputColumnPair("Month", "Month"),
                new InputOutputColumnPair("Day", "Day")
            }, DataKind.Single)
            .Append(context.Transforms.Concatenate("Features", "Year", "Month", "Day"))
            .Append(context.Transforms.CopyColumns("Label", "NumberOfVisitors"))
            .Append(context.Regression.Trainers.LightGbm());

        var model = pipeline.Fit(data);

        var predictionEngine = context.Model.CreatePredictionEngine<VisitData, VisitDataPrediction>(model);

        var newVisit = new VisitData { Year = year, Month = month, Day = day };
        var prediction = predictionEngine.Predict(newVisit);

        return prediction.PredictedVisitors;
    }
}