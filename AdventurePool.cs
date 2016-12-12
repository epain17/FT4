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
        WatingQueueAP wAP;
        Random random;
        int exit;

        public AdventurePool(int maxNrOfPeople, WatingQueueAP wAP, CommonPool CP)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            this.wAP = wAP;
            this.CP = CP;
            random = new Random();

        }

        public void AddFromWQ()
        {
            Customer addToPool;
            addToPool = wAP.DequeToPool();
            customersInAP.Enqueue(addToPool);
            ++currentNrOfPeople;
        }

        public void MoveToExitCP()
        {
            exit = random.Next(1, 2);
            if(exit == 1)
            {
                customersInAP.Dequeue();
            }
            else if(exit == 2)
            {
                Customer temp;
                temp = customersInAP.Dequeue();
                CP.AddFromAP(temp);
            }
        }

       
    }
}
