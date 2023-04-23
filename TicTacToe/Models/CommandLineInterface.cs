using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class CommandLineInterface: IUserInterface
    {
        public List<Player> Players { get; private set; }

        public void EstablishPlayerIdentity()
        {
            Players = new List<Player>();
            Players.Add(new Player("test", 'l'));
            Players.Add(new Player("test2", 'o'));
        }
    }
}
