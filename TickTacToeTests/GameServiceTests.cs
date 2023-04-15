using TicTacToe.Interfaces;
using TicTacToe.Services;

namespace TicTacToeTests
{
    public class GameServiceTests
    {
        IGameService gameService = new GameService();

        public void GameServiceCanBeConstructed()
        {
            gameService = new GameService();
        }

        public void ServiceHasPublicLaunchMethod()
        {
            gameService.LaunchGame();
        }
    }
}
