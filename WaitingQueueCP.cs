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


        public WaitingQueueCP(int total, Label l1)
        {
            waitingQueue = new Queue<Customer>();
            totalCustomers = total;
            currentCustomers = 0;
            myLock = new object();
            full = false;
            empty = true;
            this.l1 = l1;

        }

        public void EnqueToQueue(Customer customer)
        {

            waitingQueue.Enqueue(customer);
            ++currentCustomers;

            empty = false;
            if (currentCustomers >= totalCustomers) { full = true; }
            else { full = false; }

            l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
            Thread.Sleep(200);

        }

        public Customer DequeToPool()
        {

            Customer temp;
            temp = waitingQueue.Dequeue();
            --currentCustomers;

            full = false;
            if (waitingQueue.Count == 0) { empty = true; }
            else { empty = false; }

            l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
            Thread.Sleep(200);

            return temp;

        }

        public bool Full
        {
            get { return full; }
            set { full = value; }
        }
        public bool Empty
        {
            get { return empty; }
        }
        //readSemphore.WaitOne();

        //Customer temp;
        //temp = waitingQueue.Dequeue();
        //--currentCustomers;
        //l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
        //Thread.Sleep(200);


        //writeSemaphore.Release();

        //return temp;

        //writeSemaphore.WaitOne();

        //waitingQueue.Enqueue(customer);
        //++currentCustomers;
        //l1.Invoke(new Action(delegate () { l1.Text = currentCustomers.ToString(); }));
        //Thread.Sleep(200);
        //readSemphore.Release();

        //private static Semaphore writeSemaphore, readSemphore;

        //readSemphore = new Semaphore(0, totalCustomers);
        //writeSemaphore = new Semaphore(totalCustomers, totalCustomers);

    }
}
