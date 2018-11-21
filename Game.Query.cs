using System.Collections.Generic;

namespace GraphGame.Logic
{
    public partial class Game
    {
        #region 查询
        public int GetPlayerScore(string uid)
        {
            return this.GameBoard.PlayerScores[uid];
        }

        public Queue<GraphPath> GetPlayerPath(string uid)
        {
            return this.GameBoard.GetPlayerPath(uid);
        }

        public int GetPlayerColorEdgeCount(string uid, Color color, int idx)
        {
            return this.GameBoard.GetPlayerColorEdgeCount(uid, color, idx);
        }
        #endregion
    }
}
