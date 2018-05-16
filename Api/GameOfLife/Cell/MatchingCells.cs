using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class MatchingCells : BoardCells
    {

        private BoardCoordonnates neighborhood;
        private BoardCells cellsToMatch;

        public MatchingCells(BoardCoordonnates neighborhood, BoardCells cellsToMatch)
        {
            this.neighborhood = neighborhood;
            this.cellsToMatch = new CacheCells(cellsToMatch);
        }

        public MatchingCells(BoardCoordonnates neighborhood, IEnumerable<Cell> cellsToMatch)
            : this(neighborhood, new DefaultCells(cellsToMatch))
        {
        }

        public IEnumerable<Cell> Cells()
        {
            return neighborhood.Coordonnates().SelectMany(coord => cellsToMatch.Cells().Where(cell => cell.Matche(coord)));
        }
    }



}
