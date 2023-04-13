using System.Drawing;

namespace TicTacToe.Interfaces
{
    public interface IGrid
    {
        List<Point> GetAvailableCells();

        void AddToken(string token, Point coord);

        void Reset();
    }
}
