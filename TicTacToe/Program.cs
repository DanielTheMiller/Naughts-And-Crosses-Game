// See https://aka.ms/new-console-template for more information
using TicTacToe.Interfaces;
using TicTacToe.Models;
using TicTacToe.Services;

public class Program
{
    public static void Main(string[] args)
    {
        ICommandLineInputService commandLineInputService = new CommandLineInputService();
        IUserInterface commandLineInterface = new CommandLineInterface(commandLineInputService);
        GameService gameService = new GameService(commandLineInterface);
        gameService.LaunchGame();
    }

}