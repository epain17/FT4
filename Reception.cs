using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FT4
{
    class Reception
    {
        int currentWaitingCustumers;
        int maxNrCustumers;
        WaitingQueueCP wCP;
        WatingQueueAP wAP;
        Random random;
        bool open;
        int queueSelect;



        public Reception(int maxNrCustumers, WatingQueueAP wAP, WaitingQueueCP wCP)
        {
            this.maxNrCustumers = maxNrCustumers;
            currentWaitingCustumers = 0;
            this.wAP = wAP;
            this.wCP = wCP;
            open = false;

            random = new Random();

        }

        public void ChooseLine()
        {
            while (open == true)
            {
                queueSelect = random.Next(1, 3);
                Console.WriteLine(queueSelect.ToString());
                Customer newCustomer = new Customer(queueSelect);
                if (queueSelect == 1)
                {
                    wAP.EnqueToQueue(newCustomer);
                }
                else if (queueSelect == 2)
                {
                    wCP.EnqueToQueue(newCustomer);
                }

            }

            if(open == false)
            {
                Wait();
            }


        }

        private void Wait()
        {
            while(open == false) { Thread.Sleep(400); }
            ChooseLine();
        }

     

        public bool Open
        {
            set { open = value; }
        }







    }
}
