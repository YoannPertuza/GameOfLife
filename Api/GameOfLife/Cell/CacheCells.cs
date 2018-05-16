using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class CacheCells : BoardCells
    {
        public CacheCells(BoardCells cells)
        {
            this.cells = cells;
            this.cache = new List<Cell>();
        }

        private List<Cell> cache;
        private BoardCells cells;

        public IEnumerable<Cell> Cells()
        {
            if (!this.cache.Any())
            {
                this.cache.AddRange(cells.Cells());
            }

            return this.cache;
        }
    }



}
