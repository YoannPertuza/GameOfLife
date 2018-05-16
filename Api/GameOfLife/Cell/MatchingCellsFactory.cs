namespace GameOfLife
{
    public class MatchingCellsFactory
    {
        public MatchingCellsFactory(BoardCoordonnates neighborhoodCells)
        {
            this.neighborhoodCells = neighborhoodCells;
        }

        private BoardCoordonnates neighborhoodCells;

        public BoardCells MatchingCells(BoardCells livingCells)
        {
            return new MatchingCells(neighborhoodCells, livingCells.Cells());
        }
    }


    
}
