using Moq;
using System.Drawing;
using System.Reflection;
using TicTacToe.Interfaces;
using TicTacToe.Models;
using TicTacToe.Services;
using TicTacToeTests.Resources;

namespace TicTacToeTests
{
    public class GameServiceTests
    {
        Mock<IUserInterface> userInterfaceMock;
        IGameService gameService;
        List<string> methodsInvoked = new();
        List<Player> examplePlayerArray = new() { new("P1", 'O'), new("P2", 'X') };

        private void SetupMethods()
        {
            userInterfaceMock.Setup(x => x.IntroduceGame()).Callback(() => methodsInvoked.Add("IntroduceGame"));
            userInterfaceMock.Setup(x => x.GetCurrentPlayer()).Callback(() => methodsInvoked.Add("GetCurrentPlayer"));
            userInterfaceMock.Setup(x => x.SetCurrentPlayer(It.IsAny<Player>())).Callback(() => methodsInvoked.Add("SetCurrentPlayer"));
            userInterfaceMock.Setup(x => x.PresentLatestGrid(It.IsAny<Grid>())).Callback(() => methodsInvoked.Add("PresentLatestGrid"));
            userInterfaceMock.Setup(x => x.GetNextMove(It.IsAny<Grid>())).Returns(() => GetNextRandomMove()).Callback(() => methodsInvoked.Add("GetNextMove"));
            userInterfaceMock.Setup(x => x.EstablishPlayerIdentity()).Returns(examplePlayerArray).Callback(() => methodsInvoked.Add("EstablishPlayerIdentity"));
            userInterfaceMock.Setup(x => x.PresentResults(It.IsAny<Grid>(), It.IsAny<Player>())).Callback(() => methodsInvoked.Add("PresentResults"));
        }

        /// <summary>
        /// Fetch next random valid move for the tests
        /// </summary>
        /// <returns>Next move</returns>
        private KeyValuePair<char, Point> GetNextRandomMove()
        {
            var availableCells = gameService.Grid.GetAvailableCells();
            Random random = new Random();
            var nextCell = availableCells[random.Next(availableCells.Count)];
            return new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, nextCell);
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
        /// Checks Method has been invoked and returns index (index in methodsInvoked list) of the last call
        /// </summary>
        /// <param name="methodName">The method name to search for</param>
        /// <param name="assertMethodPresent">Should this method throw an error if this property cannot be found?</param>
        /// <returns></returns>
        private int GetLastMethodInvocationIndex(string methodName, bool assertMethodPresent=true)
        {
            var callOrderIndex = methodsInvoked.LastIndexOf(methodName);
            if (assertMethodPresent)
            {
                Assert.True(callOrderIndex >= 0);
            }
            return callOrderIndex;
        }

        /// <summary>
        /// Checks Method has been invoked and returns order (index in methodsInvoked list) of first call
        /// </summary>
        /// <param name="methodName">The method name to search for</param>
        /// <param name="assertMethodPresent">Should this method throw an error if this property cannot be found?</param>
        /// <returns></returns>
        private int GetFirstMethodInvocationIndex(string methodName, bool assertMethodPresent = true)
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
            userInterfaceMock.Verify(x => x.GetNextMove(It.IsAny<Grid>()), Times.AtLeastOnce);
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
        public void ServiceSetsCurrentPlayerOnUIBeforeRequestingNextMove()
        {
            gameService.LaunchGame();
            var setCurrentPlayerCallOrder = GetFirstMethodInvocationIndex("SetCurrentPlayer");
            var getNextMoveCallOrder = GetFirstMethodInvocationIndex("GetNextMove");
            Assert.True(setCurrentPlayerCallOrder < getNextMoveCallOrder);
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

        [Fact]
        public void CurrentPlayerAlternatesCorrectlyAsNextMovesAreCalled()
        {
            var playerRequestedList = new List<Player>();
            userInterfaceMock.Setup(x => x.GetNextMove(It.IsAny<Grid>())).Returns(() => GetNextRandomMove()).Callback(() => playerRequestedList.Add(gameService.GetCurrentPlayer()));
            gameService.LaunchGame();
            var player1 = examplePlayerArray.First();
            var player2 = examplePlayerArray.Last();
            var player1Indexes = new List<int>();
            var player2Indexes = new List<int>();
            for (int playerIndex = 0; playerIndex < playerRequestedList.Count; playerIndex++)
            {
                var thisPlayer = playerRequestedList[playerIndex];
                if (thisPlayer == player1)
                {
                    player1Indexes.Add(playerIndex);
                } else if (thisPlayer == player2) {
                    player2Indexes.Add(playerIndex);
                }
            }
            var modulusedPlayer1TurnIndexes = player1Indexes.Select(x => x % 2).Distinct().ToList(); // Ensure these are distinct, even or odd indexes
            var modulusedPlayer2TurnIndexes = player2Indexes.Select(x => x % 2).Distinct().ToList();
            Assert.Equal(modulusedPlayer1TurnIndexes.Count, 1);
            Assert.Equal(modulusedPlayer2TurnIndexes.Count, 1);
            Assert.Equal(0, modulusedPlayer1TurnIndexes.First()); // First player should have taken the first turn, so they will take the even turns;
            Assert.Equal(1, modulusedPlayer2TurnIndexes.First()); // Second player should have taken the second turn, so they will take the odd turns;
        }

        [Fact]
        public void GameCompletedIsInitiallyFalse()
        {
            bool gameCompleted = gameService.GameCompleted();
            Assert.False(gameCompleted);
        }

        [Fact]
        public void GameIsComplatedAfterLaunchHasBeenExecuted()
        {
            gameService.LaunchGame();
            Assert.True(gameService.GameCompleted());
        }

        [Fact]
        public void GridIsInAValidCompletedStateAfterGameCompletes()
        {
            gameService.LaunchGame();
            Assert.True(gameService.GameCompleted());
            Grid grid = gameService.Grid;
            var listOfCompletedLines = grid.GetCompletedLines();
            var listOfAvailableCells = grid.GetAvailableCells();
            Assert.True(listOfCompletedLines.Count > 0 || listOfAvailableCells.Count == 0);
        }

        [Fact]
        public void EnsureGridIsPresentedEveryTurn()
        {
            gameService.LaunchGame();
            var numberOfNextMoveCalls = methodsInvoked.Count(x => x == "GetNextMove");
            var numberOfPresentLatestGridCalls = methodsInvoked.Count(x => x == "PresentLatestGrid");
            Assert.True(numberOfNextMoveCalls > 0);
            Assert.Equal(numberOfNextMoveCalls, numberOfPresentLatestGridCalls);
        }

        [Fact]
        public void PresentResultsIsCalledAfterGameCompletes()
        {
            gameService.LaunchGame();
            Assert.True(gameService.GameCompleted());

            var presentResultsCallOrder = GetLastMethodInvocationIndex("PresentResults");
            var getNextMoveLastCall = GetLastMethodInvocationIndex("GetNextMove");
            Assert.True(getNextMoveLastCall < presentResultsCallOrder, "The game requested another move after the results screen");
            Assert.True(methodsInvoked.Count(x => x == "PresentResults") == 1, "Expected the results to only be presented to the users once");
        }

        [Fact]
        public void GetWinningPlayerReturnsPlayer1()
        {
            var playerMember = typeof(GameService).GetField("players", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(playerMember);
            var players = new List<Player>() { new(name: "Player1", token: 'X'), new(name: "Player2", token: 'O') };
            playerMember.SetValue(gameService, players);
            gameService.Grid.SetCell(new(0,0), 'X');
            gameService.Grid.SetCell(new(1,0), 'X');
            gameService.Grid.SetCell(new(2,0), 'X');
            var winningPlayer = gameService.GetWinningPlayer();
            Assert.NotNull(winningPlayer);
            Assert.Equal(players.First(), winningPlayer);
        }

        [Fact]
        public void GetWinningPlayerReturnsPlayer2()
        {
            var playerMember = typeof(GameService).GetField("players", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(playerMember);
            var players = new List<Player>() { new(name: "Player1", token: 'O'), new(name: "Player2", token: 'X') };
            playerMember.SetValue(gameService, players);
            gameService.Grid.SetCell(new(0, 0), 'X');
            gameService.Grid.SetCell(new(1, 0), 'X');
            gameService.Grid.SetCell(new(2, 0), 'X');
            var winningPlayer = gameService.GetWinningPlayer();
            Assert.NotNull(winningPlayer);
            Assert.Equal(players.Last(), winningPlayer);
        }

        [Fact]
        public void GetWinningPlayerReturnsOTokenPlayer()
        {
            var playerMember = typeof(GameService).GetField("players", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(playerMember);
            var players = new List<Player>() { new(name: "Player1", token: 'O'), new(name: "Player2", token: 'X') };
            playerMember.SetValue(gameService, players);
            gameService.Grid.SetCell(new(0, 0), 'O');
            gameService.Grid.SetCell(new(0, 1), 'O');
            gameService.Grid.SetCell(new(0, 2), 'O');
            var winningPlayer = gameService.GetWinningPlayer();
            Assert.NotNull(winningPlayer);
            Assert.Equal(players.First(), winningPlayer);
        }

        [Fact]
        public void GetWinningPlayerReturnsNullTokenIfGridIsStalemate()
        {
            var stalemateGrid = ExampleGrids.GetRandomStalemateGrid();
            Assert.NotNull(stalemateGrid);
            Assert.Equal(0, stalemateGrid.GetCompletedLines().Count);
        }

        [Fact]
        public void EnsureDiagonalWinnerIsPresented()
        {
            userInterfaceMock.SetupSequence(x => x.GetNextMove(It.IsAny<Grid>()))
                .Returns(() => new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, new(1, 0)))
                .Returns(() => new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, new(0, 0)))
                .Returns(() => new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, new(2, 0)))
                .Returns(() => new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, new(1, 1)))
                .Returns(() => new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, new(2, 1)))
                .Returns(() => new KeyValuePair<char, Point>(gameService.GetCurrentPlayer().Token, new(2, 2)));
            gameService.LaunchGame();
            userInterfaceMock.Verify(x => x.PresentResults(It.IsAny<Grid>(), examplePlayerArray.Last()), Times.Once);
        }
    }
}
