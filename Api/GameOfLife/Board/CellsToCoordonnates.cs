using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class CellsToCoordonnates : BoardCoordonnates
    {
        public CellsToCoordonnates(BoardCells cellsToConvert)
        {
            this.cellsToConvert = cellsToConvert;
        }

        private BoardCells cellsToConvert;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return this.cellsToConvert.Cells().Select(cell => cell.Coordonnate()).ToList();
        }
    }
}
