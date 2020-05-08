using NHDSolution;
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

namespace HandleServer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private LoginServer LoginServer { get; set; }
        private GameServer GameServer { get; set; }
        private DateTime startTime { get; set; }
        private System.Timers.Timer Timer { get; set; }
        private void Main_Load(object sender, EventArgs e)
        {
            this.startTime = DateTime.Now;
            this.Timer = new System.Timers.Timer();
            this.Timer.Interval = 1000;
            this.Timer.Elapsed += Timer_Elapsed;
            this.Timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            {
                var ticks = (DateTime.Now - this.startTime).Ticks;
                this.time.Text = new DateTime(ticks).ToString("HH:mm:ss");
            };
            this.BeginInvoke(methodInvokerDelegate);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
