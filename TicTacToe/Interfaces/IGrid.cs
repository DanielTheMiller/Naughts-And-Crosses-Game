using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    public interface IGrid
    {
        List<KeyValuePair<int, int>> GetAvailableCells();

        void AddToken(string token, KeyValuePair<int, int> coord);
    }
}
