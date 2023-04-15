using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class Player: IPlayer
    {
        public string Name { get; set; }

        public char Token { get; set; }

        public Player(string name, char token)
        {
            Name = name;
            Token = token;
        }
    }
}
