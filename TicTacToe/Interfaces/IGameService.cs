using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
    public interface IGameService
    {
        public void LaunchGame();

        public Player GetCurrentPlayer();

        /// <summary>
        /// Switch current contextual player to the opposing player
        /// </summary>
        /// <returns>New player</returns>
        public Player ToggleCurrentPlayer();
    }
}
