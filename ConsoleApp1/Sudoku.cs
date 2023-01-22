﻿namespace Sudoku {

    class Sudoku {

        private Dictionary<Location, Cell> cells; // Cells in the sudoku
        private Location currentCellLocation; // Current cell of the search
        private int currentValue; // Current value of the search

        public Sudoku(List<List<int>> sudokuInput) {

            cells = new Dictionary<Location, Cell>();

            // Create and pupulate an initial domain with 1-9
            List<int> initialDomain = new List<int>();
            for (int i = 1; i <= 9; i++) {
                initialDomain.Add(i);
            }

            // For every sudoku input value
            for (int y = 0; y < sudokuInput.Count; y++) {
                for (int x = 0; x < sudokuInput[y].Count; x++) {

                    int value = sudokuInput[y][x];

                    // If value is 0, then add an empty cell with an initial domain, else add the fixed value
                    if (value == 0) {
                        cells.Add(new Location(x, y), new Cell(initialDomain, value, false));
                    } else {
                        cells.Add(new Location(x, y), new Cell(new List<int>(), value, true));
                    }

                }
            }

        }

        private void makeNodeConsistent() {

            // TODO:
            //      Make data node consistent
            //      Update currentCellLocation and currentValue


        }

        private (Location, int) findNextPartialSolution(Location currentCellLocation, int currentValue) {

            // TODO:    Use Cell.getNextElementInDomain()
            //          Return 0 if no solution can be found

        }

        private bool forwardCheckIsValid() {



        }

        public void solve() {

            makeNodeConsistent();

            // Stack with the location and value of the previous succesful partial solution
            Stack<(Location, int)> chronologicalBackTrackingStack = new Stack<(Location, int)>();
            bool solved = false;

            // Loop while solution is not found
            while (!solved) {

                // Find the first possible partial solution with the currentCellLocation and the currentValue
                (Location, int) partialSolution = findNextPartialSolution(currentCellLocation, currentValue);
                currentValue = partialSolution.Item2;
                 
                // If no solution was found for the current cell then backtrack
                if (currentValue == 0) {
                    // TODO: currentCellLocation = previous cell location
                    currentValue = chronologicalBackTrackingStack.Pop().Item2;
                    continue;
                }

                // If the forward check is valid push to the stack and go to the next cell,
                // otherwise keep searching for a valid partial solution
                if (forwardCheckIsValid()) {
                    chronologicalBackTrackingStack.Push(partialSolution);
                    // TODO: currentCellLocation = next cell location
                }

            }

        }

    }

}