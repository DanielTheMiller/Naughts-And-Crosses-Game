using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class CommandLineInputService: ICommandLineInputService
    {
        public CommandLineInputService() { }

        public string ReadNextInput(string promptForUser)
        {
            Console.WriteLine(promptForUser);
            return Console.ReadLine() ?? "";
        }

        public void WritePrompt(string promptForUser)
        {
            Console.WriteLine(promptForUser);
        }
    }
}
