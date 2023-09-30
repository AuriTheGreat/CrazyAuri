using CrazyAuri.Models;
using CrazyAuriAI.SearchAlgorithms.MonteCarloSearch;
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
        public PositionState? parent = null;
        public double value = 0;
        public int visits = 0;
        public double alpha = double.MinValue;
        public double beta = double.MaxValue;
        public int depth=3;
        public List<PositionState> childPositions = new List<PositionState>();

        private double givenBonus = 0;

        public PositionState(Board board)
        {
            this.board = board;
        }
        public PositionState(Board board, Move move)
        {
            this.board = board;
            this.move = move;
        }

        public PositionState(Board board, Move move, PositionState parent)
        {
            this.board = board;
            this.move = move;
            this.parent = parent;
        }

        public void addToTree()
        {
            if (board.GetWinner() == "0")
            {
                foreach (var i in board.GetAllMoves())
                {
                    var newboard = new Board(board.ToString(), board.FormerPositions);
                    newboard.MakeMove(i);
                    this.childPositions.Add(new PositionState(newboard, i, this));
                }
            }
        }
        public void Update(double result)
        {
            this.visits += 1;
            this.value += result;
        }

        public bool IsLeaf()
        {
            if (childPositions.Count > 0)
                return false;
            return true;
        }

        public bool HasParent()
        {
            return parent != null;
        }

        public double scoreratio
        {
            get { return value / visits; }
        }

        public void updateBonus(double bonus)
        {
            this.value -= this.givenBonus;
            this.givenBonus = bonus;
            this.value += bonus;
        }
    }
}
