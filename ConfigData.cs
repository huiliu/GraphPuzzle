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
        public int BoardWidth;  // 方块数
        public int BoardHeight; // 方块数
        public List<int> unUsedSquareID = new List<int>();  // 方块ID
        public List<SquareData> Squares = new List<SquareData>();
    }

    public class SquareData
    {
        public SquareType Type;     // 方块类型
        public int Weight;          // 此类型方块权重
        public List<ColorWeightTuple> ColorWeights = new List<ColorWeightTuple>();  // 此类型方块中各颜色的权重

        public class ColorWeightTuple
        {
            public Color Color;     // 颜色
            public int Weight;      // 颜色对应的权重
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

        public static bool IsUnusedSquare(this LevelData levelData, int id)
        {
            var idx = levelData.unUsedSquareID.FindIndex(i => i == id);
            return idx != -1;
        }
    }
}
