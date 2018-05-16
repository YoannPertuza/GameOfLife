using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{


    public class CoordonnateCompare :IComparer<Coordonnate>
    {
        
        public int Compare(Coordonnate x, Coordonnate y)
        {
            if (x.GetHashCode() == y.GetHashCode())
            {
                return 0;
            }
            else if (x.GetHashCode() > y.GetHashCode())
            {
                return 1;
            }
            else 
            {
                return -1;
            }
        }
    }


    
}
