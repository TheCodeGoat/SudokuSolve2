namespace Sudoku {
     class Cell {

        public List<int> domain;
        public List<int> valuesTried;
        public int value;
        public bool isFixed;
        public Location location;

        public Cell(List<int> domain, int value, bool isFixed, Location location) {
            this.domain = domain;
            this.value = value;
            this.isFixed = isFixed;
            this.location = location;
            this.valuesTried = new List<int>();
        }

        public int getNextElementInDomain(int currentValue) {

            foreach (int domainValue in domain) {
                if (!valuesTried.Contains(domainValue)) {
                    return domainValue;
                }
            }

            return 0;

        }

    }
}
