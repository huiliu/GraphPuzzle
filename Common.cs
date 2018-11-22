using System.Collections.Generic;

namespace GraphGame.Logic
{
    public enum Color
    {
        None,
        Red,
        Green,
        Blue,
        Max,
    }

    public enum Direction
    {
        TopLeft,
        TopRight,
        DownRight,
        DownLeft,
        Max,
    }

    public static class Common
    {
    }

    public struct Point
    {
        public int Row;
        public int Col;
    }

    public struct Path
    {
        public Color Color;
        public List<Point> Nodes;
    }
}