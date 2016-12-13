using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FT4
{
    class WatingQueueAP
    {
        Queue<Customer> waitingQueue;
        int totalCustomers;
        int currentCustomers;
        Label l1;

        private static Semaphore writeSemaphore, readSemphore;

        public WatingQueueAP(int total, Label l1)
        {
            waitingQueue = new Queue<Customer>();
            totalCustomers = total;
            currentCustomers = 0;
            this.l1 = l1;

            readSemphore = new Semaphore(0, totalCustomers);
            writeSemaphore = new Semaphore(totalCustomers, totalCustomers);
        }

        public void EnqueToQueue(Customer customer)
        {
            writeSemaphore.WaitOne();

            waitingQueue.Enqueue(customer);
            ++currentCustomers;
            l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
            Thread.Sleep(200);

            readSemphore.Release();


        }

        public Customer DequeToPool()
        {
            readSemphore.WaitOne();

            Customer temp;
            temp = waitingQueue.Dequeue();
            --currentCustomers;
            l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
            Thread.Sleep(200);


            writeSemaphore.Release();

            return temp;

        }
    }
}
