using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class LivingCellss
    {
        public LivingCellss(BoardCoordonnates coordonnates)
        {
            this.coordonnates = coordonnates;
        }

        private BoardCoordonnates coordonnates;

        public int CountIn(BoardCells boardCells)
        {
            return new LivingCells(new MatchingCells(coordonnates, new DefaultCells(boardCells.Cells()))).Cells().Count();
        }
    }
}
