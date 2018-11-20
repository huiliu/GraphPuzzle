using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphGame.Logic
{
    public class ConfigData
    {
        public List<LevelData> Levels = new List<LevelData>();
    }

    public class LevelData
    {
        public int Seed;
        public int BoardWidth;
        public int BoardHeight;
        public List<int> unUsedSquareID = new List<int>();
        public List<SquareData> Squares = new List<SquareData>();
    }

    public class SquareData
    {
        public SquareType Type;
        public int Weight;
        public List<ColorWeightTuple> ColorWeights = new List<ColorWeightTuple>();

        public class ColorWeightTuple
        {
            public Color Color;
            public int Weight;
        }
    }

    public static class LevelDataHelper
    {
        public static Dictionary<SquareType, int> GetSquareWeight(this LevelData levelData)
        {
            var dict = new Dictionary<SquareType, int>();
            foreach (var s in levelData.Squares)
                dict.Add(s.Type, s.Weight);

            return dict;
        }

        public static Dictionary<SquareType, Dictionary<Color, int>> GetSquareColorWeight(this LevelData levelData)
        {
            var dict = new Dictionary<SquareType, Dictionary<Color, int>>();
            foreach (var s in levelData.Squares)
            {
                var tmp = new Dictionary<Color, int>();
                foreach (var c in s.ColorWeights)
                    tmp.Add(c.Color, c.Weight);

                dict.Add(s.Type, tmp);
            }

            return dict;
        }
    }
}
