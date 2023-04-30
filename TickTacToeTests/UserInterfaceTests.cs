using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToeTests
{
    public class UserInterfaceTests
    {
        IUserInterface userInterface;
        Mock<ICommandLineInputService> commandLineInputServiceMock;

        const string GET_PLAYER_1_NAME = "Insert the name of Player 1:";
        const string GET_PLAYER_2_NAME = "Insert the name of Player 2:";
        const string INSERT_NEXT_MOVE_PROMPT = ", Insert a number to place your next move:";
        const string INITIAL_GRID_STRING = "1|2|3\n4|5|6\n7|8|9";
        private readonly char[] validTokens = new char[] { 'X', 'O' }; 

        public UserInterfaceTests()
        {
            commandLineInputServiceMock = new Mock<ICommandLineInputService>();
            userInterface = new CommandLineInterface(commandLineInputServiceMock.Object);
        }

        [Fact]
        public void CanCreateUserInterface()
        {
            Assert.NotNull(userInterface);
        }

        [Fact]
        public void CanTriggerEstablishPlayerIdentity() {
            userInterface.EstablishPlayerIdentity();
            Assert.NotNull(userInterface.Players);
            Assert.Equal(2 , userInterface.Players.Count);
        }

        [Fact]
        public void UIGetsParametersFromTheCommandLine()
        {
            userInterface.EstablishPlayerIdentity();
            commandLineInputServiceMock.Verify(x => x.ReadNextInput(GET_PLAYER_1_NAME)); //Ensure this player input is requested
            commandLineInputServiceMock.Verify(x => x.ReadNextInput(GET_PLAYER_2_NAME));
        }

        [Fact]
        public void InitiallyReturnFirstPlayerAsTheCurrentPlayer()
        {
            userInterface.EstablishPlayerIdentity();
            var currentPlayer = userInterface.GetCurrentPlayer();
            Assert.NotNull(currentPlayer);
            Assert.Equal(userInterface.Players.First(), currentPlayer);
        }

        [Fact]
        public void GetNextMoveFromUIHasValidParams()
        {
            userInterface.EstablishPlayerIdentity();
            commandLineInputServiceMock.Setup(s => s.ReadNextInput(It.IsAny<string>())).Returns("3");
            KeyValuePair<char, Point> nextMove = userInterface.GetNextMove();
            Assert.NotNull(nextMove);
            Assert.NotNull(nextMove.Key);
            Assert.NotNull(nextMove.Value);
            Assert.Contains(nextMove.Key, validTokens);
            Assert.True(nextMove.Value.X >= 0);
            Assert.True(nextMove.Value.Y >= 0);
            Assert.True(nextMove.Value.X <= 2);
            Assert.True(nextMove.Value.Y <= 2);
        }

        [Fact]
        public void UIGetsNextMoveFromTheCommandLine()
        {
            userInterface.EstablishPlayerIdentity();
            commandLineInputServiceMock.Setup(s => s.ReadNextInput(It.IsAny<string>())).Returns("3");
            KeyValuePair<char, Point> nextMove = userInterface.GetNextMove();
            commandLineInputServiceMock.Verify(x => x.ReadNextInput(It.Is<string>((inputCommand) => inputCommand.Contains(INSERT_NEXT_MOVE_PROMPT)))); // Using a contains because this prompt should be personalised
        }

        [Fact]
        public void UIReturnsAppropriatePointForGivenIndexFromCommandLine()
        {
            userInterface.EstablishPlayerIdentity();
            var pointIndex = 1;
            Action<ICommandLineInputService> previousExpression = (s) => { return; };
            for (int yIndex = 0; yIndex <= 2; yIndex++)
            {
                for (int xIndex = 0; xIndex <= 2; xIndex++)
                {
                    commandLineInputServiceMock.Setup(s => s.ReadNextInput(It.IsAny<string>())).Returns(pointIndex.ToString());
                    KeyValuePair<char, Point> nextMove = userInterface.GetNextMove();
                    var point = nextMove.Value;
                    Assert.True(point.X == xIndex && point.Y == yIndex, $"Was expecting Point(x:{xIndex}, y:{yIndex}) from index {pointIndex}; But instead got Point(x:{point.X}, y:{point.Y})");
                    pointIndex++;
                }
            }
        }

        [Fact]
        public void EnsureGetNextMoveReturnsCurrentPlayersToken()
        {
            userInterface.EstablishPlayerIdentity();
            var currentPlayer = userInterface.GetCurrentPlayer();
            commandLineInputServiceMock.Setup(s => s.ReadNextInput(It.IsAny<string>())).Returns("3");
            var nextMove = userInterface.GetNextMove();
            Assert.Equal(nextMove.Key, currentPlayer.Token);
        }

        [Fact]
        public void UserProvidingNonNumberShouldNotThrowException()
        {
            userInterface.EstablishPlayerIdentity();
            commandLineInputServiceMock.SetupSequence(s => s.ReadNextInput(It.IsAny<string>()))
                .Returns("Not a number")
                .Returns("1");
            var nextMove = userInterface.GetNextMove();
            Assert.NotNull(nextMove);
        }

        [Fact]
        public void CanIntroduceGame()
        {
            userInterface.IntroduceGame();
            commandLineInputServiceMock.Verify(x => x.WritePrompt(It.IsAny<string>()),Times.AtLeastOnce);
        }

        [Fact]
        public void CanPresentLatestGrid()
        {
            var grid = new Grid();
            userInterface.PresentLatestGrid(grid);
            commandLineInputServiceMock.Verify(x => x.WritePrompt(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void CheckInitialGridState()
        {
            var grid = new Grid();
            userInterface.PresentLatestGrid(grid);
            commandLineInputServiceMock.Verify(x => x.WritePrompt(INITIAL_GRID_STRING), Times.Once);
        }

        [Fact]
        public void SetThreeRandomGridTokensAndCheckThatTheyArePresentedToTheUserCorrectly()
        {
            var grid = new Grid();
            var random = new Random();
            var expectedResult = INITIAL_GRID_STRING;
            for (int i = 0; i < 3; i++)
            {
                var availableCells = grid.GetAvailableCells();
                var nextCoordIndex = random.Next(maxValue: availableCells.Count);
                var coord = availableCells[nextCoordIndex];
                grid.SetCell(coord, 'X');
                var pointIndex = Grid.GetIndexFromPoint(coord);
                expectedResult = expectedResult.Replace(pointIndex.ToString(), "X");
            }
            userInterface.PresentLatestGrid(grid);
            commandLineInputServiceMock.Verify(x => x.WritePrompt(expectedResult), Times.Once);
        }
    }
}
