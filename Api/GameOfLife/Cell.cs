using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Cell
    {
        public Cell(bool alive, int coordonnateX, int coordonnateY, Neighborhood neighborhoodCells)
            : this(alive, new Coordonnate(coordonnateX, coordonnateY), neighborhoodCells, new BasicRules(alive, neighborhoodCells))
        {
        }

        public Cell(int coordonnateX, int coordonnateY)
            : this(false, new Coordonnate(coordonnateX, coordonnateY), new Neighborhood(coordonnateX, coordonnateY), new BasicRules(false, new Neighborhood(coordonnateX, coordonnateY)))
        {
        }

        public Cell(bool alive, Coordonnate coordonnate, Neighborhood neighborhoodCells)
            : this(alive, coordonnate, neighborhoodCells, new BasicRules(alive, neighborhoodCells))
        {
        }

        public Cell(bool alive, Coordonnate coordonnate)
            : this(alive, coordonnate, new Neighborhood(coordonnate), new BasicRules(alive, new Neighborhood(coordonnate)))
        {
        }

        public Cell(bool alive, Coordonnate coordonnate, Neighborhood neighborhoodCells, Rule rules)
        {
            this.coordonnate = coordonnate;
            this.neighborhoodCells = neighborhoodCells;
            this.alive = alive;
            this.rules = rules;
        }

        private Coordonnate coordonnate;
        private Neighborhood neighborhoodCells;
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
                    this.neighborhoodCells
                );
        }


        public Cell Evolve(BoardCells livingCells)
        {
            return
                new Cell(
                    this.rules.IsAlive(livingCells),
                    this.coordonnate,
                    this.neighborhoodCells
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
            return neighborhoodCells.Coordonnates().Where(nbCoord => !livingCells.Cells().Any(cell => cell.Matche(nbCoord)));
        }
    }
}
