﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public interface Rule
    {
        bool IsAlive(IEnumerable<Cell> boardCells);
    }

    public class MetaRule : Rule
    {
        public MetaRule(bool isCellAlive, BoardCoordonnates neighborhoodCells)
        {
            this.isCellAlive = isCellAlive;
            this.becomeAlive = new BecomeAlive(new LivingCellss(neighborhoodCells));
            this.stayAlive = new StayAlive(new LivingCellss(neighborhoodCells));
        }

        private bool isCellAlive;
        private Rule becomeAlive;
        private Rule stayAlive;

        public bool IsAlive(IEnumerable<Cell> boardCells)
        {
            return this.isCellAlive ? this.stayAlive.IsAlive(boardCells) : this.becomeAlive.IsAlive(boardCells);
        }
    }

    public class BecomeAlive : Rule
    {
        public BecomeAlive(LivingCellss livingCells)
        {
            this.livingCells = livingCells;
        }

        private LivingCellss livingCells;

        public bool IsAlive(IEnumerable<Cell> boardCells)
        {
            return livingCells.CountIn(boardCells) == 3;
        }
    }

    public class StayAlive : Rule
    {
        public StayAlive(LivingCellss livingCells)
        {
            this.livingCells = livingCells;
        }

        private LivingCellss livingCells;

        public bool IsAlive(IEnumerable<Cell> boardCells)
        {
            return livingCells.CountIn(boardCells) >= 2 && livingCells.CountIn(boardCells) <= 3;
        }
    }
}