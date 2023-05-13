using System;
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
        public List<Player> EstablishPlayerIdentity();

        public Player GetCurrentPlayer();

        public void SetCurrentPlayer(Player player);

        public KeyValuePair<char, Point> GetNextMove(Grid grid);

        public void IntroduceGame();

        public void PresentLatestGrid(Grid grid);

        public void PresentResults();
    }
}
