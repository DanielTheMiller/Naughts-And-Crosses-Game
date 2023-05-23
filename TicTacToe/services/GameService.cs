using TicTacToe.Interfaces;
using TicTacToe.Models;
using TicTacToe.Models.Enums;

namespace TicTacToe.Services
{
    public class GameService : IGameService
    {
        private IUserInterface userInterface;

        private List<Player> players = new();
        private Player currentPlayer;
        private GameState gameRunning = GameState.NotStarted;

        public Grid Grid { get; private set; }

        public GameService(IUserInterface userInterfaceService)
        {
            Grid = new Grid();
            this.userInterface = userInterfaceService;
        }

        public Player GetCurrentPlayer()
        {            
            return currentPlayer;
        }

        public Player GetWinningPlayer()
        {
            var completedLine = Grid.GetCompletedLines().FirstOrDefault();
            if (completedLine == null)
            {
                throw new Exception("GetWinningPlayer called when there are no winning players");
            }
            var token = completedLine.First();
            return players.First(x => x.Token == token);
        }

        public bool GameCompleted()
        {
            if (gameRunning == GameState.Completed)
            {
                return true;
            }
            if (Grid.GetCompletedLines().Count > 0)
            {
                gameRunning = GameState.Completed;
                return true;
            }
            if (Grid.GetAvailableCells().Count == 0)
            {
                gameRunning = GameState.Completed;
                return true;
            }
            return false;
        }

        public void LaunchGame()
        {
            gameRunning = GameState.Running;
            userInterface.IntroduceGame();
            players = userInterface.EstablishPlayerIdentity();
            currentPlayer = players.First();
            while (GameCompleted() == false)
            {
                userInterface.PresentLatestGrid(Grid);
                userInterface.SetCurrentPlayer(currentPlayer);
                var move = userInterface.GetNextMove(Grid);
                Grid.SetCell(move.Value, move.Key);
                ToggleCurrentPlayer();
            }
            userInterface.PresentResults(Grid, GetWinningPlayer()); // TODO: Change current player to winning player
            gameRunning = GameState.Completed;
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
