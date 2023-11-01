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

        private Object updateLock = new Object();
        public double evaluationscore = 0;
        public int matingscore = 0;
        public int visits = 0;

        public Node mostvisitedchild;
        public Node mostevaluatedchild;

        private Object nodeChildPositionsLock = new Object();
        public List<Node> childpositions = new List<Node>();

        private IEvaluationFunction evaluationfunction = new MainEvaluationFunction();

        public override string ToString()
        {
            return (move.ToString() ?? "")  + " " + Math.Round(evaluationscoreratio, 2).ToString() + " " + visits.ToString();
        }

        public double evaluationscoreratio
        {
            get { return evaluationscore / visits; }
        }

        public double matingscoreratio
        {
            get { return matingscore / visits; }
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
                    lock (nodeChildPositionsLock)
                        this.childpositions.Add(new Node(newboard, i, this));
                }
            }
        }

        public void Update(SimulationResult result)
        {
            lock (updateLock)
            {
                visits += 1;
                evaluationscore += result.evaluationScore;
                matingscore += result.matingScore;
                if (parent != null)
                {
                    if (parent.mostevaluatedchild != null)
                    {
                        if (parent.mostevaluatedchild.evaluationscoreratio < this.evaluationscoreratio)
                            parent.mostevaluatedchild = this;
                        else if (parent.mostevaluatedchild.evaluationscoreratio == this.evaluationscoreratio)
                        {
                            if (parent.mostvisitedchild.visits < this.visits)
                                parent.mostevaluatedchild = this;
                        }
                        if (parent.mostvisitedchild.visits < this.visits)
                            parent.mostvisitedchild = this;
                        else if (parent.mostvisitedchild.visits == this.visits)
                        {
                            if (parent.mostevaluatedchild.evaluationscoreratio < this.evaluationscoreratio)
                                parent.mostvisitedchild = this;
                        }
                    }
                    else
                    {
                        parent.mostevaluatedchild = this;
                        parent.mostvisitedchild = this;
                    }
                }
            }
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

        public bool MostVisitedChildEqualsMostEvaluated()
        {
            return mostevaluatedchild == mostvisitedchild;
        }
    }
}
