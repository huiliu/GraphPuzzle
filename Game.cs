using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public class Game
    {
        private Dictionary<Color, int> weights = new Dictionary<Color, int>{
            {Color.None, 100},
            {Color.Red, 500},
            {Color.Green, 300},
            {Color.Blue, 300},
        };

        private const int kRowSquare = 4;
        private const int kColSquare = 4;
        private SquareGenerator SquareGenerator;
        private GameBoard GameBoard;

        public Square CurrentSquare { get; private set; }
        public Square NextSquare { get; private set; }
        public int kTimeInterval = 3;
        public float RemainTime { get; private set; }
        public Game()
        {
            this.SquareGenerator = new SquareGenerator(this.weights, kRowSquare * kColSquare);
            this.GameBoard = new GameBoard(2 * kRowSquare + 1, 2 * kColSquare + 1);
        }

        public string RankUser { get; private set; }
        private bool startFlag = false;
        public void Start(string auid, string buid = "")
        {
            this.startFlag = true;
            this.AddPlayer(auid);
            if (!string.IsNullOrEmpty(buid))
                this.AddPlayer(buid);

            this.RankUser = auid;
            this.GameBoard.AddColor(Color.None);
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

        /// 落子
        public void Ack(string uid, int r, int c)
        {
            var tlColor = this.CurrentSquare.Nodes[0];
            var trColor = this.CurrentSquare.Nodes[1];
            var drColor = this.CurrentSquare.Nodes[2];
            var dlColor = this.CurrentSquare.Nodes[3];

            Console.WriteLine("[{0}]落下Square: {1}", this.RankUser, this.CurrentSquare.ToString());

            this.GameBoard.AddEdge(uid, r - 1, c - 1, r, c, tlColor);
            this.GameBoard.AddEdge(uid, r - 1, c + 1, r, c, trColor);
            this.GameBoard.AddEdge(uid, r + 1, c + 1, r, c, drColor);
            this.GameBoard.AddEdge(uid, r + 1, c - 1, r, c, dlColor);

            this.GameBoard.CalcScore(r, c);
            this.Next();
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

            this.CheckGameOver();
        }

        /// 玩家没有落子，随机落子
        private void RandomAck()
        {
            var r = 0;
            var c = 0;

            this.GameBoard.GetEmptyNode(this.RankUser, out r, out c);
            //Console.WriteLine("[{0}]随机落子({1}, {2})", this.RankUser, r, c);
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
        }

        public Action OnGameOver;
        private void CheckGameOver()
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
    }
}