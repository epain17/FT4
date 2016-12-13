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
        object myLock;
        Label l1;


        public CommonPool(int maxNrOfPeople, WaitingQueueCP wCP, Label l1)
        {
            this.maxNrOfPeople = maxNrOfPeople;
            currentNrOfPeople = 0;
            random = new Random();
            this.wCP = wCP;
            full = false;
            myLock = new object();
            this.l1 = l1;

        }

        public void Control()
        {
            while (full != true)
            {

                AddFromWQ();
            }
            Wait();
        }

        public void Wait()
        {
            while (full == true)
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
            l1.Invoke(new Action(delegate () { l1.Text = currentNrOfPeople.ToString(); }));


            Monitor.Pulse(myLock);
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


            Monitor.Pulse(myLock);
            Monitor.Exit(myLock);

        }

        public void MoveToExit()
        {
            Monitor.Enter(myLock);

            while (customersInCP.Count == 0)
            {
                Monitor.Wait(myLock);
            }
            customersInCP.Dequeue();
            --currentNrOfPeople;

            Monitor.Pulse(myLock);
            Monitor.Exit(myLock);


        }
    }
}
