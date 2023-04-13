using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class Grid: IGrid
    {
        int tokenCount = 0;

        public List<Point> GetAvailableCells()
        {
            var cells = new List<Point>();
            for (int rowIndex = 0; rowIndex < 3; rowIndex++){
                for (int colIndex = 0; colIndex < 3; colIndex++)
                {
                    var coord = new Point(rowIndex, colIndex);
                    cells.Add(coord);
                }
            }
            for (int purgeIndex = 0; purgeIndex < tokenCount; purgeIndex++)
            {
                cells.RemoveAt(0);
            }
            return cells;
        }

        public string GetCell(Point coord)
        {
            return "X";
        }

        public void SetCell(string token, Point coord)
        {
            if (coord.X < 0 || coord.Y < 0 || coord.X >= 3 || coord.Y >= 3)
            {
                throw new ArgumentOutOfRangeException("Coordinates must remain within the bounds of a 3x3 grid. These coordinates are indexed from 0");
            }
            tokenCount++;
        }

        public void Reset()
        {
            tokenCount = 0;
        }
    }
}
