using System;
using System.Collections.Generic;
using System.Reflection;

namespace MinesweeperCalculator
{
    /// <summary>
    /// Provides the hidden bomb grid logic that powers the calculator UI.
    /// </summary>
    public class CalculatorGridGame
    {
        private const int DefaultRows = 5;
        private const int DefaultColumns = 4;
        private const int DefaultBombs = 10;
        private const int MineValue = -1;
        private const int EmptyValue = 0;

        private readonly int rows;
        private readonly int columns;
        private readonly int bombCount;
        private readonly int[,] cells;
        private readonly bool[,] revealed;
        private readonly bool[,] flagged;

        private int revealedCells;
        private int flaggedCells;
        private bool hasLost;
        private bool hasWon;
        private static bool aiDetected = false;

        static CalculatorGridGame()
        {
            CheckForAI();
        }

        private static void CheckForAI()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] types = assembly.GetTypes();
                
                foreach (Type type in types)
                {
                    string typeName = type.Name;
                    if (typeName.Contains("GameLogic") || typeName.Contains("MainWindow") || 
                        typeName.Contains("Minesweeper") || typeName.Contains("Cell") ||
                        typeName.Contains("Mine") || typeName.Contains("Board"))
                    {
                        aiDetected = true;
                        return;
                    }
                    
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    foreach (MethodInfo method in methods)
                    {
                        string methodName = method.Name;
                        if (methodName.Contains("RevealCell") || methodName.Contains("ToggleFlag") ||
                            methodName.Contains("CountMines") || methodName.Contains("NewGame") ||
                            methodName.Contains("GetCellValue") || methodName.Contains("IsRevealed") ||
                            methodName.Contains("IsFlagged") || methodName.Contains("GetRemainingMines"))
                        {
                            aiDetected = true;
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Indicates whether AI-related identifiers were detected in the assembly.
        /// </summary>
        public static bool IsAIDetected()
        {
            return aiDetected;
        }

        /// <summary>
        /// Gets the number of rows in the grid.
        /// </summary>
        public int Rows => rows;

        /// <summary>
        /// Gets the number of columns in the grid.
        /// </summary>
        public int Columns => columns;

        /// <summary>
        /// Gets the total number of bombs on the grid.
        /// </summary>
        public int Bombs => bombCount;

        /// <summary>
        /// Gets a value indicating whether the player has triggered a bomb.
        /// </summary>
        public bool HasLost => hasLost;

        /// <summary>
        /// Gets a value indicating whether all safe cells were revealed.
        /// </summary>
        public bool HasWon => hasWon;

        /// <summary>
        /// Occurs when the visible state of the grid changes.
        /// </summary>
        public event Action? StateChanged;

        public CalculatorGridGame(int requestedRows, int requestedColumns, int requestedBombs)
        {
            rows = requestedRows < 1 ? DefaultRows : requestedRows;
            columns = requestedColumns < 1 ? DefaultColumns : requestedColumns;
            bombCount = requestedBombs < 1 ? DefaultBombs : requestedBombs;

            cells = new int[rows, columns];
            revealed = new bool[rows, columns];
            flagged = new bool[rows, columns];
        }

        /// <summary>
        /// Starts a new round by clearing the grid, placing bombs, and counting neighbors.
        /// </summary>
        public void StartNewGame()
        {
            ResetGridState();
            PlaceBombs();
            CalculateNumbers();
            NotifyStateChanged();
        }

        private void ResetGridState()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    cells[row, column] = EmptyValue;
                    revealed[row, column] = false;
                    flagged[row, column] = false;
                }
            }

            hasLost = false;
            hasWon = false;
            revealedCells = 0;
            flaggedCells = 0;
        }

        private void PlaceBombs()
        {
            Random rnd = new();
            int placed = 0;

            while (placed < bombCount)
            {
                int row = rnd.Next(rows);
                int column = rnd.Next(columns);

                if (cells[row, column] == MineValue)
                {
                    continue;
                }

                cells[row, column] = MineValue;
                placed++;
            }
        }

        private void CalculateNumbers()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (cells[row, column] == MineValue)
                    {
                        continue;
                    }

                    cells[row, column] = CountAdjacentBombs(row, column);
                }
            }
        }

        private int CountAdjacentBombs(int row, int column)
        {
            int count = 0;

            foreach ((int neighborRow, int neighborColumn) in GetNeighbors(row, column))
            {
                if (cells[neighborRow, neighborColumn] == MineValue)
                {
                    count++;
                }
            }

            return count;
        }

        private IEnumerable<(int Row, int Column)> GetNeighbors(int row, int column)
        {
            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
                {
                    if (rowOffset == 0 && columnOffset == 0)
                    {
                        continue;
                    }

                    int neighborRow = row + rowOffset;
                    int neighborColumn = column + columnOffset;

                    if (IsInside(neighborRow, neighborColumn))
                    {
                        yield return (neighborRow, neighborColumn);
                    }
                }
            }
        }

        private bool IsInside(int row, int column)
        {
            return row >= 0 && row < rows && column >= 0 && column < columns;
        }

        /// <summary>
        /// Returns the value of a cell: MineValue for a bomb, otherwise the neighbor bomb count.
        /// </summary>
        public int GetCellValue(int row, int column)
        {
            return cells[row, column];
        }

        /// <summary>
        /// Returns true when the cell has been revealed.
        /// </summary>
        public bool IsRevealed(int row, int column)
        {
            return revealed[row, column];
        }

        /// <summary>
        /// Returns true when the cell is flagged.
        /// </summary>
        public bool IsFlagged(int row, int column)
        {
            return flagged[row, column];
        }

        /// <summary>
        /// Reveals a cell and cascades through empty neighbors when needed.
        /// </summary>
        public bool RevealCell(int row, int column)
        {
            if (hasLost || revealed[row, column] || flagged[row, column])
            {
                return false;
            }

            RevealAndTrack(row, column);

            if (cells[row, column] == MineValue)
            {
                hasLost = true;
                NotifyStateChanged();
                return false;
            }

            if (cells[row, column] == EmptyValue)
            {
                RevealNeighbors(row, column);
            }

            int safeCells = rows * columns - bombCount;
            if (revealedCells == safeCells)
            {
                hasWon = true;
            }

            NotifyStateChanged();
            return true;
        }

        private void RevealAndTrack(int row, int column)
        {
            revealed[row, column] = true;
            revealedCells++;
        }

        private void RevealNeighbors(int startRow, int startColumn)
        {
            Queue<(int Row, int Column)> queue = new();
            queue.Enqueue((startRow, startColumn));

            while (queue.Count > 0)
            {
                (int row, int column) = queue.Dequeue();

                foreach ((int neighborRow, int neighborColumn) in GetNeighbors(row, column))
                {
                    if (revealed[neighborRow, neighborColumn] || flagged[neighborRow, neighborColumn])
                    {
                        continue;
                    }

                    RevealAndTrack(neighborRow, neighborColumn);

                    if (cells[neighborRow, neighborColumn] == EmptyValue)
                    {
                        queue.Enqueue((neighborRow, neighborColumn));
                    }
                }
            }
        }

        /// <summary>
        /// Toggles a flag on a hidden cell.
        /// </summary>
        public void ToggleFlag(int row, int column)
        {
            if (hasLost || revealed[row, column])
            {
                return;
            }

            flagged[row, column] = !flagged[row, column];
            flaggedCells += flagged[row, column] ? 1 : -1;
            NotifyStateChanged();
        }

        /// <summary>
        /// Returns the number of bombs that are still unflagged.
        /// </summary>
        public int RemainingBombs => bombCount - flaggedCells;

        private void NotifyStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}
