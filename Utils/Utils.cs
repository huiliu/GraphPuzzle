using System;

namespace GraphGame.Logic
{
    public static class Utils
    {
        public static Direction ToDirection(int r0, int c0, int r1, int c1)
        {
            if (r1 > r0)
                return c1 > c0 ? Direction.DownRight : Direction.DownLeft;
            else
                return c1 < c0 ? Direction.TopLeft : Direction.TopRight;
        }

        public static int LoopBufferScore { get { return 2; } }

        /// 节点得分计算策略
        public static int CalcScoreStrategy(int count)
        {
            switch (count)
            {
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
