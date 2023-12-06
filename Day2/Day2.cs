using AdventOfCode._Shared;

namespace AdventOfCode.Day2;

public class Day2 : BaseDayWithPuzzleInput
{
    public override int Day => 2;
    public override string DayName => "Day 2: Cube Conundrum";

    private const string PuzzleInputError = "Puzzle input not in expected format.";

    public override async Task SolveChallenge1()
    {
        var puzzleInput = await GetPuzzleInput();

        ICollection<Game> games = puzzleInput.Select(ParseGame).ToList();

        var possibleGames = games.Where(g => g[Colours.Red].MinimumAmount <= 12 && g[Colours.Green].MinimumAmount <= 13 && g[Colours.Blue].MinimumAmount <= 14);

        Console.WriteLine($"Sum of game ids is: {possibleGames.Select(g => g.Id).Sum()}.");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = await GetPuzzleInput();

        ICollection<Game> games = puzzleInput.Select(ParseGame).ToList();

        var powerSum = games
            .Select(g => g.ColourStates.Select(cs => cs.MinimumAmount).Aggregate((long)0, (prev, next) => prev == 0 && next == 0 ? 0 : prev == 0 ? next : prev * next))
            .Sum();

        Console.WriteLine($"Sum of the power of every game is: {powerSum}.");
    }

    private static Game ParseGame(string puzzleInput)
    {
        var split = puzzleInput.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (split.Length != 2) throw new NotSupportedException(PuzzleInputError);

        var gameSplit = split[0].Split(" ");
        if (gameSplit.Length != 2) throw new NotSupportedException(PuzzleInputError);

        var game = new Game { Id = int.Parse(gameSplit[1]) };

        var hands = split[1].Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var hand in hands)
        {
            var gameHand = new Hand();
            game.Hands.Add(gameHand);

            foreach (var colourHand in hand.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var splitColourHand = colourHand.Split(" ");
                if (splitColourHand.Length != 2) throw new NotSupportedException(PuzzleInputError);

                var count = uint.Parse(splitColourHand[0]);
                var colour = Enum.GetValues<Colours>().FirstOrDefault(c => string.Equals(c.ToString(), splitColourHand[1], StringComparison.OrdinalIgnoreCase));

                gameHand.State.Add((colour, count));
            }
        }

        game.CalculateState();

        return game;
    }

    public class Game
    {
        public required int Id { get; init; }

        public ColourState this[Colours colour]
        {
            get => ColourStates.FirstOrDefault(c => c.Colour == colour) ?? new ColourState { Colour = colour, MinimumAmount = 0 };
        }

        public void CalculateState()
        {
            var colourStates = new HashSet<ColourState>();

            foreach (var grab in Hands)
            {
                foreach (var state in grab.State)
                {
                    var colourState = colourStates.FirstOrDefault(c => c.Colour == state.Colour);

                    if (colourState is null)
                    {
                        colourState = new ColourState { Colour = state.Colour };
                        colourStates.Add(colourState);
                    }

                    colourState.MinimumAmount = colourState.MinimumAmount > state.Amount ? colourState.MinimumAmount : state.Amount;
                }
            }

            _internalColourState = colourStates;
        }

        private ICollection<ColourState>? _internalColourState;
        public ICollection<ColourState> ColourStates => _internalColourState ?? throw new ArgumentException("State has not been calculated yet.");
        public bool HasBeenCalculated => _internalColourState is not null;

        public ICollection<Hand> Hands { get; } = new List<Hand>();

    }

    public class ColourState : IEqualityComparer<ColourState>
    {
        public required Colours Colour { get; init; }

        public uint MinimumAmount { get; set; }

        #region IEqualityComparer

        public bool Equals(ColourState? x, ColourState? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Colour == y.Colour;
        }

        public int GetHashCode(ColourState obj) => obj.Colour.GetHashCode();

        #endregion
    }

    public class Hand
    {
        public ICollection<(Colours Colour, uint Amount)> State { get; } = new List<(Colours colour, uint amount)>();
    }

    public enum Colours
    {
        Red = 0,
        Blue = 1,
        Green = 2
    }
}