using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }

    public class CoordonnateComparer : IEqualityComparer<Coordonnate>
    {

        public bool Equals(Coordonnate x, Coordonnate y)
        {
            return x.CoordX() == y.CoordX() && x.CoordY() == y.CoordY();
        }

        public int GetHashCode(Coordonnate obj)
        {
            return obj.CoordX() * 10 + obj.CoordY();
        }
    }




    
}
