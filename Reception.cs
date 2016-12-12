using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void ChooseLine()
        {
            queueSelect = random.Next(1, 2);
            Customer newCustomer = new Customer(queueSelect);
            if(queueSelect == 1)
            {
                wAP.EnqueToQueue(newCustomer);
            }
            else if(queueSelect == 2)
            {
                wCP.EnqueToQueue(newCustomer);
            }


        }

        private void PutInLine()
        {

        }

        //private bool Open
        //{
        //    get
        //}


        




    }
}
