using System.Drawing;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class Grid: IGrid
    {
        private const char NO_TOKEN = '-';
        private const char O_TOKEN = 'O';
        private const char X_TOKEN = 'X';

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

        public void Reset()
        {
            grid.Clear();
        }
    }
}
