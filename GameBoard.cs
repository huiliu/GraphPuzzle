using System.Collections.Generic;

namespace GraphGame
{
    public class GameBoard
    {
        private int BoardWidth;
        private int BoardHeight;
        public GameBoard(int w, int h)
        {
            this.BoardWidth = w;
            this.BoardHeight = h;
            this.Scores = new Dictionary<string, int>();
        }

        private List<Color> colors = new List<Color>();
        private Dictionary<string, Player> players = new Dictionary<string, Player>();
        public void AddPlayer(string uid)
        {
            this.players.Add(uid, new Player(uid, this.BoardWidth, this.BoardHeight));
        }

        public void RemovePlayer(string uid)
        {
            this.players.Remove(uid);
        }

        public void AddColor(Color color)
        {
            this.colors.Add(color);
            foreach(var kvp in this.players)
                kvp.Value.AddColor(color);
        }

        public void RemoveColor(Color color)
        {
            this.colors.Remove(color);
            foreach(var kvp in this.players)
                kvp.Value.RemoveColor(color);
        }

        public void AddEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            this.players[uid].AddEdge(r0, c0, r1, c1, color);
        }
        public void RemoveEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            this.players[uid].RemoveEdge(r0, c0, r1, c1, color);
        }

        // uid+color -> score
        public Dictionary<string, int> Scores{get;private set;}
        public void CalcScore(int r, int c)
        {
            foreach (var kvp in this.players)
            {
                kvp.Value.CalcScore(r, c);
                foreach (var kkvp in kvp.Value.Scores)
                    this.Scores[kvp.Key + kkvp.Key.ToString()] = kkvp.Value;
            }
        }

        public override string ToString()
        {
            var s = "";
            foreach (var kvp in this.Scores)
            {
                s += string.Format("{0} -> {1}\n", kvp.Key, kvp.Value);
            }

            return s;
        }
    }
}