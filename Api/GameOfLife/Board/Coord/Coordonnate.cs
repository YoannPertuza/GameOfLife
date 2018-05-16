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


        public override int GetHashCode()
        {
            return this.x * 10 + this.y + this.y * 10 + this.x; 
        }
    }


    
}
