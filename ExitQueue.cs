using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FT4
{
    class ExitQueue
    {
        Queue<Customer> exitQueue = new Queue<Customer>();
        CommonPool CP;
        AdventurePool AP;
        int total;
        int current;
        int exit;
        object myLock;
        bool full;

        Label l1;
        Random rand = new Random();

        public ExitQueue(int total, CommonPool CP, AdventurePool AP, Label l1)
        {
            this.total = total;
            this.CP = CP;
            this.AP = AP;
            current = 0;
            exit = 0;
            myLock = new object();
            full = false;
            this.l1 = l1;

        }


        public void Control()
        {
            if (exitQueue.Count >= total)
            {
                full = true;
            }
            else
            {
                full = false;
            }
            while (full != true)
            {
                exit = rand.Next(1, 3);
                Thread.Sleep(rand.Next(100, 1000));
                if (CP.Empty == true && AP.Empty == true)
                {
                    break;
                }
                else if (exit == 1 && CP.Empty == false)
                {
                    EnqueToExitCP();
                }
                else if(exit == 2 && AP.Empty == false)
                {
                    EnqueToExitAP();
                }
            }
            Wait();
        }

        public void Wait()
        {
            while (full == true || CP.Empty == true || AP.Empty == true)
            {
                Thread.Sleep(rand.Next(100, 1000));
            }

            Control();
        }

        public void EnqueToExitCP()
        {            

            Customer temp;
            temp = CP.MoveToExit();
            exitQueue.Enqueue(temp);
            ++current;
            full = false;
            if (current >= total) { full = true; }
            l1.Invoke(new Action(delegate () { l1.Text = current.ToString(); }));
            Thread.Sleep(rand.Next(100, 1000));

        }

        public void EnqueToExitAP()
        {
           
            Customer temp;
            temp = AP.MoveToExit();
            exitQueue.Enqueue(temp);
            ++current;
            full = false;
            if (current >= total) { full = true; }
            l1.Invoke(new Action(delegate () { l1.Text = current.ToString(); }));
            Thread.Sleep(rand.Next(100, 1000));

        }





    }
}
