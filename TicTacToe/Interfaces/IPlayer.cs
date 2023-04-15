using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    public interface IPlayer
    {
        public string Name { get; set; }

        public char Token { get; set; }
    }
}
