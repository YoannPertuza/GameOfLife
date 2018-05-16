using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class EvolvedCells : BoardCells
    {      
        public EvolvedCells(IEnumerable<Coordonnate> livingCoords)
        {         
            this.allCells = new CacheCells(new EngagedCells(livingCoords));
            this.livingCells = new CacheCells(new CoordonnatesToCells(true, livingCoords));
        }

        private BoardCells livingCells;
        private BoardCells allCells;

        public IEnumerable<Cell> Cells()
        {
            return allCells.Cells().Select(cell => cell.Evolve(livingCells));
        }
    }



}
