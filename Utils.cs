﻿using System;

namespace GraphGame.Logic
{
    public static class Utils
    {
        public static void SafeInvoke(this Action action)
        {
            if (action == null)
                return;

            try
            {
                action.Invoke();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Assert(false, err.Message);
            }
        }

        public static void SafeInvoke<T>(this Action<T> action, T t)
        {
            if (action == null)
                return;

            try
            {
                action.Invoke(t);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Assert(false, err.Message);
            }
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 t1, T2 t2)
        {
            if (action == null)
                return;

            try
            {
                action.Invoke(t1, t2);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Assert(false, err.Message);
            }
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            if (action == null)
                return;

            try
            {
                action.Invoke(t1, t2, t3);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Assert(false, err.Message);
            }
        }

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
