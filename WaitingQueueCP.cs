using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FT4
{
    class WaitingQueueCP
    {
        Queue<Customer> waitingQueue;
        int totalCustomers;
        int currentCustomers;
        object myLock;
        bool full;
        bool empty;
        Label l1;

        Random random;


        public WaitingQueueCP(int total, Label l1)
        {
            waitingQueue = new Queue<Customer>();
            totalCustomers = total;
            currentCustomers = 0;
            myLock = new object();
            full = false;
            empty = true;
            this.l1 = l1;

            random = new Random();

        }

        /// <summary>
        /// klassen receptionen avnvänder för att lägga till kunder i väntkön
        /// </summary>
        /// <param name="customer"></param>
        public void EnqueToQueue(Customer customer)
        {

            waitingQueue.Enqueue(customer);
            ++currentCustomers;

            empty = false;
            if (currentCustomers >= totalCustomers) { full = true; }
            else { full = false; }

            l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
            Thread.Sleep(random.Next(200, 500));

        }

        /// <summary>
        /// metoden CPpoolen använder för att dequeu från väntkön till poolen
        /// </summary>
        /// <returns></returns>
        public Customer DequeToPool()
        {

            Customer temp;
            temp = waitingQueue.Dequeue();
            --currentCustomers;

            full = false;
            if (waitingQueue.Count == 0) { empty = true; }
            else { empty = false; }

            l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
            Thread.Sleep(random.Next(200, 500));

            return temp;

        }

        /// <summary>
        /// property för boolen full
        /// </summary>
        public bool Full
        {
            get { return full; }
            set { full = value; }
        }

        /// <summary>
        /// property för boolen empty
        /// </summary>
        public bool Empty
        {
            get { return empty; }
        }


    }
}
