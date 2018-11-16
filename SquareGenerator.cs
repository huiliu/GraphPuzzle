using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public class Square
    {
        // TL, TR, DR, DL
        public List<Color> Nodes { get; private set; }
        public Square()
        {
            this.Nodes = new List<Color>((int)Direction.Max);
            for (var i = 0; i < (int)Direction.Max; ++i)
                this.Nodes.Add(Color.None);
        }

        public void SetColor(Direction direction, Color color)
        {
            var i = (int)direction;
            this.Nodes[i] = color;
        }

        public void RemoveColor(int idx)
        {
            this.Nodes[idx] = Color.None;
        }

        public override string ToString()
        {
            var s = "Square Color: ";
            foreach (var c in this.Nodes)
                s += c.ToString() + ", ";

            return s.TrimEnd(' ', ',');
        }
    }

    public interface ISquareGenerator
    {
        Square GetSquare();
    }

    public class SquareGenerator
        : ISquareGenerator
    {
        private Dictionary<Color, int> ColorWeight = new Dictionary<Color, int>();
        private List<int> Weights = new List<int>();
        private List<Color> Colors = new List<Color>();
        private int TotalValue = 0;
        private int SquareCount = 0;
        private Random random;
        private const int kSquareEdgeCount = (int)Direction.Max;
        private int CurrentIndex = 0;
        private List<List<Color>> EdgeColorSource = new List<List<Color>>(kSquareEdgeCount);   // 因为只有四条边

        /// 各色彩权重字典
        /// None: 100, Red: 500, Green: 200, Blue: 400
        /// 最终权重: 
        /// 
        public SquareGenerator(Dictionary<Color, int> colorWeight, int count)
        {
            this.SquareCount = count;
            this.ColorWeight = colorWeight;
            this.random = new Random();

            this.InitWeight();
        }

        private void InitWeight()
        {
            foreach (var kvp in this.ColorWeight)
            {
                this.TotalValue += kvp.Value;
                this.Colors.Add(kvp.Key);
                this.Weights.Add(this.TotalValue);
            }

            // 每条边对应一个色彩队列
            for (var i = 0; i < kSquareEdgeCount; ++i)
                this.EdgeColorSource.Add(new List<Color>(this.SquareCount));

            // 向每条边色彩队列中随机放入颜色
            for (var i = 0; i < this.SquareCount; ++i)
            {
                foreach (var cs in this.EdgeColorSource)
                    cs.Add(this.GetColor(random.Next(0, this.TotalValue)));
            }
        }

        private Color GetColor(int w)
        {
            for (var i = 0; i < this.Weights.Count; ++i)
            {
                if (w < this.Weights[i])
                    return this.Colors[i];
            }

            return Color.None;
        }

        public bool IsEmpty { get { return this.CurrentIndex >= this.SquareCount; } }

        public Square GetSquare()
        {
            var Square = new Square();
            for (var i = 0; i < kSquareEdgeCount; ++i)
                Square.SetColor((Direction)i, this.EdgeColorSource[i][this.CurrentIndex]);

            ++this.CurrentIndex;
            return Square;
        }
    }

    public class NewGenerator
        : ISquareGenerator
    {
        private WeightRandom<SquareRandom> weightRandom;

        private readonly int SquareCount;
        public NewGenerator(int squareCount)
        {
            this.SquareCount = squareCount;
        }

        private Random random;
        public void Init(Dictionary<SquareType, int> squareWeights, Dictionary<SquareType, Dictionary<Color, int>> weights, int seed)
        {
            this.random = new Random(seed);
            var squareDict = new Dictionary<SquareRandom, int>();
            foreach (var kvp in squareWeights)
            {
                var sr = SquareRandomFactory.Create(kvp.Key, weights[kvp.Key], this.random.Next());
                squareDict.Add(sr, kvp.Value);
            }

            this.weightRandom = new WeightRandom<SquareRandom>(squareDict);
            this.weightRandom.InitRandom(this.random.Next());
        }

        private int Counter = 0;
        public Square GetSquare()
        {
            ++Counter;
            return this.weightRandom.Next().GetSquare();
        }

        public bool IsEmpty { get { return this.Counter >= this.SquareCount; } }
    }
}