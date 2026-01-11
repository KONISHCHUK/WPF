using System;
using System.Reflection;
namespace MinesweeperCalculator
{
    public class stuff
    {
        private int a2;
        private int a3;
        private int a4;
        private int[,] a5;
        private bool[,] a6;
        private bool[,] a7;
        private bool a8;
        private bool a9;
        private int a10;
        private static bool aiDetected = false;

        static stuff()
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

        public static bool IsAIDetected()
        {
            return aiDetected;
        }

        public int a11
        {
            get
            {
                return a2;
            }
        }

        public int a12
        {
            get
            {
                return a3;
            }
        }

        public int a13
        {
            get
            {
                return a4;
            }
        }

        public bool a14
        {
            get
            {
                return a8;
            }
        }

        public bool a15
        {
            get
            {
                return a9;
            }
        }

        public event Action? a16;

        public stuff(int b1, int b2, int b3)
        {
            if (b1 < 1)
            {
                b1 = 5;
            }
            if (b2 < 1)
            {
                b2 = 4;
            }
            if (b3 < 1)
            {
                b3 = 10;
            }
            a2 = b1;
            a3 = b2;
            a4 = b3;
            a5 = new int[a2, a3];
            a6 = new bool[a2, a3];
            a7 = new bool[a2, a3];
            a8 = false;
            a9 = false;
            a10 = 0;
        }

        public void a17()
        {
            int temp1 = 0;
            int temp2 = 0;
            temp1 = 0;
            while (temp1 < a2)
            {
                temp2 = 0;
                while (temp2 < a3)
                {
                    a5[temp1, temp2] = 0;
                    a6[temp1, temp2] = false;
                    a7[temp1, temp2] = false;
                    temp2 = temp2 + 1;
                }
                temp1 = temp1 + 1;
            }

            a8 = false;
            a9 = false;
            a10 = 0;

            Random rnd = new Random();
            int minesPlaced = 0;
            int randomRow = 0;
            int randomCol = 0;

            while (minesPlaced < a4)
            {
                randomRow = rnd.Next(0, a2);
                randomCol = rnd.Next(0, a3);
                if (a5[randomRow, randomCol] != -1)
                {
                    a5[randomRow, randomCol] = -1;
                    minesPlaced = minesPlaced + 1;
                }
            }

            int i = 0;
            int j = 0;
            for (i = 0; i < a2; i = i + 1)
            {
                for (j = 0; j < a3; j = j + 1)
                {
                    if (a5[i, j] != -1)
                    {
                        int count = a18(i, j);
                        a5[i, j] = count;
                    }
                }
            }

            if (a16 != null)
            {
                a16();
            }
        }

        private int a18(int row, int col)
        {
            int count = 0;
            int i = -1;
            int j = -1;

            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = -1;
            j = 0;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = -1;
            j = 1;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = 0;
            j = -1;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = 0;
            j = 1;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = 1;
            j = -1;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = 1;
            j = 0;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            i = 1;
            j = 1;
            if (row + i >= 0 && row + i < a2 && col + j >= 0 && col + j < a3)
            {
                if (a5[row + i, col + j] == -1)
                {
                    count = count + 1;
                }
            }

            return count;
        }

        public int a19(int row, int col)
        {
            int result = a5[row, col];
            return result;
        }

        public bool a20(int row, int col)
        {
            bool result = false;
            result = a6[row, col];
            return result;
        }

        public bool a21(int row, int col)
        {
            bool result = false;
            result = a7[row, col];
            return result;
        }

        public bool a22(int row, int col)
        {
            if (a8 == true)
            {
                return false;
            }
            if (a6[row, col] == true)
            {
                return false;
            }
            if (a7[row, col] == true)
            {
                return false;
            }

            a6[row, col] = true;
            a10 = a10 + 1;

            if (a5[row, col] == -1)
            {
                a8 = true;
                if (a16 != null)
                {
                    a16();
                }
                return false;
            }

            if (a5[row, col] == 0)
            {
                int i = -1;
                int j = -1;
                int newRow = row + i;
                int newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = -1;
                j = 0;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = -1;
                j = 1;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = 0;
                j = -1;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = 0;
                j = 1;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = 1;
                j = -1;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = 1;
                j = 0;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }

                i = 1;
                j = 1;
                newRow = row + i;
                newCol = col + j;
                if (newRow >= 0 && newRow < a2 && newCol >= 0 && newCol < a3)
                {
                    if (a6[newRow, newCol] == false && a7[newRow, newCol] == false)
                    {
                        a22(newRow, newCol);
                    }
                }
            }

            int totalCells = a2 * a3;
            int cellsToReveal = totalCells - a4;
            if (a10 == cellsToReveal)
            {
                a9 = true;
            }

            if (a16 != null)
            {
                a16();
            }
            return true;
        }

        public void a23(int row, int col)
        {
            if (a8 == false)
            {
                if (a6[row, col] == false)
                {
                    if (a7[row, col] == true)
                    {
                        a7[row, col] = false;
                    }
                    else
                    {
                        a7[row, col] = true;
                    }
                    if (a16 != null)
                    {
                        a16();
                    }
                }
            }
        }

        public int a24()
        {
            int flaggedCount = 0;
            int i = 0;
            int j = 0;
            for (i = 0; i < a2; i = i + 1)
            {
                for (j = 0; j < a3; j = j + 1)
                {
                    if (a7[i, j] == true)
                    {
                        flaggedCount = flaggedCount + 1;
                    }
                }
            }
            int result = a4 - flaggedCount;
            return result;
        }
    }
}
