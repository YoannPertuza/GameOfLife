using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class RelativeCoordonnates
    {
        public RelativeCoordonnates(BoardCoordonnates figures)
        {
            this.figures = figures;
            this.baseCoord = new ReferenceCoordonnate(figures);
        }

        private BoardCoordonnates figures;
        private CoordonnatesOperation baseCoord; 

        public IEnumerable<RelativeCoordonnate> RelativeCoord()
        {
            return figures.Coordonnates().Select(coord => new RelativeCoordonnate(baseCoord.Select(), coord));
        }
    }


    
}
