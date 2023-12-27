using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class HistoryHeuristic
    {
        private Dictionary<string, int> visitFrequencyAmongMoves = new Dictionary<string, int>();
        private object historyHeuristicLock = new Object();

        public string bestMove="";
        private int bestMoveCount=0;
        private int totalVisitCount = 0;

        public override string ToString()
        {
            return bestMove + " " + (((double)bestMoveCount)/((double)totalVisitCount)).ToString();
        }

        public int getMoveVisitCount(string move)
        {
            if (this.visitFrequencyAmongMoves.ContainsKey(move) && this.visitFrequencyAmongMoves[move] > 0)
                return visitFrequencyAmongMoves[move];
            return 0;
        }

        public int getHistoryHeuristicRatio(string move)
        {
            if (totalVisitCount > 0)
                return getMoveVisitCount(move) / totalVisitCount;
            return 0;
        }

        public string getKillerHeuristic()
        {
            return bestMove;
        }

        public void updateMove(string move)
        {
            lock (this.historyHeuristicLock)
            {
                if (this.visitFrequencyAmongMoves.ContainsKey(move))
                    this.visitFrequencyAmongMoves[move] = this.visitFrequencyAmongMoves[move] + 1;
                else
                    this.visitFrequencyAmongMoves[move] = 1;
                if (bestMoveCount < this.visitFrequencyAmongMoves[move])
                {
                    bestMoveCount = this.visitFrequencyAmongMoves[move];
                    bestMove = move;
                }
                this.totalVisitCount += 1;
            }
        }

    }
}
