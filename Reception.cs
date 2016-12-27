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
        int maxNrCustumers;
        WaitingQueueCP wCP;
        WatingQueueAP wAP;
        
        Random random;
        bool open, receptionFull;
        int queueSelect;



        public Reception(int maxNrCustumers, WatingQueueAP wAP, WaitingQueueCP wCP)
        {
            this.maxNrCustumers = maxNrCustumers;
            this.wAP = wAP;
            this.wCP = wCP;
            open = false;
            receptionFull = false;

            random = new Random();

        }

        /// <summary>
        /// Skapar en n kund som sedan sätts ut i någon av det två köerna. Kön får inte vara full och om receptioen är full sätts tråden i wait metoden
        /// </summary>
        public void ChooseLine()
        {
            while (open == true && receptionFull == false)
            {
                queueSelect = random.Next(1, 3);
                Customer newCustomer = new Customer(queueSelect);
                if (queueSelect == 1 && wAP.Full == false)
                {
                    receptionFull = false;
                    wAP.EnqueToQueue(newCustomer);
                    Thread.Sleep(random.Next(100, 400));
                }
                else if (queueSelect == 2 && wCP.Full == false)
                {
                    receptionFull = false;
                    wCP.EnqueToQueue(newCustomer);
                    Thread.Sleep(random.Next(100, 400));

                }

                else if(wCP.Full == true && wAP.Full == true)
                {
                    receptionFull = true;
                    Thread.Sleep(random.Next(100, 400));

                }

            }

           
                Wait();
            


        }

        /// <summary>
        /// metoden håller en tråd i sleep tills receptionen inte är full
        /// </summary>
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

        /// <summary>
        ///  sätter property för boolen open
        /// </summary>
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }

    }
}
