using CrazyAuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.Models
{
    public interface IBot
    {
        public string GetMove(Board board);

    }
}
