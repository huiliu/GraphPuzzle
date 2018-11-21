using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    class Program
    {
        static void Main(string[] args)
        {
            var colors = new HashSet<Color> { Color.Red, Color.Green, Color.Blue };
            var game = new GameBoard(11, 11, colors);
            game.AddPlayer("xxx");

#if false
            game.AddBlock("xxx", 1, 1, Color.Blue, Color.Red, Color.Red, Color.Blue);
            game.AddBlock("xxx", 1, 3, Color.Red, Color.Green, Color.Green, Color.Blue);
            game.CalcScore(1, 3);
            game.AddBlock("xxx", 1, 5, Color.Blue, Color.Blue, Color.Red, Color.Blue);
            game.CalcScore(1, 5);
            game.AddBlock("xxx", 1, 7, Color.Red, Color.Green, Color.Blue, Color.Red);
            game.CalcScore(1, 7);
            game.AddBlock("xxx", 1, 9, Color.Blue, Color.Red, Color.Red, Color.Blue);
            game.CalcScore(1, 9);

            game.AddBlock("xxx", 3, 1, Color.Red, Color.Green, Color.Red, Color.Green);
            game.CalcScore(3, 1);
            game.AddBlock("xxx", 3, 3, Color.None, Color.Red, Color.Green, Color.Blue);
            game.CalcScore(3, 3);
            game.AddBlock("xxx", 3, 5, Color.Green, Color.Green, Color.None, Color.Red);
            game.CalcScore(3, 5);
            game.AddBlock("xxx", 3, 7, Color.Red, Color.Red, Color.Red, Color.Red);
            game.CalcScore(3, 7);
            game.AddBlock("xxx", 3, 9, Color.Green, Color.Red, Color.Green, Color.Red);
            game.CalcScore(3, 9);

            game.AddBlock("xxx", 5, 1, Color.Green, Color.Green, Color.Red, Color.Red);
            game.CalcScore(5, 1);
            game.AddBlock("xxx", 5, 3, Color.Blue, Color.Green, Color.Green, Color.Red);
            game.CalcScore(5, 3);
            game.AddBlock("xxx", 5, 5, Color.Red, Color.None, Color.None, Color.Red);
            game.CalcScore(5, 5);
            game.AddBlock("xxx", 5, 7, Color.Green, Color.None, Color.Blue, Color.Blue);
            game.CalcScore(5, 7);
            game.AddBlock("xxx", 5, 9, Color.Blue, Color.Blue, Color.None, Color.Green);
            game.CalcScore(5, 9);

            game.AddBlock("xxx", 7, 1, Color.Blue, Color.Red, Color.Red, Color.Red);
            game.CalcScore(7, 1);
            game.AddBlock("xxx", 7, 3, Color.Red, Color.Red, Color.Blue, Color.Red);
            game.CalcScore(7, 3);
            game.AddBlock("xxx", 7, 5, Color.Green, Color.Green, Color.Red, Color.Green);
            game.CalcScore(7, 5);
            game.AddBlock("xxx", 7, 7, Color.Red, Color.Blue, Color.Red, Color.None);
            game.CalcScore(7, 7);
            game.AddBlock("xxx", 7, 9, Color.None, Color.Green, Color.Red, Color.Red);
            game.CalcScore(7, 9); 
#endif

            Console.WriteLine(game.ToString());

            WeightRandomTest.Instance.Run();
            Console.Read();
        }
    }
}
