using System.Collections.Generic;

namespace GameOfLife
{
    public class SimpleBoard : BoardCells
    {
        public SimpleBoard(IEnumerable<Cell> cells)
        {
            this.cells = cells;
        }

        private IEnumerable<Cell> cells;

        public IEnumerable<Cell> Cells()
        {
            return this.cells;
        }
    }



}
