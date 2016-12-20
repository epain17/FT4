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
        bool open, receptionFull;
        int queueSelect;



        public Reception(int maxNrCustumers, WatingQueueAP wAP, WaitingQueueCP wCP)
        {
            this.maxNrCustumers = maxNrCustumers;
            currentWaitingCustumers = 0;
            this.wAP = wAP;
            this.wCP = wCP;
            open = false;
            receptionFull = false;

            random = new Random();

        }

        public void ChooseLine()
        {
            while (open == true && receptionFull == false)
            {
                queueSelect = random.Next(1, 3);
                //Console.WriteLine(queueSelect.ToString());
                Customer newCustomer = new Customer(queueSelect);
                if (queueSelect == 1 && wAP.Full == false)
                {
                    receptionFull = false;
                    wAP.EnqueToQueue(newCustomer);
                }
                else if (queueSelect == 2 && wCP.Full == false)
                {
                    receptionFull = false;
                    wCP.EnqueToQueue(newCustomer);
                }

                else if(wCP.Full == true && wAP.Full == true)
                {
                    receptionFull = true;
                   
                }

            }

           
                Wait();
            


        }

        private void Wait()
        {
            while(open == false || receptionFull == true)
            {
                Thread.Sleep(1000);
                if(wCP.Full == false || wAP.Full == false)
                {
                    receptionFull = false;
                    break;
                }
            }
            ChooseLine();
        }

        public bool Open
        {
            set { open = value; }
        }

    }
}
