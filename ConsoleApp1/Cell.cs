namespace Sudoku {
     class Cell {

        public List<int> domain;
        public int value;
        public bool isFixed;

        public Cell(List<int> domain, int value, bool isFixed) {
            this.domain = domain;
            this.value = value;
            this.isFixed = isFixed;
        }

        public int getNextElementInDomain(int currentValue) {

            foreach (int domainValue in domain) {
                if (domainValue > currentValue) {
                    return domainValue;
                }
            }

            return 0;

        }

    }
}
