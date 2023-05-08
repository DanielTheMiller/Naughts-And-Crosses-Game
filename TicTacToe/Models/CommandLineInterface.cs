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

        private Player currentPlayer;

        const string INCORRECT_INPUT_TRY_AGAIN_PROMPT = "Your input must be a number between 1 and 9. Try again";
        const string INTRODUCE_GAME = "Welcome to Tic-Tac-Toe!";

        public CommandLineInterface(ICommandLineInputService commandLineInterfaceService)
        {
            _commandLineInterface = commandLineInterfaceService;
        }

        public List<Player> EstablishPlayerIdentity()
        {
            List<Player> players = new List<Player>();
            var player1name = _commandLineInterface.ReadNextInput("Insert the name of Player 1:");
            players.Add(new Player(player1name, 'X'));
            var player2name = _commandLineInterface.ReadNextInput("Insert the name of Player 2:");
            players.Add(new Player(player2name, 'O'));
            return players;
        }

        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public void SetCurrentPlayer(Player newPlayer)
        {
            currentPlayer = newPlayer;
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
                _commandLineInterface.WritePrompt(INCORRECT_INPUT_TRY_AGAIN_PROMPT);
            }
        }

        public void IntroduceGame()
        {
            _commandLineInterface.WritePrompt(INTRODUCE_GAME);
        }

        public void PresentLatestGrid(Grid grid)
        {
            var stringifiedGrid = "1|2|3\n4|5|6\n7|8|9";
            var moves = grid.GetMoves();
            foreach (var tokenPlacement in moves)
            {
                var pointIndex = tokenPlacement.Key;
                var token = tokenPlacement.Value;
                stringifiedGrid = stringifiedGrid.Replace(pointIndex.ToString(), token.ToString());
            }
            _commandLineInterface.WritePrompt(stringifiedGrid);
        }
    }
}
