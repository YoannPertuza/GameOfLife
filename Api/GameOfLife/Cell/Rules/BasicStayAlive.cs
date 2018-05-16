using System.Linq;

namespace GameOfLife
{
    public class BasicStayAlive : Rule
    {
        public BasicStayAlive(BoardCoordonnates neighborhoodCells)
        {
            this.neighborhoodCells = new MatchingCellsFactory(neighborhoodCells);
        }

        private MatchingCellsFactory neighborhoodCells;

        public bool IsAlive(BoardCells livingCells)
        {
            var matchingCells = neighborhoodCells.MatchingCells(livingCells);

            return matchingCells.Cells().Count() >= 2 && matchingCells.Cells().Count() <= 3;
        }
    }


    
}
