﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public interface BoardCells
    {
        IEnumerable<Cell> Cells();
    }

    public class SimpleBoard : BoardCells
    {
        public SimpleBoard(IEnumerable<Cell> cells)
        {
            this.cells = cells;
        }

        private IEnumerable<Cell> cells;

        public IEnumerable<Cell> Cells()
        {
            return this.cells;
        }
    }

    public class ConvertingCells : BoardCells
    {
        public ConvertingCells(bool alive, IEnumerable<Coordonnate> coordonnates) : this(alive, new DefaultCoordonnates(coordonnates))
        {
        }

        public ConvertingCells(bool alive, BoardCoordonnates neighborhoodCells)
        {
            this.neighborhoodCells = neighborhoodCells;
            this.cellFactory = new SimpleCell(alive);
        }

        private BoardCoordonnates neighborhoodCells;
        private CellFactory cellFactory;
     
        public IEnumerable<Cell> Cells()
        {
            return neighborhoodCells.Coordonnates().Select(coord => cellFactory.Cellule(coord));
        }
    }

    public class LivingCells : BoardCells
    {
        public LivingCells(BoardCells cells)
        {
            this.cells = cells;
        }

        private BoardCells cells;

        public IEnumerable<Cell> Cells()
        {
            return cells.Cells().Where(cell => cell.Alive());
        }
    }

    public class DefaultCells : BoardCells
    {
        private IEnumerable<Cell> defaultCells;

        public DefaultCells(IEnumerable<Cell> defaultCells)
        {
            this.defaultCells = defaultCells;
        }

        public IEnumerable<Cell> Cells()
        {
            return this.defaultCells;
        }
    }

    public class MatchingCells : BoardCells
    {

        private BoardCoordonnates neighborhoodCells;
        private BoardCells cellsToMatch;

        public MatchingCells(BoardCoordonnates neighborhoodCells, BoardCells cellsToMatch)
        {
            this.neighborhoodCells = neighborhoodCells;
            this.cellsToMatch = cellsToMatch;
        }

        public IEnumerable<Cell> Cells()
        {
            return neighborhoodCells.Coordonnates().SelectMany(coord => cellsToMatch.Cells().Where(cell => cell.Matche(coord)));
        }
    }


    public class EvolvedCells : BoardCells
    {
        private BoardCells changingCells;

        public EvolvedCells(IEnumerable<Coordonnate> livingCoords)
        {
            this.changingCells = new ChangingCells(livingCoords);
        }

        public IEnumerable<Cell> Cells()
        {
            var cells = this.changingCells.Cells();

            return cells.Select(cell => cell.Evolve(cells));
        }
    }

    public class ChangingCells : BoardCells
    {
        private BoardCells deadCells;
        private BoardCells livingCells;

        public ChangingCells(IEnumerable<Coordonnate> livingCoords)
        {
            this.deadCells = new ConvertingCells(false, new Distinct(new DeadNeighborhood(livingCoords)));
            this.livingCells = new ConvertingCells(true, livingCoords);
        }

        public IEnumerable<Cell> Cells()
        {
            var alivedCells = this.livingCells.Cells();
            var deadCells = this.deadCells.Cells();

            return alivedCells.Concat(deadCells);
        }
    }



}
