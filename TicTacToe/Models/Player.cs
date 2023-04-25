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

        private readonly char[] validTokens = new char[] { 'X', 'O' };

        public Player(string name, char token)
        {
            if (!validTokens.Contains(token))
            {
                throw new ArgumentException($"Invalid token {token} used in Player constructor. Valid tokens are: ({string.Join(", ",validTokens)})");
            }
            Name = name;
            Token = token;
        }
    }
}
