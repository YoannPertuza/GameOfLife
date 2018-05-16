using System.Collections.Generic;

namespace GameOfLife
{
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




    

   

    

    

   

    



    
}

