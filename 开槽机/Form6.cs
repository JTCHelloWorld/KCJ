using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 开槽机
{
    public partial class Form6 : Form
    {
        public Form3 form3 = new Form3();

        Int32 StateFlag = 0;//状态 0：初始状态，
        public Form6()
        {
            InitializeComponent();
        }

        Thread thread = null;
        private void Form6_Load(object sender, EventArgs e)
        {
            if(StateFlag == 0)
            {
                form3.form1.CylinderClamp();//电磁阀  夹紧
                textBox1.Enabled = false;
                StateFlag = 1;
            }
        }
        /// <summary>
        /// 窗口 关闭前  需要 处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            form3.form1.CylinderClampRelease();//释放  夹紧电磁阀
        }
        /// <summary>
        /// 是  确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            float Num = 0;

            switch (StateFlag)
            {
                case 1:
                    label1.Text = "请稍等送料完毕！";
                    form3.form1.ShippingCost(form3.form1.FeedingCalibrationLength);//送料
                    form3.form1.PulseReferenceCalibration = false;
                    

                    if (thread != null)
                    {
                        thread.Abort();
                    }
                    Control.CheckForIllegalCrossThreadCalls = false;
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush2);
                    thread = new Thread(ParStart);
                    thread.Start();
                    break;
                case 2:
                    label1.Text = "请将记号间的距离填入下列数据框中";
                    textBox1.Enabled = true;
                    button1.Text = "确定(OK)";
                    button2.Text = "取消(NO)";

                    StateFlag = 3;
                    break;
                case 3:
                    Num = Convert.ToSingle(textBox1.Text);
                    form3.form1.FeedingPulseReference = (Num * form3.form1.FeedingPulseReference) / form3.form1.FeedingCalibrationLength;
                    if (thread != null)
                    {
                        thread.Abort();
                    }
                    this.Close();
                    break;
            }
        }
        /// <summary>
        /// 否  取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            form3.form1.CylinderClampRelease();//释放  夹紧电磁阀
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        public void  CrossThreadFlush2(object var)
        {
            while (form3.form1.PulseReferenceCalibration == false)
            {
                ;
            }
            StateFlag = 2;
            form3.form1.CylinderClamp();//电磁阀  夹紧
            label1.Text = "请在材料上做一个标记并单击'是'按钮继续";
            thread.Abort();
        }
    }
}
