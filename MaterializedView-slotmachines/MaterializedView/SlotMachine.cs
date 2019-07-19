using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slotmachines
{
    public class SlotMachine
    {
        public string id { get; set; }
        public int totalGames { get; set; }

        public double totalMoneyInput { get; set; }

        public double totalPrizes { get; set; }

        public double totalProfit { get; set; }

        public SlotMachine(string machineId, int totalGames, double totalMoneyInput, double totalPrizes, double totalProfit)
        {
            this.id = machineId;
            this.totalGames = totalGames;
            this.totalMoneyInput = totalMoneyInput;
            this.totalPrizes = totalPrizes;
            this.totalProfit = totalProfit;
        }
    }
}
