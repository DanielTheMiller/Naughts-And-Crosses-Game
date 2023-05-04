﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
    public interface IUserInterface
    {
        List<Player> Players { get; }

        public List<Player> EstablishPlayerIdentity();

        public Player GetCurrentPlayer();

        public KeyValuePair<char, Point> GetNextMove();

        public void IntroduceGame();

        public void PresentLatestGrid(Grid grid);
    }
}
