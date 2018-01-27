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
    }

    public class UntilRoundGame : IGame 
    {
        public UntilRoundGame(int currentRound, int roundToStop, IGame currentGame)
        {
            this.currentRound = currentRound;
            this.roundToStop = roundToStop;
            this.currentGame = currentGame;
        }

        public UntilRoundGame(int roundToStop, IEnumerable<Coordonnate> livingCells) : this(0, roundToStop, new Game(livingCells))
        {
        }

        private int currentRound;
        private int roundToStop;
        private IGame currentGame;

        public IGame NextRound()
        {
            var indexRound = this.currentRound;
            var currentGame = this.currentGame;

            while (indexRound < roundToStop)
            {
                currentGame = currentGame.NextRound();
                indexRound++;
            }

            return currentGame;
        }

        public IEnumerable<Coordonnate> LivingCells()
        {
            return this.currentGame.LivingCells();
        }

        public int CurrentRound()
        {
            return this.currentGame.CurrentRound();
        }
    }

    public class Game : IGame
    {
        public Game(IEnumerable<Coordonnate> livingCells) : this(0, livingCells)
        {
        }

        public Game(int round, IEnumerable<Coordonnate> livingCells)
        {
            this.round = round;
            this.livingCells = livingCells;
            this.evolvedLivingCoords = new ConvertingCoordonnates(new LivingCells(new EvolvedCells(livingCells)));
        }

        private int round;
        private IEnumerable<Coordonnate> livingCells;
        private BoardCoordonnates evolvedLivingCoords;

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
    }

    

   

    

    

   

    



    
}

