using System.Drawing;

namespace TicTacToe.Interfaces
{
    public interface IGrid
    {
        List<Point> GetAvailableCells();

        char GetCell(Point coord);

        void SetCell(Point coord, char token);

        void Reset();
    }
}
