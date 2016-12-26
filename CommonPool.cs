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
    class CommonPool
    {
        int maxNrOfPeople;
        int currentNrOfPeople;
        int select;
        Queue<Customer> customersInCP = new Queue<Customer>();
        WaitingQueueCP wCP;
        Random random;
        bool full;
        bool empty;
        object myLock;
        Label l1, l2;
        PictureBox p1;
        AdventurePool apConnection;


        public CommonPool(int maxNrOfPeople, AdventurePool AP, WaitingQueueCP wCP, Label l1, Label l2, PictureBox p1)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            random = new Random();
            this.wCP = wCP;
            full = false;
            empty = true;
            myLock = new object();
            this.l1 = l1;
            this.l2 = l2;
            this.p1 = p1;
            apConnection = AP;
            select = 0;

        }

        public void Control()
        {
            while (full != true)
            {
                if (wCP.Empty == true)
                {
                    break;
                }

                select = random.Next(1, 3);
                if (select == 1)
                {
                    AddFromWQ();
                }
                else if (select == 2)
                {
                    AddFromAP();
                }
            }
            Wait();
        }

        public void Wait()
        {
            while (full == true || wCP.Empty == true)
            {
                Thread.Sleep(random.Next(100, 300));
            }

            Control();
        }

        public void AddFromWQ()
        {
            Monitor.Enter(myLock);
            p1.Invoke(new Action(delegate () { p1.BackColor = Color.Green; }));
            while (customersInCP.Count >= maxNrOfPeople)
            {
                p1.Invoke(new Action(delegate () { p1.BackColor = Color.Red; }));
                Monitor.Wait(myLock);

            }
            Monitor.PulseAll(myLock);

            customersInCP.Enqueue(wCP.DequeToPool());
            ++currentNrOfPeople;

            if (customersInCP.Count > maxNrOfPeople) { full = true; }
            empty = false;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));
            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);
            Thread.Sleep(random.Next(200, 500));


        }

        public void AddFromAP()
        {
            Monitor.Enter(myLock);
            l2.Invoke(new Action(delegate () { l2.Text = "Ideal"; }));

            while (customersInCP.Count >= maxNrOfPeople)
            {
                Monitor.Wait(myLock);
            }
            Monitor.PulseAll(myLock);



            Customer temp = apConnection.MoveToExit();
            customersInCP.Enqueue(temp);

            ++currentNrOfPeople;
            l2.Invoke(new Action(delegate () { l2.Text = "didit"; }));
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));


            Thread.Sleep(random.Next(200, 500));
            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);
            l2.Invoke(new Action(delegate () { l2.Text = "Ideal"; }));

        }

        public Customer MoveToExit()
        {
            Monitor.Enter(myLock);
            p1.Invoke(new Action(delegate () { p1.BackColor = Color.Green; }));

            while (customersInCP.Count == 0)
            {
                Monitor.Wait(myLock);

            }

            Monitor.PulseAll(myLock);

            Customer temp;
            temp = customersInCP.Dequeue();
            --currentNrOfPeople;
            if (customersInCP.Count <= 0) { empty = true; }
            full = false;
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));


            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);
            Thread.Sleep(random.Next(100, 300));

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
