using System.Drawing;

namespace TicTacToe.Interfaces
{
    public interface IGrid
    {
        List<Point> GetAvailableCells();

        char GetCell(Point coord);

        void SetCell(char token, Point coord);

        void Reset();
    }
}
