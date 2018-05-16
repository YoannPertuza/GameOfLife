using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class CoordonnatesToCells : BoardCells
    {
        public CoordonnatesToCells(bool alive, IEnumerable<Coordonnate> coordonnates) : this(alive, new DefaultCoordonnates(coordonnates))
        {
        }

        public CoordonnatesToCells(bool alive, BoardCoordonnates neighborhoodCells)
        {
            this.neighborhoodCells = neighborhoodCells;
            this.cellFactory = new SimpleCell(alive);
        }

        private BoardCoordonnates neighborhoodCells;
        private CellFactory cellFactory;
     
        public IEnumerable<Cell> Cells()
        {
            return neighborhoodCells.Coordonnates().Select(coord => cellFactory.Cellule(coord));
        }
    }



}
