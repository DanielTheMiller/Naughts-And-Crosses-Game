using System.Drawing;

namespace TicTacToe.Interfaces
{
    public interface IGrid
    {
        List<Point> GetAvailableCells();

        char GetCell(Point coord);

        void SetCell(Point coord, char token);

        bool IsCellOccupied(Point coord);

        /// <summary>
        /// Return a dictionary of pointIndexes and tokens in them
        /// </summary>
        /// <returns>A dictionary of pointIndexes and tokens in them</returns>
        IDictionary<int, char> GetMoves();

        void Reset();

        List<List<char>> GetCompletedLines();
    }
}
