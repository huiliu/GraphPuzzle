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
        private readonly Dictionary<Color, Graph> Graphs;

        private int BoardWidth;
        private int BoardHeight;
        private int NodeCount;
        public Player(string uid)
        {
            this.UID = uid;
            this.Graphs = new Dictionary<Color, Graph>();
        }

        public void Init(int w, int h, HashSet<Color> colors)
        {
            this.BoardWidth = w;
            this.BoardHeight = h;
            this.NodeCount = w * h;

            foreach (var c in colors)
                this.AddColor(c);
        }

        public void AddColor(Color color)
        {
            if (!this.Graphs.ContainsKey(color))
            {
                var ColorGraph = new Graph(this.NodeCount);
                this.Graphs.Add(color, ColorGraph);
            }
        }

        public void RemoveColor(Color color)
        {
            this.Graphs.Remove(color);
        }

        public void AddEdge(int src, int dst, Color color)
        {
            var g = null as Graph;
            this.Graphs.TryGetValue(color, out g);
            if (g != null)
                g.AddEdge(src, dst);
        }

        public void DeleteEdge(int src, int dst, Color color)
        {
            var g = null as Graph;
            this.Graphs.TryGetValue(color, out g);
            if (g != null)
                g.DeleteEdge(src, dst);
        }

        public bool IsEmpty(int idx)
        {
            foreach (var kvp in this.Graphs)
            {
                if (kvp.Value.GetNode(idx).AllSuccessor.Count > 0)
                    return false;
            }

            return true;
        }

        private List<Color> NodeColor = new List<Color> { Color.None, Color.None, Color.None, Color.None };
        private readonly Dictionary<int, Color> EdgeColor = new Dictionary<int, Color>(4);
        public IDictionary<int, Color> GetNodeColor(int idx)
        {
            this.EdgeColor.Clear();
            foreach (var kvp in this.Graphs)
            {
                var successors = kvp.Value.GetNode(idx).AllSuccessor;
                foreach (var s in successors)
                {
                    Debug.Assert(!this.EdgeColor.ContainsKey(s));
                    this.EdgeColor.Add(s, kvp.Key);
                }
            }

            return this.EdgeColor;
        }

        /// <summary>
        /// 取得节点idx的color边数
        /// </summary>
        /// <param name="color"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public int GetNodeColorEdgeCount(Color color, int idx)
        {
            var g = this.Graphs[color];
            return g.GetNodeEdgeCount(idx);
        }

        private Queue<GraphPath> ColorPath = new Queue<GraphPath>();
        public Queue<GraphPath> GetPath()
        {
            //foreach (var kvp in Graphs)
            //{
            //    foreach (var s in kvp.Value.Solutions)
            //        if (s.Count > 1)    // 由于落点处可能没有颜色kvp.Key的边存在，导致返回一个点的路径
            //            this.ColorPath.Enqueue(new GraphPath(kvp.Key, s));
            //}

            return this.ColorPath;
        }

        public Queue<GraphPath> FindGraphPath(int idx)
        {
            this.ColorPath.Clear();
            foreach (var kvp in this.Graphs)
            {
                var solutions = kvp.Value.Traverse(idx);
                foreach(var s in solutions)
                {
                    if (s.Count > 1)    // 由于落点处可能没有颜色kvp.Key的边存在，导致返回一个点的路径
                        this.ColorPath.Enqueue(new GraphPath(kvp.Key, s));
                }
            }

            return this.ColorPath;
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
            foreach (var kvp in this.Graphs)
            s += string.Format("Color [{0}]:\n{1}", kvp.Key, kvp.Value);

            return s+'\n';
        }
    }
}