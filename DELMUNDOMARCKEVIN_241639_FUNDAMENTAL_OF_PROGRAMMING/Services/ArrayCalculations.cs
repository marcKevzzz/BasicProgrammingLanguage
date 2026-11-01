using System.Globalization;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

public static class ArrayCalculations
{
    public static bool TryNumbers(string? input, out decimal[] values)
    {
        values = [];
        if (string.IsNullOrWhiteSpace(input) || input.Length > 3000) return false;
        var tokens = input.Split(new[] { ',', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length is < 1 or > 100) return false;
        values = new decimal[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
            if (!decimal.TryParse(tokens[i], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out values[i]) || values[i] < -1000000 || values[i] > 1000000) return false;
        return true;
    }


    public static (string Result, bool Passed) GradeAverage(decimal[] values)
    {
        decimal total = 0;
        for (int i = 0; i < values.Length; i++) total += values[i];
        decimal average = total / values.Length;
        average = Math.Round(average, 2, MidpointRounding.AwayFromZero);
        var equivalent = average switch
        {
            >= 96.50m => (1.00m, "Excellent"),
            >= 92.50m => (1.25m, "Very Good"),
            >= 88.50m => (1.50m, "Very Good"),
            >= 84.50m => (1.75m, "Good"),
            >= 80.50m => (2.00m, "Good"),
            >= 76.50m => (2.25m, "Satisfactory"),
            >= 72.50m => (2.50m, "Satisfactory"),
            >= 68.50m => (2.75m, "Fair"),
            >= 64.50m => (3.00m, "Fair"),
            _ => (5.00m, "Poor / Failed")
        };
        string remarks = average >= 64.50m ? "Passed" : "Failed";
        string result = FormattableString.Invariant($"General weighted average: {average:0.00}%\nNumerical grade: {equivalent.Item1:0.00}\nDescriptive rating: {equivalent.Item2}\nRemarks: {remarks}");
        return (result, average >= 64.50m);
    }

    public static string BubbleSort(decimal[] input, string direction)
    {
        var values = (decimal[])input.Clone();
        for (int pass = 0; pass < values.Length - 1; pass++)
        {
            for (int i = 0; i < values.Length - 1 - pass; i++)
            {
                if (direction == "ascending" ? values[i] > values[i + 1] : values[i] < values[i + 1])
                {
                    decimal temporary = values[i];
                    values[i] = values[i + 1];
                    values[i + 1] = temporary;
                }
            }
        }
        return (direction == "descending" ? "Descending: " : "Ascending: ") +
            string.Join(", ", values.Select(value => value.ToString(CultureInfo.InvariantCulture)));
    }
}
