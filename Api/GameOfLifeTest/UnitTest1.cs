using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfLife;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeTest
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void BoardGameTest3()
        {
            var board =
                new Game(
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 1),
                        new Coordonnate(2, 1),
                    })
                .NextRound()
                .LivingCells();


            Assert.IsTrue(board.Count() == 3);
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 2));
        }


        [TestMethod]
        public void BoardGameTest4()
        {
            var board =
                new Game(
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 0),
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 0),
                        new Coordonnate(1, 1),
                    })
                .NextRound();


            Assert.IsTrue(board.LivingCells().Count() == 4);
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 0));
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 1));
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));

             board =   board.NextRound();

             Assert.IsTrue(board.LivingCells().Count() == 4);
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 1));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));

             board = board.NextRound();

             Assert.IsTrue(board.LivingCells().Count() == 4);
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 1));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));           
        }


        [TestMethod]
        public void BoardGameTest5()
        {
            var board =
                new LastRound(
                    3,
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 1),
                        new Coordonnate(2, 1),
                    })
                .NextRound()
                .LivingCells();


            Assert.IsTrue(board.Count() == 3);
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 2));
        }

        [TestMethod]
        public void BoardGameTest6()
        {
            var board =
                new LastRound(
                    10,
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(1, 0),
                        new Coordonnate(2, 1),
                        new Coordonnate(2, 2),
                        new Coordonnate(1, 2),
                        new Coordonnate(0, 2),
                    })
                .NextRound();

            var chose = board.History().Select(game => game.LivingCells());

            Assert.IsTrue(board.History().Count() == 10);
            Assert.IsTrue(board.LivingCells().Count() == 5);
        }
       

    }
}
