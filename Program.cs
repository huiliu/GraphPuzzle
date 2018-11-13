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

            game.AddEdge("xxx", 1, 1, 0, 2, Color.Red);
            game.AddEdge("xxx", 1, 1, 2, 2, Color.Red);
            game.AddEdge("xxx", 1, 1, 0, 2, Color.Red);
            game.AddEdge("xxx", 1, 1, 0, 0, Color.Blue);

            game.AddEdge("xxx", 1, 3, 0, 2, Color.Red);
            game.AddEdge("xxx", 1, 3, 2, 2, Color.Red);
            game.AddEdge("xxx", 1, 3, 2, 4, Color.Red);

            game.AddEdge("xxx", 3, 1, 2, 0, Color.Red);
            game.AddEdge("xxx", 3, 1, 2, 2, Color.Red);
            game.AddEdge("xxx", 3, 1, 4, 2, Color.Red);
            game.AddEdge("xxx", 3, 1, 4, 0, Color.Green);

            game.AddEdge("xxx", 3, 3, 4, 2, Color.Red);
            game.AddEdge("xxx", 3, 3, 2, 4, Color.Red);
            game.AddEdge("xxx", 3, 3, 2, 2, Color.Green);
            game.AddEdge("xxx", 3, 3, 4, 4, Color.Blue);

            game.AddEdge("xxx", 3, 5, 2, 4, Color.Green);
            game.AddEdge("xxx", 3, 5, 4, 4, Color.Blue);
            game.AddEdge("xxx", 3, 5, 4, 6, Color.Blue);

            game.AddEdge("xxx", 5, 1, 4, 0, Color.Green);
            game.AddEdge("xxx", 5, 1, 4, 2, Color.Green);
            game.AddEdge("xxx", 5, 1, 6, 2, Color.Blue);

            game.CalcScore(3, 3);
            Console.WriteLine(game.ToString());

            Console.Read();
        }
    }
}
