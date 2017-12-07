using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;//ini
using System.Drawing.Drawing2D;

namespace 开槽机
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// INI文件读取，初始化配置变量
        /// </summary>
        private void INI_Load()
        {
            #region 判断是否存在INI文件，如果存在就读取 初始配置值
            str = Application.StartupPath + "\\ConFig\\ConFig.ini";
            //此方法也可通过：str = System.AppDomain.CurrentDomain.BaseDirectory + @"ConnectString.ini";

            if (File.Exists(str))
            {
                // 读取串口 默认检测 数量      
                serial_com_count = Convert.ToInt32(ContentReader("SERIAL", "COMCOUNT", ""));
                serialBaudRate = ContentReader("SERIAL", "BaudRate", "");
                
                //  Console.WriteLine("serial_com_count:{0} FilterAngle:{1}", serial_com_count, FilterAngle);  
                //手动送料  送料长度
                ManualFeeding = Convert.ToSingle(ContentReader("FeedingControl", "ManualFeeding", ""));
                textBox3.Text = Convert.ToString(ManualFeeding);
                //自动开槽 重复加工次数
                RepeatProcessingTimes = Convert.ToInt32(ContentReader("SlottedControl", "RepeatProcessingTimes", ""));
                textBox4.Text = Convert.ToString(RepeatProcessingTimes);
                //开槽 参数 
                //材料厚度
                MaterialThickness = Convert.ToSingle(ContentReader("SlotParameters", "MaterialThickness", ""));
                textBox5.Text = Convert.ToString(MaterialThickness);
                //材料剩余厚度
                MaterialRemainingThickness = Convert.ToSingle(ContentReader("SlotParameters", "MaterialRemainingThickness", ""));
                textBox6.Text = Convert.ToString(MaterialRemainingThickness);
                //开槽重复次数
                SlotNumberofRepetitions = Convert.ToInt32(ContentReader("SlotParameters", "SlotNumberofRepetitions", ""));
                textBox7.Text = Convert.ToString(SlotNumberofRepetitions);
                //开槽基准
                //零位校准脉冲数
                ZeroCalibrationPulseNumber = Convert.ToInt32(ContentReader("SlotParameters", "ZeroCalibrationPulseNumber", ""));
                //SlottedBenchmarks = Convert.ToSingle(ContentReader("SlotParameters", "SlottedBenchmarks", ""));
                textBox1.Text = Convert.ToString(Math.Round((float)((float)ZeroCalibrationPulseNumber / (float)ReferencePulseNumber), 4));
                //锯片累计开槽次数
                SawAccumulated = Convert.ToInt32(ContentReader("SlotParameters", "SawAccumulated", ""));
                textBox2.Text = Convert.ToString(SawAccumulated);
                // 过滤角度 读取
                FilterAngle = Convert.ToInt32(ContentReader("SlotParameters", "FilterAngle", ""));
                //首段补偿
                FirstParagraphofCompensation = Convert.ToSingle(ContentReader("SlotParameters", "FirstParagraphofCompensation", ""));
                //末段补偿
                LastParagraphofCompensation = Convert.ToSingle(ContentReader("SlotParameters", "LastParagraphofCompensation", ""));
                //推刀时间
                PushTheKnifeTime = Convert.ToInt32(ContentReader("SlotParameters", "PushTheKnifeTime", ""));
                //送料校准  送料 默认长度
                FeedingCalibrationLength = Convert.ToSingle(ContentReader("SlotParameters", "FeedingCalibrationLength", ""));
               //送料 脉冲基准，单位mm对应的 脉冲数
                FeedingPulseReference = Convert.ToSingle(ContentReader("SlotParameters", "FeedingPulseReference", ""));


                //外轮廓 放大
                OutLineZoomShrink = Convert.ToBoolean(ContentReader("OutLine", "OutLineZoomShrink", ""));
                //外轮廓 缩小
                //OutLineShrink = Convert.ToBoolean(ContentReader("OutLine", "OutLineShrink", ""));
                //外轮廓 缩放参数
                OutLineZoomParameters = Convert.ToSingle(ContentReader("OutLine", "OutLineZoomParameters", ""));

                //内轮廓 放大
                InnerContourZoomShrink = Convert.ToBoolean(ContentReader("InnerContour", "InnerContourZoomShrink", ""));
                
                //内轮廓 缩小
                //InnerContourShrink = Convert.ToBoolean(ContentReader("InnerContour", "InnerContourShrink", ""));
                //内轮廓 缩放参数
                InnerContourZoomParameters = Convert.ToSingle(ContentReader("InnerContour", "InnerContourZoomParameters ", ""));

                //末尾标记 
                EndTagEnable = Convert.ToBoolean(ContentReader("EndTag", "EndTagEnable", ""));
                //距尾段距离
                TailSectionDistance = Convert.ToInt32(ContentReader("EndTag", "TailSectionDistance", ""));
                //末尾标记　材料剩余厚度
                EndTagMaterialRemainingThickness = Convert.ToSingle(ContentReader("EndTag", "EndTagMaterialRemainingThickness", ""));

                //盘料 条料
                //条料
                ArticleTrayMaterial = Convert.ToBoolean(ContentReader("ArticleTray", "ArticleTrayMaterial", ""));
                //盘料
                //TrayMaterial = Convert.ToBoolean(ContentReader("ArticleTray", "TrayMaterial", ""));
                //开槽跨距
                SlottedSpans = Convert.ToSingle(ContentReader("ArticleTray", "SlottedSpans", ""));
                //板材长度
                PlateLength = Convert.ToSingle(ContentReader("ArticleTray", "PlateLength", ""));

                //单轮开槽对刀次数
                //角膜片磨损优化 打开此功能
                SheetLossOptimization = Convert.ToBoolean(ContentReader("SlottedKnifeTimes", "SheetLossOptimization", ""));
                //对刀次数
                NumberofKnife = Convert.ToInt32(ContentReader("SlottedKnifeTimes", "NumberofKnife", ""));

                //内外框 轮廓 识别 模式
                //自动识别
                AutomaticManualIdentification = Convert.ToBoolean(ContentReader("InsideOutsideBoxIdentification", "AutomaticManualIdentification", ""));
                //手动识别
                // ManualIdentification = Convert.ToBoolean(ContentReader("InsideOutsideBoxIdentification", "ManualIdentification", ""));

                //内外轮廓 顺逆时针
                //外轮廓 顺时针  逆时针
                OutLineCW_CCW = Convert.ToBoolean(ContentReader("AntiClockwise", "OutLineCW_CCW", ""));

                //外轮廓 逆时针
                //OutLineCounterclockwise = Convert.ToBoolean(ContentReader("AntiClockwise", "OutLineCounterclockwise", ""));
                //内轮廓 顺时针 逆时针
                InnerCW_CCW = Convert.ToBoolean(ContentReader("AntiClockwise", "InnerCW_CCW", ""));

                //内轮廓 逆时针
                // InnerContourCounterclockwise = Convert.ToBoolean(ContentReader("AntiClockwise", "InnerContourCounterclockwise", ""));

                //框图相关颜色
                //原始框图 颜色 
                EndColorEnable = Convert.ToBoolean(ContentReader("ColorSet", "EndColorEnable", ""));
                OriginalBlockDiagramColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "OriginalBlockDiagramColor", ""));       
                ZoomOutlineBlockDiagramColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "ZoomOutlineBlockDiagramColor", "")); //缩放外框
                ZoomInsideBlockDiagramColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "ZoomInsideBlockDiagramColor", "")); ;//缩放内框
                CheckedBlockDiagramColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "CheckedBlockDiagramColor ", "")); ;//选中框图 颜色
                YangAngleColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "YangAngleColor", "")); ;//阳角颜色
                YinAngleColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "YinAngleColor", "")); ;//阴角颜色
                SlottedColor = ColorTranslator.FromHtml(ContentReader("ColorSet", "SlottedColor", "")); ;//开槽点  选中框图后 开槽点颜色

                //忽略段长  长度
                IgnoreLngth = Convert.ToInt32(ContentReader("IgnoreTheLngth", "IgnoreLngth", ""));
                
            }
            else
            {
                MessageBox.Show("缺少初始化配置文件\n无法打开软件", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            #endregion
        }
        #region  串口 多线程  检测 返回 数据
        /// <summary>
        /// 串口 多线程  检测 返回 数据
        /// </summary>
        /// <param name="Var"></param>
        private void CrossThreadFlush(object Var)
        {
            Int32 i = 0, j = 0, m = 0, n = 0, f = 0;
            byte k = 0, k1 = 0, k2 = 0, k3 = 0, k4 = 0;
            bool Flag = false;
            #region //送料开槽
            if ((DataCommandType)Var == DataCommandType.FeedSlottedCommand)///送料开槽
            {
                k = (byte)DataCommandType.FeedSlottedReturnCommand;//送料开槽 返回
                k1 = (byte)DataCommandType.ChangeBladeReturnCommand;//送料开槽 返回 换刀片 
                k2 = (byte)DataCommandType.USensorAbnormalReturnCommand;//送料开槽 返回 U型传感器异常
                k3 = (byte)DataCommandType.LeftSensorAbnormalReturnCommand;//送料开槽 返回 左传感器异常
                k4 = (byte)DataCommandType.FeedSlottedCommand;//送料开槽

                i = Array.IndexOf(ReDatas, k);
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k);
                    if (CheckDataFlag)
                    {
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        goto ErrE4;
                    }
                }
                timer.Stop();
                j = Array.IndexOf(ReDatas, k1);
                m = Array.IndexOf(ReDatas, k2);
                n = Array.IndexOf(ReDatas, k3);
                f = Array.IndexOf(ReDatas, k4);

                while ((j == -1) && (m == -1) && (n == -1))
                {
                    j = Array.IndexOf(ReDatas, k1);
                    if (j >= 0)
                    {
                        break;
                    }


                    m = Array.IndexOf(ReDatas, k2);
                    if (m >= 0)
                    {
                        break;
                    }
                    n = Array.IndexOf(ReDatas, k3);
                    if (n >= 0)
                    {
                        break;
                    }
                    f = Array.IndexOf(ReDatas, k4);
                    if (f >= 0)
                    {
                        break;
                    }
                }
                if (m > 0)
                {
                    SlottedState = 2;//开槽 状态 异常返回

                    MessageBox.Show("U型传感器异常",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (j > 0)
                {
                    SlottedState = 2;//开槽 状态 异常返回

                    MessageBox.Show("换刀片 ",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (n > 0)
                {
                    SlottedState = 2;//开槽 状态 异常返回

                    MessageBox.Show("左传感器异常",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (f > 0)
                {
                    SlottedState = 1;//开槽 状态 正常返回
                }

                timer.Stop();
                ErrE4:
                return;
            }
            #endregion
            #region //单次开槽
            if ((DataCommandType)Var == DataCommandType.SingleSlotCommand)///单次开槽
            {
                // Console.WriteLine("hello SingleSlotCommand");
                k = (byte)DataCommandType.ChangeBladeReturnCommand;//开槽过程 返回 换刀片 
                k1 = (byte)DataCommandType.USensorAbnormalReturnCommand;//开槽过程 返回 U型传感器异常
                k2 = (byte)DataCommandType.LeftSensorAbnormalReturnCommand;//开槽过程 返回 左传感器异常
                k3 = (byte)DataCommandType.SingleSlotCommand;//开槽过程 返回 单次开槽 

                i = Array.IndexOf(ReDatas, k);
                j = Array.IndexOf(ReDatas, k1);
                m = Array.IndexOf(ReDatas, k2);
                n = Array.IndexOf(ReDatas, k3);
                while ((i == -1) && (j == -1) && (m == -1) && (n == -1))
                {
                    j = Array.IndexOf(ReDatas, k1);
                    if (j >= 0)
                    {
                        break;
                    }
                    i = Array.IndexOf(ReDatas, k);
                    if (i >= 0)
                    {
                        break;
                    }

                    m = Array.IndexOf(ReDatas, k2);
                    if (m >= 0)
                    {
                        break;
                    }
                    n = Array.IndexOf(ReDatas, k3);
                    if (n >= 0)
                    {
                        break;
                    }

                    if (CheckDataFlag)
                    {
                        SlottedState = 2;//开槽 状态 异常返回

                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        goto ErrE4;
                    }

                }
                if (j > 0)
                {
                    SlottedState = 2;//开槽 状态 异常返回
                    MessageBox.Show("U型传感器异常",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (i > 0)
                {
                    SlottedState = 2;//开槽 状态 异常返回
                    MessageBox.Show("换刀片 ",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (m > 0)
                {
                    SlottedState = 2;//开槽 状态 异常返回
                    MessageBox.Show("左传感器异常",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (n > 0)
                {
                    SlottedState = 1;//开槽 状态 正常返回
                }
                timer.Stop();
                ErrE4:
                return;
            }
            #endregion
            #region 角磨机和气缸 动作  试刀命令
            if ((DataCommandType)Var == DataCommandType.TestKnifeCommand)//角磨机和气缸 动作  试刀命令
            {
                k1 = (byte)DataCommandType.TestKnifeCommand;//角磨机和气缸 动作  试刀命令

                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);
                    if (CheckDataFlag)//DebuggingForm
                    {
                        //MessageBox.Show(DebuggingForm,"请检查通信连接和电源\n或串口是否已打开",
                        //    "提示信息 返回信息", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开", "提示信息 返回信息", 
                            MessageBoxButtons.OK,MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        
                        //MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                        //    "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        goto ErrE3;
                    }

                }
                timer.Stop();

                ErrE3:
                return;
            }
            #endregion
            #region 开槽数据
            if ((DataCommandType)Var == DataCommandType.SlottedDataCommand)//开槽数据
            {
                k1 = (byte)DataCommandType.SlottedDataCommand;//开槽数据

                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);
                    if (CheckDataFlag)
                    {
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        goto ErrE3;
                    }

                }
                timer.Stop();

                ErrE3:
                return;
            }
            #endregion
            #region 更换刀片
            if ((DataCommandType)Var == DataCommandType.ChangeBladeCommand)//更换刀片
            {
                k1 = (byte)DataCommandType.ChangeBladeCommand;//到达上限传感器  返回的是 更换刀片 命令

                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);
                    if (CheckDataFlag)
                    {
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        DebuggingForm.button15.Enabled = true;
                        goto ErrE3;
                    }

                }
                timer.Stop();
                DebuggingForm.button15.Enabled = true;

                MessageBox.Show("到达上限传感器",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                ErrE3:
                return;
            }
            #endregion
            #region 丝杠向上
            if ((DataCommandType)Var == DataCommandType.ScrewUpCommand)//丝杠向上
            {
                k1 = (byte)DataCommandType.ReachCeilingCommand;//到达上限传感器

                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);
                    if (CheckDataFlag)
                    {
                        //MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                        //    "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        goto ErrE3;
                    }

                }
                timer.Stop();

                MessageBox.Show("到达上限传感器",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                ErrE3:
                return;
            }
            #endregion
            #region 丝杠向下
            if ((DataCommandType)Var == DataCommandType.ScrewDownCommand)///丝杠向下
            {
                k = (byte)DataCommandType.ReachUTypeCommand;//到达U型传感器
                k1 = (byte)DataCommandType.ReachLowerLimitCommand;//到达下限传感器

                i = Array.IndexOf(ReDatas, k1);
                j = Array.IndexOf(ReDatas, k);
                while ((i == -1) && (j == -1))
                {


                    j = Array.IndexOf(ReDatas, k);
                    if (j >= 0)
                    {
                        break;
                    }
                    i = Array.IndexOf(ReDatas, k1);
                    if (i >= 0)
                    {
                        break;
                    }

                    if (CheckDataFlag)
                    {
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        goto ErrE4;
                    }

                }
                if (j > 0)
                {
                    MessageBox.Show("到达U型传感器",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                if (i > 0)
                {
                    MessageBox.Show("到达下限传感器",
                           "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                timer.Stop();
                ErrE4:
                return;
            }
            #endregion
            #region 传感器 信息 读取
            if ((DataCommandType)Var == DataCommandType.SensorInformationCommand)//传感器 信息 读取
            {
                k1 = (byte)DataCommandType.SensorInformationCommand;
                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);
                    if (CheckDataFlag)
                    {
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                            "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        goto ErrE2;
                    }

                }
                timer.Stop();
                if ((ReDatas[i - 1] == 0xEC) && (ReDatas[i + 1] == 0x02))
                {
                   // Console.WriteLine("信号 {0}", ReDatas[i + 2].ToString("X2"));
                    if ((ReDatas[i + 2] & 0x01) == 0x01)
                    {
                        DebuggingForm.button10.BackColor = Color.Red;//左传感器接通
                    }
                    else
                    {
                        DebuggingForm.button10.BackColor = Color.LawnGreen;//左传感器 没有 接通
                    }
                    if ((ReDatas[i + 2] & 0x02) == 0x02)
                    {
                        DebuggingForm.button9.BackColor = Color.Red;//U型传感器接通
                    }
                    else
                    {
                        DebuggingForm.button9.BackColor = Color.LawnGreen;//U型传感器 没有 接通
                    }
                    if ((ReDatas[i + 2] & 0x04) == 0x04)
                    {
                        DebuggingForm.button7.BackColor = Color.Red;//上传感器接通
                    }
                    else
                    {
                        DebuggingForm.button7.BackColor = Color.LawnGreen;//上传感器 没有 接通
                    }
                    if ((ReDatas[i + 2] & 0x08) == 0x08)
                    {
                        DebuggingForm.button8.BackColor = Color.Red;//下传感器接通
                    }
                    else
                    {
                        DebuggingForm.button8.BackColor = Color.LawnGreen;//下传感器 没有 接通
                    }
                }
                ErrE2:
                return;
            }
            #endregion
            #region 零位校准 返回 脉冲数据 读取
            if ((DataCommandType)Var == DataCommandType.ZeroCalibrationCommand)//零位校准 返回 脉冲数据 读取
            {
                k1 = (byte)DataCommandType.ZeroCalibrationCommand;
                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);
                    if (CheckDataFlag)
                    {
                        MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                        "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        goto ErrE2;
                    }

                }
                timer.Stop();
                if ((ReDatas[i - 1] == 0xEC) && (ReDatas[i + 1] == 0x04))
                {
                    ZeroCalibrationPulseNumber = ReDatas[i + 2] + ReDatas[i + 3] * 256 + ReDatas[i + 4] * 65536 + ReDatas[i + 5] * 16777216;//零位校准脉冲数
                    WritePrivateProfileString("SlotParameters", "ZeroCalibrationPulseNumber", Convert.ToString(ZeroCalibrationPulseNumber), str);
                    textBox1.Text = Convert.ToString(Math.Round((float)((float)ZeroCalibrationPulseNumber / (float)ReferencePulseNumber), 4));
                    //开槽基准  零位校准脉冲数
                    formCommonSettings.textBox14.Text = Convert.ToString(Math.Round((float)((float)ZeroCalibrationPulseNumber / (float)ReferencePulseNumber), 4));
                }
                ErrE2:
                return;
            }
            #endregion
            #region 送料 命令  退料 命令 停止 命令
            {
                switch ((DataCommandType)Var)
                {
                    case DataCommandType.ShippingCostCommand://送料 命令
                        k = (byte)DataCommandType.ShippingCostReturnCommand;
                        k1 = (byte)DataCommandType.ShippingCostCommand;
                        Flag = true;
                        break;
                    case DataCommandType.ReturnMaterialCommand://退料 命令
                        k = (byte)DataCommandType.ReturnMaterialReturnCommand;
                        k1 = (byte)DataCommandType.ReturnMaterialCommand;
                        Flag = true;
                        break;
                    case DataCommandType.StopCostCommand://停止 命令
                        k1 = (byte)DataCommandType.StopCostCommand;
                        Flag = false;
                        break;
                }

                if (Flag)//
                {
                    i = Array.IndexOf(ReDatas, k);//BitConverter.GetBytes(0x81)[0]
                    while (i == -1)
                    {
                        i = Array.IndexOf(ReDatas, k);
                        if (CheckDataFlag)
                        {
                            MessageBox.Show("请检查通信连接和电源\n或串口是否已打开",
                                "提示信息 返回信息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                            goto ErrE;
                        }
                    }
                }
                timer.Stop();
                i = Array.IndexOf(ReDatas, k1);//BitConverter.GetBytes(0x37)[0]
                while (i == -1)
                {
                    i = Array.IndexOf(ReDatas, k1);

                }
                PulseReferenceCalibration = true;//用于 判断 送料 完成 标志位
                ErrE:

                button1.Enabled = true;//送料
                button3.Enabled = true;//退料
                button4.Enabled = false;//停止
            }
            #endregion
            
        }

        #endregion
    }
}
