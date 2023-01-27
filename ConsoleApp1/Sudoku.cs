using System.Collections;

namespace Sudoku {

    using System.Linq;
    class Sudoku {

        private Hashtable cells; // Cells (rows) in the sudoku
        private Location currentCellLocation; // Current cell of the search
        private int currentValue; // Current value of the search 


        public Sudoku(List<List<int>> sudokuInput) {

            cells = new Hashtable();
            currentCellLocation = new Location(0, 0);
            currentValue = 0;

            // Create and populate an initial domain with 1-9
            List<int> initialDomain = new List<int>();
            for (int i = 1; i <= 9; i++) {
                initialDomain.Add(i);
            }

            // For every sudoku input value
            for (int y = 0; y < sudokuInput.Count; y++) {

                for (int x = 0; x < sudokuInput[y].Count; x++) {

                    int value = sudokuInput[y][x];
                    Location location = new Location(x, y);

                    // If value is 0, then add an empty cell with an initial domain, else add the fixed value
                    if (value == 0) {
                        cells.Add(location, new Cell(new List<int>(initialDomain), value, false, location));
                    } else {
                        cells.Add(location, new Cell(new List<int>(), value, true, location));
                    }

                }

            }

            makeNodeConsistent(); 

        }

        private Cell[] getAllCellsInRow(int row) {

            Cell[] rowCells = new Cell[9];

            for (int x = 0; x < 9; x++) {
                rowCells[x] = (Cell) cells[new Location(x, row)];
            }

            return rowCells;
        }

        private Cell[] getAllCellsInColumn(int column) {

            Cell[] columnCells = new Cell[9];

            for (int y = 0; y < 9; y++) {
                columnCells[y] = (Cell) cells[new Location(column, y)];
            }

            return columnCells;
        }

        private Cell[] getAllCellsInBlock(Location cellLocation) {

            Location getBlockLocationFromCellLocation(Location cellLocation) {
                return new Location(
                (int)Math.Floor((decimal)(cellLocation.x / 3)),
                (int)Math.Floor((decimal)(cellLocation.y / 3)));
            }

            Cell[] cellsInBlock = new Cell[9];
            Location blockLocation = getBlockLocationFromCellLocation(cellLocation);
            int index = 0;

            for (int x = blockLocation.x * 3; x < blockLocation.x * 3 + 3; x++) {

                for (int y = blockLocation.y * 3; y < blockLocation.y * 3 + 3; y++) {

                    cellsInBlock[index] = (Cell) cells[new Location(x, y)];
                    index++;

                }

            }

            return cellsInBlock;
        }

        private void makeNodeConsistent() {

            // For all cells that are non zero, remove the cell value from the domain of the cells in the column, row and block
            for (int x = 0; x < 9; x++) {

                for (int y = 0; y < 9; y++) {

                    Cell cell = (Cell) cells[new Location(x, y)];
;
                    if (cell.value != 0) {
                        removeFromDomains(new Location(x, y), cell.value);
                    }

                }

            }

        }

        private int findNextPartialSolution() {

            // Find the next possible value for the current cell
            Cell cell = (Cell)cells[currentCellLocation];
            int nextElementInDomain = cell.getNextElementInDomain(currentValue);
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
                if(!rowCell.location.equals(cellLocation)) {
                    rowCell.domain.Remove(value);
                }
            }
            
            foreach (Cell columnCell in getAllCellsInColumn(cellLocation.x)) {
                if (!columnCell.location.equals(cellLocation)) {
                    columnCell.domain.Remove(value);
                }
            }

            foreach (Cell blockCell in getAllCellsInBlock(cellLocation)) {
                if (!blockCell.location.equals(cellLocation)) {
                    blockCell.domain.Remove(value);
                }
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
        public void printSudoku()
        {
            for(int y = 1; y <= 9; y++)
            {
                for(int x = 1; x <= 9; x++)
                {
                    Cell xyCell = (Cell) cells[new Location(x-1, y-1)];
                    Console.Write(xyCell.value + " ");

                    if (x%3 == 0)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
                if(y%3 == 0)
                    {
                        Console.Write("\n");
                    }
            }
        }
        public void printSudokuDomains()
        {
            for(int y = 1; y <= 9; y++)
            {
                for(int x = 1; x <= 9; x++)
                {
                    Cell xyCell = (Cell) cells[new Location(x-1, y-1)];

                    Console.Write("(");
                    foreach(int value in xyCell.domain)
                    {
                        Console.Write(" "+ value);
                    }
                    Console.Write(") ");

                    if(x%3 == 0)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
                if(y%3 == 0)
                    {
                        Console.Write("\n");
                    }
            }
        }

        private Location getNextCellLocation() {

            int x, y;
            x = currentCellLocation.x;
            y = currentCellLocation.y;

            while (true) {

                if (x < 8) {
                    x++;
                }
                
                else if (y < 8) {
                    x = 0;
                    y++;
                }

                else {
                    return new Location(-1, -1);
                }

                Cell cell = (Cell) cells[new Location(x, y)];

                if (!cell.isFixed) {
                    return new Location(x, y);
                }

            }

        }

        public void solve() {

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

                    Console.WriteLine("Partial solution empty");

                    // Readd the current cell value to the domains since we are not going to tuse this value anymore
                    addToDomains(currentCellLocation, ((Cell) cells[currentCellLocation]).value);

                    // Set the current cell location and value to the previous element on the stack
                    (Location, int) previous = chronologicalBackTrackingStack.Pop();
                    currentCellLocation = previous.Item1;
                    currentValue = previous.Item2;

                    // Also readd the value of the previous (now current) cell to the domains,
                    // since we are also not going to use this value anymore.
                    addToDomains(currentCellLocation, currentValue);
                    
                    continue;

                }

                // If the forward check is valid push to the stack and go to the next cell,
                // otherwise keep searching for a valid partial solution
                if (forwardCheckIsValid()) {
                    
                    chronologicalBackTrackingStack.Push((currentCellLocation, currentValue));   // Add current cell value to the stack
                    removeFromDomains(currentCellLocation, currentValue);                       // Update the domains
                    Location nextCellLocation = getNextCellLocation();                          // Get next cell location

                    Cell currentCell = (Cell)cells[currentCellLocation];
                    currentCell.value = currentValue;

                    if (nextCellLocation.equals(new Location(-1, -1))) {
                        solved = true;
                        break;
                    }

                    currentCellLocation = nextCellLocation;
                    currentValue = 0;

                }

                else {
                    Console.WriteLine("Forward check invalid");
                }

                printSudoku();

            }

        }

    }

}