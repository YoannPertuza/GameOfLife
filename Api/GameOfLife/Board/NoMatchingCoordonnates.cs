using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class NoMatchingCoordonnates : BoardCoordonnates
    {

        private BoardCoordonnates coordonnates;
        private BoardCells cells;

        public NoMatchingCoordonnates(BoardCoordonnates coordonnates, BoardCells cells)
        {
            this.coordonnates = coordonnates;
            this.cells = new CacheCells(cells);
        }

        public NoMatchingCoordonnates(BoardCoordonnates coordonnates, IEnumerable<Cell> cells)
            : this(coordonnates, new DefaultCells(cells))
        {
        }

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return coordonnates.Coordonnates().Where(coord => !cells.Cells().Any(cell => cell.Matche(coord)));
        }
    }



}
