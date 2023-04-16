using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToeTests
{
    public class UserInterfaceTests
    {
        IUserInterface userInterface;

        public UserInterfaceTests()
        {
            userInterface = new CommandLineInterface();
        }

        [Fact]
        public void CanCreateUserInterface()
        {
            Assert.NotNull(userInterface);
        }

        [Fact]
        public void CanTriggerEstablishPlayerIdentity() {
            userInterface.EstablishPlayerIdentity();
        }
    }
}
