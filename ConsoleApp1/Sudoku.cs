namespace Sudoku {

    class Sudoku {

        private List<List<Cell>> cells; // Cells (rows) in the sudoku
        private Location currentCellLocation; // Current cell of the search
        private int currentValue; // Current value of the search

        public Sudoku(List<List<int>> sudokuInput) {

            cells = new List<List<Cell>>();
            currentCellLocation = new Location(0, 0);
            currentValue = 0;

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
                        cells[y][x] = new Cell(initialDomain, value, false);
                    } else {
                        cells[y][x] = new Cell(new List<int>(), value, true);
                    }

                }

            }

            makeNodeConsistent(); 

        }

        private List<Cell> getAllCellsInRow(int row) {

            return cells[row];

        }

        private List<Cell> getAllCellsInColumn(int column) {

            List<Cell> cellsInColumn = new List<Cell>();

            for (int row = 0; row < 9; row++) {
                cellsInColumn.Add(cells[row][column]);
            }

            return cellsInColumn;

        }

        private List<Cell> getAllCellsInBlock(Location cellLocation) {

            Location getBlockLocationFromCellLocation(Location cellLocation) {
                return new Location(
                (int)Math.Floor((decimal)(cellLocation.x / 3)),
                (int)Math.Floor((decimal)(cellLocation.y / 3)));
            }

           List<Cell> cellsInBlock = new List<Cell>();
           Location blockLocation = getBlockLocationFromCellLocation(cellLocation);

            for (int x = blockLocation.x * 3; x < blockLocation.x * 3 + 3; x++) {

                for (int y = blockLocation.y * 3; y < blockLocation.y * 3 + 3; y++) {
                    cellsInBlock.Add(cells[y][x]);
                }

            }

            return cellsInBlock;

        }

        private void makeNodeConsistent() {

            // For all cells that are non zero, remove the cell value from the domain of the cells in the column, row and block
            for (int x = 0; x < 9; x++) {

                for (int y = 0; y < 9; y++) {

                    Cell cell = cells[y][x]
;
                    if (cell.value != 0) {
                        removeFromDomains(new Location(x, y), cell.value);
                    }

                }

            }

        }

        private int findNextPartialSolution() {

            // Find the next possible value for the current cell
            int nextElementInDomain = cells[currentCellLocation.y][currentCellLocation.x].getNextElementInDomain(currentValue);
            return nextElementInDomain;

        }

        private bool forwardCheckIsValid() {

            // Check if there is a domain that is empty for cells in the same row, column, and block as the current cell

            foreach (Cell rowCell in getAllCellsInRow(currentCellLocation.y)) {
                if (rowCell.domain.Count == 0 && !rowCell.isFixed) {
                    return false;
                }
            }

            foreach (Cell columnCell in getAllCellsInColumn(currentCellLocation.x)) {
                if (columnCell.domain.Count == 0 && !columnCell.isFixed) {
                    return false;
                }
            }

            foreach (Cell blockCell in getAllCellsInBlock(currentCellLocation)) {
                if (blockCell.domain.Count == 0 && !blockCell.isFixed) {
                    return false;
                }
            }

            return true;
        }

        // Remove a value from all the domains of the cells in the row,column and block of a cell
        private void removeFromDomains(Location cellLocation, int value) {

            foreach (Cell rowCell in getAllCellsInRow(cellLocation.y)) {
                rowCell.domain.Remove(value);
            }

            foreach (Cell columnCell in getAllCellsInColumn(cellLocation.x)) {
                columnCell.domain.Remove(value);
            }

            foreach (Cell blockCell in getAllCellsInBlock(cellLocation)) {
                blockCell.domain.Remove(value);
            }

        }

        // Add a value to all the domains of the cells in the row,column and block of a cell
        private void addToDomains(Location cellLocation, int value) {

            foreach (Cell rowCell in getAllCellsInRow(cellLocation.y)) {
                rowCell.domain.Add(value);
            }

            foreach (Cell columnCell in getAllCellsInColumn(cellLocation.x)) {
                columnCell.domain.Add(value);
            }

            foreach (Cell blockCell in getAllCellsInBlock(cellLocation)) {
                blockCell.domain.Add(value);
            }

        }

        public void solve() {

            makeNodeConsistent();

            // Stack with the location and value of the previous succesful partial solution
            Stack<(Location, int)> chronologicalBackTrackingStack = new Stack<(Location, int)>();
            bool solved = false;

            // Loop while solution is not found
            while (!solved) {

                // Find the first possible partial solution with the currentCellLocation and the currentValue
                int partialSolution = findNextPartialSolution();
                currentValue = partialSolution;
                 
                // If no solution was found for the current cell then backtrack
                if (currentValue == 0) {

                    // Readd the current cell value to the domains since we are not going to tuse this value anymore
                    addToDomains(currentCellLocation, cells[currentCellLocation.y][currentCellLocation.x].value);

                    // Set the current cell location and value to the previous element on the stack
                    (Location, int) previous = chronologicalBackTrackingStack.Pop();
                    currentCellLocation = previous.Item1;
                    currentValue = previous.Item2;
                    
                    continue;

                }

                // If the forward check is valid push to the stack and go to the next cell,
                // otherwise keep searching for a valid partial solution
                if (forwardCheckIsValid()) {
                    
                    chronologicalBackTrackingStack.Push((currentCellLocation, currentValue));   // Add current cell value to the stack
                    removeFromDomains(currentCellLocation, currentValue);                       // Update the domains

                    // TODO: currentCellLocation = next cell location

                }

            }

        }

    }

}