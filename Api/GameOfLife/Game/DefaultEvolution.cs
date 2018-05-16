using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    public class DefaultEvolution : IEvolutionGame
    {
        public DefaultEvolution(IEnumerable<Coordonnate> livingCells) : this(0, livingCells)
        {
        }

        public DefaultEvolution(int round, IEnumerable<Coordonnate> livingCells)
            : this(round, livingCells, new List<IEvolutionGame>())
        {
        }

        public DefaultEvolution(int round, IEnumerable<Coordonnate> livingCells, IEnumerable<IEvolutionGame> history)
        {
            this.round = round;
            this.livingCells = livingCells;         
            this.history = history;
            this.evolvedLivingCoords = new CellsToCoordonnates(new LivingCells(new EvolvedCells(livingCells)));
        }

        private int round;
        private IEnumerable<Coordonnate> livingCells;
        private BoardCoordonnates evolvedLivingCoords;
        private IEnumerable<IEvolutionGame> history;


        public IEvolutionGame EvolutionnateGame()
        {
            return 
                new DefaultEvolution(
                    ++round, 
                    evolvedLivingCoords.Coordonnates(), 
                    new List<IEvolutionGame>(this.History()) { new DefaultEvolution(round, livingCells) });
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.livingCells;
        }

        public int CurrentEvolution()
        {
            return this.round;
        }

        public IEnumerable<IEvolutionGame> History()
        {
            return this.history;
        }
    }




    

   

    

    

   

    



    
}

