using CrazyAuri.Models;
using CrazyAuriAI.Bots;

namespace CrazyAuriAI
{
    public class MainBot
    {
        private IBot bot = new MinimaxBot();

        public string GetMove(Board board)
        {
            var newboard = new Board(board.ToString(), board.FormerPositions);
            return bot.GetMove(newboard);
        }
    }
}