using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveMQMonitor
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueueTest newForm = new QueueTest();
            newForm.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TopicTest newForm = new TopicTest();
            newForm.ShowDialog();
        }
    }
}
