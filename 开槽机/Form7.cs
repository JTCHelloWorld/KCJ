using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 开槽机
{
    public partial class Form7 : Form
    {
       public Form1 form1 = new Form1();
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(form1.FeedingPulseReference);
        }
    }
}
