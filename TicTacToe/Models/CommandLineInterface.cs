﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Services;

namespace TicTacToe.Models
{
    public class CommandLineInterface: IUserInterface
    {
        private ICommandLineInputService _commandLineInterface;

        public List<Player> Players { get; private set; }

        public void EstablishPlayerIdentity()
        {
            var player1name = _commandLineInterface.ReadNextInput("Insert the name of Player 1:");
            Players.Add(new Player(player1name, 'X'));
            var player2name = _commandLineInterface.ReadNextInput("Insert the name of Player 2:");
            Players.Add(new Player(player2name, 'O'));
        }
        public CommandLineInterface(ICommandLineInputService commandLineInterfaceService)
        {
            _commandLineInterface = commandLineInterfaceService;
            Players = new List<Player>();
        }
    }
}
