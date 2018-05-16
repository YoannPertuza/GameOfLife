using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class EngagedCells : BoardCells
    {
        private BoardCells deadCells;
        private BoardCells livingCells;

        public EngagedCells(IEnumerable<Coordonnate> livingCoords)
        {
            this.deadCells = new CoordonnatesToCells(false, new Distinct(new DeadNeighborhood(livingCoords)));
            this.livingCells = new CoordonnatesToCells(true, livingCoords);
        }

        public IEnumerable<Cell> Cells()
        {
            var alivedCells = this.livingCells.Cells();
            var deadCells = this.deadCells.Cells();

            return alivedCells.Concat(deadCells);
        }
    }



}
