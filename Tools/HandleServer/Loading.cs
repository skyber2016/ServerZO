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
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }
        public delegate void InvokeDelegate();
        private void Loading_Load(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    Common.GameServer = new NHDSolution.GameServer();
                    Common.GameServer.Start(); 
                    Common.LoginServer = new NHDSolution.LoginServer();
                    Common.LoginServer.Start();
                    this.BeginInvoke(new InvokeDelegate(() =>
                    {
                        this.Hide();
                    }));
                    new Main().ShowDialog();
                    Environment.Exit(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Environment.Exit(0);
                }
            }).Start();
            
            
        }
    }
}
