using NHDSolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
