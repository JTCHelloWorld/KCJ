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
    public partial class Form2 : Form
    {
        public Form1 form1 = new Form1();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(form1.serialBaudRate))
            {
                case 9600:
                    cbxBaudRate.SelectedIndex = 0;//默认 9600  
                    break;
                case 115200:
                    cbxBaudRate.SelectedIndex = 1;//  
                    break;
            }
            switch (Convert.ToInt32(form1.serialDataBits))
            {
                case 8:
                    cbxDataBits.SelectedIndex = 0;//默认 8  
                    break;
                case 7:
                    cbxDataBits.SelectedIndex = 1;//  
                    break;
                case 6:
                    cbxDataBits.SelectedIndex = 2;// 
                    break;
                case 5:
                    cbxDataBits.SelectedIndex = 3;//  
                    break;
            }

            cbxParity.SelectedIndex = form1.serialParitySelectedIndex;//默认停止位 1 位

            switch (Convert.ToInt32(form1.serialStopBits))
            {
                case 1:
                    cbxStopBits.SelectedIndex = 0;//停止位 1 
                    break;
                case 2:
                    cbxStopBits.SelectedIndex = 1;//  
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            form1.serialBaudRate = this.cbxBaudRate.SelectedItem.ToString();
            Form1.WritePrivateProfileString("SERIAL", "BaudRate", form1.serialBaudRate,form1.str);
            form1.serialDataBits = this.cbxDataBits.SelectedItem.ToString();
            form1.serialParitySelectedIndex = this.cbxParity.SelectedIndex;
            form1.serialStopBits = this.cbxStopBits.SelectedItem.ToString();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
