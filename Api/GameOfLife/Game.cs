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

    public class LastEvolution : IEvolutionGame 
    {
        public LastEvolution(int numberOfGeneration, IEvolutionGame currentGame)
        {
            this.currentGame = currentGame;
            this.gameBuilder = new NGenerationBuilder(currentGame, numberOfGeneration);
        }

        public LastEvolution(int numberOfGeneration, IEnumerable<Coordonnate> livingCells)
            : this(numberOfGeneration, new DefaultEvolution(livingCells))
        {
        }

        private IEvolutionGame currentGame;
        private IEvolutionBuilder gameBuilder;
  
        public IEvolutionGame EvolutionnateGame()
        {       
            return gameBuilder.Next(currentGame).CurrentGame();
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.gameBuilder.CurrentGame().LivingCells();
        }

        public int CurrentEvolution()
        {
            return this.gameBuilder.CurrentGame().CurrentEvolution();
        }

        public IEnumerable<IEvolutionGame> History()
        {
            return this.gameBuilder.CurrentGame().History();
        }
    }

    public interface IEvolutionBuilder
    {
        EvolutionBuilder Next(IEvolutionGame currentGame);
        
        IEvolutionGame CurrentGame();

        IEnumerable<IEnumerable<Coordonnate>> CoordonnatesHistory();
        
    }

    public class EvolutionBuilder : IEvolutionBuilder
    {
        public EvolutionBuilder(IEvolutionGame initialGame)
        {
            this.currentGame = new EvolutionWithHistory(initialGame);
        }

        private IEvolutionGame currentGame;

        public EvolutionBuilder Next(IEvolutionGame currentGame)
        {
            return new EvolutionBuilder(new EvolutionWithHistory(currentGame.EvolutionnateGame()));
        }

        public IEvolutionGame CurrentGame()
        {
            return this.currentGame;
        }

        public IEnumerable<IEnumerable<Coordonnate>> CoordonnatesHistory()
        {
            return this.currentGame.History().Select(game => game.LivingCells());
        }
    }

    public class NGenerationBuilder : IEvolutionBuilder
    {
        public NGenerationBuilder(IEvolutionGame initialGame, int numberOfGeneration)
        {
            this.evolutionBuilder = new EvolutionBuilder(new EvolutionWithHistory(initialGame));
            this.numberOfGeneration = numberOfGeneration;
        }

        private EvolutionBuilder evolutionBuilder;
        private int numberOfGeneration;

        public EvolutionBuilder Next(IEvolutionGame currentGame)
        {
            EvolutionBuilder builder = this.evolutionBuilder;
            for (int generation = 0; generation <= numberOfGeneration; generation++)
            {
                builder = builder.Next(builder.CurrentGame());
            }

            return builder;
        }

        public IEvolutionGame CurrentGame()
        {
            return this.evolutionBuilder.CurrentGame();
        }

        public IEnumerable<IEnumerable<Coordonnate>> CoordonnatesHistory()
        {
            return this.evolutionBuilder.CoordonnatesHistory();
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
            this.evolvedLivingCoords = new ConvertingCoordonnates(new LivingCells(new EvolvedCells(livingCells)));
            this.history = history;
        }

        private int round;
        private IEnumerable<Coordonnate> livingCells;
        private BoardCoordonnates evolvedLivingCoords;
        private IEnumerable<IEvolutionGame> history;


        public IEvolutionGame EvolutionnateGame()
        {
            return new DefaultEvolution(++round, evolvedLivingCoords.Coordonnates());
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

    public class EvolutionWithHistory : IEvolutionGame
    {
        public EvolutionWithHistory(IEvolutionGame currentGame)
        {
            this.currentGame = currentGame;
        }

        private IEvolutionGame currentGame;

        public IEvolutionGame EvolutionnateGame()
        {
            return 
                new DefaultEvolution(
                    this.currentGame.CurrentEvolution() + 1, 
                    this.currentGame.EvolutionnateGame().LivingCells(),
                    new List<IEvolutionGame>(this.currentGame.History()) { this.currentGame}
                );
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



    

   

    

    

   

    



    
}

