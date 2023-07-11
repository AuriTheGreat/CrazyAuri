using CrazyAuri.Models;
using CrazyAuriAI.Models;

namespace CrazyAuriAI
{
    public class MainBot
    {
        private IBot bot = new Bot();

        public string GetMove(Board board)
        {
            return bot.GetMove(board);
        }
    }
}