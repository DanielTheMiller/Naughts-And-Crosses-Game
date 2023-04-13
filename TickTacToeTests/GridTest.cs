using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TickTacToeTests
{
    public class GridTest
    {
        private Grid grid = new Grid();

        private const string X_TOKEN = "X"; // This might become an enum or clas
        private const string O_TOKEN = "O";

        private const int GRID_LENGTH = 3;
        private const int GRID_HEIGHT = 3;
        private readonly int GRID_CELL_COUNT = GRID_HEIGHT * GRID_LENGTH;

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

        [Fact]
        public void NewGridReturns9AvailableCells()
        {
            var availableCells = grid.GetAvailableCells();
            Assert.NotNull(availableCells);
            Assert.Equal(9, availableCells.Count);
        }

        [Fact]
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

        [Fact]
        public void GridAddRejectsInvalidCoordinates()
        {
            var currentToken = O_TOKEN;
            var attempts = 0;
            for (int rowIndex = -1; rowIndex < 4; rowIndex+=4)
            {
                for (int colIndex = -1; colIndex < 4; colIndex+=4)
                {
                    Assert.Throws<ArgumentOutOfRangeException>(() => grid.AddToken(currentToken, new(rowIndex, colIndex)));
                    currentToken = currentToken == O_TOKEN ? X_TOKEN : O_TOKEN; // Toggle current token
                    attempts++;
                }
            }
            Assert.Equal(4, attempts); // I just want to make sure the loop went all of the way through
        }

        [Fact]
        public void FillingGridWithTokensLimitsAvailableGridCellsReturned()
        {
            CanAddTokensToGrid();
            var availableCells = grid.GetAvailableCells();
            Assert.NotNull(availableCells);
            Assert.Empty(availableCells);
        }

        [Fact]
        public void GridCanBeReset()
        {
            grid.Reset();
        }

        [Fact]
        public void FillGridWithTokensAndThenResettingMakesAllCellsAvailableAgain()
        {
            CanAddTokensToGrid();
            var availableCells = grid.GetAvailableCells();
            Assert.Empty(availableCells);
            grid.Reset();
            availableCells = grid.GetAvailableCells();
            Assert.Equal(GRID_CELL_COUNT, availableCells.Count);
        }
    }

}