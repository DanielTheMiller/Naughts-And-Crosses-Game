using System.Drawing;

namespace TicTacToe.Interfaces
{
    public interface IGrid
    {
        List<Point> GetAvailableCells();

        string GetCell(Point coord);

        void SetCell(string token, Point coord);

        void Reset();
    }
}
