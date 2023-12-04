using AdventOfCode._Shared;

namespace AdventOfCode.Day1;

public class Day1 : BaseDayWithPuzzleInput
{
    public override int Day => 1;
    public override string DayName => "Day 1: Trebuchet?!";

    public override async Task SolveChallenge1()
    {
        var puzzleInput = await GetPuzzleInput();

        var sumOfCalibrationValues = puzzleInput.Aggregate(0, (previousValue, input) =>
        {
            var firstDigit = input.First(char.IsDigit);
            var lastDigit = input.Last(char.IsDigit);

            var number = int.Parse(new[] { firstDigit, lastDigit });

            return previousValue + number;
        });

        Console.WriteLine($"Sum of calibration values is: {sumOfCalibrationValues}.");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = await GetPuzzleInput();

        var sumOfCalibrationValues = puzzleInput.Aggregate(0, (previousValue, input) => previousValue + GetCalibrationNumber(input));

        Console.WriteLine($"Sum of calibration values is: {sumOfCalibrationValues}.");
    }

    private int GetCalibrationNumber(string input)
    {
        var digits = "0123456789".ToCharArray();

        var firstDigitIndex = input.IndexOfAny(digits);
        int? firstNumber = null;

        if (firstDigitIndex != -1)
        {
            firstNumber = int.Parse(input[firstDigitIndex].ToString());
        }

        var lastDigitIndex = input.LastIndexOfAny(digits);
        int? lastNumber = null;

        if (firstDigitIndex != -1)
        {
            lastNumber = int.Parse(input[lastDigitIndex].ToString());
        }

        var words = Enum.GetValues(typeof(Numbers)).Cast<Numbers>();

        int? firstWordIndex = null;
        int? firstWordNumber = null;

        int? lastWordIndex = null;
        int? lastWordNumber = null;

        foreach (var word in words)
        {
            var firstIndex = input.IndexOf(word.ToString(), StringComparison.OrdinalIgnoreCase);

            if (firstIndex != -1 && (firstWordIndex is null || firstIndex < firstWordIndex))
            {
                firstWordIndex = firstIndex;
                firstWordNumber = (int)word;
            }

            var lastIndex = input.LastIndexOf(word.ToString(), StringComparison.OrdinalIgnoreCase);

            if (lastIndex != -1 && (lastWordIndex is null || lastIndex > lastWordIndex))
            {
                lastWordIndex = lastIndex;
                lastWordNumber = (int)word;
            }
        }

        var finalFirstNumber = firstDigitIndex == -1
            ? firstWordNumber!.Value
            : firstWordIndex is null
                ? firstNumber!.Value
                : firstDigitIndex < firstWordIndex!.Value
                    ? firstNumber!.Value
                    : firstWordNumber!.Value;

        var finalLastNumber = lastDigitIndex == -1
            ? lastWordNumber!.Value
            : lastWordIndex is null
                ? lastNumber!.Value
                : lastDigitIndex > lastWordIndex!.Value
                    ? lastNumber!.Value
                    : lastWordNumber!.Value;

        return int.Parse(finalFirstNumber.ToString() + finalLastNumber.ToString());
    }

    private enum Numbers
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9
    }
}