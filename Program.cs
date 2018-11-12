using System;

namespace GraphGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameBoard(5,5);
            game.AddPlayer("xxx");
            game.AddColor(Color.Red);

            // game.AddEdge("xxx", 1,1, 0,0, Color.Red);
            // game.AddEdge("xxx", 1,1, 0,2, Color.Red);
            game.AddEdge("xxx", 1,1, 2,0, Color.Red);
            game.AddEdge("xxx", 1,1, 2,2, Color.Red);

            game.AddEdge("xxx", 1,3, 0,4, Color.Red);
            game.AddEdge("xxx", 1,3, 2,2, Color.Red);
            game.AddEdge("xxx", 1,3, 2,4, Color.Red);

            game.AddEdge("xxx", 2,0, 3,1, Color.Red);
            game.AddEdge("xxx", 2,2, 3,1, Color.Red);
            game.AddEdge("xxx", 2,2, 3,3, Color.Red);
            game.AddEdge("xxx", 2,4, 3,3, Color.Red);

            game.CalcScore(1,1);
            Console.WriteLine(game.ToString());
        }
    }
}
