using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class DeadNeighborhood : BoardCoordonnates
    {
        public DeadNeighborhood(IEnumerable<Coordonnate> livingCoords)
            : this(new CoordonnatesToCells(true, livingCoords))
        {           
        }

        public DeadNeighborhood(BoardCells livingCells)
        {
            this.livingCells = new CacheCells(livingCells);
        }

        private BoardCells livingCells;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return livingCells.Cells().SelectMany(cell => cell.DeadNeighborhood(livingCells));           
        }
    }
}
