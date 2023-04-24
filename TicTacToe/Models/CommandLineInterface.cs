using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Services;

namespace TicTacToe.Models
{
    public class CommandLineInterface: IUserInterface
    {
        private ICommandLineInputService _commandLineInterface;

        public List<Player> Players { get; private set; }

        public void EstablishPlayerIdentity()
        {
            Players.Add(new Player("test", 'l'));
            Players.Add(new Player("test2", 'o'));
        }
        public CommandLineInterface(ICommandLineInputService commandLineInterfaceService)
        {
            _commandLineInterface = commandLineInterfaceService;
            Players = new List<Player>();
        }
    }
}
