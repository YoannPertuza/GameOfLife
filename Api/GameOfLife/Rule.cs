using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public interface Rule
    {
        bool IsAlive(BoardCells livingCells);
    }

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
