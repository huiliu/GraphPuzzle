using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public partial class GameBoard
    {
        public LevelData Cfg { get; private set; }
        public int RowSquareCount { get; private set; }
        public int ColSquareCount { get; private set; }

        private int BoardNodeWidth;     // 节点数
        private int BoardNodeHeight;    // 节点数
        private Array2LinearHelper Array2LinearHelper;
        private List<int> SquareNodeID;
        private HashSet<Color> Colors;
        private NewGenerator SquareGenerator;
        public void Init(LevelData cfg)
        {
            this.Cfg = cfg;

            this.RowSquareCount = this.Cfg.BoardWidth;
            this.ColSquareCount = this.Cfg.BoardHeight;
            this.Colors = this.Cfg.Colors();
            this.PlayerScores = new Dictionary<string, int>();

            this.BoardNodeWidth = this.ColSquareCount * 2 + 1;
            this.BoardNodeHeight = this.RowSquareCount * 2 + 1;
            this.Array2LinearHelper = new Array2LinearHelper(this.BoardNodeWidth, this.BoardNodeHeight);

            this.SquareGenerator = new NewGenerator(this.RowSquareCount * this.ColSquareCount - this.Cfg.unUsedSquareID.Count);
            this.SquareGenerator.Init(this.Cfg.GetSquareWeight(), this.Cfg.GetSquareColorWeight(), this.Cfg.Seed);

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

        private Dictionary<string, Player> players = new Dictionary<string, Player>();
        private void AddPlayer(string uid)
        {
            var p = new Player(uid);
            p.Init(this.BoardNodeWidth * this.BoardNodeHeight, this.Colors);

            this.players.Add(uid, p);
            this.PlayerScores.Add(uid, 0);
        }

        private void RemovePlayer(string uid)
        {
            this.players.Remove(uid);
            this.PlayerScores.Remove(uid);
        }

        private void AddColor(Color color)
        {
            this.Colors.Add(color);
            foreach (var kvp in this.players)
                kvp.Value.AddColor(color);
        }

        private void RemoveColor(Color color)
        {
            this.Colors.Remove(color);
            foreach (var kvp in this.players)
                kvp.Value.RemoveColor(color);
        }

        private void AddEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            var src = this.Array2LinearHelper.GetLinearIndex(r0, c0);
            var dst = this.Array2LinearHelper.GetLinearIndex(r1, c1);

            this.players[uid].AddEdge(src, dst, color);
        }

        private void RemoveEdge(string uid, int r0, int c0, int r1, int c1, Color color)
        {
            var src = this.Array2LinearHelper.GetLinearIndex(r0, c0);
            var dst = this.Array2LinearHelper.GetLinearIndex(r1, c1);

            this.players[uid].DeleteEdge(src, dst, color);
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
    }
}