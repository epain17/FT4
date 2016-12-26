using System;
using System.Collections.Generic;
using System.Drawing;
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
        WatingQueueAP wAP;
        Random random;
        object myLock;
        bool full;
        bool empty;
        Label l1;
        PictureBox p1;

        public AdventurePool(int maxNrOfPeople, WatingQueueAP wAP, Label l1, PictureBox p1)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            this.wAP = wAP;
            random = new Random();
            myLock = new object();

            this.l1 = l1;
            this.p1 = p1;
            full = false;
            empty = true;


        }

        public void Control()
        {
            while (full != true)
            {
                if (wAP.Empty == true)
                {
                    break;
                }

                AddFromWQ();
            }
            Wait();
        }

        public void Wait()
        {
            while (full == true || wAP.Empty == true)
            {
                Thread.Sleep(200);
            }

            Control();
        }

        public void AddFromWQ()
        {
            Monitor.Enter(myLock);
            p1.Invoke(new Action(delegate () { p1.BackColor = Color.Green; }));
            while (customersInAP.Count >= maxNrOfPeople)
            {
                Monitor.Wait(myLock);
                p1.Invoke(new Action(delegate () { p1.BackColor = Color.Red; }));

            }
            Monitor.PulseAll(myLock);


            customersInAP.Enqueue(wAP.DequeToPool());
            ++currentNrOfPeople;

            if (customersInAP.Count > maxNrOfPeople) { full = true; }
            empty = false;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));

            Thread.Sleep(random.Next(200, 500));

            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);
        }

        public Customer MoveToExit()
        {
            Monitor.Enter(myLock);
            p1.Invoke(new Action(delegate () { p1.BackColor = Color.Green; }));

            while (customersInAP.Count == 0)
            {
                Monitor.Wait(myLock);
            }

            Monitor.PulseAll(myLock);

            Customer temp;
            temp = customersInAP.Dequeue();
            --currentNrOfPeople;
            if (customersInAP.Count <= 0) { empty = true; }
            full = false;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));

            Thread.Sleep(random.Next(200, 500));
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
            get
            {
                return empty;
            }
        }
    }
}
