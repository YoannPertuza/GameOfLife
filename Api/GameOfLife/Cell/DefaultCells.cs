using System.Collections.Generic;

namespace GameOfLife
{
    public class DefaultCells : BoardCells
    {
        private IEnumerable<Cell> defaultCells;

        public DefaultCells(IEnumerable<Cell> defaultCells)
        {
            this.defaultCells = defaultCells;
        }

        public IEnumerable<Cell> Cells()
        {
            return this.defaultCells;
        }
    }



}
