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

        public static Direction ToDirection(int r0, int c0, int r1, int c1)
        {
            if (r1 > r0)
                return c1 > c0 ? Direction.DownRight : Direction.DownLeft;
            else
                return c1 < c0 ? Direction.TopLeft : Direction.TopRight;
        }
    }
}