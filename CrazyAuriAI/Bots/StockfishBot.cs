using CrazyAuri.Models;
using CrazyAuriAI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrazyAuriAI.Bots
{
    public class StockfishBot : IBot
    {
        private Process stockfishProcess;
        public int difficultylevel=1;
        private string move = null;

        public StockfishBot()
        {
            ProcessStartInfo si = new ProcessStartInfo()
            {
                FileName = "fairy_stockfish.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            stockfishProcess = new Process();
            stockfishProcess.StartInfo = si;
            try
            {
                // throws an exception on win98
                stockfishProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            catch { }

            stockfishProcess.OutputDataReceived += new DataReceivedEventHandler(myProcess_OutputDataReceived);

            stockfishProcess.Start();
            stockfishProcess.BeginErrorReadLine();
            stockfishProcess.BeginOutputReadLine();

            SendLine("uci");
            SendLine("isready");
            SendLine("ucinewgame");
            SendLine("setoption name UCI_Variant value crazyhouse");
            SendLine("setoption name Skill Level value " + difficultylevel);
        }

        ~StockfishBot()
        {
            SendLine("quit");
            stockfishProcess.Dispose();
        }
        public string GetMove(Board board)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            move = "";

            SendLine(GetMoves(board));
            SendLine("go movetime 3000");

            while (move == ""){}

            var color = "White";
            if (board.CurrentColor == true)
                color = "Black";

            Console.WriteLine(color + " move chosen: " + move + " After " + double.Round(stopwatch.Elapsed.TotalSeconds, 2) + "s.");

            return move;
        }

        private void SendLine(string command)
        {
            stockfishProcess.StandardInput.WriteLine(command);
            stockfishProcess.StandardInput.Flush();
        }

        private void myProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string text = e.Data;

            string pattern = "bestmove (.*) ponder";
            Regex rg = new Regex(pattern);
            Match matchedMove = rg.Match(text);
            if (matchedMove.Success)
            {
                move = matchedMove.Groups[1].Value;
                return;
            }

            pattern = "bestmove (.*)";
            rg = new Regex(pattern);
            matchedMove = rg.Match(text);
            if (matchedMove.Success)
            {
                move = matchedMove.Groups[1].Value;
                return;
            }
            //Console.WriteLine("[UCI] " + text);
        }

        private string GetMoves(Board board)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(" position startpos moves ");
            foreach (var i in board.movehistory)
            {
                sb.Append(i);
                sb.Append(" ");
            }
            return sb.ToString();
        }

    }
}
