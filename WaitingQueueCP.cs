using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT4
{
    class WaitingQueueCP
    {
        Queue<Customer> waitingQueue;
        int totalCustomers;
        int currentCustomers;

        

        public WaitingQueueCP(int total)
        {
            waitingQueue = new Queue<Customer>();
            totalCustomers = total;
            currentCustomers = 0;            
        }

        public void EnqueToQueue(Customer customer)
        {
          if(currentCustomers != totalCustomers)
            {
                waitingQueue.Enqueue(customer);
                ++currentCustomers;

            }   
        }

        public Customer DequeToPool()
        {
                Customer temp;
                temp = waitingQueue.Dequeue();
                --currentCustomers;
                return temp;
            
        }


    }
}
