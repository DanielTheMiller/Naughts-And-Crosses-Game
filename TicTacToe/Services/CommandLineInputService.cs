using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Services
{
    public class CommandLineInputService
    {
        public CommandLineInputService() { }

        public string ReadNextInput()
        {
            return Console.ReadLine();
        }
    }
}
