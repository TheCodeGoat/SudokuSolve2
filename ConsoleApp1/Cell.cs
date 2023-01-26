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

        public int getNextElementInDomain(int startValue) {

            foreach (int domainValue in domain) {
                if (domainValue >= startValue) {
                    return domainValue;
                }
            }

            return 0;

        }

    }
}
