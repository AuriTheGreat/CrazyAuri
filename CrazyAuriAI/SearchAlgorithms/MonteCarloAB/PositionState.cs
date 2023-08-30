using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloAB
{
    public class PositionState
    {
        public Board board;
        public Move? move;
        public double alpha = double.MinValue;
        public double beta = double.MaxValue;
        public int depth;
        public List<PositionState> childPositions = new List<PositionState>();

        public PositionState(Board board)
        {
            this.board = board;
        }
        public PositionState(Board board, Move move)
        {
            this.board = board;
            this.move = move;
        }
    }
}
