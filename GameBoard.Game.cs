using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphGame.Logic
{
    public partial class GameBoard
    {
        public Square CurrentSquare { get; private set; }
        public Square NextSquare { get; private set; }
        public const int kTimeInterval = 30;  // 秒
        public float RemainTime { get; private set; }
        private bool startFlag = false;
        public void Start(string A, string B="")
        {
            this.startFlag = true;
            this.RemainTime = kTimeInterval;
            this.CurrentSquare = this.SquareGenerator.GetSquare();
            this.NextSquare = this.SquareGenerator.GetSquare();

            this.AddPlayer(A);
            if (!string.IsNullOrEmpty(B))
                this.AddPlayer(B);
        }

        public void Stop()
        {
            this.startFlag = false;
        }

        private int CurrentUserIndex;
        public string CurrentUser
        {
            get
            {
                var count = this.Players.Count;
                if (count <= 0)
                    return null;

                if (this.CurrentUserIndex >= count || this.CurrentUserIndex <= 0)
                    this.CurrentUserIndex = 0;

                return this.Players[this.CurrentUserIndex].UID;
            }
        }

        #region 查询
        /// <summary>
        /// 取得一个玩家[uid]可用的空节点
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public void GetEmptyNode(string uid, out int r, out int c)
        {
            r = c = -1;
            foreach (var idx in this.SquareNodeID)
            {
                bool isEmpty = true;
                foreach(var p in this.Players)
                {
                    if (!p.IsEmpty(idx))
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
        private IList<Color> GetNodeColor(int idx)
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
            foreach (var p in this.Players)
            {
                var colors = p.GetNodeColor(idx);
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

        public IList<Color> GetSquareColor(int r, int c)
        {
            var idx = this.Array2LinearHelper.GetLinearIndex(r, c);
            return this.GetNodeColor(idx);
        }

        private Queue<Path> PlayerPath;
        public Queue<Path> GetPlayerPath(string uid)
        {
            this.PlayerPath.Clear();
            var p = this.GetPlayer(uid);
            if (p == null)
                return this.PlayerPath;

            var graphPath = p.GetPath();
            foreach(var gp in graphPath)
            {
                if (gp.Path.Count <= 1)
                    continue;

                Path path;
                path.Color = gp.Color;
                path.Nodes = new List<Point>();
                foreach(var nid in gp.Path)
                {
                    Point point;
                    this.Array2LinearHelper.GetRowCol(nid, out point.Row, out point.Col);
                    path.Nodes.Add(point);
                }
                this.PlayerPath.Enqueue(path);
            }

            return this.PlayerPath;
        }

        public int GetPlayerEdgeCount(string uid, Color color, int r, int c)
        {
            var p = this.GetPlayer(uid);
            if (p == null)
                return 0;

            var idx = this.Array2LinearHelper.GetLinearIndex(r, c);
            return p.GetNodeColorEdgeCount(color, idx);
        }

        public int GetPlayerScore(string uid)
        {
            if (!string.IsNullOrEmpty(uid) && this.PlayerScores.ContainsKey(uid))
                return this.PlayerScores[uid];

            return 0;
        }
        #endregion

        #region 计算得分
        // uid -> score
        public Dictionary<string, int> PlayerScores{ get; private set; }
        private void CalcPathAndScore(int r, int c)
        {
            var root = this.Array2LinearHelper.GetLinearIndex(r, c);
            foreach(var p in this.Players)
            {
                var graphPaths = p.FindGraphPath(root);
                this.PlayerScores[p.UID] += this.CalcPlayerScore(p, graphPaths);
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
        #endregion

        #region 落子

        private bool CanAck(string uid, int r, int c)
        {
            if (string.IsNullOrEmpty(uid))
                return false;

            if (this.CurrentUser != uid)
                return false;

            if (this.GetPlayer(uid) == null)
                return false;

            return true;
        }

        private void DoAck(string uid, int r, int c)
        {
            var tlColor = this.CurrentSquare.Nodes[0];
            var trColor = this.CurrentSquare.Nodes[1];
            var drColor = this.CurrentSquare.Nodes[2];
            var dlColor = this.CurrentSquare.Nodes[3];

            this.AddBlock(uid, r, c, tlColor, trColor, drColor, dlColor);
            this.Record(uid, r, c);
            this.CalcPathAndScore(r, c);
            this.Next();

            this.FireAckEvent();
            this.TryFireGameOverEvent();
        }

        /// 落子
        public void Ack(string uid, int r, int c)
        {
            if (this.CanAck(uid, r, c))
                this.DoAck(uid, r, c);
        }

        // 悔棋
        public void Rollback()
        {

        }

        public void Update(float dt)
        {
            if (!this.startFlag || this.isGameOver)
                return;

            this.RemainTime -= dt;
            if (this.RemainTime <= 0f)
            {
                this.RandomAck();
            }
        }

        /// 玩家没有落子，随机落子
        private void RandomAck()
        {
            var r = 0;
            var c = 0;

            this.GetEmptyNode(this.CurrentUser, out r, out c);
            this.Ack(this.CurrentUser, r, c);
        }

        private void Next()
        {
            this.isGameOver = true;
            if (this.NextSquare == null)
                return;

            this.RemainTime = kTimeInterval;
            this.isGameOver = false;

            ++this.CurrentUserIndex;
            this.CurrentSquare = this.NextSquare;
            this.NextSquare = this.SquareGenerator.IsEmpty ? null : this.SquareGenerator.GetSquare();
        }
        #endregion

        #region Event
        private bool isGameOver = false;
        private bool IsGameOver
        {
            get { return this.startFlag && this.isGameOver; }
        }

        public event Action OnGameOver;
        private void TryFireGameOverEvent()
        {
            if (this.IsGameOver)
            {
                this.startFlag = false;
                this.isGameOver = false;

                this.OnGameOver.SafeInvoke();
                this.TrySaveRecord();
            }
        }

        private void TrySaveRecord()
        {
            if (this.Recorder == null)
                return;

            this.Recorder.Data.PlayerA = this.Players[0].UID;
            if (this.Players.Count == 2)
                this.Recorder.Data.PlayerB = this.Players[1].UID;

            this.Recorder.Save();
        }

        // 落子事件
        public event Action OnSquareAck;
        private void FireAckEvent()
        {
            this.OnSquareAck.SafeInvoke();
        }
        #endregion

        private void Record(string uid, int r, int c, StepType type = StepType.Normal)
        {
            if (this.Recorder == null)
                return;

            MoveStep step;
            step.UID = uid;
            step.Type = type;
            step.Row = r;
            step.Col = c;

            this.Recorder.EqueueStep(step);
        }
    }
}
