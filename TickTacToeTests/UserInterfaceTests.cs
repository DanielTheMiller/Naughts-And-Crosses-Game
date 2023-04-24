using Moq;
using System;
using System.Collections.Generic;
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
    }
}
