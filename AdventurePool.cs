using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT4
{
    class AdventurePool
    {
        int maxNrOfPeople;
        int currentNrOfPeople;
        Queue<Customer> customersInAP = new Queue<Customer>();
        CommonPool CP;
        Random random;

        public AdventurePool(int maxNrOfPeople, CommonPool CP)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;

            this.CP = CP;
            random = new Random();

        }
    }
}
