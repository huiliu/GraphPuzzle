using System.Diagnostics;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public struct GraphPath
    {
        public Color Color { get; private set; }
        public IList<int> Path { get; private set; }

        public GraphPath(Color color, IList<int> p)
        {
            this.Color = color;
            this.Path = p;
        }
    }

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
            this.ColorGraph = new Graph(this.NodeCount);
            this.graphs.Add(color, this.ColorGraph);
            this.Scores.Add(color, 0);
        }

        public void RemoveColor(Color color)
        {
            this.graphs.Remove(color);
            this.Scores.Remove(color);
        }

        public void AddEdge(int r0, int c0, int r1, int c1, Color color)
        {
            if (color == Color.None)
                return;

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

        // 记录任一颜色的graph，用于查找一个空节点
        private Graph ColorGraph;
        public void GetEmptyNode(out int r, out int c)
        {
            r = c = -1;
            for (var i = 0; i < this.NodeCount; ++i)
            {
                var node = this.ColorGraph.GetNode(i);
                if (node.AllSuccessor.Count > 0)
                    continue;

                var r1 = 0;
                var c1 = 0;
                this.GetRowCol(i, out r1, out c1);
                if (r1 % 2 != 1 || c1 % 2 != 1)
                    continue;

                bool foundFlag = true;
                foreach (var kvp in this.graphs)
                {
                    if (kvp.Value.GetNode(i).AllSuccessor.Count != 0)
                    {
                        foundFlag = false;
                        break;
                    }
                }

                if (foundFlag)
                {
                    r = r1;
                    c = c1;
                    return;
                }
            }
        }

        private List<Color> NodeColor = new List<Color> { Color.None, Color.None, Color.None, Color.None };
        public IList<Color> GetNodeColor(int idx)
        {
            for (var i = 0; i < (int)Direction.Max; ++i)
            {
                this.NodeColor[i] = Color.None;
            }

            var r = -1;
            var c = -1;
            this.GetRowCol(idx, out r, out c);

            foreach (var kvp in this.graphs)
            {
                var successors = kvp.Value.GetNode(this.GetNodeIndex(r, c)).AllSuccessor;
                foreach (var s in successors)
                {
                    var r1 = -1;
                    var c1 = -1;
                    this.GetRowCol(s, out r1, out c1);

                    var i = (int)Utils.ToDirection(r, c, r1, c1);
                    Debug.Assert(this.NodeColor[i] == Color.None);
                    this.NodeColor[i] = kvp.Key;
                }
            }

            return this.NodeColor.AsReadOnly();
        }

        /// <summary>
        /// 取得节点idx的color边数
        /// </summary>
        /// <param name="color"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public int GetNodeColorEdgeCount(Color color, int idx)
        {
            var g = this.graphs[color];
            return g.GetNodeEdgeCount(idx);
        }

        private Queue<GraphPath> ColorPath = new Queue<GraphPath>();
        public Queue<GraphPath> GetPath()
        {
            foreach (var kvp in graphs)
            {
                foreach (var s in kvp.Value.Solutions)
                    if (s.Count > 1)    // 由于落点处可能没有颜色kvp.Key的边存在，导致返回一个点的路径
                        this.ColorPath.Enqueue(new GraphPath(kvp.Key, s));
            }

            return this.ColorPath;
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
                int nodeScore = 1;
                foreach (var id in s)
                {
                    var rr = 0;
                    var cc = 0;
                    this.GetRowCol(id, out rr, out cc);
                    if (rr % 2 == 0 && cc % 2 == 0)
                    {
                        var node = g.GetNode(id);
                        nodeScore *= Utils.CalcScoreStrategy(node.AllSuccessor.Count);
                        score += nodeScore;
                    }
                }

                if (s.Count > 1 && s[0] == s[s.Count - 1])
                    score += Utils.LoopBufferScore;
            }

            return score;
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

        public override string ToString()
        {
            var s = string.Format("Player[{0}]'s GameBoard: \n", this.UID);
            foreach (var kvp in this.graphs)
            s += string.Format("Color [{0}]:\n{1}", kvp.Key, kvp.Value);

            return s+'\n';
        }
    }
}