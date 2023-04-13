using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TickTacToeTests
{
    public class GridTest
    {
        private Grid grid = new Grid();

        private const char X_TOKEN = 'X'; // This might become an enum or clas
        private const char O_TOKEN = 'O';

        private const int GRID_LENGTH = 3;
        private const int GRID_HEIGHT = 3;
        private readonly int GRID_CELL_COUNT = GRID_HEIGHT * GRID_LENGTH;

        private void RunLambdaOnAllCells(Action<Point> lambdaExpression)
        {
            for (int rowIndex = 0; rowIndex < GRID_HEIGHT; rowIndex++)
            {
                for (int colIndex = 0; colIndex < GRID_LENGTH; colIndex++)
                {
                    lambdaExpression(new(rowIndex, colIndex));
                }
            }
        }

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

            RunLambdaOnAllCells((Point coord) =>
            {
                grid.SetCell(currentToken, coord);
                currentToken = currentToken == O_TOKEN ? X_TOKEN : O_TOKEN; // Toggle current token
            });
        }

        [Fact]
        public void GridAddRejectsInvalidCoordinates()
        {
            var currentToken = O_TOKEN;
            var attempts = 0;
            for (int rowIndex = -1; rowIndex < GRID_HEIGHT+1; rowIndex+= GRID_HEIGHT+1)
            {
                for (int colIndex = -1; colIndex < GRID_LENGTH+1; colIndex+=GRID_LENGTH+1)
                {
                    Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetCell(currentToken, new(rowIndex, colIndex)));
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

        [Fact]
        public void CanGetEachCellFromNewGrid()
        {
            RunLambdaOnAllCells((Point coord) =>
            {
                var token = grid.GetCell(coord);
                Assert.NotNull(token);
            });
        }

        [Fact]
        public void AssureEqualityAfterGettingAndSettingEachCell()
        {
            RunLambdaOnAllCells((Point coord) =>
            {
                grid.SetCell(X_TOKEN, coord);
                var token = grid.GetCell(coord);
                Assert.NotNull(token);
                Assert.Equal(X_TOKEN, token);
                grid.SetCell(O_TOKEN, coord);
                token = grid.GetCell(coord);
                Assert.NotNull(token);
                Assert.Equal(O_TOKEN, token);
            });
        }
    }

}