using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class DeadCells : BoardCells
    {
        public DeadCells(BoardCells cells)
        {
            this.cells = cells;
        }

        private BoardCells cells;

        public IEnumerable<Cell> Cells()
        {
            return cells.Cells().Where(cell => !cell.Alive());
        }
    }



}
