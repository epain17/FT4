using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        object myLock;
        bool full;
        Label l1;

        public AdventurePool(int maxNrOfPeople, WatingQueueAP wAP, CommonPool CP, Label l1)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            this.wAP = wAP;
            this.CP = CP;
            random = new Random();
            myLock = new object();

            this.l1 = l1;
            full = false;

        }

        public void Control()
        {
            while(full != true)
            {
               
                AddFromWQ();
            }
            Wait();
        }

        public void Wait()
        {
            while(full == true)
            {
                Thread.Sleep(200);
            }

            Control();
        }

        public void AddFromWQ()
        {
            Monitor.Enter(myLock);

            while (customersInAP.Count >= maxNrOfPeople)
            {
                Monitor.Wait(myLock);
            }

            customersInAP.Enqueue(wAP.DequeToPool());
            ++currentNrOfPeople;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));
            Monitor.Pulse(myLock);
            Monitor.Exit(myLock);
        }

        public void MoveToExitCP()
        {
            exit = random.Next(1, 3);

            Monitor.Enter(myLock);

            while (customersInAP.Count == 0)
            {
                Monitor.Wait(myLock);
            }

            if (exit == 1)
            {
                customersInAP.Dequeue();
            }
            else if (exit == 2)
            {
                Customer temp;
                temp = customersInAP.Dequeue();
                CP.AddFromAP(temp);
            }
            --currentNrOfPeople;

            Monitor.Pulse(myLock);
            Monitor.Exit(myLock);


        }

        public bool Full
        {
            set { full = value; }
        }
    }
}
