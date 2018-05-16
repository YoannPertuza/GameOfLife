using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Distinct : BoardCoordonnates
    {
        public Distinct(BoardCoordonnates coords)
        {
            this.coords = coords;
            this.coordonnateComparer = new CoordonnateComparer();
        }

        private IEqualityComparer<Coordonnate> coordonnateComparer;
        private BoardCoordonnates coords;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return this.coords.Coordonnates().Distinct(coordonnateComparer);             
        }
    }
}
