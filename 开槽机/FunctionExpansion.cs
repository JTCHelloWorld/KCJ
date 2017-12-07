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
using System.Threading;

namespace 开槽机
{
    public partial class Form1
    {
        #region 线段 显示 计算
        /// <summary>
        /// 线段 显示 计算
        /// </summary>
        private void DisplayCalculation()
        {
            Int32 i = 0, j = 0, k = 0, k2 = 0;
            Int32 SA1 = 0, SA2 = 0, SA3 = 0, SA11 = 0, SA22 = 0;
            float Data = 0.0f;
             float KA1 = 0, KA2 = 0; Int32 Kk2 = 0, Kk3 = 0;



            Data = AmplificationFactor * FlagScale;

            for (i = 0; i < SlottedTaskInformations.Count; i++)
            {
                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j <= SlottedTaskInformations[i].OriginalEndPoint2; j++)
                {
                    PointXY_2_2[j].X = PointXY_2[j].X * Data;
                    PointXY_2_2[j].Y = PointXY_2[j].Y * Data;
                }
            }

                // 显示 缩放后的  框图 开槽点  根据阴阳角 显示 不同的颜色
            for (i = 0; i < SlottedTaskInformations.Count; i++)
            {
                //线段 显示


                // DisplayCalculationInformations[i].UserGraphicsPaths[0].Reset();
                DisplayCalculationInformations[i].UserGraphicsPaths[1].Reset();
                DisplayCalculationInformations[i].UserGraphicsPaths[2].Reset();
                DisplayCalculationInformations[i].UserGraphicsPaths[3].Reset();
                            

                #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 0//显示 基础 字框
                SA1 = SlottedTaskInformations[i].OriginalStartPoint2; SA2 = SlottedTaskInformations[i].OriginalEndPoint2;
                SA3 = SA2 - SA1;

                DisplayCalculationInformations[i].UserGraphicsPaths[1].AddCurve(PointXY_2_2, SA1, SA3, 0);

                if (SlotManagementChecked)//字框 拾取  结束  没有选中的 线段 和 开槽点  浅颜色
                {
                    DisplayCalculationInformations[i].UserPens[1].Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色
                }
                else
                {
                    if (SlottedTaskInformations[i].StartEndSlottedPointCount == 2)
                    {
                        DisplayCalculationInformations[i].UserPens[1].Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色
                    }
                    else
                    {
                        if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.OutsideBox)
                        {
                            DisplayCalculationInformations[i].UserPens[1].Color = ZoomOutlineBlockDiagramColor;
                            DisplayCalculationInformations[i].UserPens[2].Color = ZoomOutlineBlockDiagramColor;
                        }
                        else
                        {
                            DisplayCalculationInformations[i].UserPens[1].Color = ZoomInsideBlockDiagramColor;
                            DisplayCalculationInformations[i].UserPens[2].Color = ZoomInsideBlockDiagramColor;
                        }
                    }
                }

                //  g.DrawPath(DisplayCalculationInformations[i].UserPens[1], DisplayCalculationInformations[i].UserGraphicsPaths[1]);

                #endregion

                if (SlottedTaskInformations[i].DefaultDirection == SlottedTaskInformations[i].SlottedDirection)//顺逆  时针 方向  一致
                {
                    #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 0      没有设置  起始 终点 开槽点  显示 方向箭头
                    if (!SlotManagementChecked)//字框选择 了有有效的 选择 开始箭头 不显示
                    {
                        if (SlottedTaskInformations[i].StartEndSlottedPointCount == 0)
                        {
                            Kk2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                            //箭头 所在直线 只有第一段线段的  一半
                            KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk2 + 1].X - PointXY_2_2[Kk2].X) * 2) / 3;
                            KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk2 + 1].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                        }
                    }

                    #endregion

                    #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 1      设置了 开槽起始点 
                    if (SlottedTaskInformations[i].StartEndSlottedPointCount == 1)
                    {
                        k2 = SlottedTaskInformations[i].SlottedNodeInformations.Count - 1;//当前任务 对象 开槽点 数组 集合 最后一个开槽点的 索引
                        k = SlottedTaskInformations[i].SlottedNodeInformations[k2].SlottedSectionPoint;////当前任务 对象 开槽点 数组 集合 索引 对应的 开槽点在数组中的 下标索引
                        if (SlottedTaskInformations[i].SlottedNodeInformations.Count > 1)//一个点 显示一个 箭头
                        {
                            if (SlottedTaskInformations[i].StartSlottedPoint == k)//当前选中的起始开槽点  是否是 任务 原始起始开槽点
                            {
                                if (SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint == SlottedTaskInformations[i].ZoomStartPoint)
                                {//原始开槽点 是否  = 文件任务起始点
                                    SA1 = k; SA2 = SlottedTaskInformations[i].ZoomEndPoint;
                                    SA3 = SA2 - SA1;
                                    //GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();

                                    DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;
                                    //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                }
                                else
                                {

                                    SA1 = k; SA2 = SlottedTaskInformations[i].ZoomEndPoint;

                                    SA3 = SA2 - SA1;
                                    //  GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();

                                    DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    SA1 = SlottedTaskInformations[i].ZoomStartPoint; SA2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;

                                    SA3 = SA2 - SA1;
                                    //   GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[3] = new GraphicsPath();

                                    DisplayCalculationInformations[i].UserGraphicsPaths[3].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;

                                    //   g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                    //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[3]);
                                }
                            }
                            else
                            {
                                for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count - 1; j++)
                                {
                                    k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;

                                    if (k == SlottedTaskInformations[i].StartSlottedPoint)
                                    {
                                        SA1 = k; SA2 = SlottedTaskInformations[i].SlottedNodeInformations[j + 1].SlottedSectionPoint;
                                        SA3 = SA2 - SA1;
                                        //   GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();

                                        DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                        DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;
                                        //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        //// 方向 箭头 线段
                        DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                        Kk2 = k;
                        //箭头 所在直线 只有第一段线段的  一半
                        KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk2 + 1].X - PointXY_2_2[Kk2].X) * 2) / 3;
                        KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk2 + 1].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                    }
                    #endregion

                    #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 2  设置了 开槽起始点 终点
                    if (SlottedTaskInformations[i].StartEndSlottedPointCount == 2)
                    {
                        if (SlottedTaskInformations[i].StartSlottedPoint == SlottedTaskInformations[i].EndSlottedPoint)
                        {

                            DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;

                            SA1 = SlottedTaskInformations[i].ZoomStartPoint; SA2 = SlottedTaskInformations[i].ZoomEndPoint;
                            SA3 = SA2 - SA1;

                            DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                            //  g.DrawPath(DisplayCalculationInformations[i].UserPens[1], DisplayCalculationInformations[i].UserGraphicsPaths[1]);
                        }
                        if (SlottedTaskInformations[i].StartSlottedPoint < SlottedTaskInformations[i].EndSlottedPoint)//起始点 小于 终点
                        {
                            for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;

                                if (k == SlottedTaskInformations[i].EndSlottedPoint)
                                {
                                    SA2 = k;

                                    SA3 = SA2 - SA1;
                                    //  GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();


                                    DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;
                                    //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                    break;

                                }
                                else if (k == SlottedTaskInformations[i].StartSlottedPoint)
                                {
                                    SA1 = k;
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (SlottedTaskInformations[i].StartSlottedPoint > SlottedTaskInformations[i].EndSlottedPoint)//起始点 大于 终点
                        {
                            for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;


                                if (k == SlottedTaskInformations[i].StartSlottedPoint)
                                {
                                    SA22 = k;

                                    SA1 = SlottedTaskInformations[i].ZoomStartPoint;

                                    SA3 = SA11 - SA1;

                                    //   GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();
                                    if (SA3 > 0)
                                    {
                                        DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);
                                    }

                                    SA2 = SlottedTaskInformations[i].ZoomEndPoint;
                                    SA1 = SA22;
                                    SA3 = SA2 - SA22;
                                    //  GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[3] = new GraphicsPath();

                                    DisplayCalculationInformations[i].UserGraphicsPaths[3].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;
                                    //if (SA3 > 0)
                                    //{
                                    //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                    //}

                                    //g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[3]);
                                    break;
                                }
                                else if (k == SlottedTaskInformations[i].EndSlottedPoint)
                                {
                                    SA11 = k;
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        //// 方向 箭头 线段
                        DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                        Kk2 = SlottedTaskInformations[i].StartSlottedPoint;// k;
                                                                            //箭头 所在直线 只有第一段线段的  一半
                        KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk2 + 1].X - PointXY_2_2[Kk2].X) * 2) / 3;
                        KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk2 + 1].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                    }
                    #endregion


                }
                else//方向不一样
                {
                    #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 0      没有设置  起始 终点 开槽点  显示 方向箭头
                    if (!SlotManagementChecked)//字框选择 了有有效的 选择 开始箭头 不显示
                    {
                        if (SlottedTaskInformations[i].StartEndSlottedPointCount == 0)
                        {   ////显示 箭头
                            if (SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint == SlottedTaskInformations[i].ZoomStartPoint)
                            {
                                Kk2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                Kk3 = SlottedTaskInformations[i].ZoomEndPoint - 1;
                            }
                            else
                            {
                                Kk2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                Kk3 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint - 1;
                            }

                            //箭头 所在直线 只有第一段线段的  一半
                            KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                            KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                        }
                    }

                    #endregion

                    #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 1    设置了 开槽起始点    
                    if (SlottedTaskInformations[i].StartEndSlottedPointCount == 1)
                    {
                        k = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;

                        if (SlottedTaskInformations[i].StartSlottedPoint == k)
                        {
                            if (SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint == SlottedTaskInformations[i].ZoomStartPoint)
                            {
                                k2 = SlottedTaskInformations[i].SlottedNodeInformations.Count - 1;
                                SA1 = SlottedTaskInformations[i].SlottedNodeInformations[k2].SlottedSectionPoint; SA2 = SlottedTaskInformations[i].ZoomEndPoint;
                                SA3 = SA2 - SA1;

                                if (SlottedTaskInformations[i].SlottedNodeInformations.Count > 1)//一个点 显示一个 箭头
                                    DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;

                                ////显示 箭头                                      
                                Kk2 = SlottedTaskInformations[i].ZoomEndPoint;
                                Kk3 = Kk2 - 1;

                                DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                                //箭头 所在直线 只有第一段线段的  一半
                                KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                                KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                            }
                            else
                            {
                                k2 = SlottedTaskInformations[i].SlottedNodeInformations.Count - 1;
                                SA1 = SlottedTaskInformations[i].SlottedNodeInformations[k2].SlottedSectionPoint; SA2 = SlottedTaskInformations[i].ZoomEndPoint;

                                SA3 = SA2 - SA1;
                                if (SlottedTaskInformations[i].SlottedNodeInformations.Count > 1)//一个点 显示一个 箭头
                                    DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);
                                SA1 = SlottedTaskInformations[i].ZoomStartPoint; SA2 = k;

                                SA3 = SA2 - SA1;
                                if (SlottedTaskInformations[i].SlottedNodeInformations.Count > 1)//一个点 显示一个 箭头
                                    DisplayCalculationInformations[i].UserGraphicsPaths[3].AddCurve(PointXY_2_2, SA1, SA3, 0);
                                //  Console.WriteLine("{0} ", SlottedTaskInformations[i].SlottedNodeInformations.Count);
                                DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;

                                ////显示 箭头                                      
                                Kk2 = k;
                                Kk3 = Kk2 - 1;

                                DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                                //箭头 所在直线 只有第一段线段的  一半
                                KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                                KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                            }
                        }
                        else
                        {
                            for (j = 1; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;

                                if (k == SlottedTaskInformations[i].StartSlottedPoint)
                                {
                                    SA1 = SlottedTaskInformations[i].SlottedNodeInformations[j - 1].SlottedSectionPoint; SA2 = k;
                                    SA3 = SA2 - SA1;

                                    if (SlottedTaskInformations[i].SlottedNodeInformations.Count > 1)//一个点 显示一个 箭头
                                        DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;

                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            ////显示 箭头                                      
                            Kk2 = k;
                            Kk3 = Kk2 - 1;

                            DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                            //箭头 所在直线 只有第一段线段的  一半
                            KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                            KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                        }

                    }
                    #endregion

                    #region  //SlottedTaskInformations[i].StartEndSlottedPointCount = 2 设置了 开槽起始点 终点
                    if (SlottedTaskInformations[i].StartEndSlottedPointCount == 2)
                    {
                        if (SlottedTaskInformations[i].StartSlottedPoint == SlottedTaskInformations[i].EndSlottedPoint)
                        {
                            DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;

                            SA1 = SlottedTaskInformations[i].ZoomStartPoint; SA2 = SlottedTaskInformations[i].ZoomEndPoint;
                            SA3 = SA2 - SA1;

                            DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                            ////显示 箭头
                            if (SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint == SlottedTaskInformations[i].StartSlottedPoint)//开槽起点 与 任务级 起点 一致
                            {
                                if (SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint == SlottedTaskInformations[i].ZoomStartPoint)//任务级开槽点 与 任务起点一致
                                {
                                    Kk2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                    Kk3 = SlottedTaskInformations[i].ZoomEndPoint - 1;
                                }
                                else
                                {
                                    Kk2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                    Kk3 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint - 1;
                                }
                            }
                            else
                            {
                                Kk2 = SlottedTaskInformations[i].StartSlottedPoint;
                                Kk3 = SlottedTaskInformations[i].StartSlottedPoint - 1;
                            }





                            DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;//******
                                                                    //箭头 所在直线 只有第一段线段的  一半
                            KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                            KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;

                        }
                        if (SlottedTaskInformations[i].StartSlottedPoint > SlottedTaskInformations[i].EndSlottedPoint)//起始点 大于 终点
                        {
                            for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;

                                if (k == SlottedTaskInformations[i].StartSlottedPoint)
                                {
                                    SA2 = k;

                                    SA3 = SA2 - SA1;
                                    //     GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();


                                    DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;
                                    //  g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                    break;

                                }
                                else if (k == SlottedTaskInformations[i].EndSlottedPoint)
                                {
                                    SA1 = k;
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            ////显示 箭头                                      
                            Kk2 = k;
                            Kk3 = Kk2 - 1;

                            DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                            //箭头 所在直线 只有第一段线段的  一半
                            KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                            KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                        }
                        if (SlottedTaskInformations[i].StartSlottedPoint < SlottedTaskInformations[i].EndSlottedPoint)//起始点 小于 终点
                        {
                            for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;


                                if (k == SlottedTaskInformations[i].EndSlottedPoint)
                                {
                                    SA22 = k;

                                    SA1 = SlottedTaskInformations[i].ZoomStartPoint;

                                    SA3 = SA11 - SA1;
                                    //        GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[2] = new GraphicsPath();

                                    if (SA3 > 0)
                                    {
                                        DisplayCalculationInformations[i].UserGraphicsPaths[2].AddCurve(PointXY_2_2, SA1, SA3, 0);
                                    }

                                    SA2 = SlottedTaskInformations[i].ZoomEndPoint;
                                    SA1 = SA22;
                                    SA3 = SA2 - SA22;
                                    //   GraphicsPath DisplayCalculationInformations[i].UserGraphicsPaths[3] = new GraphicsPath();

                                    DisplayCalculationInformations[i].UserGraphicsPaths[3].AddCurve(PointXY_2_2, SA1, SA3, 0);

                                    DisplayCalculationInformations[i].UserPens[3].Color = CheckedBlockDiagramColor;
                                    //if (SA3 > 0)
                                    //{
                                    //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                                    //}

                                    //    g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[3]);
                                    break;
                                }
                                else if (k == SlottedTaskInformations[i].StartSlottedPoint)
                                {
                                    SA11 = k;
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            ////显示 箭头  
                            if (SlottedTaskInformations[i].StartSlottedPoint == SlottedTaskInformations[i].ZoomStartPoint)
                            {
                                Kk2 = SlottedTaskInformations[i].ZoomEndPoint;
                                Kk3 = Kk2 - 1;
                            }
                            else
                            {
                                Kk2 = SA11;
                                Kk3 = Kk2 - 1;
                            }
                            DisplayCalculationInformations[i].UserPens[2].Color = CheckedBlockDiagramColor;
                            //箭头 所在直线 只有第一段线段的  一半
                            KA1 = PointXY_2_2[Kk2].X + ((PointXY_2_2[Kk3].X - PointXY_2_2[Kk2].X) * 2) / 3;
                            KA2 = PointXY_2_2[Kk2].Y + ((PointXY_2_2[Kk3].Y - PointXY_2_2[Kk2].Y) * 2) / 3;
                        }
                    }
                    #endregion
                }
                DisplayCalculationInformations[i].KA1 = KA1;
                DisplayCalculationInformations[i].KA2 = KA2;
                DisplayCalculationInformations[i].Kk2 = Kk2;
                }

            }
        #endregion
        #region 框图  显示 重绘
        /// <summary>
        /// 框图  显示 重绘
        /// </summary>
        public void BlockDiagramDrawing()//BlockDiagramDrawing
        {
            Int32 i = 0, j = 0,k = 0,k2 = 0;

            float Data = 0.0f;
     
            PointF pointfT = new PointF();
            
  
                Data = AmplificationFactor * FlagScale;
    
                        
            if (g != null)
            {
                g.Clear(Color.Black);
                if (MoveRefresh == false)
                {
                    DisplayCalculation(); //线段 显示 计算
                }
                    
                // 显示 缩放后的  框图 开槽点  根据阴阳角 显示 不同的颜色
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    //线段 显示
                        
                    if (DisplayCalculationInformations[i].UserGraphicsPaths[1].PointCount > 0)
                    {
                        g.DrawPath(DisplayCalculationInformations[i].UserPens[1], DisplayCalculationInformations[i].UserGraphicsPaths[1]);

                    }
                    if (DisplayCalculationInformations[i].UserGraphicsPaths[2].PointCount > 0)
                    {
                        g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[2]);
                    }
                    if (DisplayCalculationInformations[i].UserGraphicsPaths[3].PointCount > 0)
                    {
                        g.DrawPath(DisplayCalculationInformations[i].UserPens[3], DisplayCalculationInformations[i].UserGraphicsPaths[3]);
                    }
                    if ((!SlotManagementChecked) || (SlottedTaskInformations[i].StartEndSlottedPointCount == 2))//字框选择 了有有效的 选择 开始箭头 不显示
                    {
                        // 方向 箭头 显示
                        g.DrawLine(DisplayCalculationInformations[i].UserPens[2], PointXY_2_2[DisplayCalculationInformations[i].Kk2].X, PointXY_2_2[DisplayCalculationInformations[i].Kk2].Y, DisplayCalculationInformations[i].KA1, DisplayCalculationInformations[i].KA2);

                    }

                    ///显示  段号
                    g.ScaleTransform(1, -1);//y轴 方向 调换
                    if (SlottedTaskInformations[i].DefaultDirection == SlottedTaskInformations[i].SlottedDirection)//顺逆  时针 方向  一致
                    {
                        for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                        {
                            if(j == 0)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
                                g.DrawString(Convert.ToString(i + 1), new Font("Arial", 12, FontStyle.Underline | FontStyle.Italic), new SolidBrush(Color.Aqua), new PointF(PointXY_2_2[k].X, -PointXY_2_2[k].Y - 8));
                            }
                            pointfT = SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum;
                            pointfT.X *= Data;
                            pointfT.Y *= Data;
                            g.DrawString(Convert.ToString(j + 1), new Font("Arial", 12), new SolidBrush(Color.Yellow), new PointF(pointfT.X - 6, -pointfT.Y - 8));

                        }
                    }
                    else
                    {
                        for (j = SlottedTaskInformations[i].SlottedNodeInformations.Count; j > 0 ; j--)
                        {
                            if (j == SlottedTaskInformations[i].SlottedNodeInformations.Count)
                            {
                                k = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                g.DrawString(Convert.ToString(i + 1), new Font("Arial", 12, FontStyle.Underline | FontStyle.Italic), new SolidBrush(Color.Aqua), new PointF(PointXY_2_2[k].X, -PointXY_2_2[k].Y - 8));
                            }
                            pointfT = SlottedTaskInformations[i].SlottedNodeInformations[j - 1].PointNum;
                            pointfT.X *= Data;
                            pointfT.Y *= Data;
                            g.DrawString(Convert.ToString(SlottedTaskInformations[i].SlottedNodeInformations.Count - j + 1), new Font("Arial", 12), new SolidBrush(Color.Yellow), new PointF(pointfT.X - 6, -pointfT.Y - 8));

                        }
                    }
                        
                    g.ScaleTransform(1, -1);//y轴 方向 调换
                    //节点显示
                    bool Flag = false;
                    if (AddNode)//增加节点 显示 所有节点
                    {
                        k = 0;
                        for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < SlottedTaskInformations[i].OriginalEndPoint2; j++)
                        {
                            if (k < SlottedTaskInformations[i].SlottedNodeInformations.Count)
                            {
                                k2 = SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionPoint; ;
                                if (j == k2)
                                {
                                    k++;
                                    Flag = true;//continue;
                                }
                            }
                            if (Flag)
                            {
                                if (SlottedTaskInformations[i].SlottedNodeInformations[k - 1].NewDeleteNode)//若是新增 开槽点
                                {
                                    brush.Color = SlottedColor;//选中 增加的 开槽点 
                                }
                                else
                                {
                                    brush.Color = AddDeleteNodeColor;//已是开槽点 了  浅颜色显示  不可以 再 增加了 
                                }
                            }
                            else
                            {
                                brush.Color = YangAngleColor;
                            }

                            Flag = false;
                            RectangleF RectangleF1Slotted = new RectangleF((PointXY_2_2[j].X - (SizeSlottedWidth / 2)), (PointXY_2_2[j].Y - (SizeSlottedHeight / 2)), SizeSlotted.Width, SizeSlotted.Height);

                            g.FillRectangle(brush, RectangleF1Slotted);
                        }
                    }
                    else
                    {
                        for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                        {
                            k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
                            //   Console.WriteLine("开槽点 {0}  {1}  {2}  {3}", i, j, k, PointXY_2_2[k]);
                            RectangleF RectangleF1Slotted = new RectangleF((PointXY_2_2[k].X - (SizeSlottedWidth / 2)), (PointXY_2_2[k].Y - (SizeSlottedHeight / 2)), SizeSlotted.Width, SizeSlotted.Height);
                        //    if (MoveRefresh == false)
                            {
                                if ((AllSelectContour) || (SelectContour) || (SlotManagementChecked))//字框 拾取中 或 结束
                                {
                                   

                                    if (SlottedTaskInformations[i].StartEndSlottedPointCount == 2)
                                    {
                                        if (SlottedTaskInformations[i].StartSlottedPoint == SlottedTaskInformations[i].EndSlottedPoint)
                                        {
                                            brush.Color = SlottedColor;//开槽点  选中框图后 开槽点颜色
                                        }
                                        if (SlottedTaskInformations[i].DefaultDirection == SlottedTaskInformations[i].SlottedDirection)//顺逆  时针 方向  一致
                                        {
                                            if (SlottedTaskInformations[i].StartSlottedPoint < SlottedTaskInformations[i].EndSlottedPoint)//起始点 小于 终点
                                            {
                                                if ((k >= SlottedTaskInformations[i].StartSlottedPoint) && (k <= SlottedTaskInformations[i].EndSlottedPoint))
                                                {
                                                    brush.Color = SlottedColor;//开槽点  选中框图后 开槽点颜色
                                                }
                                                else
                                                {
                                                    brush.Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色
                                                }
                                            }
                                            if (SlottedTaskInformations[i].StartSlottedPoint > SlottedTaskInformations[i].EndSlottedPoint)//起始点 大于 终点
                                            {
                                                if ((k < SlottedTaskInformations[i].StartSlottedPoint) && (k > SlottedTaskInformations[i].EndSlottedPoint))
                                                {
                                                    brush.Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色 
                                                }
                                                else
                                                {
                                                    brush.Color = SlottedColor;//开槽点  选中框图后 开槽点颜色
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (SlottedTaskInformations[i].StartSlottedPoint > SlottedTaskInformations[i].EndSlottedPoint)//起始点 大于 终点
                                            {
                                                if ((k < SlottedTaskInformations[i].StartSlottedPoint) && (k > SlottedTaskInformations[i].EndSlottedPoint))
                                                {
                                                    brush.Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色 
                                                }
                                                else
                                                {
                                                    brush.Color = SlottedColor;//开槽点  选中框图后 开槽点颜色
                                                }
                                            }
                                            if (SlottedTaskInformations[i].StartSlottedPoint < SlottedTaskInformations[i].EndSlottedPoint)//起始点 小于 终点
                                            {
                                                if ((k <= SlottedTaskInformations[i].StartSlottedPoint) || (k >= SlottedTaskInformations[i].EndSlottedPoint))
                                                {
                                                    brush.Color = SlottedColor;//开槽点  选中框图后 开槽点颜色
                                                }
                                                else
                                                {
                                                    brush.Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (SlotManagementChecked)
                                        {
                                            brush.Color = NotSelectColor;//没有被选择的  线段 和 开槽点 颜色
                                        }
                                        else
                                        {
                                            brush.Color = SlottedColor;//开槽点  选中框图后 开槽点颜色
                                        }

                                    }

                                    if (SlotManagementChecked)//放到最后 
                                    {
                                        if (SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode)//删除 开槽点
                                        {
                                            brush.Color = YangAngleColor;//AddDeleteNodeColor;//选中 需要  删除的 开槽点  浅颜色 显示
                                        }
                                        //else
                                        //{
                                        //    if (DeleteNode)//删除 开槽点
                                        //    {
                                        //        brush.Color = YangAngleColor;//删除 开槽点  模式  开槽点 显示  颜色
                                        //    }
                                        //}
                                    }

                                }
                                else
                                {
                                    if (SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode)//删除 开槽点
                                    {
                                        brush.Color = AddDeleteNodeColor;//选中 需要  删除的 开槽点  浅颜色 显示
                                    }
                                    else
                                    {
                                        if (DeleteNode)//删除 开槽点
                                        {
                                            brush.Color = YangAngleColor;//删除 开槽点  模式  开槽点 显示  颜色
                                        }
                                        else
                                        {
                                            if (SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus == CornerType.PositiveAngle)
                                            {
                                                brush.Color = YangAngleColor;
                                            }
                                            else if (SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus == CornerType.NegativeAngle)
                                            {
                                                brush.Color = YinAngleColor;
                                            }
                                        }

                                    }
                                }
                            }
                            g.FillRectangle(brush, RectangleF1Slotted);
                        }
                    }


                }//for end

                pictureBox1.Image = buffer;
                MoveRefresh = false;//*****//移动的时候不需要更行 数据  加快显示数据
            }
        }
        #endregion
        /// <summary>
        /// 顺时针：第3个点在 1，2点线段方向 左边(返回值 大于 0 )，第2个点为 阴角；第3个点在 1，2点线段方向 右边(返回值 小于 0 )，第2个点为 阳角；返回值 为 0 ，3点同一条线
        /// 逆时针：第3个点在 1，2点线段方向 左边(返回值 大于 0 )，第2个点为 阳角；第3个点在 1，2点线段方向 右边(返回值 小于 0 )，第2个点为 阴角；返回值 为 0 ，3点同一条线
        /// 返回值 为 0：3个点 在一条直线上,可利用此功能 去除斜率一样的多余点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        float left_right(PointF a, PointF b, PointF c)
        {
            a.X -= c.X; a.Y -= c.Y;
            b.X -= c.X; b.Y -= c.Y;
            return a.X * b.Y - a.Y * b.X;
        }
        #region  去除冗余的 斜率一样的一条线上的 多余点
        /// <summary>
        /// 去除冗余的 斜率一样的一条线上的 多余点
        /// </summary>
        private void RemoveRedundancyCoordinatePoints()
        {
            Int32 i = 0, j = 0, j2 = 0, k1 = 0, k2 = 0, k3 = 0, k4 = 0, count = 0;
            float s1 = 0.0f;
            bool Flag = false,Flag2 = false;

            coordinatepoints2.Clear();

            count = 0;

            CoordinateHighVerysmall.AbscissaMax_X = 0.0f;
            CoordinateHighVerysmall.AbscissaMin_X = 0.0f;
            CoordinateHighVerysmall.OrdinateMax_Y = 0.0f;
            CoordinateHighVerysmall.OrdinateMin_Y = 0.0f;//横纵 坐标 极值 初始化

            for (i = 0; i < SlottedTaskInformations.Count; )
            {
              //  Console.WriteLine("头 {0} {1}  {2}", i, coordinatepoints[SlottedTaskInformations[i].OriginalStartPoint].PointXY, coordinatepoints[SlottedTaskInformations[i].OriginalEndPoint].PointXY);
                if ((coordinatepoints[SlottedTaskInformations[i].OriginalEndPoint].PointXY != coordinatepoints[SlottedTaskInformations[i].OriginalStartPoint].PointXY))
                {
                    SlottedTaskInformations.RemoveAt(i);
              //      Console.WriteLine("头尾相等 {0} {1}", i, SlottedTaskInformations.Count);
                    continue;
                }

                if (((SlottedTaskInformations[i].OriginalEndPoint - SlottedTaskInformations[i].OriginalStartPoint) <= 2) && (coordinatepoints[SlottedTaskInformations[i].OriginalEndPoint].PointXY == coordinatepoints[SlottedTaskInformations[i].OriginalStartPoint].PointXY))
                {
                    SlottedTaskInformations.RemoveAt(i);
                  //  Console.WriteLine("一条线{0} {1}", i, SlottedTaskInformations.Count);
                    continue;
                }

                Flag = false; Flag2 = false;

                SlottedTaskInformations[i].OriginalStartPoint2 = count;
                j2 = count;
                for (j = SlottedTaskInformations[i].OriginalStartPoint; j <= SlottedTaskInformations[i].OriginalEndPoint - 1; j++)
                {
                    if (j == SlottedTaskInformations[i].OriginalStartPoint)
                    {

                        Flag = true;
                    }

                    if (Flag)
                    {
                        k1 = SlottedTaskInformations[i].OriginalEndPoint - 1;
                        k2 = j;
                        k3 = j + 1;

                        s1 = 0.0f;
                        s1 = (float)Math.Round(left_right(coordinatepoints[k1].PointXY, coordinatepoints[k2].PointXY, coordinatepoints[k3].PointXY),2);
                    //   Console.WriteLine("S11 {0} {1} {2}", i, j, s1,count);
                       
                    //    Console.WriteLine("{0} {1} {2}", coordinatepoints[k1].PointXY, coordinatepoints[k2].PointXY, coordinatepoints[k3].PointXY);
                        if (s1 == 0.0f)
                        {

                            continue;
                        }
                        else
                        {
                            Flag = false;
                            coordinatepoints2.Add(coordinatepoints[k2]);
                            #region
                            PointXY_1[count] = coordinatepoints[k2].PointXY;//用于 显示坐标点

                            if(count == 0)
                            {                              
                                CoordinateHighVerysmall.AbscissaMax_X = PointXY_1[count].X;                              
                                CoordinateHighVerysmall.AbscissaMin_X = PointXY_1[count].X;
                                                                
                                CoordinateHighVerysmall.OrdinateMax_Y = PointXY_1[count].Y;                               
                                CoordinateHighVerysmall.OrdinateMin_Y = PointXY_1[count].Y;
                                
                            }
                            else
                            {
                                if (PointXY_1[count].X > CoordinateHighVerysmall.AbscissaMax_X)//横坐标 极值判断
                                {
                                    CoordinateHighVerysmall.AbscissaMax_X = PointXY_1[count].X;
                                }
                                if (PointXY_1[count].X < CoordinateHighVerysmall.AbscissaMin_X)
                                {
                                    CoordinateHighVerysmall.AbscissaMin_X = PointXY_1[count].X;
                                }

                                if (PointXY_1[count].Y > CoordinateHighVerysmall.OrdinateMax_Y)//纵坐标 极值判断
                                {
                                    CoordinateHighVerysmall.OrdinateMax_Y = PointXY_1[count].Y;
                                }
                                if (PointXY_1[count].Y < CoordinateHighVerysmall.OrdinateMin_Y)
                                {
                                    CoordinateHighVerysmall.OrdinateMin_Y = PointXY_1[count].Y;
                                }
                            }
                            #endregion

                            count++;
                        }
                    }
                    else
                    {
                        if(Flag2)
                        {
                            k1 = k4;
                            Flag2 = false;
                          //  Console.WriteLine("Flag2{0}  {1}", k1,j);
                        }
                        else
                        {
                            k1 = j - 1;
                            k4 = k1;
                        }
                        k2 = j;
                        k3 = j + 1;
                      

                        s1 = 0.0f;
                        s1 = (float)Math.Round(left_right(coordinatepoints[k1].PointXY, coordinatepoints[k2].PointXY, coordinatepoints[k3].PointXY),2);


                       // Console.WriteLine("S12  {0} {1} {2}", i, j, s1);
                        //if ((j >= 833) && (j <= 837))
                        //{
                        //    Console.WriteLine("{0} {1} {2}", coordinatepoints[k1].PointXY, coordinatepoints[k2].PointXY, coordinatepoints[k3].PointXY);
                        //}
                        //  Console.WriteLine("{0} {1} {2}", coordinatepoints[k1].PointXY, coordinatepoints[k2].PointXY, coordinatepoints[k3].PointXY);
                        if (s1 == 0.0f)
                        {
                            Flag2 = true;//此时 上一个点 不变
                           // Console.WriteLine("Flag2{0} {1}", Flag2,j);
                            continue;
                        }
                        else
                        {
                            Flag = false;
                            coordinatepoints2.Add(coordinatepoints[k2]);
                            #region
                            PointXY_1[count] = coordinatepoints[k2].PointXY;//用于 显示坐标点

                            if (PointXY_1[count].X > CoordinateHighVerysmall.AbscissaMax_X)//横坐标 极值判断
                            {
                                CoordinateHighVerysmall.AbscissaMax_X = PointXY_1[count].X;
                            }
                            if (PointXY_1[count].X < CoordinateHighVerysmall.AbscissaMin_X)
                            {
                                CoordinateHighVerysmall.AbscissaMin_X = PointXY_1[count].X;
                            }

                            if (PointXY_1[count].Y > CoordinateHighVerysmall.OrdinateMax_Y)//纵坐标 极值判断
                            {
                                CoordinateHighVerysmall.OrdinateMax_Y = PointXY_1[count].Y;
                            }
                            if (PointXY_1[count].Y < CoordinateHighVerysmall.OrdinateMin_Y)
                            {
                                CoordinateHighVerysmall.OrdinateMin_Y = PointXY_1[count].Y;
                            }
                            #endregion
                            count++;
                        }
                    }
                }
                CoordinatePoints TemporaryPoint = new CoordinatePoints();
                TemporaryPoint.PointXY = coordinatepoints2[j2].PointXY;
                coordinatepoints2.Add(TemporaryPoint);

               // coordinatepoints2.Add(coordinatepoints2[j2]);
                #region
                PointXY_1[count] = coordinatepoints2[j2].PointXY;//用于 显示坐标点

                if (PointXY_1[count].X > CoordinateHighVerysmall.AbscissaMax_X)//横坐标 极值判断
                {
                    CoordinateHighVerysmall.AbscissaMax_X = PointXY_1[count].X;
                }
                if (PointXY_1[count].X < CoordinateHighVerysmall.AbscissaMin_X)
                {
                    CoordinateHighVerysmall.AbscissaMin_X = PointXY_1[count].X;
                }

                if (PointXY_1[count].Y > CoordinateHighVerysmall.OrdinateMax_Y)//纵坐标 极值判断
                {
                    CoordinateHighVerysmall.OrdinateMax_Y = PointXY_1[count].Y;
                }
                if (PointXY_1[count].Y < CoordinateHighVerysmall.OrdinateMin_Y)
                {
                    CoordinateHighVerysmall.OrdinateMin_Y = PointXY_1[count].Y;
                }
                #endregion
                SlottedTaskInformations[i].OriginalEndPoint2 = count;
                count++;

                i++;//中途 会改变 存储的 大小
            }

            // //Console.WriteLine("除去冗余点总任务数1：{0}", SlottedTaskInformations.Count);

            // //for (i = 0; i < SlottedTaskInformations.Count; i++)
            // //{
            // //    Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].OriginalStartPoint2, SlottedTaskInformations[i].OriginalEndPoint2);
            // //    for (j = SlottedTaskInformations[i].OriginalStartPoint2; j <= SlottedTaskInformations[i].OriginalEndPoint2; j++)
            // //    {
            // //        Console.WriteLine("X:{0},Y:{1} {2}", coordinatepoints2[j].PointXY.X, coordinatepoints2[j].PointXY.Y, j);
            // //    }
            // //}
            ////根据最大最小点 移位  XY轴 左右 上下 对称
            float X1 = 0.0f, Y1 = 0.0f;

            // Console.WriteLine("X_MAX:{0} X_MIN:{1} Y_MAX:{2} Y_MIN:{3}", CoordinateHighVerysmall.AbscissaMax_X, CoordinateHighVerysmall.AbscissaMin_X, CoordinateHighVerysmall.OrdinateMax_Y, CoordinateHighVerysmall.OrdinateMin_Y);

            X1 = (CoordinateHighVerysmall.AbscissaMax_X - (CoordinateHighVerysmall.AbscissaDifference / 2.0f));
            Y1 = (CoordinateHighVerysmall.OrdinateMax_Y - (CoordinateHighVerysmall.OrdinateDifference / 2.0f));
            //Console.WriteLine("{0}   {1}", X1, Y1);
            count = 0;
            for (i = 0; i < SlottedTaskInformations.Count; i++)
            {
                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j <= SlottedTaskInformations[i].OriginalEndPoint2; j++)
                {
                    // j = SlottedTaskInformations[i].OriginalStartPoint2;
                    // Console.WriteLine("{0} {1}", j,coordinatepoints2[j].PointXY);
                    coordinatepoints2[j].PointXY.X -= X1;
                    coordinatepoints2[j].PointXY.Y -= Y1;
                    // Console.WriteLine("{0} {1}", j, coordinatepoints2[j].PointXY);

                    PointXY_1[count] = coordinatepoints2[j].PointXY;//用于 显示坐标点

                    if (count == 0)
                    {
                        CoordinateHighVerysmall.AbscissaMax_X = PointXY_1[count].X;
                        CoordinateHighVerysmall.AbscissaMin_X = PointXY_1[count].X;

                        CoordinateHighVerysmall.OrdinateMax_Y = PointXY_1[count].Y;
                        CoordinateHighVerysmall.OrdinateMin_Y = PointXY_1[count].Y;

                    }
                    else
                    {
                        if (PointXY_1[count].X > CoordinateHighVerysmall.AbscissaMax_X)//横坐标 极值判断
                        {
                            CoordinateHighVerysmall.AbscissaMax_X = PointXY_1[count].X;
                        }
                        if (PointXY_1[count].X < CoordinateHighVerysmall.AbscissaMin_X)
                        {
                            CoordinateHighVerysmall.AbscissaMin_X = PointXY_1[count].X;
                        }

                        if (PointXY_1[count].Y > CoordinateHighVerysmall.OrdinateMax_Y)//纵坐标 极值判断
                        {
                            CoordinateHighVerysmall.OrdinateMax_Y = PointXY_1[count].Y;
                        }
                        if (PointXY_1[count].Y < CoordinateHighVerysmall.OrdinateMin_Y)
                        {
                            CoordinateHighVerysmall.OrdinateMin_Y = PointXY_1[count].Y;
                        }
                    }
                    count++;
                }
            }

           //// 原始坐标点，已经除去冗余点
            //Console.WriteLine("除去冗余点总任务数2：{0}", SlottedTaskInformations.Count);

            //for (i = 0; i < SlottedTaskInformations.Count; i++)
            //{
            //    Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].OriginalStartPoint2, SlottedTaskInformations[i].OriginalEndPoint2);
            //    for (j = SlottedTaskInformations[i].OriginalStartPoint2; j <= SlottedTaskInformations[i].OriginalEndPoint2; j++)
            //    {
            //        Console.WriteLine("{0}  {1}", coordinatepoints2[j].PointXY, j);
            //    }
            //}
            //Console.WriteLine("X_MAX:{0} X_MIN:{1} Y_MAX:{2} Y_MIN:{3}", CoordinateHighVerysmall.AbscissaMax_X, CoordinateHighVerysmall.AbscissaMin_X, CoordinateHighVerysmall.OrdinateMax_Y, CoordinateHighVerysmall.OrdinateMin_Y);
        }
        #endregion
        #region 缩放前期准备  数值计算
        /// <summary>
        /// 缩放前期准备  数值计算
        /// </summary>
        private void Deal_DataXY()
        {
            Int32 j = 0;

            //计算dpList
            for (int i = 0; i < SlottedTaskInformations.Count; i++)//任务循环
            {
                //   Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].StartPoint, SlottedTaskInformations[i].EndPoint);
                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < SlottedTaskInformations[i].OriginalEndPoint2; j++)
                {
                    dpListcoordinatepoints[j].PointXY.X = (float)(coordinatepoints2[j == (SlottedTaskInformations[i].OriginalEndPoint2 - 1) ? SlottedTaskInformations[i].OriginalStartPoint2 : (j + 1)].PointXY.X - coordinatepoints2[j].PointXY.X);
                    dpListcoordinatepoints[j].PointXY.Y = (float)(coordinatepoints2[j == (SlottedTaskInformations[i].OriginalEndPoint2 - 1) ? SlottedTaskInformations[i].OriginalStartPoint2 : (j + 1)].PointXY.Y - coordinatepoints2[j].PointXY.Y);

                    //  Console.WriteLine("{0},{1}", dpListcoordinatepoints[j].PointXY.X, dpListcoordinatepoints[j].PointXY.Y);
                }
            }
            //计算ndpList
            for (int i = 0; i < SlottedTaskInformations.Count; i++)//任务循环
            {
                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < SlottedTaskInformations[i].OriginalEndPoint2; j++)
                {
                    ndpListcoordinatepoints[j].PointXY.X = (float)(dpListcoordinatepoints[j].PointXY.X * (1.0f / (float)Math.Sqrt(Math.Pow(dpListcoordinatepoints[j].PointXY.X, 2) + Math.Pow(dpListcoordinatepoints[j].PointXY.Y, 2))));
                    ndpListcoordinatepoints[j].PointXY.Y = (float)(dpListcoordinatepoints[j].PointXY.Y * (1.0f / (float)Math.Sqrt(Math.Pow(dpListcoordinatepoints[j].PointXY.X, 2) + Math.Pow(dpListcoordinatepoints[j].PointXY.Y, 2))));
                    //   Console.WriteLine("{0},{1}", ndpListcoordinatepoints[j].PointXY.X, ndpListcoordinatepoints[j].PointXY.Y);
                }
            }
        }
        #endregion

        #region 缩放计算 根据 内外框 及 顺逆时针 方向 放大 缩小
        /// <summary>
        /// 缩放计算 根据 内外框 及 顺逆时针 方向 放大 缩小
        /// </summary>
        /// <param name="Zoom"></param>
        private void Calculation_Zoom()
        {
            //Console.WriteLine("计算新的顶点");

            PointF vector = new PointF();
            Int32 startIndex = 0, endIndex = 0, count = 0, j = 0, temcount = 0;//temcount :有过处理过程会把无用的点除去(斜率相同的冗余点)
            double sina = 0, length = 0;

            double dist = 0;//正数 放大 负数缩小

            //dist = Zoom;
            temcount = 0;  //********************分内外框的 
            for (int i = 0; i < SlottedTaskInformations.Count; i++)//任务循环
            {
               // Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].OriginalStartPoint, SlottedTaskInformations[i].OriginalEndPoint);

                count = SlottedTaskInformations[i].OriginalEndPoint2;

                SlottedTaskInformations[i].ZoomStartPoint = temcount;
                //Console.WriteLine("{0}  {1} {2}", i, OutLineZoomParameters, InnerContourZoomParameters);
               // Console.WriteLine("{0}  {1}", OutLineZoomShrink, InnerContourZoomShrink);
                if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.OutsideBox)//是否是 外框 外轮廓
                {
                    if((OutLineZoomShrink == true) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Clockwise))//外轮廓 放大
                    {
                        dist = OutLineZoomParameters;
                    }
                    else if ((OutLineZoomShrink == true) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Counterclockwise))//外轮廓 放大
                    {
                        dist = OutLineZoomParameters * -1.0f;
                    }
                    else if ((OutLineZoomShrink == false) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Clockwise))//外轮廓 缩小
                    {
                        dist = OutLineZoomParameters * -1.0f;
                    }
                    else if ((OutLineZoomShrink == false) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Counterclockwise))//外轮廓 缩小
                    {
                        dist = OutLineZoomParameters;
                    }
                    //Console.WriteLine("OutsideBox {0}  {1}", i, dist);
                }
                else if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.InsideBox)//是否是 内框 内轮廓
                {
                    if ((InnerContourZoomShrink == true) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Clockwise))//内轮廓 放大
                    {
                        dist = InnerContourZoomParameters; 
                    }
                    else if ((InnerContourZoomShrink == true) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Counterclockwise))//内轮廓 放大
                    {
                        dist = InnerContourZoomParameters * -1.0f;
                    }
                    else if ((InnerContourZoomShrink == false) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Clockwise))//内轮廓 缩小
                    {
                        dist = InnerContourZoomParameters * -1.0f;
                    }
                    else if ((InnerContourZoomShrink == false) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Counterclockwise))//内轮廓 缩小
                    {
                        dist = InnerContourZoomParameters;
                    }
                    // Console.WriteLine("InsideBox {0}  {1}", i, dist);
                }

                
                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < count; j++)
                {
                    startIndex = (j == SlottedTaskInformations[i].OriginalStartPoint2 ? count - 1 : j - 1);
                    endIndex = j;

                    sina = ndpListcoordinatepoints[startIndex].PointXY.X * ndpListcoordinatepoints[endIndex].PointXY.Y - ndpListcoordinatepoints[startIndex].PointXY.Y * ndpListcoordinatepoints[endIndex].PointXY.X;

                    length = dist / sina;
                    //
                    // Console.WriteLine("sina {0} length {1}  {2}", sina, length,j);

                    if (sina == 0)//会出现 斜率相等的 数据 过滤掉 否则 无法计算
                    {
                        continue;
                    }

                    vector.X = ndpListcoordinatepoints[endIndex].PointXY.X - ndpListcoordinatepoints[startIndex].PointXY.X;
                    vector.Y = ndpListcoordinatepoints[endIndex].PointXY.Y - ndpListcoordinatepoints[startIndex].PointXY.Y;

                    newListcoordinatepoints[temcount].PointXY.X = (float)(coordinatepoints2[j].PointXY.X + vector.X * length);
                    newListcoordinatepoints[temcount].PointXY.Y = (float)(coordinatepoints2[j].PointXY.Y + vector.Y * length);

                    PointXY_2[temcount] = newListcoordinatepoints[temcount].PointXY;//用于 显示坐标点

                    //Console.WriteLine("{0},{1} {2},{3} {4},{5} {6},{7} {8},{9} {10},{11} sina {12} length {13} {14} {15} {16} {17} {18}",
                    //    coordinatepoints[startIndex].PointXY.X, coordinatepoints[startIndex].PointXY.Y,
                    //    coordinatepoints[endIndex].PointXY.X, coordinatepoints[endIndex].PointXY.Y,
                    //    dpListcoordinatepoints[startIndex].PointXY.X, dpListcoordinatepoints[startIndex].PointXY.Y,
                    //    dpListcoordinatepoints[endIndex].PointXY.X, dpListcoordinatepoints[endIndex].PointXY.Y,
                    //    ndpListcoordinatepoints[startIndex].PointXY.X, ndpListcoordinatepoints[startIndex].PointXY.Y,
                    //    ndpListcoordinatepoints[endIndex].PointXY.X, ndpListcoordinatepoints[endIndex].PointXY.Y,
                    //    sina, length,
                    //    newListcoordinatepoints[j].PointXY.X, newListcoordinatepoints[j].PointXY.Y,j,
                    //    startIndex, endIndex);
                    //Console.WriteLine("{0},{1} {2}", newListcoordinatepoints[temcount].PointXY.X, newListcoordinatepoints[temcount].PointXY.Y,j);

                    temcount++;
                }
                newListcoordinatepoints[temcount].PointXY.X = newListcoordinatepoints[SlottedTaskInformations[i].ZoomStartPoint].PointXY.X;
                newListcoordinatepoints[temcount].PointXY.Y = newListcoordinatepoints[SlottedTaskInformations[i].ZoomStartPoint].PointXY.Y;

                PointXY_2[temcount] = newListcoordinatepoints[temcount].PointXY;//用于 显示坐标点Point.Round(

            SlottedTaskInformations[i].ZoomEndPoint = temcount;
                temcount++;

                //Console.WriteLine("{0},{1}", newListcoordinatepoints[SlottedTaskInformations[i].ZoomEndPoint].PointXY.X, newListcoordinatepoints[SlottedTaskInformations[i].ZoomEndPoint].PointXY.Y);
            }

            //Console.WriteLine("新总任务数：{0}", SlottedTaskInformations.Count);

            //for (int i = 0; i < SlottedTaskInformations.Count; i++)
            //{
            //    Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].ZoomStartPoint, SlottedTaskInformations[i].ZoomEndPoint);
            //    for (j = SlottedTaskInformations[i].ZoomStartPoint; j <= SlottedTaskInformations[i].ZoomEndPoint; j++)
            //    {
            //        Console.WriteLine("X:{0},Y:{1} {2}", newListcoordinatepoints[j].PointXY.X, newListcoordinatepoints[j].PointXY.Y, j);
            //    }
            //}
        }
        #endregion
        #region 具体的开槽角度计算，开槽点的计算即对应开槽段长度计算,顺逆时针计算
        /// <summary>
        /// 具体的开槽角度计算，开槽点的计算即对应开槽段长度计算,顺逆时针计算
        /// </summary>
        private void IntegratedOperation()
        {
            Int32 i = 0,j = 0, j2 = 0, count = 0;
            float Length = 0.0f, Length2 = 0.0f;//Length2 :若第一个点不是开槽点，Length2作为第一段的线段长度累计计数暂存变量
            Int32 Anglea = 0, Angleb = 0;
            Int16 Flag = 0;//若第一个点不是开槽点，标记

            float s = 0.0f;//当s为正则顺时针，s为负则逆时针，不分凹凸 
            Int32 k1 = 0, k2 = 0;

            #region  具体的开槽角度计算，开槽点的计算即对应开槽段长度计算
            count = 0;//开槽段数 计数
            Length = 0.0f;
            Length2 = 0.0f;

             for (i = 0; i < SlottedTaskInformations.Count; i++)
            {
                SlottedTaskInformations[i].SlottedNodeInformations.Clear();
                j2 = SlottedTaskInformations[i].OriginalEndPoint2 - 1;
                //Console.WriteLine("j2 {0}",j2);
               // Console.WriteLine("任务 {0}", i);
                Flag = 0; Length2 = 0.0f;
                count = 0;//每一个任务都是从第一个开始的，即初始化 为0;每一个任务记录自己的开槽点及开槽段长，
                //第一个开槽点作为第一个开槽点开始开槽，若第一个点不是开槽点，第一个段并到最后一段
                //每一个任务记录自己的开槽点及开槽段长，不记录最后一个开槽点（即与第一个重叠的点）
                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < SlottedTaskInformations[i].OriginalEndPoint2; j++)
                {

                    if (j == SlottedTaskInformations[i].OriginalStartPoint2)
                    {
                        //Console.WriteLine("j {0} j2 {1} j2X {2} j2Y {3}", j, j2, coordinatepoints2[j2].PointXY.X, coordinatepoints2[j2].PointXY.Y);
                        Anglea = (Int32)((Math.Atan2((coordinatepoints2[j2].PointXY.Y - coordinatepoints2[j].PointXY.Y), (coordinatepoints2[j2].PointXY.X - coordinatepoints2[j].PointXY.X)) * 180.0f) / Math.PI);
                        Angleb = (Int32)((Math.Atan2((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), (coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X)) * 180.0f) / Math.PI);
                      //  Console.WriteLine("1*{0} {1} {2}  {3}", j, Anglea, Angleb, Math.Abs(Angleb - Anglea));
                        if ((Math.Abs(Angleb - Anglea) <= (float)FilterAngle) || (Math.Abs(Angleb - Anglea) >= (360.0f - (float)FilterAngle)))
                        {
                            Length = (float)Math.Round((double)(Math.Sqrt(Math.Pow((coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X), 2) + Math.Pow((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), 2))), 4);

                            SlottedNodeInformation TemporaryNodeInformation = new SlottedNodeInformation();

                            TemporaryNodeInformation.SlottedSectionPoint = j;
                            TemporaryNodeInformation.SlottedSectionLength = Length;

                            SlottedTaskInformations[i].SlottedNodeInformations.Add(TemporaryNodeInformation);
                      //        Console.WriteLine("1*{0} {1}", i, SlottedTaskInformations[i].SlottedNodeInformations.Count);
                        }
                        else
                        {
                            Flag = 1; //第一个不是开槽点
                            Length = (float)Math.Round((double)(Math.Sqrt(Math.Pow((coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X), 2) + Math.Pow((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), 2))), 4);
                            Length2 = Length;
                        }
                    }
                    else
                    {
                        Anglea = (Int32)((Math.Atan2((coordinatepoints2[j - 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), (coordinatepoints2[j - 1].PointXY.X - coordinatepoints2[j].PointXY.X)) * 180.0f) / Math.PI);
                        Angleb = (Int32)((Math.Atan2((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), (coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X)) * 180.0f) / Math.PI);
                      //  Console.WriteLine("2*{0} {1} {2}  {3}", j, Anglea, Angleb, Math.Abs(Angleb - Anglea));
                        if ((Math.Abs(Angleb - Anglea) <= (float)FilterAngle) || (Math.Abs(Angleb - Anglea) >= (360.0f - (float)FilterAngle)))
                        {
                            if (Flag == 1)//第一个点不是开槽点，开槽计数不需要增加
                            {
                                Flag = 2;
                            }
                            else
                            {
                                count++;//开槽点及段 计数 增1 ;若第一个点不是开槽点 不增
                            }


                            Length = (float)Math.Round((double)(Math.Sqrt(Math.Pow((coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X), 2) + Math.Pow((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), 2))), 4);

                            SlottedNodeInformation TemporaryNodeInformation = new SlottedNodeInformation();

                            TemporaryNodeInformation.SlottedSectionPoint = j;
                            TemporaryNodeInformation.SlottedSectionLength = Length;

                            SlottedTaskInformations[i].SlottedNodeInformations.Add(TemporaryNodeInformation);
                       //       Console.WriteLine("2*{0} {1}",i, SlottedTaskInformations[i].SlottedNodeInformations.Count);
                        }
                        else
                        {
                            if (Flag == 1)//第一个不是开槽点 
                            {
                                Length = (float)Math.Round((double)(Math.Sqrt(Math.Pow((coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X), 2) + Math.Pow((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), 2))), 4);
                                Length2 += Length;
                            }
                            else
                            {
                                Length = (float)Math.Round((double)(Math.Sqrt(Math.Pow((coordinatepoints2[j + 1].PointXY.X - coordinatepoints2[j].PointXY.X), 2) + Math.Pow((coordinatepoints2[j + 1].PointXY.Y - coordinatepoints2[j].PointXY.Y), 2))), 4);
                            //    Console.WriteLine("3*{0} {1} {2}", i,count, SlottedTaskInformations[i].SlottedNodeInformations.Count);
                                SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionLength += Length;
                            }
                        }
                    }
                }//for end
                if (Flag == 2)//最后一段 加上起始第一段 开槽长度
                {
                    SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionLength += Length2;
                    Flag = 0;
                }
                if(SlottedTaskInformations[i].SlottedNodeInformations.Count == 0)//没有 开槽点  
                {
                    SlottedNodeInformation TemporaryNodeInformation = new SlottedNodeInformation();

                    TemporaryNodeInformation.SlottedSectionPoint = SlottedTaskInformations[i].OriginalStartPoint2;
                    TemporaryNodeInformation.SlottedSectionLength = Length2;

                    SlottedTaskInformations[i].SlottedNodeInformations.Add(TemporaryNodeInformation);
                }
            }//for end

            for (i = 0; i < SlottedTaskInformations.Count; i++)// 有的段 小于 一定的值 将下一段 段长 合并过来 删除下一个开槽点
            {
                for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                {
                    if(SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength <= IgnoreLngth)
                    {
                        if(j < SlottedTaskInformations[i].SlottedNodeInformations.Count - 1)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength += SlottedTaskInformations[i].SlottedNodeInformations[j + 1].SlottedSectionLength;
                            SlottedTaskInformations[i].SlottedNodeInformations.RemoveAt(j + 1);
                        }
                    }
                }
            }//for end

            // 显示 开槽点对应的坐标下标索引，即对应的开槽段长度 打印
            //Console.WriteLine("开槽点运算后总任务数：{0}", SlottedTaskInformations.Count);

            //for (i = 0; i < SlottedTaskInformations.Count; i++)
            //{
            //    j2 = SlottedTaskInformations[i].SlottedNodeInformations.Count;
            //    Console.WriteLine("{0} {1}", i, j2);
            //    Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint, SlottedTaskInformations[i].SlottedNodeInformations[j2 - 1].SlottedSectionPoint);
            //    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
            //    {
            //        k1 = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
            //        Console.WriteLine("开槽点 {0},段长 {1} {2} {3}", SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint, SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength, coordinatepoints2[k1].PointXY, j);
            //    }
            //}//for end
            #endregion

            //顺时针 逆时针 判断  注意不可以使用 开槽点 来 判断 因为 有的框图没有真正的开槽点 只有一个起始开槽点
            for (i = 0; i < SlottedTaskInformations.Count; i++)//任务数
            {
                s = 0.0f;//每个任务需要 初始化

                for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < SlottedTaskInformations[i].OriginalEndPoint2; j++)
                {
                    //Console.WriteLine("X:{0},Y:{1} {2}", coordinatepoints2[j].PointXY.X, coordinatepoints2[j].PointXY.Y, j);
                    k1 = j;

                    if (j == (SlottedTaskInformations[i].OriginalEndPoint2 - 1))//开槽点坐标没有记录最后一个开槽点(与第一个开槽点坐标一样)
                    {
                        k2 = SlottedTaskInformations[i].OriginalStartPoint2;
                    }
                    else
                    {
                        k2 = j + 1;
                    }

                    s += (coordinatepoints2[k2].PointXY.X - coordinatepoints2[k1].PointXY.X) * (coordinatepoints2[k2].PointXY.Y + coordinatepoints2[k1].PointXY.Y) * 0.5f;
                }//for end

                if (s > 0.0f)
                {
                    SlottedTaskInformations[i].DefaultDirection = DirectionType.Clockwise;//顺时针
                }
                else if (s < 0.0f)
                {
                    SlottedTaskInformations[i].DefaultDirection = DirectionType.Counterclockwise;//逆时针
                }
              //  Console.WriteLine("任务：{0} 方向{1} {2} ", i,s, SlottedTaskInformations[i].DefaultDirection);
            }//for end       
        }
        #endregion
        #region 内外框 识别
        /// <summary>
        /// 内外框 识别
        /// </summary>
        private void InsideOutsideBoxIdentification()
        {
            Int32 i = 0, i2 = 0, j = 0, j2 = 0, j3 = 0;
            Int32 k1 = 0, k2 = 0;
            Int32 ContainPolygons = 0;//此任务框图 里面 包含 有多少个 框图
            Int32 FramePolygons = 0;//此任务框图 外面 有多少个 框图 包含此 框图

            bool c = false;

            //内外框  判断  c:true 点在多边形内；；false:点在多边形外
            for (i2 = 0; i2 < SlottedTaskInformations.Count; i2++)//内外框  判断 
            {
                j2 = SlottedTaskInformations[i2].SlottedNodeInformations[0].SlottedSectionPoint;
                ContainPolygons = 0;
                FramePolygons = 0;
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    if (i == i2)//同一个任务  跳过 
                    {
                        continue;
                    }
                    c = false;
                  
                    for (j = SlottedTaskInformations[i].OriginalStartPoint2,j3 = (SlottedTaskInformations[i].OriginalEndPoint2 - 1); j < SlottedTaskInformations[i].OriginalEndPoint2; j3 = j++)
                    {
                    //Console.WriteLine("X:{0},Y:{1} {2}", coordinatepoints2[j].PointXY.X, coordinatepoints2[j].PointXY.Y, j);
                        k1 = j;//SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
                        k2 = j3;// SlottedTaskInformations[i].SlottedNodeInformations[j3].SlottedSectionPoint; 

                        if (((coordinatepoints2[k1].PointXY.Y > coordinatepoints2[j2].PointXY.Y) != (coordinatepoints2[k2].PointXY.Y > coordinatepoints2[j2].PointXY.Y)) &&
                            (coordinatepoints2[j2].PointXY.X < (coordinatepoints2[k2].PointXY.X - coordinatepoints2[k1].PointXY.X) *
                            (coordinatepoints2[j2].PointXY.Y - coordinatepoints2[k1].PointXY.Y) /
                            (coordinatepoints2[k2].PointXY.Y - coordinatepoints2[k1].PointXY.Y) + coordinatepoints2[k1].PointXY.X))
                        {
                            c = !c;
                        }
                    }//for end


                    if (c)
                    {
                        FramePolygons++;//此任务框图 外面 有多少个 框图 包含此 框图
                    }
                    else
                    {
                        ContainPolygons++;//此任务框图 里面 包含 有多少个 框图
                    }

                  //  Console.WriteLine("任务 {0} {1} {2}",i2,i,c);
                }//for end

                if((FramePolygons % 2) == 1)
                {
                    SlottedTaskInformations[i2].InsideoutsideBox = InsideoutsideBoxType.InsideBox;
                   
                    if (InnerCW_CCW)//顺时针
                    {
                        SlottedTaskInformations[i2].SlottedDirection = DirectionType.Clockwise;
                    }
                    else
                    {
                        SlottedTaskInformations[i2].SlottedDirection = DirectionType.Counterclockwise;
                    }
                }
                else if ((FramePolygons % 2) == 0)
                {
                    SlottedTaskInformations[i2].InsideoutsideBox = InsideoutsideBoxType.OutsideBox;
                    if (OutLineCW_CCW)//顺时针
                    {
                        SlottedTaskInformations[i2].SlottedDirection = DirectionType.Clockwise;
                    }
                    else
                    {
                        SlottedTaskInformations[i2].SlottedDirection = DirectionType.Counterclockwise;
                    }

                }
              //  Console.WriteLine("任务* {0} {1} {2} {3} {4}", i2, SlottedTaskInformations[i2].InsideoutsideBox, OutLineCW_CCW, InnerCW_CCW,SlottedTaskInformations[i2].SlottedDirection);
            }//for end

        }
        #endregion
        #region 内外 框  轮廓  阴阳角识别
        /// <summary>
        /// 内外 框  轮廓  阴阳角识别
        /// </summary>
        private void  SlottedCornerYinYangStatus()
        {
            Int32 i = 0 ,j = 0;
            float s1 = 0.0f;//
            Int32 k1 = 0, k2 = 0, k3 = 0, k4 = 0, count = 0;

            //阴阳角 判断  分 内外框的
            for (i = 0; i < SlottedTaskInformations.Count; i++)//任务数
            {
                if(SlottedTaskInformations[i].SlottedNodeInformations.Count <= 3)
                {
                    if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.OutsideBox)
                    {
                        for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus = CornerType.PositiveAngle;
                        }
                    }
                    else if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.InsideBox)
                    {
                        for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus = CornerType.NegativeAngle;
                        }
                    }
                }
                else
                {
                    //起始点与开槽第一个点 是否  一样  //计算的时候 是从第二个点 开始 计算的
                    if (SlottedTaskInformations[i].OriginalStartPoint2 == SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint)
                    {
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }
                  //  Console.WriteLine("count {0} Flag {1}",count,Flag );

                    for (j = SlottedTaskInformations[i].OriginalStartPoint2; j < SlottedTaskInformations[i].OriginalEndPoint2; j++)
                    {
                        //Console.WriteLine("X:{0},Y:{1} {2}", coordinatepoints2[j].PointXY.X, coordinatepoints2[j].PointXY.Y, j);
                        if (j == (SlottedTaskInformations[i].OriginalEndPoint2 - 2))
                        {
                            k1 = j;
                            k2 = j + 1;
                            k3 = SlottedTaskInformations[i].OriginalStartPoint2;

                            k4 = j + 1;
                            
                        }
                        else if (j == (SlottedTaskInformations[i].OriginalEndPoint2 - 1))
                        {
                            k1 = j;
                            k2 = SlottedTaskInformations[i].OriginalStartPoint2;
                            k3 = SlottedTaskInformations[i].OriginalStartPoint2 + 1;

                            k4 = SlottedTaskInformations[i].OriginalStartPoint2;
                        }
                        else
                        {
                            k1 = j;
                            k2 = j + 1;
                            k3 = j + 2;

                            k4 = j + 1;
                        }
                        s1 = 0.0f;
                        s1 = left_right(coordinatepoints2[k1].PointXY, coordinatepoints2[k2].PointXY, coordinatepoints2[k3].PointXY);
                     //    Console.WriteLine("{0} {1} {2} {3} 值 {4} {5} {6} {7}",i, coordinatepoints2[k1].PointXY, coordinatepoints2[k2].PointXY, coordinatepoints2[k3].PointXY,s1,k4,count, SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionPoint);
                        if (((s1 > 0.0f) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Counterclockwise)) || ((s1 < 0.0f) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Clockwise)))
                        {//逆时针 大于 0，及 顺时针 小于 0，阳角 **外框是这样 内框 正好 相反**
                            if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.OutsideBox)
                            {
                                if(k4 == SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionPoint)
                                {
                                    SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedCornerStatus = CornerType.PositiveAngle;
                                //    Console.WriteLine("任务K1");
                                    if (count == (SlottedTaskInformations[i].SlottedNodeInformations.Count - 1))
                                    {
                                        count = 0;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                    
                                }                               
                            }
                            else if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.InsideBox)
                            {
                                if (k4 == SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionPoint)
                                {
                                    SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedCornerStatus = CornerType.NegativeAngle;
                                  //  Console.WriteLine("任务K2");
                                    if (count == (SlottedTaskInformations[i].SlottedNodeInformations.Count - 1))
                                    {
                                        count = 0;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }                                
                            }
                        }
                        else if (((s1 < 0.0f) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Counterclockwise)) || ((s1 > 0.0f) && (SlottedTaskInformations[i].DefaultDirection == DirectionType.Clockwise)))
                        {//逆时针 小于 0，及 顺时针 大于 0，阴角 **外框是这样 内框 正好 相反**
                            if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.OutsideBox)
                            {
                                if (k4 == SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionPoint)
                                {
                                    SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedCornerStatus = CornerType.NegativeAngle;
                                   // Console.WriteLine("任务K3");
                                    if (count == (SlottedTaskInformations[i].SlottedNodeInformations.Count - 1))
                                    {
                                        count = 0;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }
                            }
                            else if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.InsideBox)
                            {
                                if (k4 == SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedSectionPoint)
                                {
                                    SlottedTaskInformations[i].SlottedNodeInformations[count].SlottedCornerStatus = CornerType.PositiveAngle;

                                    //Console.WriteLine("任务K4");
                                    if (count == (SlottedTaskInformations[i].SlottedNodeInformations.Count - 1))
                                    {
                                        count = 0;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                       // Console.WriteLine("任务K：{0} 框方向 {1} 值 {2} 阴阳角 {3}", i, SlottedTaskInformations[i].DefaultDirection, s1, SlottedTaskInformations[i].SlottedNodeInformations[count - 1].SlottedCornerStatus);

                    }
                }
            }
            ////阴阳角 打印
            //for (i = 0; i < SlottedTaskInformations.Count; i++)
            //{
            //    // j2 = SlottedTaskInformations.Count - 1;
            //    // Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint, SlottedTaskInformations[i].SlottedNodeInformations[j2 - 1].SlottedSectionPoint);
            //    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
            //    {
            //        Console.WriteLine("任务：{0} 框方向 {1} 阴阳角 {2}", i, SlottedTaskInformations[i].DefaultDirection,
            //SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus);
            //    }
            //}
        }
#endregion
        #region 集成调用 在 参数发生变化后 可以实时更改数据 及 显示
        /// <summary>
        /// 集成调用 在 参数发生变化后 可以实时更改数据 及 显示
        /// </summary>
        public void IntegratedCall()
        {
            AllSelectContour = false;//全部拾取字框 
            SelectContour = false;//拾取字框 
            AddNode = false;//增加节点 
            DeleteNode = false;//删除节点
            拾取字框toolStripMenuItem.Checked = false;
            增加节点ToolStripMenuItem.Checked = false;
            删除节点ToolStripMenuItem.Checked = false;
            拾取全部ToolStripMenuItem.Checked = false;

            SlotManagementChecked = false;
            SlotManagementInformations.Clear();

            ZoomSlottedSectionLengthSum = 0;//缩放后  字框  总长
            SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
            SlottedSectionNumSum = 0;//选择加工 总段数
            SlottedSelectBoxsNumSum = 0;//选择加工 总字框数

            SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
            SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
            for (int i = 0; i < SlottedTaskInformations.Count; i++)
            {
                SlottedTaskInformations[i].StartEndSlottedPointCount = 0;
            }
            SelectContourCount = 0;//每次进入 字框拾取 时 拾取计数 用于撤销 和取消


            IntegratedOperation();//具体的开槽角度计算，开槽点的计算即对应开槽段长度计算,顺逆时针计算

            InsideOutsideBoxIdentification();//内外框 识别
            SlottedCornerYinYangStatus();//内外 框  轮廓  阴阳角识别

            Deal_DataXY();//坐标缩放 预处理
            Calculation_Zoom();//缩放处理
            ZoomLengthCalculation();//缩放后 每一段 长度的计算

            //listView2.Items[0].SubItems[1].Text = Convert.ToString(ZoomSlottedSectionLengthSum);

            SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
        }
        #endregion

        #region  缩放后的 数据存储 转换 
        /// <summary>
        /// 缩放后的 数据存储 转换 
        /// </summary>
        private void ConvertStorage()
        {
            DisplayCalculationInformations.Clear();
            for (int i = 0; i < SlottedTaskInformations.Count; i++)//根据实际任务数，初始化线段显示 计算信息
            {
                DisplayCalculationInformation DisplayCalculationInformation_TH = new DisplayCalculationInformation();

                Pen UserPen1 = new Pen(Color.Red);
                Pen UserPen2 = new Pen(Color.Red);
                Pen UserPen3 = new Pen(Color.Red);
                Pen UserPen4 = new Pen(Color.Red);

                UserPen3.CustomEndCap = myArrow;

                GraphicsPath gp1 = new GraphicsPath();
                GraphicsPath gp2 = new GraphicsPath();
                GraphicsPath gp3 = new GraphicsPath();
                GraphicsPath gp4 = new GraphicsPath();

                DisplayCalculationInformation_TH.UserPens.Add(UserPen1);
                DisplayCalculationInformation_TH.UserPens.Add(UserPen2);
                DisplayCalculationInformation_TH.UserPens.Add(UserPen3);
                DisplayCalculationInformation_TH.UserPens.Add(UserPen4);
                DisplayCalculationInformation_TH.UserGraphicsPaths.Add(gp1);
                DisplayCalculationInformation_TH.UserGraphicsPaths.Add(gp2);
                DisplayCalculationInformation_TH.UserGraphicsPaths.Add(gp3);
                DisplayCalculationInformation_TH.UserGraphicsPaths.Add(gp4);
                DisplayCalculationInformations.Add(DisplayCalculationInformation_TH);
            }
        }
        #endregion
        /// <summary>
        /// 缩放后 每一段 长度的计算
        /// </summary>
        private void ZoomLengthCalculation()
        {
            Int32 i = 0, j = 0,j2 = 0,k = 0,k2 = 0;

            #region  
            ZoomSlottedSectionLengthSum = 0;//缩放后  字框  总长
            for (i = 0; i < SlottedTaskInformations.Count; i++)
            {
                SlottedTaskInformations[i].ZoomSlottedSectionTotalLength = 0;
                for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count - 1; j++)//最后一段 需要分情况 后面 计算
                {
                    SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength2 = 0.0f;
                    for(j2 = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;j2 < SlottedTaskInformations[i].SlottedNodeInformations[j + 1].SlottedSectionPoint;j2++)
                    {
                        SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength2 += 
                            (float)Math.Round((double)(Math.Sqrt(Math.Pow((newListcoordinatepoints[j2 + 1].PointXY.X - newListcoordinatepoints[j2].PointXY.X), 2) +
                                            Math.Pow((newListcoordinatepoints[j2 + 1].PointXY.Y - newListcoordinatepoints[j2].PointXY.Y), 2))), 4);
                    }
                    SlottedTaskInformations[i].ZoomSlottedSectionTotalLength += SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength2;
                    //字段序号  坐标 计算
                    if ((SlottedTaskInformations[i].SlottedNodeInformations[j + 1].SlottedSectionPoint - SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint) > 1)
                    {
                        k2 = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint + ((SlottedTaskInformations[i].SlottedNodeInformations[j + 1].SlottedSectionPoint - SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint) / 2);
                        SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum = PointXY_2[k2];
                     //   Console.WriteLine("#1 {0}", SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum);
                    }  
                    else
                    {
                        k2 = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;

                        //显示数字  所在线段   一半
                        SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum.X = PointXY_2[k2].X + ((PointXY_2[k2 + 1].X - PointXY_2[k2].X) * 1) / 2;
                        SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum.Y = PointXY_2[k2].Y + ((PointXY_2[k2 + 1].Y - PointXY_2[k2].Y) * 1) / 2;
                   //     Console.WriteLine("#2 {0}", SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum);
                    }                                                    
                }
                //计算 最后一段 长度
                k = SlottedTaskInformations[i].SlottedNodeInformations.Count - 1;
                SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionLength2 = 0.0f;
                for (j2 = SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionPoint; j2 < SlottedTaskInformations[i].ZoomEndPoint; j2++)
                {
                    SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionLength2 +=
                        (float)Math.Round((double)(Math.Sqrt(Math.Pow((newListcoordinatepoints[j2 + 1].PointXY.X - newListcoordinatepoints[j2].PointXY.X), 2) +
                                        Math.Pow((newListcoordinatepoints[j2 + 1].PointXY.Y - newListcoordinatepoints[j2].PointXY.Y), 2))), 4);
                }
                

                //同一个 SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionLength2 
                if (SlottedTaskInformations[i].ZoomStartPoint != SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint)//任务级 开槽起点 != 任务起点
                {   //继续 计算 最后一段 长度
                    for (j2 = SlottedTaskInformations[i].ZoomStartPoint; j2 < SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint; j2++)
                    {
                        SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionLength2 +=
                            (float)Math.Round((double)(Math.Sqrt(Math.Pow((newListcoordinatepoints[j2 + 1].PointXY.X - newListcoordinatepoints[j2].PointXY.X), 2) +
                                            Math.Pow((newListcoordinatepoints[j2 + 1].PointXY.Y - newListcoordinatepoints[j2].PointXY.Y), 2))), 4);
                    }
                    SlottedTaskInformations[i].ZoomSlottedSectionTotalLength += SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength2;
                }
                else
                {
                    SlottedTaskInformations[i].ZoomSlottedSectionTotalLength += SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength2;
                }
                //段数 序号 坐标 计算
                if (SlottedTaskInformations[i].ZoomStartPoint != SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint)//任务级 开槽起点 != 任务起点
                {
                    SlottedTaskInformations[i].SlottedNodeInformations[k].PointNum = PointXY_2[SlottedTaskInformations[i].ZoomEndPoint];
                 //   Console.WriteLine("#3 {0}", SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum);
                }
                else
                {
                    k2 = SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionPoint;

                    //显示数字  所在线段   一半
                    SlottedTaskInformations[i].SlottedNodeInformations[k].PointNum.X = PointXY_2[k2].X + ((PointXY_2[k2 + 1].X - PointXY_2[k2].X) * 1) / 2;
                    SlottedTaskInformations[i].SlottedNodeInformations[k].PointNum.Y = PointXY_2[k2].Y + ((PointXY_2[k2 + 1].Y - PointXY_2[k2].Y) * 1) / 2;
                //    Console.WriteLine("#4 {0}", SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum);
                }
                //Console.WriteLine("每个任务总长 {0}", SlottedTaskInformations[i].ZoomSlottedSectionTotalLength);
                ZoomSlottedSectionLengthSum += SlottedTaskInformations[i].ZoomSlottedSectionTotalLength;//缩放后  字框 总长  累加
            }//for end

            //for (i = 0; i < SlottedTaskInformations.Count; i++)//打印 
            //{
            //    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)//最后一段 需要分情况 后面 计算
            //    {
            //        Console.WriteLine("任务 {0} 段号 {1} 原始长度 {2} 现有长度 {3} 序号坐标{4}",
            //            i, j, SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength, SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionLength2,
            //            SlottedTaskInformations[i].SlottedNodeInformations[j].PointNum);
            //    }
            //}//for end
            //Console.WriteLine("路径总长 {0}", ZoomSlottedSectionLengthSum);

            #endregion
        }
        #region 界面 恢复 
        /// <summary>
        /// 界面 恢复 
        /// </summary>
        private void InterfaceDisplayProcessing()
        {
            textBox3.Enabled = true;//送退料 长度输入 框
            button1.Enabled = true;//手动送料  按键
            button3.Enabled = true;//手动退料  按键
            button4.Enabled = false;//手动送料 退料 停止  按键
            button5.Enabled = true;//自动开槽  按键
            button6.Enabled = false;//自动开槽  暂停 按键
            button7.Enabled = false;//自动开槽  停止 按键
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;//开槽 数据 框
            button8.Enabled = true;//设置  按键
            button9.Enabled = true;//调试  按键
            button10.Enabled = true;//开槽参数 按键
            button11.Enabled = true;//送料 数据   按键
        }
        #endregion



        #region 气缸夹紧
        /// <summary>
        /// 气缸夹紧
        /// </summary>
        public void CylinderClamp()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ClampElectromagneticValveCommand, 0);

                //  Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.ClampElectromagneticValveCommand);
                //timer2.Interval = 500;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 气缸夹紧松开
        /// <summary>
        /// 气缸夹紧松开
        /// </summary>
        public void CylinderClampRelease()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ClampElectromagneticValveReleaseCommand, 0);

                //  Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.ClampElectromagneticValveReleaseCommand);
                //timer2.Interval = 500;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 调试模式 取消
        /// <summary>
        /// 调试模式 取消  
        /// </summary>
        public void DebugModeCancel()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.DebugModeCancelCommand, 0);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                //  Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.ClampElectromagneticValveCommand);
                //timer2.Interval = 500;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region  推刀
        /// <summary>
        /// 推刀
        /// </summary>
        public void PushKnife()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.PushKnifeCommand, 0);

                //  Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.ClampElectromagneticValveCommand);
                //timer2.Interval = 500;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region  退刀
        public void RetreatKnife()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.RetreatKnifeCommand, 0);

                //  Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.ClampElectromagneticValveCommand);
                //timer2.Interval = 500;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region  传感器信息 读取
        public void SensorInformation()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.SensorInformationCommand, 0);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.SensorInformationCommand);
                timer.Interval = 1000;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region  丝杠向上
        /// <summary>
        /// 丝杠向上
        /// </summary>
        public void ScrewUp()
        {
            Int32 Length = 0;
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                Length = DebuggingForm.Displacement;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ScrewUpCommand, Length);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.ScrewUpCommand);
                timer.Interval = 5000;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 丝杠向下
        /// <summary>
        /// 丝杠向上
        /// </summary>
        public void ScrewDown()
        {
            Int32 Length = 0;
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                Length = DebuggingForm.Displacement;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ScrewDownCommand, Length);
                timer.Stop();
                if(thread != null)
                {
                    thread.Abort();
                }
                
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.ScrewDownCommand);
                timer.Interval = 5000;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 开槽电机 单独 运行
        /// <summary>
        /// 开槽电机 单独 运行
        /// </summary>
        public void SlottedMotorRun()
        {
          //  Int32 Length = 0;
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

             //   Length = DebuggingForm.Displacement;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.SlottedMotorRunCommand, 0);

                //Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.SlottedMotorRunCommand);
                //timer2.Interval = 30000;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 开槽电机 单独 停止
        /// <summary>
        /// 开槽电机 单独 停止
        /// </summary>
        public void SlottedMotorCancel()
        {
            //  Int32 Length = 0;
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //   Length = DebuggingForm.Displacement;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.SlottedMotorCancelCommand, 0);

                //Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                //Thread thread = new Thread(ParStart);
                //thread.Start(DataCommandType.SlottedMotorCancelCommand);
                //timer2.Interval = 30000;
                //timer2.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region  零位校准 命令
        /// <summary>
        /// 零位校准 命令
        /// </summary>
        public void ZeroCalibration()
        {
            Int32 Length = 0;
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                Length = DebuggingForm.Displacement;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组

                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ZeroCalibrationCommand, Length);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.ZeroCalibrationCommand);
                timer.Interval = 10000;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region  送料
        /// <summary>
        /// 送料
        /// </summary>
        /// <param name="Length"></param>
        public void ShippingCost(float Length)
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组
                
                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ShippingCostCommand, Length);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.ShippingCostCommand);
                timer.Interval = 500;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
    
        #region 退料
        /// <summary>
        /// 退料
        /// </summary>
        /// <param name="Length"></param>
        public void  ReturnMaterial(float Length)
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组

                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ReturnMaterialCommand, Length);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.ReturnMaterialCommand);
                timer.Interval = 500;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region 更换刀片
        /// <summary>
        /// 更换刀片
        /// </summary>
        public void ChangeBlade()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组

                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.ChangeBladeCommand, 0);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.ChangeBladeCommand);
                timer.Interval = 2000;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 角磨机和气缸 动作  试刀命令
        /// <summary>
        /// 角磨机和气缸 动作  试刀命令
        /// </summary>
        public void TestKnife()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组

                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.TestKnifeCommand, 0);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
               // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.TestKnifeCommand);
                timer.Interval = PushTheKnifeTime * 2 + 300;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 开槽数据
        /// <summary>
        /// 开槽数据
        /// </summary>
        public void SlottedData()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组

                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.SlottedDataCommand, 0);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.SlottedDataCommand);
                timer.Interval = 800;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 单次开槽
        /// <summary>
        /// 单次开槽
        /// </summary>
        public void SingleSlot()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组

                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.SingleSlotCommand, 0);
                //timer2.Stop();
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
              //  ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                //thread.Priority = ThreadPriority.AboveNormal;
                thread.Start(DataCommandType.SingleSlotCommand);

                timer.Interval = PushTheKnifeTime * 2 + 1500;

                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 送退 料 开槽  停止
        /// <summary>
        /// 送退料 开槽停止
        /// </summary>
        public void StopCost()
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                //Console.WriteLine("hello");
                ReDatas = new byte[150];//清除数组
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.StopCostCommand, 0);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
              //  ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.StopCostCommand);
                timer.Interval = 800;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region  送料 开槽
        /// <summary>
        /// 送料  开槽
        /// </summary>
        /// <param name="Length"></param>
        public void FeedSlotted(float Length)
        {
            if (ComDevice.IsOpen)
            {
                ReadDataNum2 = 0;
                ReadDataNum = 0;
                CheckDataFlag = false;

                ReDatas = new byte[150];//清除数组

                // Console.WriteLine("Length {0} {1}  {2}", str, Convert.ToSingle(str) * 100, Length);
                SerialSendDatas.ComSendData(this, ComDevice, DataCommandType.FeedSlottedCommand, Length);
                timer.Stop();
                if (thread != null)
                {
                    thread.Abort();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
                // ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CrossThreadFlush);
                thread = new Thread(ParStart);
                thread.Start(DataCommandType.FeedSlottedCommand);
                timer.Interval = 1000;//等待 第一个 返回数据  时间
                timer.Start();
            }
            else
            {
                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

    }
}
