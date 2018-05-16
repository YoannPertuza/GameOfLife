namespace GameOfLife
{
    public class BasicRules : Rule
    {
        public BasicRules(bool isCellAlive, BoardCoordonnates neighborhoodCells)
            : this(isCellAlive, new BasicStayAlive(neighborhoodCells),  new BasicBecomeAlive(neighborhoodCells))
        {
        }

        public BasicRules(bool isCellAlive, Rule stayAlive, Rule becomeAlive)
        {
            this.isCellAlive = isCellAlive;
            this.becomeAlive = becomeAlive;
            this.stayAlive = stayAlive;
        }

        private bool isCellAlive;
        private Rule becomeAlive;
        private Rule stayAlive;

        public bool IsAlive(BoardCells livingCells)
        {
            return this.isCellAlive ? this.stayAlive.IsAlive(livingCells) : this.becomeAlive.IsAlive(livingCells);
        }
    }


    
}
