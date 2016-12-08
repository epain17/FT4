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
        AdventurePool AP;
        CommonPool CP;
        Random random;
        bool open;

        Queue<Customer> waitingCP = new Queue<Customer>();
        Queue<Customer> waitingAP = new Queue<Customer>();

        public Reception(int maxNrCustumers, AdventurePool AP, CommonPool CP)
        {
            this.maxNrCustumers = maxNrCustumers;
            currentWaitingCustumers = 0;
            this.AP = AP;
            this.CP = CP;
            open = false;

            random = new Random();

        }

        private void PutInLine()
        {

        }

        private int ChooseLine
        {

        }

        private Customer AddToReception
        {

        }

        private bool Open
        {
            get
        }


        




    }
}
