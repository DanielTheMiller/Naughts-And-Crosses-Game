using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Services;

namespace TicTacToeTests
{
    public class CommandLineInputServiceTests
    {
        private ICommandLineInputService _service;

        public CommandLineInputServiceTests()
        {
            _service = new CommandLineInputService();
        }

        [Fact]
        public void CanCreateService()
        {
            Assert.NotNull(_service);
        }

        [Fact]
        public void ClassHasReadNextCommandInputMethod()
        {
            var method = typeof(CommandLineInputService).GetMethod("ReadNextInput" , BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            Assert.NotNull(method);
        }
    }
}
