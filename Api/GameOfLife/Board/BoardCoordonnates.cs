using System.Collections.Generic;

namespace GameOfLife
{
    public interface BoardCoordonnates
    {
        IEnumerable<Coordonnate> Coordonnates();
    }
}
