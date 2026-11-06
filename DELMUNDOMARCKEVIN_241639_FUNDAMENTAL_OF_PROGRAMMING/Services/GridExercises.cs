namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

public static class GridExercises
{
    // Five fixed maps keep this exercise focused on 2D indexing, not maze generation.
    private static readonly string[][] Maps =
    [
        ["1111111111111111111111111", "S000001000100000000000001", "1111101010101111101111111", "1000101010000000100000001", "1011101011111110111111101", "1000001010001010001010001", "1011111010101011101010101", "1000001010101000000010101", "1111101110101111111110101", "1000101000100000000000101", "1010101011111111111111101", "1010100010000000100010001", "1010111110111111101010111", "101000000000000000100000E", "1111111111111111111111111"],
        ["1111111111111111111111111", "S010001000100000000000101", "1010101010101111111110101", "1010101010100010000010101", "1010101010111010111010101", "1000100010100010001000101", "1111111110101111101111101", "1000000010001000100000101", "1011101111111110111110101", "1000101000000000000010001", "1011101011111011111111101", "1010001000001010000000101", "1010111111101010111110101", "101000000000100000001000E", "1111111111111111111111111"],
        ["1111111111111111111111111", "S010000000101000000000101", "1011101110101011101110101", "1000001000100010101000001", "1111111011101110101111101", "1000001000100000100010101", "1011101110111110111010101", "1010000010001000001010001", "1110111011101111101010111", "1000101000100010001010001", "1011101011111011111011101", "1000100010001000001010101", "1010111110101111101010101", "101000000010000000001000E", "1111111111111111111111111"],
        ["1111111111111111111111111", "S000100000100010000000001", "1110101110101110111011101", "1010100010001000100010001", "1010101011111011101110111", "1000101000000010001000101", "1011101111111110111011101", "1010000000000010101010001", "1011111111101110101010101", "1010000000101000100010101", "1010111110111011101111101", "1010100010001000101000001", "1010101011101110101011101", "100010100000000010001000E", "1111111111111111111111111"],
        ["1111111111111111111111111", "S000000000100000100000001", "1111111110101011101111101", "1000100000101000001000101", "1010101111111011111010101", "1010101000001010000010001", "1010101011101010111111111", "1010001010001010100000001", "1011111010111110101111101", "1010001010100010000000101", "1010101010101011111110101", "1010100010001000100010101", "1010111111111110101011101", "100010000000000000100000E", "1111111111111111111111111"]
    ];

    public static char[,] MazeMap(int layout)
    {
        if (layout is < 0 or >= 5) throw new ArgumentOutOfRangeException(nameof(layout));
        char[,] map = new char[15, 25];
        for (int row = 0; row < map.GetLength(0); row++)
            for (int column = 0; column < map.GetLength(1); column++)
                map[row, column] = Maps[layout][row][column];
        return map;
    }

    public static bool TryMove(char[,] map, ref int row, ref int column, string direction)
    {
        int nextRow = row, nextColumn = column;
        if (direction == "up") nextRow--;
        else if (direction == "down") nextRow++;
        else if (direction == "left") nextColumn--;
        else if (direction == "right") nextColumn++;
        else return false;
        if (nextRow < 0 || nextRow >= map.GetLength(0) || nextColumn < 0 || nextColumn >= map.GetLength(1)) return false;
        if (map[nextRow, nextColumn] == '1') return false;
        row = nextRow; column = nextColumn;
        return true;
    }

    public static bool[,] SelectedSeats(string state)
    {
        bool[,] selected = new bool[5, 8];
        for (int row = 0; row < 5; row++)
            for (int column = 0; column < 8; column++)
                selected[row, column] = state[row * 8 + column] == '1';
        return selected;
    }

    public static decimal[,] SeatPrices()
    {
        decimal[,] prices = new decimal[5, 8];
        for (int row = 0; row < 5; row++)
            for (int column = 0; column < 8; column++)
                prices[row, column] = row < 2 ? 150m : row < 4 ? 300m : 200m;
        return prices;
    }

    public static decimal SeatTotal(bool[,] selected, decimal[,] prices)
    {
        decimal total = 0m;
        for (int row = 0; row < selected.GetLength(0); row++)
            for (int column = 0; column < selected.GetLength(1); column++)
                if (selected[row, column]) total += prices[row, column];
        return total;
    }

    public static char[,] GameBoard(string state)
    {
        char[,] board = new char[3, 3];
        for (int row = 0; row < 3; row++)
            for (int column = 0; column < 3; column++)
                board[row, column] = state[row * 3 + column];
        return board;
    }

    public static int[] WinningCells(char[,] board)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != '.' && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]) return [i * 3, i * 3 + 1, i * 3 + 2];
            if (board[0, i] != '.' && board[0, i] == board[1, i] && board[1, i] == board[2, i]) return [i, i + 3, i + 6];
        }
        if (board[1, 1] != '.')
        {
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) return [0, 4, 8];
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) return [2, 4, 6];
        }
        return [];
    }

    public static string SaveBoard(char[,] board)
    {
        string state = "";
        for (int row = 0; row < board.GetLength(0); row++)
            for (int column = 0; column < board.GetLength(1); column++) state += board[row, column];
        return state;
    }
}
