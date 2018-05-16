using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
   
    public class Cell
    {
        public Cell(bool alive, int coordonnateX, int coordonnateY, BoardCoordonnates neighborhoodCoords)
            : this(alive, new Coordonnate(coordonnateX, coordonnateY), neighborhoodCoords, new BasicRules(alive, neighborhoodCoords))
        {
        }

        public Cell(int coordonnateX, int coordonnateY)
            : this(false, new Coordonnate(coordonnateX, coordonnateY), new Neighborhood(coordonnateX, coordonnateY), new BasicRules(false, new Neighborhood(coordonnateX, coordonnateY)))
        {
        }

        public Cell(bool alive, Coordonnate coordonnate, BoardCoordonnates neighborhoodCoords)
            : this(alive, coordonnate, neighborhoodCoords, new BasicRules(alive, neighborhoodCoords))
        {
        }

        public Cell(bool alive, Coordonnate coordonnate)
            : this(alive, coordonnate, new Neighborhood(coordonnate), new BasicRules(alive, new Neighborhood(coordonnate)))
        {
        }

        public Cell(bool alive, Coordonnate coordonnate, BoardCoordonnates neighborhoodCoords, Rule rules)
        {
            this.coordonnate = coordonnate;
            this.neighborhoodCoords = neighborhoodCoords;
            this.alive = alive;
            this.rules = rules;
        }

        private Coordonnate coordonnate;
        private BoardCoordonnates neighborhoodCoords;
        private bool alive;
        private Rule rules;

        public bool Alive()
        {
            return this.alive;
        }

        public Cell Alived()
        {
            return
                new Cell(
                    true,
                    this.coordonnate,
                    this.neighborhoodCoords
                );
        }


        public Cell Evolve(BoardCells livingCells)
        {
            return
                new Cell(
                    this.rules.IsAlive(livingCells),
                    this.coordonnate,
                    this.neighborhoodCoords
                );
        }

        public bool Matche(Coordonnate coord)
        {
            return this.coordonnate.CoordX() == coord.CoordX() && this.coordonnate.CoordY() == coord.CoordY();
        }

        public bool Matche(int x, int y)
        {
            return Matche(new Coordonnate(x, y));
        }

        public Coordonnate Coordonnate()
        {
            return this.coordonnate;
        }

        public IEnumerable<Coordonnate> DeadNeighborhood(BoardCells livingCells)
        {
            return
                new NoMatchingCoordonnates(
                    this.neighborhoodCoords,
                    livingCells
                ).Coordonnates();
        }
    }
}
