using System.Runtime.CompilerServices;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TickTacToeTests
{
    public class GridTest
    {
        private Grid grid = new Grid();

        private const string X_TOKEN = "X"; // This might become an enum or clas
        private const string O_TOKEN = "O";

        [Fact]
        public void CanInstantiateGridClassSuccessfully()
        {
            Assert.NotNull(grid);
        }

        [Fact]
        public void GridIsOfGridInterface()
        {
            Assert.NotNull(grid);
            Assert.IsAssignableFrom<IGrid>(grid);
        }

        [Fact]
        public void GridCanReturnAvailableCells()
        {
            var availableCells = grid.GetAvailableCells();
            Assert.NotNull(availableCells);
        }

        [Fact]
        public void NewGridAvailableCellsAreDistinct()
        {
            var availableCells = grid.GetAvailableCells();
            var distinctCells = availableCells.Distinct().ToList();
            Assert.Equal(9, distinctCells.Count);
        }

        public void NewGridReturns9AvailableCells()
        {
            var availableCells = grid.GetAvailableCells();
            Assert.NotNull(availableCells);
            Assert.Equal(9, availableCells.Count);
        }

        public void CanAddTokensToGrid()
        {
            var currentToken = O_TOKEN;
            for (int rowIndex = 0; rowIndex < 3; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 3; colIndex++)
                {
                    grid.AddToken(currentToken, new(rowIndex, colIndex));
                    currentToken = currentToken == O_TOKEN ? X_TOKEN : O_TOKEN; // Toggle current token
                }
            }
        }
    }
}