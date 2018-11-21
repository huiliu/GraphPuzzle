using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public class Game
    {
        // 落子事件
        public event Action OnSquareAck;

        private const int kRowSquare = 10;
        private const int kColSquare = 10;
        private NewGenerator SquareGenerator;
        private GameBoard GameBoard;

        public Square CurrentSquare { get; private set; }
        public Square NextSquare { get; private set; }
        public int kTimeInterval = 30;  // 秒
        public float RemainTime { get; private set; }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public int GraphWidth { get; private set; }

        public LevelData Cfg { get; private set; }
        public Game(LevelData config)
        {
            this.Cfg = config;

            this.RowCount = this.Cfg.BoardHeight;
            this.ColCount = this.Cfg.BoardWidth;
            this.GraphWidth = 2 * this.ColCount + 1;

            //this.SquareGenerator = new SquareGenerator(this.weights, this.RowCount * this.ColCount);
            this.SquareGenerator = new NewGenerator(this.RowCount * this.ColCount - this.Cfg.unUsedSquareID.Count);
            this.SquareGenerator.Init(this.Cfg.GetSquareWeight(), this.Cfg.GetSquareColorWeight(), this.Cfg.Seed);
            this.GameBoard = new GameBoard(2 * this.RowCount + 1, 2 * this.ColCount + 1);
        }
        public string RankUser { get; private set; }
        private bool startFlag = false;
        public void Start(string auid, string buid = "")
        {
            this.startFlag = true;
            this.RemainTime = kTimeInterval;
            this.AddPlayer(auid);
            if (!string.IsNullOrEmpty(buid))
                this.AddPlayer(buid);

            this.RankUser = auid;
            this.GameBoard.AddColor(Color.Red);
            this.GameBoard.AddColor(Color.Green);
            this.GameBoard.AddColor(Color.Blue);

            this.CurrentSquare = this.SquareGenerator.GetSquare();
            this.NextSquare = this.SquareGenerator.GetSquare();
        }

        public void Stop()
        {
            this.startFlag = false;
        }

        public void AddPlayer(string uid)
        {
            this.GameBoard.AddPlayer(uid);
        }

        public void RemovePlayer(string uid)
        {
            this.GameBoard.RemovePlayer(uid);
        }

        public int GetPlayerScore(string uid)
        {
            return this.GameBoard.PlayerScores[uid];
        }

        public Queue<GraphPath> GetPlayerPath(string uid)
        {
            return this.GameBoard.GetPlayerPath(uid);
        }

        public int GetPlayerColorEdgeCount(string uid, Color color, int idx)
        {
            return this.GameBoard.GetPlayerColorEdgeCount(uid, color, idx);
        }

        /// 落子
        public void Ack(string uid, int r, int c)
        {
            var tlColor = this.CurrentSquare.Nodes[0];
            var trColor = this.CurrentSquare.Nodes[1];
            var drColor = this.CurrentSquare.Nodes[2];
            var dlColor = this.CurrentSquare.Nodes[3];

            this.GameBoard.AddBlock(uid, r, c, tlColor, trColor, drColor, dlColor);
            this.GameBoard.CalcScore(r, c);
            this.Next();

            this.FireAckEvent();
            this.TryFireGameOverEvent();
        }

        public void Update(float dt)
        {
            //Console.WriteLine("Game.Update: {0}/{1}", this.RemainTime, dt);
            if (!this.startFlag || this.isGameOver)
                return;

            this.RemainTime -= dt;
            if (this.RemainTime <= 0f)
            {
                this.RandomAck();
            }
        }

        public IList<Color> GetSquareColor(int idx)
        {
            return this.GameBoard.GetNodeColor(idx);
        }

        /// 玩家没有落子，随机落子
        private void RandomAck()
        {
            var r = 0;
            var c = 0;

            this.GameBoard.GetEmptyNode(this.RankUser, out r, out c);
            this.Ack(this.RankUser, r, c);
        }

        private bool isGameOver = false;
        private void Next()
        {
            this.isGameOver = true;
            if (this.NextSquare == null)
                return;

            this.RemainTime = kTimeInterval;
            this.isGameOver = false;

            this.CurrentSquare = this.NextSquare;
            this.NextSquare = this.SquareGenerator.IsEmpty ? null : this.SquareGenerator.GetSquare();

            //UnityEngine.Debug.Log(string.Format("Square Current: [{0}] Next: [{1}]", this.CurrentSquare.ToString(), this.NextSquare.ToString()));
        }

        public void IndxConvertToRowCol(int idx, out int r, out int c)
        {
            c = idx % this.GraphWidth;
            r = (idx - c) / this.GraphWidth;
        }

        public event Action OnGameOver;
        private void TryFireGameOverEvent()
        {
            if (this.IsGameOver)
            {
                this.startFlag = false;
                this.isGameOver = false;

                OnGameOver.SafeInvoke();
            }
        }

        public bool IsGameOver
        {
            get { return this.startFlag && this.isGameOver; }
        }

        public override string ToString()
        {
            return this.GameBoard.ToString();
        }

        private void FireAckEvent()
        {
            this.OnSquareAck.SafeInvoke();
        }
    }
}