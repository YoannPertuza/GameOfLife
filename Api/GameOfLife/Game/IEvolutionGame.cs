using System.Collections.Generic;

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




    

   

    

    

   

    



    
}

