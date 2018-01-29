using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    public interface IGame
    {
        IGame NextRound();

        IEnumerable<Coordonnate> LivingCells();
        
        int CurrentRound();

        IEnumerable<IGame> History();
    }

    public class LastRound : IGame 
    {
        public LastRound(int currentRound, int roundToStop, IGame currentGame)
        {
            this.currentRound = currentRound;
            this.roundToStop = roundToStop;
            this.gameBuilder = new HistoricalGameBuilder(currentGame);
        }

        public LastRound(int roundToStop, IEnumerable<Coordonnate> livingCells) : this(0, roundToStop, new Game(livingCells))
        {
        }

        public LastRound(int currentRound, int roundToStop, IEnumerable<Coordonnate> livingCells)
            : this(currentRound, roundToStop, new Game(livingCells))
        {
        }

        private int currentRound;
        private int roundToStop;
        private HistoricalGameBuilder gameBuilder;
  
        public IGame NextRound()
        {
            var indexRound = this.currentRound;
            HistoricalGameBuilder tempBuilder = this.gameBuilder;
     
            while (indexRound < roundToStop)
            {
                tempBuilder = tempBuilder.Next(tempBuilder.CurrentGame());
                indexRound++;
            }

            return 
                new Game(
                    tempBuilder.CurrentGame().CurrentRound(),
                    tempBuilder.CurrentGame().LivingCells(),
                    tempBuilder.CurrentGame().History()
                );
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.gameBuilder.CurrentGame().LivingCells();
        }

        public int CurrentRound()
        {
            return this.gameBuilder.CurrentGame().CurrentRound();
        }

        public IEnumerable<IGame> History()
        {
            return this.gameBuilder.CurrentGame().History();
        }
    }


    public class HistoricalGameBuilder
    {
        public HistoricalGameBuilder(IGame initialGame)
        {
            this.currentGame = new HistoricalGame(initialGame);
        }

        private IGame currentGame;

        public HistoricalGameBuilder Next(IGame currentGame)
        {
            return new HistoricalGameBuilder(new HistoricalGame(currentGame.NextRound()));
        }

        public IGame CurrentGame()
        {
            return this.currentGame;
        }

        public IEnumerable<IEnumerable<Coordonnate>> CoordonnatesHistory()
        {
            return this.currentGame.History().Select(game => game.LivingCells());
        }
    }

    public class Game : IGame
    {
        public Game(IEnumerable<Coordonnate> livingCells) : this(0, livingCells)
        {
        }

        public Game(int round, IEnumerable<Coordonnate> livingCells)
            : this(round, livingCells, new List<IGame>())
        {
        }

        public Game(int round, IEnumerable<Coordonnate> livingCells, IEnumerable<IGame> history)
        {
            this.round = round;
            this.livingCells = livingCells;
            this.evolvedLivingCoords = new ConvertingCoordonnates(new LivingCells(new EvolvedCells(livingCells)));
            this.history = history;
        }

        private int round;
        private IEnumerable<Coordonnate> livingCells;
        private BoardCoordonnates evolvedLivingCoords;
        private IEnumerable<IGame> history;


        public IGame NextRound()
        {
            return new Game(++round, evolvedLivingCoords.Coordonnates());
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.livingCells;
        }

        public int CurrentRound()
        {
            return this.round;
        }

        public IEnumerable<IGame> History()
        {
            return this.history;
        }
    }

    public class HistoricalGame : IGame
    {
        public HistoricalGame(IGame currentGame)
        {
            this.currentGame = currentGame;
        }

        private IGame currentGame;

        public IGame NextRound()
        {
            return 
                new Game(
                    this.currentGame.CurrentRound() + 1, 
                    this.currentGame.NextRound().LivingCells(),
                    new List<IGame>(this.currentGame.History()) { this.currentGame}
                );
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.currentGame.LivingCells();
        }

        public int CurrentRound()
        {
            return this.currentGame.CurrentRound();
        }

        public IEnumerable<IGame> History()
        {
            return this.currentGame.History();
        }
    }



    

   

    

    

   

    



    
}

