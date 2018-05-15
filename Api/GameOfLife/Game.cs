using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    /// <summary>
    /// Represent a game of evolution
    /// </summary>
    public interface IEvolutionGame
    {
        /// <summary>
        /// Return a new evolution step : can be the next generation or more (see LastEvolution)
        /// </summary>
        /// <returns></returns>
        IEvolutionGame EvolutionnateGame();

        /// <summary>
        /// Return the cells that are alive in this game
        /// </summary>
        /// <returns></returns>
        IEnumerable<Coordonnate> LivingCells();
        
        /// <summary>
        /// Represent the number of evolution for this game
        /// </summary>
        /// <returns></returns>
        int CurrentEvolution();

        /// <summary>
        /// Return the evolution history of this game
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEvolutionGame> History();
    }

    public class EvolutionUntil : IEvolutionGame
    {
        public EvolutionUntil(int numberOfGeneration, IEvolutionGame currentGame)
        {
            this.numberOfGeneration = numberOfGeneration;
            this.currentGame = currentGame;
        }

        public EvolutionUntil(int numberOfGeneration, IEnumerable<Coordonnate> livingCells)
            : this(numberOfGeneration, new DefaultEvolution(livingCells))
        {            
        }

        private IEvolutionGame currentGame;
        private int numberOfGeneration;

        public IEvolutionGame EvolutionnateGame()
        {
            IEvolutionGame last = this.currentGame.EvolutionnateGame();
            for (int generation = 1; generation <= numberOfGeneration; generation++)
            {
                last = last.EvolutionnateGame();
            }
            return last;
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.currentGame.LivingCells();
        }

        public int CurrentEvolution()
        {
            return this.currentGame.CurrentEvolution();
        }

        public IEnumerable<IEvolutionGame> History()
        {
            return this.currentGame.History();
        }
    }

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
            this.evolvedLivingCoords = new ConvertingCoordonnates(new LivingCells(new EvolvedCells(livingCells)));
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

