using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 开槽机
{
   class SlotManagementCalc
    {
        private Int32 i = 0, j = 0, k = 0, k1 = 0, CountNum = 0, SegmentCountNum = 0;
        public void LengthCalculation(Form1 Var1, List<SlottedTaskInformation> Var2, List<SlotManagementInformation> Var3, Int32 TaskNum,bool Single = false)// Int32 SelectTaskNum,
        {
            if (Var3.Count > 0)//理论上  是检测 SlotManagementChecked 
            {
                if(Single)
                {
                    Var1.SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
                    Var1.SlottedSectionNumSum = 0;//选择加工 总段数
                    Var1.SlottedSelectBoxsNumSum = 0;//选择加工 总字框数
                }
                
                for (i = 0; i < Var3.Count; i++)//选取:字框拾取后的 开槽点，任务级:是 没有选取是的 开槽点
                {
                    k = Var3[i].TaskNum;//具体 哪一个 任务 字框
                    if (Single == false)//单个 任务 计算 
                    {
                        if(k != TaskNum)
                        {
                            continue;
                        }
                        else
                        {
                            Var3[i].DisNumCoordinatePoint.Clear();
                        }
                    }
                    else
                    {
                        if(Var2[k].StartEndSlottedPointCount != 2)
                        {
                            continue;
                        }
                        else if (Var2[k].StartEndSlottedPointCount == 2)
                        {
                            Var3[i].DisNumCoordinatePoint.Clear();
                        }
                    }
                    Var1.SlottedSelectBoxsNumSum++;//选择加工 总字框数
                   
                    Var3[i].SlottedTotalLength = 0;//单个 字框  开槽 选取后 的 总长
                    if (Var2[k].DefaultDirection == Var2[k].SlottedDirection)//顺逆  时针 方向  一致
                    {
                        if (Var2[k].StartSlottedPoint == Var2[k].EndSlottedPoint)//选取 开槽 起点 终点 一样，
                        {
                                //从起始点 到 任务级 最后一个开槽点
                            for (j = 0; j < Var2[k].SlottedNodeInformations.Count; j++)//计算 从第一个任务级开槽点 到 任务级 最后一个开槽点之间的 线段长度
                            {//计算 每2个任务级开槽点之间的距离
                                if (Var2[k].SlottedNodeInformations[j].SlottedSectionPoint >= Var2[k].StartSlottedPoint)//直到 选取 开槽起始点 开始计算
                                {
                                    DisNumCoordinatePoints DisNumCoordinatePointsTH2 = new DisNumCoordinatePoints();

                                    DisNumCoordinatePointsTH2.SlottedLength = Var2[k].SlottedNodeInformations[j].SlottedSectionLength2;
                                    Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH2.SlottedLength;//单个 选取 任务  累计总长
                                    // DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH2);
                                    Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH2);
                                    //Console.WriteLine("当前序号00 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH2.SlottedLength);
                                }
                            }

                              
                                
                            for (j = 0; j < Var2[k].SlottedNodeInformations.Count - 1; j++)
                            {
                                if (Var2[k].SlottedNodeInformations[j].SlottedSectionPoint < Var2[k].EndSlottedPoint)
                                {
                                    DisNumCoordinatePoints DisNumCoordinatePointsTH2 = new DisNumCoordinatePoints();

                                    DisNumCoordinatePointsTH2.SlottedLength = Var2[k].SlottedNodeInformations[j].SlottedSectionLength2;
                                    Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH2.SlottedLength;//单个 选取 任务  累计总长
                                    Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH2);
                                   // Console.WriteLine("当前序号01 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH2.SlottedLength);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (Var2[k].StartSlottedPoint < Var2[k].EndSlottedPoint)////起始点 小于 终点
                            {
                                //从起始点 到 任务级 最后一个开槽点
                                for (j = 0; j < Var2[k].SlottedNodeInformations.Count - 1; j++)//计算 从第一个任务级开槽点 到 任务级 最后一个开槽点之间的 线段长度
                                {//计算 每2个任务级开槽点之间的距离
                                    if ((Var2[k].SlottedNodeInformations[j].SlottedSectionPoint >= Var2[k].StartSlottedPoint) && (Var2[k].SlottedNodeInformations[j].SlottedSectionPoint < Var2[k].EndSlottedPoint))//直到 选取 开槽起始点 开始计算
                                    {
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH2 = new DisNumCoordinatePoints();

                                        DisNumCoordinatePointsTH2.SlottedLength = Var2[k].SlottedNodeInformations[j].SlottedSectionLength2;
                                        Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH2.SlottedLength;//单个 选取 任务  累计总长
                                        Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH2);
                                       // Console.WriteLine("当前序号02 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH2.SlottedLength);
                                    }
                                }
                            }
                            if (Var2[k].StartSlottedPoint > Var2[k].EndSlottedPoint)////起始点 大于 终点
                            {
                                //从起始点 到 任务级 最后一个开槽点
                                for (j = 0; j < Var2[k].SlottedNodeInformations.Count - 1; j++)//计算 从第一个任务级开槽点 到 任务级 最后一个开槽点之间的 线段长度
                                {//计算 每2个任务级开槽点之间的距离
                                    if ((Var2[k].SlottedNodeInformations[j].SlottedSectionPoint >= Var2[k].StartSlottedPoint))//直到 选取 开槽起始点 开始计算
                                    {
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH2 = new DisNumCoordinatePoints();

                                        DisNumCoordinatePointsTH2.SlottedLength = Var2[k].SlottedNodeInformations[j].SlottedSectionLength2;
                                        Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH2.SlottedLength;//单个 选取 任务  累计总长
                                        Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH2);
                                        //Console.WriteLine("当前序号01 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH2.SlottedLength);                                        
                                    }
                                }
                                //任务级 最后 一个 开槽点 
                                        k1 = Var2[k].SlottedNodeInformations.Count - 1;
                                        
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH = new DisNumCoordinatePoints();

                                        DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[k1].SlottedSectionLength2;

                                Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH.SlottedLength;//单个 选取 任务  累计总长
                                Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH);
                              //  Console.WriteLine("当前序号012 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH.SlottedLength);

                                //从起始点 到 任务级 最后一个开槽点
                                for (j = 0; j < Var2[k].SlottedNodeInformations.Count - 1; j++)//计算 从第一个任务级开槽点 到 任务级 最后一个开槽点之间的 线段长度
                                {//计算 每2个任务级开槽点之间的距离
                                    if ((Var2[k].SlottedNodeInformations[j].SlottedSectionPoint < Var2[k].EndSlottedPoint))//直到 选取 开槽起终点 结束
                                    {
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH2 = new DisNumCoordinatePoints();

                                        DisNumCoordinatePointsTH2.SlottedLength = Var2[k].SlottedNodeInformations[j].SlottedSectionLength2;
                                        Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH2.SlottedLength;//单个 选取 任务  累计总长
                                        Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH2);
                                       // Console.WriteLine("当前序号03 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH2.SlottedLength);
                                    }
                                }
                            }
                        }
                    }
                    else//顺逆时针  不一样
                    {
                        if (Var2[k].StartSlottedPoint == Var2[k].EndSlottedPoint)//选取 开槽 起点 终点 一样，
                        {
                            k1 = Var2[k].SlottedNodeInformations.Count - 1;
                            for(j = Var2[k].SlottedNodeInformations.Count; j > 0;j--)
                            {
                                if (Var2[k].SlottedNodeInformations[j - 1].SlottedSectionPoint <= Var2[k].StartSlottedPoint)
                                {
                                    DisNumCoordinatePoints DisNumCoordinatePointsTH = new DisNumCoordinatePoints();
                                    if(j == 1)
                                    {
                                        DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[k1].SlottedSectionLength2;
                                    }
                                    else
                                    {
                                        DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[j - 2].SlottedSectionLength2;
                                    }


                                    Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH.SlottedLength;//单个 选取 任务  累计总长
                                    Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH);
                                    //Console.WriteLine("当前序号#00 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH.SlottedLength);
                                }
                            }

                            for (j = Var2[k].SlottedNodeInformations.Count; j > 0; j--)
                            {
                                if (Var2[k].SlottedNodeInformations[j - 1].SlottedSectionPoint > Var2[k].EndSlottedPoint)
                                {
                                    DisNumCoordinatePoints DisNumCoordinatePointsTH = new DisNumCoordinatePoints();
                                    //if (j == 1)
                                    //{
                                    //    DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[k].SlottedSectionLength2;
                                    //}
                                    //else
                                    {
                                        DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[j - 2].SlottedSectionLength2;
                                    }


                                    Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH.SlottedLength;//单个 选取 任务  累计总长
                                    Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH);
                                   // Console.WriteLine("当前序号#01 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH.SlottedLength);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else//开槽 起点 与 终点 不重合
                        {
                            if (Var2[k].StartSlottedPoint > Var2[k].EndSlottedPoint)////起始点 大于 终点
                            {
                                for (j = Var2[k].SlottedNodeInformations.Count; j > 0; j--)
                                {
                                    if ((Var2[k].SlottedNodeInformations[j - 1].SlottedSectionPoint <= Var2[k].StartSlottedPoint) && (Var2[k].SlottedNodeInformations[j - 1].SlottedSectionPoint > Var2[k].EndSlottedPoint))
                                    {
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH = new DisNumCoordinatePoints();
                                        //if (j == 1)
                                        //{
                                        //    DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[k1].SlottedSectionLength2;
                                        //}
                                        //else
                                        {
                                            DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[j - 2].SlottedSectionLength2;
                                        }


                                        Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH.SlottedLength;//单个 选取 任务  累计总长
                                        Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH);
                                        //Console.WriteLine("当前序号#02 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH.SlottedLength);
                                    }
                                }
                            }

                            
                            if (Var2[k].StartSlottedPoint < Var2[k].EndSlottedPoint)////起始点 小于 终点
                            {
                                k1 = Var2[k].SlottedNodeInformations.Count - 1;
                                for (j = Var2[k].SlottedNodeInformations.Count; j > 0; j--)
                                {
                                    if (Var2[k].SlottedNodeInformations[j - 1].SlottedSectionPoint <= Var2[k].StartSlottedPoint)
                                    {
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH = new DisNumCoordinatePoints();
                                        if (j == 1)
                                        {
                                            DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[k1].SlottedSectionLength2;
                                        }
                                        else
                                        {
                                            DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[j - 2].SlottedSectionLength2;
                                        }
                                        Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH.SlottedLength;//单个 选取 任务  累计总长
                                        Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH);
                                       // Console.WriteLine("当前序号#03 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH.SlottedLength);
                                    }
                                }
                                for (j = Var2[k].SlottedNodeInformations.Count; j > 0; j--)
                                {
                                    if (Var2[k].SlottedNodeInformations[j - 1].SlottedSectionPoint > Var2[k].EndSlottedPoint)
                                    {
                                        DisNumCoordinatePoints DisNumCoordinatePointsTH = new DisNumCoordinatePoints();
                                        //if (j == 1)
                                        //{
                                        //    DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[k].SlottedSectionLength2;
                                        //}
                                        //else
                                        {
                                            DisNumCoordinatePointsTH.SlottedLength = Var2[k].SlottedNodeInformations[j - 2].SlottedSectionLength2;
                                        }


                                        Var3[i].SlottedTotalLength += DisNumCoordinatePointsTH.SlottedLength;//单个 选取 任务  累计总长
                                        Var3[i].DisNumCoordinatePoint.Add(DisNumCoordinatePointsTH);
                                       // Console.WriteLine("当前序号#04 {0} 长度 {1}", Var3[i].DisNumCoordinatePoint.Count, DisNumCoordinatePointsTH.SlottedLength);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }


                  //  Console.WriteLine("任务 {0} {1}总段数{2} 总长度 {3}",k,i, Var3[i].DisNumCoordinatePoint.Count, Var3[i].SlottedTotalLength);
                    Var1.SlottedSectionNumSum += Var3[i].DisNumCoordinatePoint.Count;//选择加工 总段数
                    Var1.SlottedSectionLengthSum += Var3[i].SlottedTotalLength;//开槽选取后  字框  总长 累加
                }
               // Console.WriteLine("总加工长度{0} 总加工段数{1} 总字框数{2}", Var1.SlottedSectionLengthSum, Var1.SlottedSectionNumSum, Var1.SlottedSelectBoxsNumSum);              
            }
        }
        public void ListView_1_Display(Form1 Var1, List<SlotManagementInformation> Var3)
        {
            Var1.listView1.Clear();
            Var1.listView1.Columns.Add("序号", 50, HorizontalAlignment.Center);
            Var1.listView1.Columns.Add("长度(mm)", 60);
            CountNum = 0;
            SegmentCountNum = 0;//段 序号
            if (Var3.Count > 0)//理论上  是检测 SlotManagementChecked 
            {
                for (i = 0; i < Var3.Count; i++)//选取:字框拾取后的 开槽点，任务级:是 没有选取是的 开槽点
                {
                    if (Var3[i].DisNumCoordinatePoint.Count > 0)
                    {
                        ListViewItem var = new ListViewItem();
                        var.Text = "顺序号";
                        var.ForeColor = System.Drawing.Color.Red;
                       // var.Font = new Font("宋体", 9, FontStyle.Strikeout);
                        Var1.listView1.Items.Add(var);
                        Var1.listView1.Items[CountNum].SubItems.Add(Convert.ToString(Var3[i].TaskNum + 1));
                        CountNum++;
                        for (j = 0; j < Var3[i].DisNumCoordinatePoint.Count; j++)
                        {
                            Var1.listView1.Items.Add(Convert.ToString(SegmentCountNum + 1));
                            Var1.listView1.Items[CountNum].SubItems.Add(Convert.ToString(Var3[i].DisNumCoordinatePoint[j].SlottedLength));
                            CountNum++; SegmentCountNum++;
                        }
                        Var1.listView1.Items.Add("总长度");
                        Var1.listView1.Items[CountNum].SubItems.Add(Convert.ToString(Var3[i].SlottedTotalLength));
                        CountNum++;
                    }
                }
            }
        }
        public void ListView_2_Display(Form1 Var1)
        {
            Var1.listView2.Items[0].SubItems[1].Text = Convert.ToString(Var1.ZoomSlottedSectionLengthSum);//路径总长
            Var1.listView2.Items[1].SubItems[1].Text = Convert.ToString(Var1.SlottedSectionLengthSum);//加工总长
            Var1.listView2.Items[2].SubItems[1].Text  = Convert.ToString(Var1.SlottedSectionLengthSum + Var1.SlottedSpans);//加工用料长度
            Var1.listView2.Items[3].SubItems[1].Text = Convert.ToString(Var1.SlottedSelectBoxsNumSum);//加工总字框数
            Var1.listView2.Items[4].SubItems[1].Text = Convert.ToString(Var1.SlottedSectionNumSum);//加工总段数
        }
    }
}
