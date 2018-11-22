
namespace GraphGame.Logic
{
    /// <summary>
    /// 二维下标到线性下标转换
    /// </summary>
    public class Array2LinearHelper
    {
        private readonly int w;
        private readonly int h;
        public Array2LinearHelper(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public int GetLinearIndex(int r, int c)
        {
            return r * this.w + c;
        }

        public void GetRowCol(int idx, out int r, out int c)
        {
            r = c = -1;

            c = idx % this.w;
            r = (idx - c) / this.w;
        }
    }
}
