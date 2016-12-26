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
            if (reception.Open == true)
            {
                reception.Open = false;
                OpenClosePoll.Invoke(new Action(delegate () { OpenClosePoll.Text = "Open"; }));
            }

            else if(reception.Open == false)
            {
                reception.Open = true;
                OpenClosePoll.Invoke(new Action(delegate () { OpenClosePoll.Text = "Closed"; }));
            }
        }

        private void CreateObjects() 
        {
            wAP = new WatingQueueAP(10, PeopleWaitingAP);
            wCP = new WaitingQueueCP(10, PeopleWaitingCP);
            reception = new Reception(10, wAP, wCP);
            aPool = new AdventurePool(10, wAP, VisitorsInAPlabel, APpictureBox);
            cPool = new CommonPool(10, aPool, wCP, VistorsInCplabel, SwitchLabel, CPpicturebox);
            exitQ = new ExitQueue(100, cPool, aPool, exit);
            APpictureBox.BackColor = Color.Green;
            CPpicturebox.BackColor = Color.Green;
            
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
