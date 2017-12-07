using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace 开槽机
{
    public partial class Form3 : Form
    {
        public Form1 form1 = new Form1();
        Form6 PulseCalibrationFORM = null;
        bool ModifyTag = false;//缩放参数  内外轮廓 顺逆时针  过滤角度  忽略段长 都需要重新计算 显示
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox1, "运算包含此数据本身");
            
            if (form1.EndColorEnable)//颜色设置
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }

            if (form1.OutLineZoomShrink)//外轮廓 放大
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }                
            else// 外轮廓 缩小
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
                

            //if (!form1.OutLineZoomShrink) //外轮廓 缩小
            //    radioButton2.Checked = true;
            //else
            //    radioButton2.Checked = false;

            textBox2.Text = Convert.ToString(form1.OutLineZoomParameters);//外轮廓 缩放参数

            if (form1.InnerContourZoomShrink) //内轮廓 放大
            {
                radioButton3.Checked = true;
                radioButton4.Checked = false;
            }
            else//内轮廓 缩小
            {
                radioButton3.Checked = false;
                radioButton4.Checked = true;
            }
 
                

            textBox3.Text = Convert.ToString(form1.InnerContourZoomParameters);//内轮廓 缩放参数
            textBox4.Text = Convert.ToString(form1.FilterAngle);//过滤角度
            textBox5.Text = Convert.ToString(form1.FirstParagraphofCompensation);//首段补偿
            textBox6.Text = Convert.ToString(form1.LastParagraphofCompensation);//末段补偿
            textBox7.Text = Convert.ToString(form1.PushTheKnifeTime);//推刀时间
            //末尾标记 
            if (form1.EndTagEnable)
            {
                checkBox2.Checked = true;
                textBox15.Enabled = true;
                textBox16.Enabled = true;
            }
            else
            {
                checkBox2.Checked = false;
                textBox15.Enabled = false;
                textBox16.Enabled = false;
            }

            textBox15.Text = Convert.ToString(form1.TailSectionDistance);//距尾段距离
            textBox16.Text = Convert.ToString(form1.EndTagMaterialRemainingThickness);//末尾标记　材料剩余厚度

            if (form1.ArticleTrayMaterial)//条料
            {
                radioButton5.Checked = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;

                radioButton6.Checked = false;
            }
            else//盘料
            {
                radioButton5.Checked = false;

                radioButton6.Checked = true;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
            }


            textBox8.Text = Convert.ToString(form1.SlottedSpans); //开槽跨距
            textBox9.Text = Convert.ToString(form1.PlateLength); //板材长度

            //单轮开槽对刀次数
            //角膜片磨损优化 打开此功能
            if(form1.SheetLossOptimization)
            {
                checkBox1.Checked = true;
                textBox10.Enabled = true;
            }
            else
            {
                checkBox1.Checked = false;
                textBox10.Enabled = false;
            }
            textBox10.Text = Convert.ToString(form1.NumberofKnife); //对刀次数
            textBox11.Text = Convert.ToString(form1.SlotNumberofRepetitions); //开槽重复次数
            
            //内外框 轮廓 识别 模式
                //自动识别
            if (form1.AutomaticManualIdentification)
            {
                radioButton7.Checked = true;
                radioButton8.Checked = false;
            }                
            else//手动识别
            {
                radioButton7.Checked = false;
                radioButton8.Checked = true;
            }
                
               

            //内外轮廓 顺逆时针
            //外轮廓 顺时针
            if (form1.OutLineCW_CCW)
            {
                radioButton9.Checked = true;
                radioButton10.Checked = false;
            }
                
            else// 逆时针
            {
                radioButton9.Checked = false;
                radioButton10.Checked = true;
            }
   
                

            // 内轮廓 顺时针
            if (form1.InnerCW_CCW)
            {
                radioButton11.Checked = true;
                radioButton12.Checked = false;
            }
                
            else// 逆时针
            {
                radioButton11.Checked = false;
                radioButton12.Checked = true;
            }
              


            //锯片累计开槽次数
            textBox12.Text = Convert.ToString(form1.SawAccumulated);
            //送料校准  送料 默认长度
            textBox13.Text = Convert.ToString(form1.FeedingCalibrationLength);
            //开槽基准  零位校准脉冲数
            // textBox14.Text = Convert.ToString(form1.SlottedBenchmarks);
            textBox14.Text = Convert.ToString(Math.Round((float)((float)form1.ZeroCalibrationPulseNumber / (float)Form1.ReferencePulseNumber),4));
            
            ////忽略段长  长度
            textBox1.Text = Convert.ToString(form1.IgnoreLngth);
        }
        /// <summary>
        /// 确认推出界面 按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            #region  参数确认
            bool Flag = false,Flag2 = false;
            string str = textBox2.Text;//外轮廓缩放参数 有效性 判断


            Flag = Regex.IsMatch(str, @"^[0-9]$|^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0.0 0.123\"为无效数据",
                    "提示信息 外轮廓缩放参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox3.Text;//内轮廓缩放参数 有效性 判断

            Flag = Regex.IsMatch(str, @"^[0-9]$|^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0.0 0.123\"为无效数据",
                    "提示信息 内轮廓缩放参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox4.Text;//临界角数据 有效性 判断

            Flag = Regex.IsMatch(str, @"^[1][0-7][0-9]$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入3位有效的正数整数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "数据范围 100 - 179\n" +
                    "例如:\"0 0.5 98 182\"为无效数据",
                    "提示信息 临界角数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox5.Text;//首段补偿数据 有效性 判断

            Flag = Regex.IsMatch(str, @"^[0-9]\d*\.?\d{0,2}$");//"^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[0-9]$"

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0.123 10.123\"为无效数据",
                    "提示信息 首段补偿数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox6.Text;//末段补偿数据 有效性 判断

            Flag = Regex.IsMatch(str, @"^[0-9]\d*\.?\d{0,2}$");//"^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[0-9]$"

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0.123 10.123\"为无效数据",
                    "提示信息 末段补偿数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox7.Text;//推刀时间数据 有效性 判断
           
            Flag = Regex.IsMatch(str, @"^[1-9][0-9][0-9][0-9]*$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入有效的正数整数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "数据范围 大于 100\n" +
                    "例如:\"0 0.5 98 \"为无效数据",
                    "提示信息 推刀时间数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }
            

            Flag = false;
            str = textBox15.Text;//距尾段距离数据 有效性 判断
           
            Flag = Regex.IsMatch(str, @"^[1-9][0-9]*$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入大于 1 的正整数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "无小数点的正整数\n" +
                    "例如:\"0 1.2 -5\"为无效数据",
                    "提示信息 距尾段距离数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox16.Text;//末尾标记材料剩余厚度 有效性 判断

            Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,1}$|^0\.[1-9]$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多1位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过1位数\n" +
                    "例如:\"0.0 0.12\"为无效数据",
                    "提示信息 末尾标记材料剩余厚度", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox8.Text;//开槽跨度数据 有效性 判断

            Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0.00 10.123\"为无效数据",
                    "提示信息 开槽跨度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox9.Text;//板材长度数据 有效性 判断

            Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");

            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0.00 10.123\"为无效数据",
                    "提示信息 板材长度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Flag2 = true;
            }

            Flag = false;
            str = textBox14.Text;//零位 数据 有效性 判断

            //Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,4}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");
            Flag = Regex.IsMatch(str, @"^[0-9]\.?\d{0,4}$");
            if (Flag == false)
            {
                Flag2 = false;
                MessageBox.Show("请输入最多4位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n",
                        "提示信息 零位校准 数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                 form1.textBox1.Text = Convert.ToString(Math.Round((float)((float)form1.ZeroCalibrationPulseNumber / (float)Form1.ReferencePulseNumber), 4));
                //开槽基准  零位校准脉冲数
                textBox14.Text = Convert.ToString(Math.Round((float)((float)form1.ZeroCalibrationPulseNumber / (float)Form1.ReferencePulseNumber), 4));
                Flag2 = true;
            }

            #endregion
            if (Flag2)  //检测 一切数据 都在正常范围 退出 设置 窗口
            {
                if(ModifyTag)
                {
                    ModifyTag = false;

                    form1.IntegratedCall();
                    form1.BlockDiagramDrawing();
                }

                this.Close();
            }
                
        }
        /// <summary>
        /// 外轮廓 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                form1.OutLineZoomShrink = false;
                Form1.WritePrivateProfileString("OutLine", "OutLineZoomShrink", "False", form1.str);
                ModifyTag = true;
            }
            //Console.WriteLine("ra2");
        }
        /// <summary>
        /// 外轮廓 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                form1.OutLineZoomShrink = true;
                Form1.WritePrivateProfileString("OutLine", "OutLineZoomShrink", "True", form1.str);

                ModifyTag = true;
            }
            //Console.WriteLine("ra1");
        }
        /// <summary>
        /// 外轮廓 缩放参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox2.Text;

            if (str.Length != 0)
            {//
               // Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,1}$|^0\.?[1-9]?$");
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$");
                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.0 0.123\"为无效数据",
                        "提示信息 外轮廓缩放参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.OutLineZoomParameters = Convert.ToSingle(textBox2.Text);
                    Form1.WritePrivateProfileString("OutLine", "OutLineZoomParameters", textBox2.Text, form1.str);

                    ModifyTag = true;
                }
            }
        }
        /// <summary>
        /// 内轮廓 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                form1.InnerContourZoomShrink = true;
                Form1.WritePrivateProfileString("InnerContour", "InnerContourZoomShrink", "True", form1.str);

                ModifyTag = true;
            }
        }
        /// <summary>
        /// 内轮廓 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                form1.InnerContourZoomShrink = false;
                Form1.WritePrivateProfileString("InnerContour", "InnerContourZoomShrink ", "False", form1.str);

                ModifyTag = true;
            }
        }
        /// <summary>
        /// 内轮廓 缩放参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox3.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.0 0.123\"为无效数据",
                        "提示信息 内轮廓缩放参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.InnerContourZoomParameters = Convert.ToSingle(textBox3.Text);
                    Form1.WritePrivateProfileString("InnerContour", "InnerContourZoomParameters", textBox3.Text, form1.str);

                    ModifyTag = true;
                }
            }
        }
        /// <summary>
        /// 临界角 数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox4.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1][0-7]?[0-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入3位有效的正数整数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "数据范围 100 - 179\n" +
                        "例如:\"0 0.5 98 182\"为无效数据",
                        "提示信息 临界角数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.FilterAngle = Convert.ToInt32(textBox4.Text);
                    Form1.WritePrivateProfileString("SlotParameters", "FilterAngle", textBox4.Text, form1.str);

                    ModifyTag = true;
                }
            }
        }
        /// <summary>
        /// 首段补偿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox5.Text;

            if (str.Length != 0)
            {                       //(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$")
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[0-9]?$|^0\.?[1-9]?[0-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.123 10.123\"为无效数据",
                        "提示信息 首段补偿数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.FirstParagraphofCompensation = Convert.ToSingle(textBox5.Text);
                    Form1.WritePrivateProfileString("SlotParameters", "FirstParagraphofCompensation", textBox5.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 末段补偿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox6.Text;

            if (str.Length != 0)
            {                       //(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$")
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[0-9]?$|^0\.?[1-9]?[0-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.123 10.123\"为无效数据",
                        "提示信息 末段补偿数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.LastParagraphofCompensation = Convert.ToSingle(textBox6.Text);
                    Form1.WritePrivateProfileString("SlotParameters", "LastParagraphofCompensation", textBox6.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 推刀时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox7.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9][0-9]*$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入有效的正数整数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "数据范围 大于 100\n" +
                        "例如:\"0 0.5 98 \"为无效数据",
                        "提示信息 推刀时间数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.PushTheKnifeTime = Convert.ToInt32(textBox7.Text);
                    Form1.WritePrivateProfileString("SlotParameters", "PushTheKnifeTime", textBox7.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 末尾标记 打开此功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                form1.EndTagEnable = true;
                textBox15.Enabled = true;
                textBox16.Enabled = true;

                Form1.WritePrivateProfileString("EndTag", "EndTagEnable", "True", form1.str);
            }
            else
            {
                form1.EndTagEnable = false;
                textBox15.Enabled = false;
                textBox16.Enabled = false;

                Form1.WritePrivateProfileString("EndTag", "EndTagEnable", "False", form1.str);
            }
        }
        /// <summary>
        /// 距尾段距离数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox15.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9][0-9]*$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入大于 1 的正整数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "无小数点的正整数\n" +
                        "例如:\"0 1.2 -5\"为无效数据",
                        "提示信息 距尾段距离数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.TailSectionDistance = Convert.ToInt32(textBox15.Text);
                    Form1.WritePrivateProfileString("EndTag", "TailSectionDistance", textBox15.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 末尾标记 材料 剩余厚度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox16.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,1}$|^0\.?[1-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多1位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过1位数\n" +
                        "例如:\"0.0 0.12\"为无效数据",
                        "提示信息 末尾标记材料剩余厚度", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.EndTagMaterialRemainingThickness = Convert.ToSingle(textBox16.Text);
                    Form1.WritePrivateProfileString("EndTag", "EndTagMaterialRemainingThickness", textBox16.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 条料 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton5.Checked)
            {
                form1.ArticleTrayMaterial = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;

                Form1.WritePrivateProfileString("ArticleTray", "ArticleTrayMaterial", "True", form1.str);
            //}
            //else
            //{
            //    form1.ArticleTrayMaterial = false;

            //    Form1.WritePrivateProfileString("ArticleTray", "ArticleTrayMaterial", "False", form1.str);
            }
        }
        /// <summary>
        /// 盘料 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
            //    form1.ArticleTrayMaterial = true;
            //    textBox8.Enabled = false;
            //    textBox9.Enabled = false;

            //    Form1.WritePrivateProfileString("ArticleTray", "ArticleTrayMaterial", "True", form1.str);
            //}
            //else
            //{
                form1.ArticleTrayMaterial = false;

                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                    Form1.WritePrivateProfileString("ArticleTray", "ArticleTrayMaterial", "False", form1.str);
            }
        }
        /// <summary>
        /// 开槽跨度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox8.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.00 10.123\"为无效数据",
                        "提示信息 开槽跨度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.SlottedSpans = Convert.ToSingle(textBox8.Text);
                    Form1.WritePrivateProfileString("ArticleTray", "SlottedSpans", textBox8.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 板材长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox9.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.00 10.123\"为无效数据",
                        "提示信息 板材长度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.PlateLength = Convert.ToSingle(textBox9.Text);
                    Form1.WritePrivateProfileString("ArticleTray", "PlateLength", textBox9.Text, form1.str);
                }
            }
        }
        /// <summary>
        /// 内外框 轮廓 识别 模式 自动识别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {           
            if(radioButton7.Checked)
            {
                form1.AutomaticManualIdentification = true;
                Form1.WritePrivateProfileString("InsideOutsideBoxIdentification", "AutomaticManualIdentification", "True", form1.str);
            //}
            //else
            //{
            //    form1.AutomaticManualIdentification = false;
            //    Form1.WritePrivateProfileString("InsideOutsideBoxIdentification", "AutomaticManualIdentification, "False", form1.str);
            }
        }
        /// <summary>
        /// 内外框 轮廓 识别 模式 手动识别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
            //    form1.AutomaticManualIdentification = true;
            //    Form1.WritePrivateProfileString("InsideOutsideBoxIdentification", "AutomaticManualIdentification", "True", form1.str);
            //}
            //else
            //{
                form1.AutomaticManualIdentification = false;
                Form1.WritePrivateProfileString("InsideOutsideBoxIdentification", "AutomaticManualIdentification", "False", form1.str);
            }
        }
        /// <summary>
        /// 内外轮廓 顺逆时针 外轮廓 顺时针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                form1.OutLineCW_CCW = true;
                Form1.WritePrivateProfileString("AntiClockwise", "OutLineCW_CCW", "True", form1.str);

                ModifyTag = true;
                //}
                //else
                //{
                //    form1.OutLineCW_CCW = false;
                //    Form1.WritePrivateProfileString("AntiClockwise", "OutLineCW_CCW", "False", form1.str);
            }
        }
        /// <summary>
        /// 内外轮廓 顺逆时针 外轮廓 逆时针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
            //    form1.OutLineCW_CCW = true;
            //    Form1.WritePrivateProfileString("AntiClockwise", "OutLineCW_CCW", "True", form1.str);
            //}
            //else
            //{
                form1.OutLineCW_CCW = false;
                Form1.WritePrivateProfileString("AntiClockwise", "OutLineCW_CCW", "False", form1.str);

                ModifyTag = true;
            }
        }
        /// <summary>
        /// 内外轮廓 顺逆时针 内轮廓 顺时针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked)
            {
                form1.InnerCW_CCW = true;
                Form1.WritePrivateProfileString("AntiClockwise", "InnerCW_CCW", "True", form1.str);

                ModifyTag = true;
                //}
                //else
                //{
                //    form1.InnerCW_CCW = false;
                //    Form1.WritePrivateProfileString("AntiClockwise", "InnerCW_CCW", "False", form1.str);
            }
        }
        /// <summary>
        /// 内外轮廓 顺逆时针 内轮廓 逆时针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked)
            {
            //    form1.InnerCW_CCW = true;
            //    Form1.WritePrivateProfileString("AntiClockwise", "InnerCW_CCW", "True", form1.str);
            //}
            //else
            //{
                form1.InnerCW_CCW = false;
                Form1.WritePrivateProfileString("AntiClockwise", "InnerCW_CCW", "False", form1.str);

                ModifyTag = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 颜色设置 选择 选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                form1.EndColorEnable = true;
                form1.button12.Enabled = form1.EndColorEnable;
                Form1.WritePrivateProfileString("ColorSet", "EndColorEnable", "True", form1.str);
            }
            else
            {
                form1.EndColorEnable = false;
                form1.button12.Enabled = form1.EndColorEnable;
                Form1.WritePrivateProfileString("ColorSet", "EndColorEnable",  "False", form1.str);
            }
        }
        /// <summary>
        /// 忽略段长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            form1.IgnoreLngth = Convert.ToInt32(textBox1.Text);
            if(form1.IgnoreLngth > 1)
            {
                MessageBox.Show("请注意此数字 > 1mm\n" +
                        "可能带来意想不到的结果\n" ,
                        "提示信息 忽略段长数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
                Form1.WritePrivateProfileString("IgnoreTheLngth", "IgnoreLngth", textBox1.Text, form1.str);

            ModifyTag = true;
        }
        /// <summary>
        /// 零位 校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            form1.ZeroCalibration();
            
        }
        /// <summary>
        /// 零位校准 数据  输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox14.Text;

            if (str.Length != 0)
            {
                 //Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$");
                Flag = Regex.IsMatch(str, @"^[0-9]\.?\d{0,4}$");//^[1-9]\d*\.?\d{0,4}$|

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多4位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" ,
                        "提示信息 零位校准 数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    form1.ZeroCalibrationPulseNumber = (Int32)(Convert.ToSingle(textBox14.Text) * Form1.ReferencePulseNumber);
                    Form1.WritePrivateProfileString("SlotParameters", "ZeroCalibrationPulseNumber", Convert.ToString(form1.ZeroCalibrationPulseNumber), form1.str);

                   // form1.textBox1.Text = Convert.ToString(Math.Round((float)((float)form1.ZeroCalibrationPulseNumber / (float)Form1.ReferencePulseNumber), 4));
                    //开槽基准  零位校准脉冲数
                    //textBox14.Text = Convert.ToString(Math.Round((float)((float)form1.ZeroCalibrationPulseNumber / (float)Form1.ReferencePulseNumber), 4));
                }
            }
        }
        /// <summary>
        /// 脉冲当量  校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (form1.ComDevice.IsOpen)
            {
                PulseCalibrationFORM = new Form6();
                PulseCalibrationFORM.form3 = this;
                PulseCalibrationFORM.ShowDialog();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #region 送料校准
        /// <summary>
        /// 送料校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            float num = 0;
             num = Convert.ToSingle(textBox13.Text); ;

            if (num <= 0.0f)
            {
                MessageBox.Show("请输入大于0正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n",
                    "提示信息 送料校准 数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                form1.FeedingCalibrationLength = num;
                Form1.WritePrivateProfileString("SlotParameters", "FeedingCalibrationLength", Convert.ToString(form1.FeedingCalibrationLength), form1.str);
            }
        }
        #endregion
    }
}
