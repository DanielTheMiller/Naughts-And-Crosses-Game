using System.Drawing;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class Grid: IGrid
    {
        private const char NO_TOKEN = '-';
        private const char O_TOKEN = 'O';
        private const char X_TOKEN = 'X';
        private const int LENGTH_AND_HEIGHT = 3;

        private Dictionary<Point, char> grid = new();

        public List<Point> GetAvailableCells()
        {
            var cells = new List<Point>();
            for (int rowIndex = 0; rowIndex < 3; rowIndex++){
                for (int colIndex = 0; colIndex < 3; colIndex++)
                {
                    var coord = new Point(rowIndex, colIndex);
                    if (!grid.ContainsKey(coord))
                    {
                        cells.Add(coord);
                    }
                }
            }
            return cells;
        }

        public char GetCell(Point coord)
        {
            char tokenRetrieved;
            grid.TryGetValue(coord, out tokenRetrieved);
            if (tokenRetrieved == char.MinValue)
            {
                tokenRetrieved = NO_TOKEN;
            }
            return tokenRetrieved;
        }

        public void SetCell(Point coord, char token)
        {
            if (coord.X < 0 || coord.Y < 0 || coord.X >= 3 || coord.Y >= 3)
            {
                throw new ArgumentOutOfRangeException("Coordinates must remain within the bounds of a 3x3 grid. These coordinates are indexed from 0");
            }
            if (!(token == NO_TOKEN || token == X_TOKEN || token == O_TOKEN))
            {
                throw new ArgumentException($"Invalid token parameter {token} used");
            }
            if (grid.ContainsKey(coord))
            {
                grid[coord] = token;
            }
            else
            {
                grid.Add(coord, token);
            }
        }

        public IDictionary<int, char> GetMoves()
        {
            return grid.ToDictionary(x => GetIndexFromPoint(x.Key), x => x.Value);
        }

        public static Point GetPointFromIndex(int pointIndex)
        {
            pointIndex--;
            int yIndex = pointIndex / 3;
            int xIndex = pointIndex % 3;
            return new Point(xIndex, yIndex);
        }

        public static int GetIndexFromPoint(Point point)
        {
            return point.Y * 3 + point.X + 1;
        }

        public void Reset()
        {
            grid.Clear();
        }

        public List<List<char>> GetCompletedLines()
        {
            return getLines()
                .Where(l => l.Contains(NO_TOKEN) == false) // Where lines contained no spaces
                .Where(l => l.Distinct().ToList().Count == 1) // Where lines contain only 1 distinct token (i.e. X,X,X)
                .ToList();
        }

        private List<List<char>> getLines()
        {
            List<List<char>> lines = new() {};

            //Grab row and column lines
            for (int rowIndex = 0; rowIndex < 3; rowIndex++)
            {
                List<char> row = new();
                List<char> column = new();
                for (int colIndex = 0; colIndex < 3; colIndex++)
                {
                    row.Add(GetCell(new(rowIndex, colIndex)));
                    column.Add(GetCell(new(colIndex, rowIndex)));
                }
                lines.Add(row);
                lines.Add(column);
            }

            List<char> diag1 = new();
            List<char> diag2 = new();
            //Grab diagonal lines
            for (int index = 0; index < LENGTH_AND_HEIGHT; index++)
            {
                diag1.Add(GetCell(new(index, index)));
                diag2.Add(GetCell(new(index, LENGTH_AND_HEIGHT - (index + 1))));
            }
            lines.Add(diag1);
            lines.Add(diag2);

            return lines;
        }
    }
}
