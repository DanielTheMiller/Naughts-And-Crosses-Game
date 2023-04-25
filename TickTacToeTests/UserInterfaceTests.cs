using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public void GetNextMoveFromUI()
        {
            userInterface.EstablishPlayerIdentity();
            KeyValuePair<char, Point> nextMove = userInterface.GetNextMove();
            Assert.NotNull(nextMove);
            Assert.Contains(nextMove.Key, validTokens);
        }
    }
}
