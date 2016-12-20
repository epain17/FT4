using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FT4
{
    public partial class FT4 : Form
    {
        private AdventurePool aPool;
        private CommonPool cPool;
        private WatingQueueAP wAP;
        private WaitingQueueCP wCP;
        private Reception reception;
        private ExitQueue exitQ;

        private Thread receptionThread, aPoolThread, cPoolThread, exitThread;


        public FT4()
        {
            InitializeComponent();
            CreateObjects();
            CreateThreads();
        }

        private void OpenClosePoll_Click(object sender, EventArgs e)
        {
            reception.Open = true;
            
        }

        private void CreateObjects()
        {
            wAP = new WatingQueueAP(10, PeopleWaitingAP);
            wCP = new WaitingQueueCP(10, PeopleWaitingCP);
            reception = new Reception(10, wAP, wCP);
            cPool = new CommonPool(10, wCP, VistorsInCplabel);
            aPool = new AdventurePool(10, wAP, cPool, VisitorsInAPlabel);
            exitQ = new ExitQueue(100, cPool, aPool, exit);
        }

        private void CreateThreads()
        {
            receptionThread = new Thread(reception.ChooseLine);
            aPoolThread = new Thread(aPool.Control);
            cPoolThread = new Thread(cPool.Control);
            exitThread = new Thread(exitQ.Control);

            receptionThread.Start();
            aPoolThread.Start();
            cPoolThread.Start();
            exitThread.Start();
        }
    }
}
