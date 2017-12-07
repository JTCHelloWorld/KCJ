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
    public partial class Form4 : Form
    {
        public Form1 form1 = new Form1();

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            button1.BackColor = form1.OriginalBlockDiagramColor;//原始框图 颜色 
            button3.BackColor = form1.ZoomOutlineBlockDiagramColor;//缩放外框

            button3.BackColor = form1.ZoomOutlineBlockDiagramColor;//缩放外框
            button5.BackColor = form1.ZoomInsideBlockDiagramColor;//缩放内框
            button7.BackColor = form1.CheckedBlockDiagramColor;//选中框图 颜色
            button9.BackColor = form1.YangAngleColor;//阳角颜色
            button11.BackColor = form1.YinAngleColor;//阴角颜色
            button16.BackColor = form1.SlottedColor;//开槽点  选中框图后 开槽点颜色
        }
        /// <summary>
        /// 原始框图颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            form1.colorDialog1.ShowDialog();
            form1.OriginalBlockDiagramColor = form1.colorDialog1.Color;
            button1.BackColor = form1.OriginalBlockDiagramColor;
            String str = ColorTranslator.ToHtml(form1.OriginalBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "OriginalBlockDiagramColor", str, form1.str);
          //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g  != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing(); 
            }
        }
        /// <summary>
        /// 缩放后外框颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            form1.colorDialog2.ShowDialog();
            form1.ZoomOutlineBlockDiagramColor = form1.colorDialog2.Color;
            button3.BackColor = form1.ZoomOutlineBlockDiagramColor;
            String str = ColorTranslator.ToHtml(form1.ZoomOutlineBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "ZoomOutlineBlockDiagramColor", str, form1.str);
            //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }
        /// <summary>
        /// 缩放后内框颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            form1.colorDialog3.ShowDialog();
            form1.ZoomInsideBlockDiagramColor = form1.colorDialog3.Color;
            button5.BackColor = form1.ZoomInsideBlockDiagramColor;
            String str = ColorTranslator.ToHtml(form1.ZoomInsideBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "ZoomInsideBlockDiagramColor", str, form1.str);
            //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }
        /// <summary>
        /// 框图选中颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            form1.colorDialog4.ShowDialog();
            form1.CheckedBlockDiagramColor = form1.colorDialog4.Color;
            button7.BackColor = form1.CheckedBlockDiagramColor;
            String str = ColorTranslator.ToHtml(form1.CheckedBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "CheckedBlockDiagramColor", str, form1.str);
            //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }
        /// <summary>
        /// 阳角颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            form1.colorDialog5.ShowDialog();
            form1.YangAngleColor = form1.colorDialog5.Color;
            button9.BackColor = form1.YangAngleColor;
            String str = ColorTranslator.ToHtml(form1.YangAngleColor);
            Form1.WritePrivateProfileString("ColorSet", "YangAngleColor", str, form1.str);
            //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }
        /// <summary>
        /// 阴角颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            form1.colorDialog6.ShowDialog();
            form1.YinAngleColor = form1.colorDialog6.Color;
            button11.BackColor = form1.YinAngleColor;
            String str = ColorTranslator.ToHtml(form1.YinAngleColor);
            Form1.WritePrivateProfileString("ColorSet", "YinAngleColor", str, form1.str);
            //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }
        /// <summary>
        /// 框图选中开槽点颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            form1.colorDialog7.ShowDialog();
            form1.SlottedColor = form1.colorDialog7.Color;
            button16.BackColor = form1.SlottedColor;
            String str = ColorTranslator.ToHtml(form1.SlottedColor);
            Form1.WritePrivateProfileString("ColorSet", "SlottedColor", str, form1.str);
            //  Console.WriteLine("OriginalBlockDiagramColor {0}", str);
            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }
        /// <summary>
        /// 恢复默认颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            String str = null;

            form1.OriginalBlockDiagramColor = Color.LightSkyBlue;//原始框图 颜色
            button1.BackColor = form1.OriginalBlockDiagramColor;
            str = ColorTranslator.ToHtml(form1.OriginalBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "OriginalBlockDiagramColor", str, form1.str);

            //
            form1.ZoomOutlineBlockDiagramColor = Color.White;//缩放外框
            button3.BackColor = form1.ZoomOutlineBlockDiagramColor;
            str = ColorTranslator.ToHtml(form1.ZoomOutlineBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "ZoomOutlineBlockDiagramColor", str, form1.str);
            //
            form1.ZoomInsideBlockDiagramColor = Color.Red;//缩放内框
            button5.BackColor = form1.ZoomInsideBlockDiagramColor;
            str = ColorTranslator.ToHtml(form1.ZoomInsideBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "ZoomInsideBlockDiagramColor", str, form1.str);
            //
            form1.CheckedBlockDiagramColor = Color.Red;//选中框图 颜色
            button7.BackColor = form1.CheckedBlockDiagramColor;
            str = ColorTranslator.ToHtml(form1.CheckedBlockDiagramColor);
            Form1.WritePrivateProfileString("ColorSet", "CheckedBlockDiagramColor", str, form1.str);
            //
            form1.YangAngleColor = Color.Yellow;//阳角颜色
            button9.BackColor = form1.YangAngleColor;
            str = ColorTranslator.ToHtml(form1.YangAngleColor);
            Form1.WritePrivateProfileString("ColorSet", "YangAngleColor", str, form1.str);
            //
            form1.YinAngleColor = Color.Cyan;//阴角颜色
            button11.BackColor = form1.YinAngleColor;
            str = ColorTranslator.ToHtml(form1.YinAngleColor);
            Form1.WritePrivateProfileString("ColorSet", "YinAngleColor", str, form1.str);
            //
            form1.SlottedColor = Color.Yellow;//开槽点  选中框图后 开槽点颜色
            button16.BackColor = form1.SlottedColor;
            str = ColorTranslator.ToHtml(form1.SlottedColor);
            Form1.WritePrivateProfileString("ColorSet", "SlottedColor", str, form1.str);

            if (form1.g != null)//有框图显示 实时 显示 更改的 颜色
            {
                form1.MoveRefresh = true;
                form1.BlockDiagramDrawing();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
