using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    public interface ICommandLineInputService
    {
        public string ReadNextInput(string promptForUser);

        public void WritePrompt(string prompt);
    }
}
