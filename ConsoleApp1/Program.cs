using System;

namespace Sudoku {

    struct Location {

        public int x, y;
        public Location(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public bool equals(Location location) {
            return x == location.x && y == location.y;
        }

    }

    class Program {

        public static List<List<int>> convertRawSudokuInput(String rawInput) {

            // Split input on spaces and initialize a variable for the rows
            string[] input = rawInput.Trim().Split(' ');
            List<List<int>> rows = new List<List<int>>();

            // Populate the 'rows' variable with the input data
            for (int row = 0; row < 9; row++) {

                rows.Add(new List<int>());

                for (int column = 0; column < 9; column++) {

                    int inputValue = int.Parse(input[row * 9 + column]);
                    rows[row].Add(inputValue);

                }

            }

            return rows;

        }

        public static void Main(String[] args) {
            Console.WriteLine("Please input sudoku:");
            String rawSudokuInput = Console.ReadLine();
            List<List<int>> sudokuInput = convertRawSudokuInput(rawSudokuInput);
            Sudoku sudoku = new Sudoku(sudokuInput);
            Console.WriteLine("Unsolved sudoku:");
            Console.WriteLine("White text represent changable numbers, while blue ones are fixed.");
            sudoku.printSudoku();
            Console.WriteLine("Solving!\n");
            sudoku.solve();
            Console.WriteLine("Solved!");
            sudoku.printSudoku();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

        }

    }

}