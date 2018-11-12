using System.Collections.Generic;

namespace GraphGame
{
    public class Player
    {
        public string UID { get; private set; }
        public int TotalScore { get; private set; }

        private Dictionary<Color, Graph> graphs = new Dictionary<Color, Graph>();

        private int BoardWidth;
        private int BoardHeight;
        private int NodeCount;
        public Player(string uid, int w, int h)
        {
            this.UID = uid;
            this.BoardWidth = w;
            this.BoardHeight = h;
            this.NodeCount = w * h;
            this.Scores = new Dictionary<Color, int>();
        }

        public void AddColor(Color color)
        {
            this.graphs.Add(color, new Graph(this.NodeCount));
            this.Scores.Add(color, 0);
        }

        public void RemoveColor(Color color)
        {
            this.graphs.Remove(color);
            this.Scores.Remove(color);
        }

        public void AddEdge(int r0, int c0, int r1, int c1, Color color)
        {
            var g = this.graphs[color];
            var src = this.GetNodeIndex(r0, c0);
            var dst = this.GetNodeIndex(r1, c1);

            g.AddEdge(src, dst);
        }

        public void RemoveEdge(int r0, int c0, int r1, int c1, Color color)
        {
            var g = this.graphs[color];
            var src = this.GetNodeIndex(r0, c0);
            var dst = this.GetNodeIndex(r1, c1);

            g.RemoveEdge(src, dst);
        }

        // color -> score
        public Dictionary<Color, int> Scores { get; private set; }
        public int CalcScore(int r, int c)
        {
            var score = 0;
            foreach (var kvp in this.graphs)
            {
                var s = this.CalcGraphScore(kvp.Value, r, c);
                score += s;
                this.Scores[kvp.Key] += s;
            }

            return score;
        }

        private int CalcGraphScore(Graph g, int r, int c)
        {
            var root = this.GetNodeIndex(r, c);
            var resolvers = g.Traverse(root);

            var score = 0;
            foreach (var s in resolvers)
            {
                foreach (var id in s)
                {
                    var rr = 0;
                    var cc = 0;
                    this.GetRowCol(id, out rr, out cc);
                    if (rr % 2 == 0 && cc % 2 == 0)
                    {
                        var node = g.GetNode(id);
                        score += this.CalcScoreStrategy(node.AllSuccessor.Count);
                    }
                }

                if (s[0] == s[s.Count - 1])
                    score += this.LoopBufferScore;
            }

            return score;
        }

        private int LoopBufferScore { get { return 2; } }
        /// 节点得分计算策略
        private int CalcScoreStrategy(int count)
        {
            switch (count)
            {
                case 0:
                    return 0;
                case 1:
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                default:
                    return 3;
            }
        }

        private int GetNodeIndex(int r, int c)
        {
            return r * this.BoardWidth + c;
        }

        private void GetRowCol(int idx, out int r, out int c)
        {
            r = c = -1;

            c = idx % this.BoardWidth;
            r = (idx - c) / this.BoardWidth;
        }
    }
}