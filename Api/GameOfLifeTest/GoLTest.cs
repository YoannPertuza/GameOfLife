using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfLife;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameOfLifeTest
{
    [TestClass]
    public class GoLTest
    {

        [TestMethod]
        public void DefaultEvolution_OneEvolution_With_Flasheur()
        {
            var board =
                new DefaultEvolution(
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 1),
                        new Coordonnate(2, 1),
                    })
                .EvolutionnateGame()
                .LivingCells();


            Assert.IsTrue(board.Count() == 3);
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 2));
        }


        [TestMethod]
        public void DefaultEvolution_Three_Consecutive_Evolutions_With_Square()
        {
            var board =
                new DefaultEvolution(
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 0),
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 0),
                        new Coordonnate(1, 1),
                    })
                .EvolutionnateGame();


            Assert.IsTrue(board.LivingCells().Count() == 4);
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 0));
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 1));
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
            Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));

             board =   board.EvolutionnateGame();

             Assert.IsTrue(board.LivingCells().Count() == 4);
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 1));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));

             board = board.EvolutionnateGame();

             Assert.IsTrue(board.LivingCells().Count() == 4);
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 0 && coord.CoordY() == 1));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
             Assert.IsTrue(board.LivingCells().Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));           
        }


        [TestMethod]
        public void EvolutionUntil_Four_Evolutions_With_Flasheur()
        {
            var board =
                new EvolutionUntil(
                    4,
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 1),
                        new Coordonnate(2, 1),
                    })
                .EvolutionnateGame()
                .LivingCells();


            Assert.IsTrue(board.Count() == 3);
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 0));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 1));
            Assert.IsTrue(board.Any(coord => coord.CoordX() == 1 && coord.CoordY() == 2));
        }

        [TestMethod]
        public void EvolutionUntil_10_Evolutions_With_Planeur()
        {
            var board =
                new EvolutionUntil(
                    10,
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(1, 0),
                        new Coordonnate(2, 1),
                        new Coordonnate(2, 2),
                        new Coordonnate(1, 2),
                        new Coordonnate(0, 2),
                    })
                .EvolutionnateGame();

            var chose = board.History().Select(game => game.LivingCells());

            Assert.IsTrue(board.History().Count() == 11);
            Assert.IsTrue(board.LivingCells().Count() == 5);
        }

        [TestMethod]
        public void EvolutionUntil_10_Evolutions_With_Ligne()
        {
            
            var board =
                new EvolutionUntil(
                    10,
                    new List<Coordonnate>() 
                    { 
                        new Coordonnate(0, 1),
                        new Coordonnate(1, 1),
                        new Coordonnate(2, 1),
                        new Coordonnate(3, 1),
                        new Coordonnate(4, 1),
                        new Coordonnate(5, 1),
                        new Coordonnate(6, 1),
                        new Coordonnate(7, 1),
                        new Coordonnate(8, 1),
                        new Coordonnate(9, 1),
                    })
                .EvolutionnateGame();

            var chose = board.History().Select(game => game.LivingCells());

            Assert.IsTrue(board.History().Count() == 11);        
        }

        [TestMethod]
        public void ReferenceCoordonnate()
        {
            var coord =
                new ReferenceCoordonnate(
                    new DefaultCoordonnates(new List<Coordonnate>() 
                    { 
                        new Coordonnate(1, 0),
                        new Coordonnate(2, 1),
                        new Coordonnate(2, 2),
                        new Coordonnate(1, 2),
                        new Coordonnate(0, 2),
                    })).Select();


            Assert.AreEqual(coord.CoordX(), 1);
            Assert.AreEqual(coord.CoordY(), 0);
        }

        [TestMethod]
        public void RealCoordonnate()
        {
            var coord = 
                new RealCoordonnate(
                    new Coordonnate(1, 0), 
                    new RelativeCoordonnate(
                        new Coordonnate(1, 0), 
                        new Coordonnate(2, 1)
                    )
                ).Select();

            Assert.IsTrue(coord.GetHashCode() == new Coordonnate(2, 1).GetHashCode());
        }

        [TestMethod]
        [Ignore]
        public void TestMongoDb()
        {
            var client = new MongoDB.Driver.MongoClient("mongodb://yoann:Monaco58898@gameoflife-shard-00-00-iohzq.mongodb.net:27017,gameoflife-shard-00-01-iohzq.mongodb.net:27017,gameoflife-shard-00-02-iohzq.mongodb.net:27017/admin?replicaSet=GameOfLife-shard-0&ssl=true");

            var database = client.GetDatabase("gameOfLife");
            var collection = database.GetCollection<Test>("Figures");


            var test2 = new Test() { test = "test2" };

            collection.InsertOne(test2);


        }

        public class Test
        {
            public string test { get; set; }
        }

    }
}
