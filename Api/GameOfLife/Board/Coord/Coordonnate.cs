using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Coordonnate 
    {
        public Coordonnate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private int x;
        private int y;

        public Cell CelluleIn(IEnumerable<Cell> boardCells)
        {
            return boardCells.Where(cells => cells.Matche(x, y)).First();
        }

        public int CoordX()
        {
            return this.x;
        }

        public int CoordY()
        {
            return this.y;
        }

        /**
         * Bijective algorithm
         */
        public override int GetHashCode()
        {
            int tmp = (y + ((x + 1) / 2));
            return x + (tmp * tmp);
        }
    }


    
}
