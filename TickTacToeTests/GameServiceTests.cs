using Moq;
using System.Reflection;
using TicTacToe.Interfaces;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToeTests
{
    public class GameServiceTests
    {
        Mock<IUserInterface> userInterfaceMock;
        IGameService gameService;

        public GameServiceTests()
        {
            userInterfaceMock = new Mock<IUserInterface>();
            IUserInterface userInterface = userInterfaceMock.Object;
            gameService = new GameService(userInterface);
        }

        [Fact]
        public void GameServiceCanBeConstructed()
        {
            Assert.NotNull(gameService);
        }

        [Fact]
        public void ServiceHasPublicLaunchMethod()
        {
            gameService.LaunchGame();
        }

        [Fact]
        public void GameServiceHasAConnectedUserInterfaceService()
        {
            var userInterfaceProp = typeof(GameService).GetField("userInterface", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(userInterfaceProp);
            var userInterface = userInterfaceProp.GetValue(gameService);
            Assert.NotNull(userInterface);
            Assert.IsAssignableFrom<IUserInterface>(userInterface);
        }

        [Fact]
        public void ServiceInitialltyCallsEstablishPlayerIdentityOfUserInterface()
        {
            gameService.LaunchGame();
            userInterfaceMock.Verify(x => x.EstablishPlayerIdentity(), Times.Once);
        }
    }
}
