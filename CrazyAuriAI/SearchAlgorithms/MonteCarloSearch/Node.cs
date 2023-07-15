using CrazyAuri.Models;
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
        public List<Node> childpositions = new List<Node>();
        public List<Move> childpositionmoves = new List<Move>();
        public int T = 0;
        public int N = 0;
        public Node parent;
        public Boolean done = false;
        private Random random = new Random();
        public Node(Board board, Node parent, bool done)
        {
            this.board = board;
            this.parent = parent;
            this.done = done;
        }
        public double getUCBscore()
        {
            if (this.N == 0)
                return Double.MaxValue;
            var topnode = this;
            if (topnode.parent != null)
            {
                topnode = topnode.parent;
            }
            var c = 1.2;
            return (this.T / this.N) + c * Math.Sqrt(Math.Log(topnode.N) / this.N);
        }

        public void createChild()
        {
            if (this.done == true)
                return;

            foreach(var i in board.GetAllMoves())
            {
                var newboard = new Board(board.ToString(), board.FormerPositions);
                newboard.MakeMove(i);
                bool done = false;
                if (newboard.GetWinner() != "0")
                    done = true;
                this.childpositions.Add(new Node(newboard, this, done));
                this.childpositionmoves.Add(i);
            }
        }

        public void explore()
        {
            var current = this;
            while (current.childpositions.Count > 0)
            {
                double maxU = -1;
                foreach (var i in current.childpositions)
                {
                    if (i.getUCBscore() > maxU)
                        maxU = i.getUCBscore();
                }
                current = current.childpositions[random.Next(current.childpositions.Count)];
            }
            if (current.N < 1)
            {
                current.T = current.T + current.rollout();
            }
            else
            {
                current.createChild();
                if (current.childpositions.Count > 0)
                {
                    current = current.childpositions[random.Next(current.childpositions.Count)];
                }
                current.T=current.T + current.rollout();
            }
            current.N += 1;
            
            var localparent = current;

            while (parent != null)
            {
                localparent = localparent.parent;
                localparent.N += 1;
                localparent.T = localparent.T + current.T;
            }

        }

        public int rollout()
        {
            if (this.done==true)
                return 0;
            var v = 0;
            done = false;
            var newboard = new Board(board.ToString(), board.FormerPositions);
            while (done != false)
            {
                var moves = newboard.GetAllMoves();
                newboard.MakeMove(moves[random.Next(moves.Count)]);
                if (newboard.GetWinner() != "0")
                {
                    if (newboard.GetWinner() == "w")
                    {
                        v += 1;
                    }
                    else if (newboard.GetWinner() == "b")
                    {
                        v -= 1;
                    }
                    done=true;
                }
            }
            return v;
        }

        public Node next()
        {
            int maxN = 0;
            List<Node> maxPositions = new List<Node>();

            foreach (var i in childpositions)
            {
                if (i.N> maxN)
                    maxN = i.N;
            }

            foreach(var i in childpositions)
            {
                if (i.N==maxN)
                    maxPositions.Add(i);
            }

            return maxPositions[random.Next(maxPositions.Count)];

        }

        public Move findTheBoardMove(Node node)
        {
            for (int i=0; i<childpositions.Count; i++)
            {
                if (childpositions[i] == node)
                {
                    return childpositionmoves[i];
                }
            }
            return null;

        }

    }
}
