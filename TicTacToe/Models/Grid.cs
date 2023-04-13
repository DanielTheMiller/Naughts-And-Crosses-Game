using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class Grid: IGrid
    {
        public List<KeyValuePair<int,int>> GetAvailableCells()
        {
            var cells = new List<KeyValuePair<int, int>>();
            for (int rowIndex = 0; rowIndex < 3; rowIndex++){
                for (int colIndex = 0; colIndex < 3; colIndex++)
                {
                    var coord = new KeyValuePair<int, int>(rowIndex, colIndex);
                    cells.Add(coord);
                }
            }
            return cells;
        }

        public void AddToken(string token, KeyValuePair<int,int> coord)
        {

        }
    }
}
