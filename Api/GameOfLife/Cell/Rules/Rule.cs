using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public interface Rule
    {
        bool IsAlive(BoardCells livingCells);
    }


    
}
