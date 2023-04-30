using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Services
{
    public class GameService : IGameService
    {
        private IUserInterface userInterface;

        public GameService(IUserInterface userInterfaceService)
        {
            this.userInterface = userInterfaceService;
        }

        public void LaunchGame()
        {
            userInterface.EstablishPlayerIdentity();
        }
    }
}
