using CrazyAuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.Bots
{
    public interface IBot
    {
        public string GetMove(Board board);

    }
}
