using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class KillerHeuristic
    {
        private Dictionary<string, int> moveFrequencyAmongKillerMoves = new Dictionary<string, int>();
        public int totalNodeCount = 0;
        private object killerHeuristicLock = new Object();

        public string bestMove="";
        private int bestMoveCount=0;

        public double isBestMoveRatio(string move)
        {
            if (this.moveFrequencyAmongKillerMoves.ContainsKey(move) && totalNodeCount > 0)
                return moveFrequencyAmongKillerMoves[move]/totalNodeCount;
            return 0;
        }

        public void addNewMove(string oldmove, string newmove)
        {
            lock (this.killerHeuristicLock)
            {
                if (moveFrequencyAmongKillerMoves.ContainsKey(oldmove))
                {
                    moveFrequencyAmongKillerMoves[oldmove] -= 1;
                    if (oldmove == bestMove)
                        bestMoveCount -= 1;
                    newMoveAdder(newmove);
                }
            }
        }

        public void addNewMove(string newmove)
        {
            lock (this.killerHeuristicLock) 
            { 
                newMoveAdder(newmove);
            }
        }

        private void newMoveAdder(string newmove)
        {
            if (moveFrequencyAmongKillerMoves.ContainsKey(newmove))
                moveFrequencyAmongKillerMoves[newmove] = moveFrequencyAmongKillerMoves[newmove] + 1;
            else
                moveFrequencyAmongKillerMoves[newmove] = 1;
            if (moveFrequencyAmongKillerMoves[newmove] > bestMoveCount)
            {
                bestMoveCount = moveFrequencyAmongKillerMoves[newmove];
                bestMove = newmove;
            }
        }

    }
}
