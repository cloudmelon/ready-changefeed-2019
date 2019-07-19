using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slotmachines
{
   public class Player
    {
        public string id { get; set; }
        public double balance { get; set; }

        public Player(string id,double balance)
        {
            this.id = id;
            this.balance = balance;
        }
    }
}
