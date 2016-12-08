using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT4
{
    class Customer
    {
        int turnNumber;
        float timeInPool;
        Random random = new Random();

        public Customer(int turnNumber)
        {
            this.turnNumber = turnNumber;
            timeInPool = random.Next(100, 2000);

        }
    }
}
