using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace 开槽机
{
    public partial class Form5 : Form
    {
        public Form1 form1 = new Form1();
        public Int32 Displacement = 0;//上升下降 位移
        public Form5()
        {
            InitializeComponent();
        }


        private void Form5_Load(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(5) ;
            
            button2.Enabled = false; //放开;
            
            button4.Enabled = false;//退刀

            button6.Enabled = false;//关闭

        }
        /// <summary>
        /// 夹紧电磁阀 夹紧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            form1.CylinderClamp();
            button1.Enabled = false;//夹紧
            button2.Enabled = true; //放开;
            //button3.Enabled = false;//推刀
            //button4.Enabled = false;//退刀
            //button5.Enabled = false;//打开
            //button6.Enabled = false;//关闭
            //button11.Enabled = false;//信号刷新
            //button12.Enabled = false;//上升
            //button13.Enabled = false;//下降
            //button14.Enabled = true;//停止
        }
        /// <summary>
        /// 夹紧电磁阀 放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            form1.CylinderClampRelease();//DebugModeCancel();
            button1.Enabled = true;//夹紧
            button2.Enabled = false; //放开;
            //button3.Enabled = true;//推刀
            //button4.Enabled = true;//退刀
            //button5.Enabled = true;//打开
            //button6.Enabled = true;//关闭
            //button11.Enabled = true;//信号刷新
            //button12.Enabled = true;//上升
            //button13.Enabled = true;//下降
            //button14.Enabled = true;//停止
        }
        /// <summary>
        /// 推刀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            form1.PushKnife();
            //button1.Enabled = false;//夹紧
            //button2.Enabled = false; //放开;
            button3.Enabled = false;//推刀
            button4.Enabled = true;//退刀
            //button5.Enabled = false;//打开
            //button6.Enabled = false;//关闭
            //button11.Enabled = false;//信号刷新
            //button12.Enabled = false;//上升
            //button13.Enabled = false;//下降
            //button14.Enabled = true;//停止
        }
        /// <summary>
        /// 退刀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            form1.RetreatKnife();
            //button1.Enabled = true;//夹紧
            //button2.Enabled = true; //放开;
            button3.Enabled = true;//推刀
            button4.Enabled = false;//退刀
            //button5.Enabled = true;//打开
            //button6.Enabled = true;//关闭
            //button11.Enabled = true;//信号刷新
            //button12.Enabled = true;//上升
            //button13.Enabled = true;//下降
            //button14.Enabled = true;//停止
        }

        private void button11_Click(object sender, EventArgs e)
        {
            form1.SensorInformation();
        }
        /// <summary>
        /// 上升下降位移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;//^([1-9]\d*\.\d{2})|(0\.[1-9]0)|(0\.0[1-9])$
            string str = textBox1.Text;
            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入大于0的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "没有小数点及小数位数\n" +
                        "例如:\"0.00 10.123\"为无效数据",
                        "提示信息 上升下降位移", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    Displacement = Convert.ToInt32(textBox1.Text);
                }
            }
        }
        /// <summary>
        /// 上升
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            form1.ScrewUp();
        }
        /// <summary>
        /// 丝杠向下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            form1.ScrewDown();
        }

        /// <summary>
        /// 角磨机 单独运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            form1.SlottedMotorRun();
            //button1.Enabled = false;//夹紧
            //button2.Enabled = false; //放开;
            //button3.Enabled = false;//推刀
            //button4.Enabled = false;//退刀
            button5.Enabled = false;//打开
            button6.Enabled = true;//关闭
            //button11.Enabled = false;//信号刷新
            //button12.Enabled = false;//上升
            //button13.Enabled = false;//下降
            //button14.Enabled = true;//停止
        }
        /// <summary>
        /// 角磨机单独 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            form1.SlottedMotorCancel();
            //button1.Enabled = true;//夹紧
            //button2.Enabled = true; //放开;
            //button3.Enabled = true;//推刀
            //button4.Enabled = true;//退刀
            button5.Enabled = true;//打开
            button6.Enabled = false;//关闭
            //button11.Enabled = true;//信号刷新
            //button12.Enabled = true;//上升
            //button13.Enabled = true;//下降
            //button14.Enabled = true;//停止
        }
        /// <summary>
        /// 停止按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            form1.DebugModeCancel();
            button1.Enabled = true;//夹紧
            button2.Enabled = false; //放开;
            button3.Enabled = true;//推刀
            button4.Enabled = false;//退刀
            button5.Enabled = true;//打开
            button6.Enabled = false;//关闭
            button11.Enabled = true;//信号刷新
            button12.Enabled = true;//上升
            button13.Enabled = true;//下降
            button14.Enabled = true;//停止
        }
        /// <summary>
        /// 关闭窗口  失能所有操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form1.ComDevice.IsOpen)//检测  串口 是否打开 因为串口 没有打开 所以 也没有 所谓 清理，所以不需要显示 提示信息
            {
                form1.DebugModeCancel();
                button1.Enabled = true;//夹紧
                button2.Enabled = false; //放开;
                button3.Enabled = true;//推刀
                button4.Enabled = false;//退刀
                button5.Enabled = true;//打开
                button6.Enabled = false;//关闭
                button11.Enabled = true;//信号刷新
                button12.Enabled = true;//上升
                button13.Enabled = true;//下降
                button14.Enabled = true;//停止
            }
        }
        /// <summary>
        /// 更换锯片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            button15.Enabled = false;
            form1.ChangeBlade();//更换刀片
        }
        /// <summary>
        /// 试刀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button17_Click(object sender, EventArgs e)
        {
            form1.SlottedData();//开槽数据
            form1.TestKnife();//角磨机和气缸 动作  试刀命令
        }
        /// <summary>
        /// 单次开槽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            form1.SlottedData();//开槽数据
            form1.SingleSlot();//单次开槽
        }
    }
}
