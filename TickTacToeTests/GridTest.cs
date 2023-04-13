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
        private const char NO_TOKEN = '-';

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
        public void SetTokenDoesNotAppearInAvailableCells()
        {
            var token = new Point(1, 1);
            grid.SetCell(token, X_TOKEN);
            var availableCells = grid.GetAvailableCells();
            Assert.NotNull(availableCells);
            Assert.DoesNotContain(token, availableCells);
        }

        [Fact]
        public void CanAddTokensToGrid()
        {
            var currentToken = O_TOKEN;

            RunLambdaOnAllCells((Point coord) =>
            {
                grid.SetCell(coord, currentToken);
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
                    Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetCell(new(rowIndex, colIndex), currentToken));
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
        public void ACellCanBeSetAndThenReset()
        {
            var coord = new Point(1, 1);
            var currentToken = grid.GetCell(coord);
            Assert.Equal(NO_TOKEN, currentToken);
            grid.SetCell(coord, X_TOKEN);
            currentToken = grid.GetCell(coord);
            Assert.Equal(X_TOKEN, currentToken);
            grid.Reset();
            currentToken = grid.GetCell(coord);
            Assert.Equal(NO_TOKEN, currentToken);
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
                grid.SetCell(coord, X_TOKEN);
                var token = grid.GetCell(coord);
                Assert.Equal(X_TOKEN, token);
                grid.SetCell(coord, O_TOKEN);
                token = grid.GetCell(coord);
                Assert.Equal(O_TOKEN, token);
            });
        }

        [Fact]
        public void EmptyTokenIsSymbol() // Confirm that the empty token is not null or empty string
        {
            Assert.Equal(NO_TOKEN, grid.GetCell(new(0, 0)));
        }

        [Fact]
        public void InvalidTokensThrowException()
        {
            var coord = new Point(1, 1);
            var validTokens = new char[] { NO_TOKEN, X_TOKEN, O_TOKEN };
            for (char token = char.MinValue; token < char.MaxValue; token++)
            {
                if (validTokens.Contains(token))
                {
                    grid.SetCell(coord, token);
                    grid.Reset();
                } else
                {
                    Assert.Throws<ArgumentException>(() => { grid.SetCell(coord, token); });
                    grid.Reset();
                }
            }
        }
    }

}