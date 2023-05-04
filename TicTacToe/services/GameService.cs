using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class GameService : IGameService
    {
        private IUserInterface userInterface;

        private List<Player> players = new();
        private Player currentPlayer;

        public GameService(IUserInterface userInterfaceService)
        {
            this.userInterface = userInterfaceService;
        }

        public Player GetCurrentPlayer()
        {            
            return currentPlayer;
        }

        public void LaunchGame()
        {
            userInterface.IntroduceGame();
            players = userInterface.EstablishPlayerIdentity();
            currentPlayer = players.First();
            var grid = new Grid();
            userInterface.PresentLatestGrid(grid);
            userInterface.GetNextMove();
        }

        public Player ToggleCurrentPlayer()
        {
            var currentPlayerIndex = players.IndexOf(currentPlayer);
            var nextPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            currentPlayer = players[nextPlayerIndex]; // Get next index along, or loop back around to the start of the array
            return currentPlayer;
        }
    }
}
