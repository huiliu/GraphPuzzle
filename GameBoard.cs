using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public partial class GameBoard
    {
        public LevelData Cfg { get; private set; }
        public int RowSquareCount { get; private set; }
        public int ColSquareCount { get; private set; }
        public Recorder Recorder { get; set; }

        private int BoardNodeWidth;     // 节点数
        private int BoardNodeHeight;    // 节点数
        private ArrayToLinearHelper Array2LinearHelper;
        private List<int> SquareNodeID;
        private HashSet<Color> Colors;
        private NewGenerator SquareGenerator;
        public void Init(LevelData cfg, int seed)
        {
            this.Cfg = cfg;

            this.RowSquareCount = this.Cfg.BoardWidth;
            this.ColSquareCount = this.Cfg.BoardHeight;
            this.Colors = this.Cfg.Colors();
            this.PlayerScores = new Dictionary<string, int>();
            this.Players = new List<Player>();

            this.BoardNodeWidth = this.ColSquareCount * 2 + 1;
            this.BoardNodeHeight = this.RowSquareCount * 2 + 1;
            this.Array2LinearHelper = new ArrayToLinearHelper(this.BoardNodeWidth, this.BoardNodeHeight);

            this.SquareGenerator = new NewGenerator(this.RowSquareCount * this.ColSquareCount - this.Cfg.unUsedSquareID.Count);
            this.SquareGenerator.Init(this.Cfg.GetSquareWeight(), this.Cfg.GetSquareColorWeight(), seed);

            /// 感觉这个设计不好，有坏代码的味道
            this.SquareNodeID = new List<int>();
            for(var i = 0; i < this.BoardNodeHeight; ++i)
            {
                for (var j = 0; j < this.BoardNodeWidth; ++j)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                        this.SquareNodeID.Add(i * this.BoardNodeWidth + j);
                }
            }
        }

        private List<Player> Players;
        private void AddPlayer(string uid)
        {
            var p = new Player(uid);
            p.Init(this.BoardNodeWidth * this.BoardNodeHeight, this.Colors);

            this.Players.Add(p);
            this.PlayerScores.Add(uid, 0);
        }

        private void RemovePlayer(string uid)
        {
            var p = this.GetPlayer(uid);
            if (p != null)
                this.Players.Remove(p);

            this.PlayerScores.Remove(uid);
        }

        private Player GetPlayer(string uid)
        {
            return this.Players.Find(p => p.UID == uid);
        }

        private void AddColor(Color color)
        {
            this.Colors.Add(color);
            foreach (var p in this.Players)
                p.AddColor(color);
        }

        private void RemoveColor(Color color)
        {
            this.Colors.Remove(color);
            foreach (var p in this.Players)
                p.RemoveColor(color);
        }

        private void AddEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            var p = this.GetPlayer(uid);
            if (p == null)
                return;

            var src = this.Array2LinearHelper.GetLinearIndex(r0, c0);
            var dst = this.Array2LinearHelper.GetLinearIndex(r1, c1);

            p.AddEdge(src, dst, color);
        }

        private void DeleteEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            var p = this.GetPlayer(uid);
            if (p == null)
                return;

            var src = this.Array2LinearHelper.GetLinearIndex(r0, c0);
            var dst = this.Array2LinearHelper.GetLinearIndex(r1, c1);

            p.DeleteEdge(src, dst, color);
        }

        private void AddBlock(string uid, int r, int c, Color color1, Color color2, Color color3, Color color4)
        {
            if (color1 != Color.None) this.AddEdge(uid, r, c, r - 1, c - 1, color1);
            if (color2 != Color.None) this.AddEdge(uid, r, c, r - 1, c + 1, color2);
            if (color3 != Color.None) this.AddEdge(uid, r, c, r + 1, c + 1, color3);
            if (color4 != Color.None) this.AddEdge(uid, r, c, r + 1, c - 1, color4);
        }

        public override string ToString()
        {
            var s = "";

            // // 输出图信息
            // foreach (var kvp in this.players)
            //     s += kvp.Value.ToString();

            return s;
        }

        /// <summary>
        /// 二维下标到线性下标转换
        /// </summary>
        internal class ArrayToLinearHelper
        {
            private readonly int w;
            private readonly int h;
            public ArrayToLinearHelper(int w, int h)
            {
                this.w = w;
                this.h = h;
            }

            public int GetLinearIndex(int r, int c)
            {
                return r * this.w + c;
            }

            public void GetRowCol(int idx, out int r, out int c)
            {
                r = c = -1;

                c = idx % this.w;
                r = (idx - c) / this.w;
            }
        }
    }
}