using System.Collections.Generic;

namespace GraphGame.Logic
{
    public class GameBoard
    {
        private int BoardWidth;     // 节点数
        private int BoardHeight;    // 节点数
        private Array2LinearHelper Array2LinearHelper;
        private List<int> SquareNodeID = new List<int>();

        /// w, h 指节点数
        public GameBoard(int w, int h, HashSet<Color> initialColors)
        {
            this.BoardWidth = w;
            this.BoardHeight = h;
            this.Colors = initialColors;
            this.PlayerScores = new Dictionary<string, int>();
            this.Array2LinearHelper = new Array2LinearHelper(w, h);

            /// 感觉这个设计不好，有坏代码的味道
            for(var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; ++j)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                        this.SquareNodeID.Add(i * w + j);
                }
            }
        }

        private readonly HashSet<Color> Colors = new HashSet<Color>();
        private Dictionary<string, Player> players = new Dictionary<string, Player>();
        public void AddPlayer(string uid)
        {
            var p = new Player(uid);
            p.Init(this.BoardWidth, this.BoardHeight, this.Colors);

            this.players.Add(uid, p);
            this.PlayerScores.Add(uid, 0);
        }

        public void RemovePlayer(string uid)
        {
            this.players.Remove(uid);
            this.PlayerScores.Remove(uid);
        }

        public void AddColor(Color color)
        {
            this.Colors.Add(color);
            foreach (var kvp in this.players)
                kvp.Value.AddColor(color);
        }

        public void RemoveColor(Color color)
        {
            this.Colors.Remove(color);
            foreach (var kvp in this.players)
                kvp.Value.RemoveColor(color);
        }

        public void AddEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            var src = this.Array2LinearHelper.GetLinearIndex(r0, c0);
            var dst = this.Array2LinearHelper.GetLinearIndex(r1, c1);

            this.players[uid].AddEdge(src, dst, color);
        }

        public void RemoveEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            var src = this.Array2LinearHelper.GetLinearIndex(r0, c0);
            var dst = this.Array2LinearHelper.GetLinearIndex(r1, c1);

            this.players[uid].DeleteEdge(src, dst, color);
        }

        public void AddBlock(string uid, int r, int c, Color color1, Color color2, Color color3, Color color4)
        {
            if (color1 != Color.None) this.AddEdge(uid, r, c, r - 1, c - 1, color1);
            if (color2 != Color.None) this.AddEdge(uid, r, c, r - 1, c + 1, color2);
            if (color3 != Color.None) this.AddEdge(uid, r, c, r + 1, c + 1, color3);
            if (color4 != Color.None) this.AddEdge(uid, r, c, r + 1, c - 1, color4);
        }

        /// 感觉这个设计不好，有坏代码的味道
        public void GetEmptyNode(string uid, out int r, out int c)
        {
            r = c = -1;
            foreach (var idx in this.SquareNodeID)
            {
                bool isEmpty = true;
                foreach(var kvp in this.players)
                {
                    if (!kvp.Value.IsEmpty(idx))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    this.Array2LinearHelper.GetRowCol(idx, out r, out c);
                    return;
                }
            }
        }

        private List<Color> NodeColor = new List<Color> { Color.None, Color.None, Color.None, Color.None };
        public IList<Color> GetNodeColor(int idx)
        {
            var r0 = -1;
            var c0 = -1;
            this.Array2LinearHelper.GetRowCol(idx, out r0, out c0);

            for (var i = 0; i < (int)Direction.Max; ++i)
            {
                this.NodeColor[i] = Color.None;
            }

            var r1 = -1;
            var c1 = -1;
            foreach (var kvp in this.players)
            {
                var colors = kvp.Value.GetNodeColor(idx);
                foreach(var ckvp in colors)
                {
                    this.Array2LinearHelper.GetRowCol(ckvp.Key, out r1, out c1);
                    var i = (int)Utils.ToDirection(r0, c0, r1, c1);
                    if (this.NodeColor[i] == Color.None && ckvp.Value != Color.None)
                        this.NodeColor[i] = ckvp.Value;
                }
            }

            return this.NodeColor;
        }

        public Queue<GraphPath> GetPlayerPath(string uid)
        {
            return this.players[uid].GetPath();
        }

        public int GetPlayerColorEdgeCount(string uid, Color color, int idx)
        {
            return this.players[uid].GetNodeColorEdgeCount(color, idx);
        }

        // uid -> score
        public Dictionary<string, int> PlayerScores{ get; private set; }
        public void CalcPathAndScore(int r, int c)
        {
            var root = this.Array2LinearHelper.GetLinearIndex(r, c);
            foreach(var kvp in this.players)
            {
                var graphPaths = kvp.Value.FindGraphPath(root);
                this.PlayerScores[kvp.Key] += this.CalcPlayerScore(kvp.Value, graphPaths);
            }
        }

        private int CalcPlayerScore(Player player, Queue<GraphPath> paths)
        {
            var score = 0;
            var rr = -1;
            var cc = -1;
            foreach(var p in paths)
            {
                int nodeScore = 1;
                var path = p.Path;
                if (path.Count > 1 && path[0] == path[path.Count - 1])
                    nodeScore += Utils.LoopBufferScore;

                foreach (var nid in p.Path)
                {
                    this.Array2LinearHelper.GetRowCol(nid, out rr, out cc);
                    if (rr % 2 == 0 && cc % 2 == 0)
                    {
                        var edgeCount = player.GetNodeColorEdgeCount(p.Color, nid);
                        nodeScore *= Utils.CalcScoreStrategy(edgeCount);
                        score += nodeScore;
                    }
                }
            }

            return score;
        }

        public override string ToString()
        {
            var s = "";

            // // 输出图信息
            // foreach (var kvp in this.players)
            //     s += kvp.Value.ToString();

            return s;
        }
    }
}