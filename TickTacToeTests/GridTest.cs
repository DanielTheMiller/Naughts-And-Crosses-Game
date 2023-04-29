using System.Drawing;
using System.Reflection;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToeTests
{
    public class GridTest
    {
        private IGrid grid = new Grid();

        private const char X_TOKEN = 'X'; // This might become an enum or clas
        private const char O_TOKEN = 'O';
        private const char NO_TOKEN = '-';

        private const int GRID_LENGTH_HEIGHT = 3;
        private readonly int GRID_CELL_COUNT = GRID_LENGTH_HEIGHT * GRID_LENGTH_HEIGHT;

        private void RunLambdaOnAllCells(Action<Point> lambdaExpression)
        {
            for (int rowIndex = 0; rowIndex < GRID_LENGTH_HEIGHT; rowIndex++)
            {
                for (int colIndex = 0; colIndex < GRID_LENGTH_HEIGHT; colIndex++)
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
            for (int rowIndex = -1; rowIndex < GRID_LENGTH_HEIGHT + 1; rowIndex+= GRID_LENGTH_HEIGHT + 1)
            {
                for (int colIndex = -1; colIndex < GRID_LENGTH_HEIGHT + 1; colIndex+= GRID_LENGTH_HEIGHT + 1)
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

        [Fact]
        public void GetLinesReturnsListOfLines()
        {
            var getLinesMethod = typeof(Grid).GetMethod("getLines", BindingFlags.NonPublic | BindingFlags.Instance);
            var lines = (List<List<char>>)getLinesMethod.Invoke(grid, Array.Empty<object>());
            Assert.NotNull(lines);
            Assert.NotEmpty(lines);
            var firstLine = lines.FirstOrDefault();
            Assert.NotNull(firstLine);
            Assert.Contains(NO_TOKEN, firstLine);
            Assert.DoesNotContain(O_TOKEN, firstLine);
            Assert.DoesNotContain(X_TOKEN, firstLine);
        }

        [Fact]
        public void GetLinesReturnsAllXWhenGridIsAllX()
        {
            RunLambdaOnAllCells((point) => grid.SetCell(point, X_TOKEN));
            var getLinesMethod = typeof(Grid).GetMethod("getLines", BindingFlags.NonPublic | BindingFlags.Instance);
            var lines = (List<List<char>>)getLinesMethod.Invoke(grid, Array.Empty<object>());
            var firstLine = lines.FirstOrDefault();
            Assert.NotNull(firstLine); Assert.Contains(X_TOKEN, firstLine);
            Assert.DoesNotContain(O_TOKEN, firstLine);
            Assert.DoesNotContain(NO_TOKEN, firstLine);
        }

        [Fact]
        public void GetLinesReturnsAllLines()
        {
            var noOfLinesToAnticipate = (GRID_LENGTH_HEIGHT * 2)+2; // Line for each row/column and 2 diagonal lines
            var getLinesMethod = typeof(Grid).GetMethod("getLines", BindingFlags.NonPublic | BindingFlags.Instance);
            var lines = (List<List<char>>)getLinesMethod.Invoke(grid, Array.Empty<object>());
            Assert.NotNull(lines);
            Assert.Equal(noOfLinesToAnticipate, lines.Count);
        }

        [Fact]
        public void GetLinesRespondsToChange()
        {
            RunLambdaOnAllCells((point) => grid.SetCell(point, X_TOKEN));
            var getLinesMethod = typeof(Grid).GetMethod("getLines", BindingFlags.NonPublic | BindingFlags.Instance);
            var lines = (List<List<char>>)getLinesMethod.Invoke(grid, Array.Empty<object>());
            var firstLine = lines.FirstOrDefault();
            Assert.NotNull(firstLine); Assert.Contains(X_TOKEN, firstLine);
            Assert.DoesNotContain(O_TOKEN, firstLine);
            Assert.DoesNotContain(NO_TOKEN, firstLine);
            
            //Change all of the cells
            RunLambdaOnAllCells((point) => grid.SetCell(point, O_TOKEN));
            lines = (List<List<char>>)getLinesMethod.Invoke(grid, Array.Empty<object>());
            firstLine = lines.FirstOrDefault();
            Assert.NotNull(firstLine); Assert.Contains(O_TOKEN, firstLine); // Ensure it's actually changed
            Assert.DoesNotContain(X_TOKEN, firstLine);
            Assert.DoesNotContain(NO_TOKEN, firstLine);
        }

        [Fact]
        public void GetLinesShouldOnlyHave1CompleteLineWhenTheFirstRowIsComplete()
        {
            grid.SetCell(new Point(0, 0), X_TOKEN);
            grid.SetCell(new Point(1, 0), X_TOKEN);
            grid.SetCell(new Point(2, 0), X_TOKEN);
            var getLinesMethod = typeof(Grid).GetMethod("getLines", BindingFlags.NonPublic | BindingFlags.Instance);
            var lines = getLinesMethod.Invoke(grid, Array.Empty<object>()) as List<List<char>>;

            var successLineCount = 0;
            foreach (List<char> line in lines) {
                if (line.Count(x => x == X_TOKEN) == GRID_LENGTH_HEIGHT) { 
                    successLineCount++; // Count the successful lines
                } 
            }

            Assert.Equal(1, successLineCount);
        }

        [Fact]
        public void GridHasGetCompleteLinesMethod()
        {
            var completedLines = grid.GetCompletedLines();
            Assert.NotNull(completedLines);
            Assert.Empty(completedLines); // A new grid should have no compelete lines
        }

        [Fact]
        public void GridHasMaxCompleteLinesWhenGridIsFull()
        {
            var anticipatedLines = (GRID_LENGTH_HEIGHT * 2) + 2;
            RunLambdaOnAllCells((point) => { grid.SetCell(point, X_TOKEN); });
            var completedLines = grid.GetCompletedLines();
            Assert.NotNull(completedLines);
            Assert.Equal(anticipatedLines, completedLines.Count);
        }

        [Fact]
        public void GetPointFromIndexReturnsCorrectValues()
        {
            var pointIndex = 1;
            for (int yIndex = 0; yIndex < GRID_LENGTH_HEIGHT; yIndex++)
            {
                for (int xIndex = 0; xIndex < GRID_LENGTH_HEIGHT; xIndex++)
                {
                    var point = Grid.GetPointFromIndex(pointIndex);
                    Assert.NotNull(point);
                    Assert.Equal(new Point(xIndex, yIndex), point);
                    pointIndex++;
                }
            }
        }
    }
}