using System;

namespace GraphGame.Logic
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameBoard(7, 7);
            game.AddPlayer("xxx");
            game.AddColor(Color.Red);
            game.AddColor(Color.Blue);
            game.AddColor(Color.Green);

            game.AddBlock("xxx", 1, 1, Color.Blue, Color.None, Color.Blue, Color.Blue);
            game.AddBlock("xxx", 3, 1, Color.Blue, Color.None, Color.Red, Color.Blue);
            game.AddBlock("xxx", 1, 3, Color.Blue, Color.None, Color.Blue, Color.Green);
            game.AddBlock("xxx", 3, 3, Color.None, Color.Green, Color.Red, Color.Red);

            game.CalcScore(3, 3);
            Console.WriteLine(game.ToString());

            Console.Read();
        }
    }
}
