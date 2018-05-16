using System.Collections.Generic;

namespace GameOfLife
{
    public class CoordonnateComparer : IEqualityComparer<Coordonnate>
    {

        public bool Equals(Coordonnate x, Coordonnate y)
        {
            return x.CoordX() == y.CoordX() && x.CoordY() == y.CoordY();
        }

        public int GetHashCode(Coordonnate obj)
        {
            return obj.GetHashCode();
        }
    }


    
}
