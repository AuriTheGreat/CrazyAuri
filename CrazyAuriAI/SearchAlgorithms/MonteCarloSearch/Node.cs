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
        public bool originalColor=false;

        private Object updateLock = new Object();
        public double evaluationscore = 0;
        public int matingscore = 0;
        public int visits = 0;

        public int searchedMinimaxDepth = 0;
        public double minimaxValue = 0;

        public Node mostvisitedchild;
        public Node mostevaluatedchild;

        private Object nodeChildPositionsLock = new Object();
        public List<Node> childpositions = new List<Node>();

        public KillerHeuristic killerHeuristic=new KillerHeuristic();

        public override string ToString()
        {
            return (move.ToString() ?? "")  + " " + Math.Round(evaluationscoreratio, 2).ToString() + " " + visits.ToString();
        }

        public double evaluationscoreratio
        {
            get {
                if (Math.Abs(minimaxValue) > 1000)
                    return minimaxValue;
                return evaluationscore / visits; 
            }
        }

        public double matingscoreratio
        {
            get { return matingscore / visits; }
        }

        public Node(Board board)
        {
            this.board = board;
            this.originalColor = board.CurrentColor;
        }
        public Node(Board board, Move move, Node parent)
        {
            this.board = board;
            this.move = move;
            this.parent = parent;
            this.originalColor = parent.originalColor;
        }

        public void ExpandNode()
        {
            if (IsLeaf())
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
                        {
                            if (hasGreatParent()) // killer heuristic
                                parent.parent.parent.killerHeuristic.addNewMove(parent.mostvisitedchild.move.ToString(), this.move.ToString());
                            parent.mostvisitedchild = this;
                        }
                        else if (parent.mostvisitedchild.visits == this.visits)
                        {
                            if (parent.mostvisitedchild.visits < this.visits)
                            {
                                if (hasGreatParent()) // killer heuristic
                                    parent.parent.parent.killerHeuristic.addNewMove(parent.mostvisitedchild.move.ToString(), this.move.ToString());
                                parent.mostvisitedchild = this;
                            }
                        }
                    }
                    else
                    {
                        parent.mostevaluatedchild = this;
                        parent.mostvisitedchild = this;
                        if(hasGreatParent()) // killer heuristic
                            parent.parent.parent.killerHeuristic.addNewMove(move.ToString());
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

        public bool hasGreatParent()
        {
            return HasParent() && parent.parent != null && parent.parent.parent!=null;
        }

        public bool MostVisitedChildEqualsMostEvaluated()
        {
            return mostevaluatedchild == mostvisitedchild;
        }
    }
}
