using System.Collections.Generic;

namespace GameOfLife
{
    public class Neighborhood : BoardCoordonnates
    {
        public Neighborhood(Coordonnate cellCoordonnates)
        {
            this.cellCoordonnates = cellCoordonnates;
        }

        public Neighborhood(int coordX, int coordY) : this(new Coordonnate(coordX, coordY))
        {
        }

        private Coordonnate cellCoordonnates;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return
                new List<Coordonnate>()
                {
                    new Coordonnate(cellCoordonnates.CoordX() - 1, cellCoordonnates.CoordY() - 1),
                    new Coordonnate(cellCoordonnates.CoordX(), cellCoordonnates.CoordY() - 1),
                    new Coordonnate(cellCoordonnates.CoordX() + 1, cellCoordonnates.CoordY() - 1),
                    new Coordonnate(cellCoordonnates.CoordX() + 1, cellCoordonnates.CoordY()),
                    new Coordonnate(cellCoordonnates.CoordX() + 1, cellCoordonnates.CoordY() + 1),
                    new Coordonnate(cellCoordonnates.CoordX(), cellCoordonnates.CoordY() + 1),
                    new Coordonnate(cellCoordonnates.CoordX() - 1, cellCoordonnates.CoordY() + 1),
                    new Coordonnate(cellCoordonnates.CoordX() - 1, cellCoordonnates.CoordY()),
                };
        }
    }
}
