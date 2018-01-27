using GameOfLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252);
          
            Console.WriteLine("--- Game of Life ---");
            Console.WriteLine("Map size");
            Console.WriteLine("Height");
            var height = Console.ReadLine();
            Console.WriteLine("Width");
            var width = Console.ReadLine();

            var gameOfLife = new Game(int.Parse(width), int.Parse(height));

            PrintBoard(gameOfLife, int.Parse(height), int.Parse(width));

            do
            {
                gameOfLife = DisplayMenu(gameOfLife);
                PrintBoard(gameOfLife, int.Parse(height), int.Parse(width));
            }while(true);
        }

        static Game DisplayMenu(Game gameOfLife)
        {   
            Console.WriteLine("1. Reset Game");
            Console.WriteLine("2. Alive a cell");
            Console.WriteLine("3. Next Round");
            var result = Console.ReadLine();

            if (result == "1")
            {
                return new Game(3, 3);
            }

            if (result == "2")
            {
                Console.WriteLine("Enter x coordannate");
                var x = Console.ReadLine();
                Console.WriteLine("Enter y coordannate");
                var y = Console.ReadLine();

                return gameOfLife.LivelyCell(int.Parse(x), int.Parse(y));
            }

            if (result == "3")
            {              
                return gameOfLife.NextRound();
            }

            return gameOfLife;
            
        }

        static void PrintBoard(Game gameOfLife, int height, int width)
        {

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                if (gameOfLife.CelluleAt(x, y).Alive())
                {
                    Console.Write((char)219);
                }
                else
                {
                    Console.Write((char)176);
                }

            }
            Console.WriteLine();
        }
        }
    }

}
