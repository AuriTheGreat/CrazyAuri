using CrazyAuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriConsole.Players
{
    public interface IPlayer
    {
        public void MakeMove(Board board);
    }
}
