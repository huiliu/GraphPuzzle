using System;
using System.Collections.Generic;

namespace GraphGame.Logic
{
    public enum SquareType
    {
        A,
        B,     // 两条线相邻
        C,     // 两条线相对
        D,
        E,
    }

    /// <summary>
    /// 产生一个随机的Square
    /// </summary>
    public abstract class SquareRandom
    {
        public SquareType Type { get; private set; }
        public WeightRandom<Color> ColorRandom;

        protected Random Random;
        public SquareRandom(SquareType type, Dictionary<Color, int> colorWeights, int seed = 0)
        {
            this.Type = type;
            this.Random = new Random(seed);
            this.ColorRandom = new WeightRandom<Color>(colorWeights);
            this.ColorRandom.InitRandom(Random.Next());
        }

        public abstract Square GetSquare();
    }

    public class SquareARandom
        : SquareRandom
    {
        public SquareARandom(SquareType type, Dictionary<Color, int> colorWeights, int seed = 0)
            : base(type, colorWeights, seed)
        {
        }

        public override Square GetSquare()
        {
            var c = this.ColorRandom.Next();
            var r = this.Random.Next(0, 4);
            var s = new Square();
            switch (r)
            {
                case 0:
                    s.SetColor(Direction.TopLeft, c);
                    break;
                case 1:
                    s.SetColor(Direction.TopRight, c);
                    break;
                case 2:
                    s.SetColor(Direction.DownRight, c);
                    break;
                case 3:
                    s.SetColor(Direction.DownLeft, c);
                    break;
                default :
                    break;
            }

            return s;
        }
    }

    public class SquareBRandom
        : SquareRandom
    {
        public SquareBRandom(SquareType type, Dictionary<Color, int> colorWeights, int seed = 0)
            : base(type, colorWeights, seed)
        {
        }

        public override Square GetSquare()
        {
            var c0 = this.ColorRandom.Next();
            var c1 = this.ColorRandom.Next();
            var r = this.Random.Next(0, 4);
            var s = new Square();
            switch (r)
            {
                case 0:
                    s.SetColor(Direction.TopLeft, c0);
                    s.SetColor(Direction.TopRight, c1);
                    break;
                case 1:
                    s.SetColor(Direction.TopRight, c0);
                    s.SetColor(Direction.DownRight, c1);
                    break;
                case 2:
                    s.SetColor(Direction.DownRight, c0);
                    s.SetColor(Direction.DownLeft, c1);
                    break;
                case 3:
                    s.SetColor(Direction.DownLeft, c0);
                    s.SetColor(Direction.TopLeft, c1);
                    break;
                default :
                    break;
            }

            return s;
        }
    }

    public class SquareCRandom
        : SquareRandom
    {
        public SquareCRandom(SquareType type, Dictionary<Color, int> colorWeights, int seed = 0)
            : base(type, colorWeights, seed)
        {
        }

        public override Square GetSquare()
        {
            var c0 = this.ColorRandom.Next();
            var c1 = this.ColorRandom.Next();
            var r = this.Random.Next(0, 2);
            var s = new Square();
            switch (r)
            {
                case 0:
                    s.SetColor(Direction.TopLeft, c0);
                    s.SetColor(Direction.DownRight, c1);
                    break;
                case 1:
                    s.SetColor(Direction.TopRight, c0);
                    s.SetColor(Direction.DownLeft, c1);
                    break;
                default :
                    break;
            }

            return s;
        }
    }

    public class SquareDRandom
        : SquareRandom
    {
        public SquareDRandom(SquareType type, Dictionary<Color, int> colorWeights, int seed = 0)
            : base(type, colorWeights, seed)
        {
        }

        public override Square GetSquare()
        {
            var c0 = this.ColorRandom.Next();
            var c1 = this.ColorRandom.Next();
            var c2 = this.ColorRandom.Next();
            var r = this.Random.Next(0, 4);
            var s = new Square();
            switch (r)
            {
                case 0:
                    s.SetColor(Direction.TopLeft, c0);
                    s.SetColor(Direction.TopRight, c1);
                    s.SetColor(Direction.DownRight, c2);
                    break;
                case 1:
                    s.SetColor(Direction.TopRight, c0);
                    s.SetColor(Direction.DownRight, c1);
                    s.SetColor(Direction.DownLeft, c2);
                    break;
                case 2:
                    s.SetColor(Direction.DownRight, c0);
                    s.SetColor(Direction.DownLeft, c1);
                    s.SetColor(Direction.TopLeft, c2);
                    break;
                case 3:
                    s.SetColor(Direction.DownLeft, c0);
                    s.SetColor(Direction.TopLeft, c1);
                    s.SetColor(Direction.TopRight, c2);
                    break;
                default :
                    break;
            }

            return s;
        }
    }

    public class SquareERandom
        : SquareRandom
    {
        public SquareERandom(SquareType type, Dictionary<Color, int> colorWeights, int seed = 0)
            : base(type, colorWeights, seed)
        {
        }

        public override Square GetSquare()
        {
            var s = new Square();
            s.SetColor(Direction.TopLeft, this.ColorRandom.Next());
            s.SetColor(Direction.TopRight, this.ColorRandom.Next());
            s.SetColor(Direction.DownRight, this.ColorRandom.Next());
            s.SetColor(Direction.DownLeft, this.ColorRandom.Next());

            return s;
        }
    }

    public static class SquareRandomFactory
    {
        public static SquareRandom Create(SquareType type, Dictionary<Color, int> w, int seed)
        {
            switch (type)
            {
                case SquareType.A:
                default:
                    return new SquareARandom(type, w, seed);
                case SquareType.B:
                    return new SquareBRandom(type, w, seed);
                case SquareType.C:
                    return new SquareCRandom(type, w, seed);
                case SquareType.D:
                    return new SquareDRandom(type, w, seed);
                case SquareType.E:
                    return new SquareERandom(type, w, seed);
            }
        }
    }
}
