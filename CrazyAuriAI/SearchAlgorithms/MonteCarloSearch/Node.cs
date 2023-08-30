using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class Node
    {
        public Board board;
        public Move? move;
        public Node? parent=null;
        public double score = 0;
        public int visits = 0;

        public List<Node> childpositions = new List<Node>();
        public List<Move> childpositionmoves = new List<Move>();
        private IEvaluationFunction evaluationfunction = new MainEvaluationFunction();

        public double scoreratio
        {
            get { return score / visits; }
        }

        public Node(Board board)
        {
            this.board = board;
        }
        public Node(Board board, Move move, Node parent)
        {
            this.board = board;
            this.move = move;
            this.parent = parent;
        }
        
        public void ExpandNode()
        {
            if (board.GetWinner() == "0")
            {
                foreach (var i in board.GetAllMoves())
                {
                    var newboard = new Board(board.ToString(), board.FormerPositions);
                    newboard.MakeMove(i);
                    this.childpositions.Add(new Node(newboard, i, this));
                    this.childpositionmoves.Add(i);
                }
            }
        }

        public void Update(double result)
        {
            this.visits += 1;
            this.score += result;
        }

        public bool IsLeaf()
        {
            if (childpositions.Count>0)
                return false;
            return true;
        }

        public bool HasParent()
        {
            return parent != null;
        }
    }
}
