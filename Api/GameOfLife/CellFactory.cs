using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    

    public interface BoardCoordonnates
    {
        IEnumerable<Coordonnate> Coordonnates();
    }

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

    public class DeadNeighborhood : BoardCoordonnates
    {
        public DeadNeighborhood(IEnumerable<Coordonnate> livingCoords)
            : this(new ConvertingCells(true, livingCoords))
        {           
        }

        public DeadNeighborhood(BoardCells livingCells)
        {
            this.livingCells = livingCells;
        }

        private BoardCells livingCells;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            var livingCells = this.livingCells.Cells();
            return livingCells.SelectMany(cell => cell.DeadNeighborhood(livingCells));           
        }
    }

   

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

    public class ConvertingCoordonnates : BoardCoordonnates
    {
        public ConvertingCoordonnates(BoardCells cellsToConvert)
        {
            this.cellsToConvert = cellsToConvert;
        }

        private BoardCells cellsToConvert;

        public IEnumerable<Coordonnate> Coordonnates()
        {
            return this.cellsToConvert.Cells().Select(cell => cell.Coordonnate()).ToList();
        }
    }

    public interface CellFactory
    {
        Cell Cellule(Coordonnate coord);
    }

    public class SimpleCell : CellFactory
    {
        public SimpleCell(bool alive)
        {
            this.alive = alive;
        }

        private bool alive;

        public Cell Cellule(Coordonnate coord)
        {
            return
                new Cell(
                    alive,
                    new Coordonnate(coord.CoordX(), coord.CoordY()),
                    new Neighborhood(coord.CoordX(), coord.CoordY())
                );
        }
    }
}
