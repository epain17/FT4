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

        /// <summary>
        /// skickar tråden att ta in kunder från väntkön eller ta kunder från AP. Är poolen full skcikas tråden till wait
        /// </summary>
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
                else if (select == 2 && apConnection.Empty == false)
                {
                    AddFromAP();
                }
            }
            Wait();
        }

        /// <summary>
        /// håller tråden vid liv tills poolen inte är full
        /// </summary>
        public void Wait()
        {
            while (full == true || wCP.Empty == true)
            {
                Thread.Sleep(500);
            }

            Control();
        }

        /// <summary>
        /// lägger in kunder från väntkön till poolen
        /// </summary>
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

        /// <summary>
        /// hämtar kunder från AP
        /// </summary>
        public void AddFromAP()
        {
            Monitor.Enter(myLock);
            l2.Invoke(new Action(delegate () { l2.Text = "Idel"; }));

            while (customersInCP.Count >= maxNrOfPeople)
            {
                Monitor.Wait(myLock);
            }
            Monitor.PulseAll(myLock);



            Customer temp = apConnection.MoveToExit();
            customersInCP.Enqueue(temp);

            ++currentNrOfPeople;
            l2.Invoke(new Action(delegate () { l2.Text = "Moved One"; }));
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));


            Thread.Sleep(random.Next(200, 500));
            Monitor.PulseAll(myLock);
            Monitor.Exit(myLock);
            l2.Invoke(new Action(delegate () { l2.Text = "Ideal"; }));

        }

        /// <summary>
        /// metoden exitqueue klassen använder för att tömma poolen
        /// </summary>
        /// <returns></returns>
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
