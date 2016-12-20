using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FT4
{
    class CommonPool
    {
        int maxNrOfPeople;
        int currentNrOfPeople;
        Queue<Customer> customersInCP = new Queue<Customer>();
        WaitingQueueCP wCP;
        Random random;
        bool full;
        bool empty;
        object myLock;
        Label l1;


        public CommonPool(int maxNrOfPeople, WaitingQueueCP wCP, Label l1)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            random = new Random();
            this.wCP = wCP;
            full = false;
            empty = true;
            myLock = new object();
            this.l1 = l1;

        }

        public void Control()
        {
            while (full != true )
            {
                if(wCP.Empty == true)
                {
                    break;
                }
              
                AddFromWQ();
            }
            Wait();
        }

        public void Wait()
        {
            while (full == true || wCP.Empty == true)
            {
                Thread.Sleep(200);
            }

            Control();
        }

        public void AddFromWQ()
        {
            Monitor.Enter(myLock);
            while (customersInCP.Count >= maxNrOfPeople)
            {
                Monitor.Wait(myLock);
            }
            customersInCP.Enqueue(wCP.DequeToPool());
            ++currentNrOfPeople;

            if (customersInCP.Count > maxNrOfPeople) { full = true; }
            empty = false;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));
            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);

        }

        public void AddFromAP(Customer customer)
        {
            Monitor.Enter(myLock);

            while (customersInCP.Count >= maxNrOfPeople)
            {
                Monitor.Wait(myLock);

            }

            customersInCP.Enqueue(customer);
            ++currentNrOfPeople;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));


            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);

        }

        public Customer MoveToExit()
        {
            Monitor.Enter(myLock);

            while (customersInCP.Count == 0)
            {
                Monitor.Wait(myLock);

            }
            Customer temp;
            temp = customersInCP.Dequeue();
            --currentNrOfPeople;
            if (customersInCP.Count <= 0) { empty = true; }
            full = false;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));


            Thread.Sleep(200);
            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);

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
    }
}
