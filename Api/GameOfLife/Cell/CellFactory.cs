using System;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    public interface CellFactory
    {
        Cell Cellule(Coordonnate coord);
    }
}
