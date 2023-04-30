using System;
using System.Collections.Generic;
using System.Drawing;
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

        const string INCORRECT_INPUT_TRY_AGAIN_PROMPT = "Your input must be a number between 1 and 9. Try again";

        public CommandLineInterface(ICommandLineInputService commandLineInterfaceService)
        {
            _commandLineInterface = commandLineInterfaceService;
            Players = new List<Player>();
        }

        public void EstablishPlayerIdentity()
        {
            var player1name = _commandLineInterface.ReadNextInput("Insert the name of Player 1:");
            Players.Add(new Player(player1name, 'X'));
            var player2name = _commandLineInterface.ReadNextInput("Insert the name of Player 2:");
            Players.Add(new Player(player2name, 'O'));
        }

        public Player GetCurrentPlayer()
        {
            return Players.First();
        }

        public KeyValuePair<char, Point> GetNextMove()
        {
            while (true)
            {
                var currentPlayer = GetCurrentPlayer();
                var prompt = $"{currentPlayer.Name}, Insert a number to place your next move:";
                var nextMoveIndex = _commandLineInterface.ReadNextInput(prompt);
                int nextMoveIndexInt;
                var nextInputWasInteger = int.TryParse(nextMoveIndex, out nextMoveIndexInt);
                if (nextInputWasInteger)
                {
                    var nextPoint = Grid.GetPointFromIndex(nextMoveIndexInt);
                    return new KeyValuePair<char, Point>(currentPlayer.Token, nextPoint);
                }
                Console.WriteLine(INCORRECT_INPUT_TRY_AGAIN_PROMPT);
            }
        }
    }
}
