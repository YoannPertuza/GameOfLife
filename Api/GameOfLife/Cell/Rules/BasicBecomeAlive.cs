using System.Linq;

namespace GameOfLife
{
    public class BasicBecomeAlive : Rule
    {
        public BasicBecomeAlive(BoardCoordonnates neighborhoodCells)
        {
            this.neighborhoodCells = new MatchingCellsFactory(neighborhoodCells);
        }

        private MatchingCellsFactory neighborhoodCells;

        public bool IsAlive(BoardCells livingCells)
        {
            return this.neighborhoodCells.MatchingCells(livingCells).Cells().Count() == 3;
        }
    }


    
}
