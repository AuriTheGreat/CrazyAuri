using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.Minimax
{
    public class TranspositionTable
    {
        private Dictionary<string, TranspositionEntry> entrytable = new Dictionary<string, TranspositionEntry>();
        
        public TranspositionEntry GetEntry(string position, int depth)
        {
            if (entrytable.ContainsKey(position))
            {
                if (entrytable[position].depth >= depth)
                {
                    return entrytable[position];
                }
            }
            return null;
        }

        public void AddEntry(string position, string move, double value, int depth, string flag)
        {
            var transpositionentry = new TranspositionEntry(move, value, depth, flag);
            //if (entrytable.ContainsKey(position))
            //{
            //    entrytable[position] = transpositionentry;
            //}
            entrytable.TryAdd(position, transpositionentry);
        }

    }

    public class TranspositionEntry
    {
        public string move;
        public double value;
        public int depth;
        public string flag;
        public TranspositionEntry(string move, double value, int depth, string flag)
        {
            this.move = move;
            this.value = value;
            this.depth = depth;
            this.flag = flag;
        }
    }
}
