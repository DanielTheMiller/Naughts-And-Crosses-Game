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
        List<string> methodsInvoked = new(); //TODO: Remember to setup mocks so they feed this list
        List<Player> examplePlayerArray = new() { new("P1", 'O'), new("P2", 'X') };

        private void SetupMethods()
        {
            userInterfaceMock.Setup(x => x.IntroduceGame()).Callback(() => methodsInvoked.Add("IntroduceGame"));
            userInterfaceMock.Setup(x => x.GetCurrentPlayer()).Callback(() => methodsInvoked.Add("GetCurrentPlayer"));
            userInterfaceMock.Setup(x => x.PresentLatestGrid(It.IsAny<Grid>())).Callback(() => methodsInvoked.Add("PresentLatestGrid"));
            userInterfaceMock.Setup(x => x.GetNextMove()).Callback(() => methodsInvoked.Add("GetNextMove"));
            userInterfaceMock.Setup(x => x.EstablishPlayerIdentity()).Returns(examplePlayerArray).Callback(() => methodsInvoked.Add("EstablishPlayerIdentity")); ;
        }

        public GameServiceTests()
        {
            userInterfaceMock = new Mock<IUserInterface>();
            SetupMethods();
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

        [Fact]
        public void ServiceCallsIntroduceGameAfterLaunching()
        {
            gameService.LaunchGame();
            userInterfaceMock.Verify(x => x.IntroduceGame(), Times.Once);
            userInterfaceMock.Verify(x => x.EstablishPlayerIdentity(), Times.Once);
        }

        [Fact]
        public void ServiceDrawsGridAfterLaunch()
        {
            gameService.LaunchGame();
            userInterfaceMock.Verify(x => x.PresentLatestGrid(It.IsAny<Grid>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Checks Method has been invoked and returns order (index in methodsInvoked list) of first call
        /// </summary>
        /// <param name="methodName">The method name to search for</param>
        /// <param name="assertMethodPresent">Should this method throw an error if this property cannot be found?</param>
        /// <returns></returns>
        private int GetFirstMethodInvocationIndex(string methodName, bool assertMethodPresent=true)
        {
            var callOrderIndex = methodsInvoked.IndexOf(methodName);
            if (assertMethodPresent)
            {
                Assert.True(callOrderIndex >= 0);
            }
            return callOrderIndex;
        }

        [Fact]
        public void ServicePresentsGridAndGetsFirstMoveAfterLaunch()
        {
            gameService.LaunchGame();
            userInterfaceMock.Verify(x => x.IntroduceGame(), Times.Once);
            userInterfaceMock.Verify(x => x.EstablishPlayerIdentity(), Times.Once);
            userInterfaceMock.Verify(x => x.GetNextMove(), Times.AtLeastOnce);
            userInterfaceMock.Verify(x => x.PresentLatestGrid(It.IsAny<Grid>()), Times.AtLeastOnce);

            var establishPlayerIdentityCallOrder = GetFirstMethodInvocationIndex("EstablishPlayerIdentity");
            var introduceGameCallOrder = GetFirstMethodInvocationIndex("IntroduceGame");
            var presentGridCallOrder = GetFirstMethodInvocationIndex("PresentLatestGrid");
            var getNextMoveCallOrder = GetFirstMethodInvocationIndex("GetNextMove");

            Assert.True(introduceGameCallOrder < establishPlayerIdentityCallOrder, "IntroduceGame was called after EstablishPlayerIdentity! (The wrong order!)");
            Assert.True(establishPlayerIdentityCallOrder < presentGridCallOrder, "establishPlayerIdentity was called after presentGrid! (The wrong order!)");
            Assert.True(presentGridCallOrder < getNextMoveCallOrder, "presentGrid was called after GetNextMove! (The wrong order!)");
        }

        [Fact]
        public void CanChangeContextPlayer()
        {
            var playersListField = typeof(GameService).GetField("players", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(playersListField);
            playersListField.SetValue(gameService, examplePlayerArray);
            gameService.GetCurrentPlayer();//Get Current Player should probably be on the game service
            gameService.ToggleCurrentPlayer();
        }

        [Fact]
        public void ToggleCurrentPlayerChangesPlayer()
        {
            gameService.LaunchGame();
            var player1 = gameService.GetCurrentPlayer();
            var player2 = gameService.ToggleCurrentPlayer();
            Assert.NotNull(player1);
            Assert.NotNull(player2);
            Assert.NotEqual(player1, player2);
            var player3 = gameService.GetCurrentPlayer();
            Assert.Equal(player2, player3);
            var player4 = gameService.ToggleCurrentPlayer();
            Assert.NotEqual(player3, player4);
            Assert.Equal(player1, player4);
        }

        [Fact]
        public void PlayersListIsBeingPopulatedFromUIEstablishPlayerIdentity()
        {
            gameService.LaunchGame();
            var player1 = gameService.GetCurrentPlayer();
            var player2 = gameService.ToggleCurrentPlayer();
            Assert.Contains(player1, examplePlayerArray);
            Assert.Contains(player2, examplePlayerArray);
        }
    }
}
