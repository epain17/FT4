using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT4
{
    class CommonPool
    {
        int maxNrOfPeople;
        int currentNrOfPeople;
        Queue<Customer> customersInCP = new Queue<Customer>();
        WaitingQueueCP wCP;
        Random random;


        public CommonPool(int maxNrOfPeople, WaitingQueueCP wCP)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            random = new Random();
            this.wCP = wCP;

        }

        public void AddFromWQ()
        {
            Customer addToPool;
            addToPool = wCP.DequeToPool();
            customersInCP.Enqueue(addToPool);
            ++currentNrOfPeople;
        }

        public void AddFromAP(Customer customer)
        {
            customersInCP.Enqueue(customer);
            ++currentNrOfPeople;

        }

        public void MoveToExit()
        {
            customersInCP.Dequeue();
        }
    }
}
