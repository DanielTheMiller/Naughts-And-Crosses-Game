using System;
using TicTacToe.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TicTacToeTests.Resources
{
    public static class ExampleGrids
    {
        public static char[] GridTokens = { 'X', 'O' };

        public static Grid GetRandomStalemateGrid(Grid grid = null, char previousToken = 'X')
        {
            grid ??= new Grid(); // Create new Grid if one wasn't passed
            var currentToken = previousToken == 'X' ? 'O' : 'X';
            Random rnd = new Random();
            var availableCells = grid.GetAvailableCells();
            var gridIndexes = availableCells.Select(x => Grid.GetIndexFromPoint(x)).ToList();
            while (gridIndexes.Count > 0) {
                var randomIndex = rnd.Next(gridIndexes.Count);
                var gridIndex = gridIndexes[randomIndex];
                var gridPoint = Grid.GetPointFromIndex(gridIndex);
                gridIndexes.Remove(gridIndex);
                grid.SetCell(gridPoint, currentToken);
                if (grid.GetCompletedLines().Count > 0)
                {
                    grid.SetCell(gridPoint, '-');
                    continue;
                }
                var gridAvailableCellsBefore = grid.GetAvailableCells().Count();
                if (gridAvailableCellsBefore == 0)
                {
                    break;
                }
                GetRandomStalemateGrid(grid, currentToken);
                if (gridAvailableCellsBefore > grid.GetAvailableCells().Count())
                {
                    break;
                }
                //call getRandomStalemateGrid
                //call the next random available cell, remove as an option from the list of available indexes
                //if theres no change, 
                //If after that there's still no change, return grid with no change
            }
            return grid;
        }
    }
}
