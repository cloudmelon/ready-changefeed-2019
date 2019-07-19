using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slotmachines
{
    public class Game
    {
        public string id { get; set; }
        public string playerId { get; set; }
        public string machineId { get; set; }
        public double gameCost { get; set; }
        public double prizeAward { get; set; }

        public Game(string id, string playerId, string machineId, double gameCost, double prizeAward)
        {
            this.id = id;
            this.playerId = playerId;
            this.machineId = machineId;
            this.gameCost = gameCost;
            this.prizeAward = prizeAward;
        }
    }
}

