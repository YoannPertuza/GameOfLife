using System.Collections.Generic;

namespace GameOfLife
{
    public class DefaultCoordonnates : BoardCoordonnates
    {
        public DefaultCoordonnates(IEnumerable<Coordonnate> coords)
        {
            this.coords = coords;
        }

        private IEnumerable<Coordonnate> coords;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return this.coords;             
        }
    }
}
