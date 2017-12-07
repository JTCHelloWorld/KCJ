using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace 开槽机
{
    //MessageBox.Show("请检查通信连接和电源\n或串口是否已打开","提示信息 返回信息", 
    //                        MessageBoxButtons.OK,MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        
    enum DataCommandType   //所有的命令  枚举
    {
        None = 0,               //初始化
        ShippingCostCommand = 0x37,                 //送料 命令
        ShippingCostReturnCommand = 0x81,           //送料 返回 命令
        ReturnMaterialCommand = 0x35,               //退料 命令
        ReturnMaterialReturnCommand = 0x82,         //退料 返回 命令
        StopCostCommand = 0xC5,                     //送退料 开槽 停止命令
        ClampElectromagneticValveCommand = 0xA7,           //夹紧电磁阀
        ClampElectromagneticValveReleaseCommand = 0xA4,           //夹紧电磁阀  松开
        RetreatKnifeCommand = 0xA2,                 //开槽气缸 退刀
        PushKnifeCommand = 0xA3,                    //开槽气缸 推刀
        DebugModeCancelCommand = 0x44,              //调试模式 取消所有 命令 夹紧放开 
        SensorInformationCommand = 0xA5,            //传感器信息 读取 命令
        ScrewUpCommand = 0xA9,                      //丝杠向上
        ScrewDownCommand = 0xA8,                    //丝杠向下
        ReachCeilingCommand = 0x2D,                 //到达上限传感器
        ReachLowerLimitCommand = 0x2C,              //到达下限传感器
        ReachUTypeCommand = 0x2E,                   //到达U型传感器
        SlottedMotorRunCommand = 0xC9,              //开槽电机 单独 运行
        SlottedMotorCancelCommand = 0xC8,           //开槽电机 单独 停止
        ZeroCalibrationCommand = 0xB9,              //零位校准 命令
        ChangeBladeCommand = 0xC7,                  //更换刀片 命令
        
        SlottedDataCommand = 0x95,                  //开槽数据
        TestKnifeCommand = 0xA1,                    //角磨机和气缸 动作  试刀命令
        SingleSlotCommand = 0x77,                   //单次开槽
        ChangeBladeReturnCommand = 0xB6,            //开槽过程 返回 换刀片 
        USensorAbnormalReturnCommand = 0x11,        //开槽过程 返回 U型传感器异常
        LeftSensorAbnormalReturnCommand = 0x12,     //开槽过程 返回 左传感器异常
        FeedSlottedCommand = 0x76,                  //送料开槽 命令
        FeedSlottedReturnCommand = 0x83,            //送料开槽 返回

    }                 
    class SerialSendData
    {
        byte[] SendDatas = new byte[50];
        Int32 Length = 0;
        public void ComSendData(Form1 Var,SerialPort Var1, DataCommandType Var2, float Var3)
        {
            switch (Var2)
            {
                case DataCommandType.ShippingCostCommand://送料 命令
                    {
                        Length = (Int32)(Var3 * Var.FeedingPulseReference);//145.513
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ShippingCostCommand;
                        SendDatas[2] = 0x04;
                        SendDatas[3] = (byte)(Length % 256);
                        SendDatas[4] = (byte)((Length % 65536) / 256);
                        SendDatas[5] = (byte)((Length % 16777216) / 65536);
                        SendDatas[6] = (byte)(Length / 16777216);
                        SendDatas[7] = 0xEE;
                        SendDatas[8] = 0xEF;
                        Var1.Write(SendDatas, 0, 9);
                        break;
                    }
                case DataCommandType.ReturnMaterialCommand://送料 命令
                    {
                        Length = (Int32)(Var3 * Var.FeedingPulseReference);//145.513
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ReturnMaterialCommand;
                        SendDatas[2] = 0x04;
                        SendDatas[3] = (byte)(Length % 256);
                        SendDatas[4] = (byte)((Length % 65536) / 256);
                        SendDatas[5] = (byte)((Length % 16777216) / 65536);
                        SendDatas[6] = (byte)(Length / 16777216);
                        SendDatas[7] = 0xEE;
                        SendDatas[8] = 0xEF;
                        Var1.Write(SendDatas, 0, 9);
                        break;
                    }
                case DataCommandType.StopCostCommand://停止 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.StopCostCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.ClampElectromagneticValveCommand://夹紧电磁阀
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ClampElectromagneticValveCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.ClampElectromagneticValveReleaseCommand://夹紧电磁阀    松开
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ClampElectromagneticValveReleaseCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.DebugModeCancelCommand://调试模式 取消所有 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.DebugModeCancelCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.RetreatKnifeCommand://开槽气缸 退刀
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.RetreatKnifeCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.PushKnifeCommand://开槽气缸 推刀
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.PushKnifeCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.SensorInformationCommand://传感器信息 读取 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.SensorInformationCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.ScrewUpCommand://丝杠向上
                    {
                        Length = (Int32)(Var3 * Form1.ReferencePulseNumber);
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ScrewUpCommand;
                        SendDatas[2] = 0x04;
                        SendDatas[3] = (byte)(Length % 256);
                        SendDatas[4] = (byte)((Length % 65536) / 256);
                        SendDatas[5] = (byte)((Length % 16777216) / 65536);
                        SendDatas[6] = (byte)(Length / 16777216);
                        SendDatas[7] = 0xEE;
                        SendDatas[8] = 0xEF;
                        Var1.Write(SendDatas, 0, 9);
                        break;
                    }
                case DataCommandType.ScrewDownCommand://丝杠向下
                    {
                        Length = (Int32)(Var3 * Form1.ReferencePulseNumber);
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ScrewDownCommand;
                        SendDatas[2] = 0x04;
                        SendDatas[3] = (byte)(Length % 256);
                        SendDatas[4] = (byte)((Length % 65536) / 256);
                        SendDatas[5] = (byte)((Length % 16777216) / 65536);
                        SendDatas[6] = (byte)(Length / 16777216);
                        SendDatas[7] = 0xEE;
                        SendDatas[8] = 0xEF;
                        Var1.Write(SendDatas, 0, 9);
                        break;
                    }
                case DataCommandType.SlottedMotorRunCommand://开槽电机 单独 运行
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.SlottedMotorRunCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.SlottedMotorCancelCommand://开槽电机 单独 停止
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.SlottedMotorCancelCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.ZeroCalibrationCommand://零位校准 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ZeroCalibrationCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.ChangeBladeCommand://更换刀片 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.ChangeBladeCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.TestKnifeCommand://角磨机和气缸 动作  试刀命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.TestKnifeCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.SlottedDataCommand://开槽数据 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.SlottedDataCommand;
                        SendDatas[2] = 0x0B;
                        SendDatas[3] = (byte)(Var.PushTheKnifeTime % 256);
                        SendDatas[4] = (byte)((Var.PushTheKnifeTime % 65536) / 256);//退刀时间
                        SendDatas[5] = (byte)(Var.SlotNumberofRepetitions);//开槽重复次数
                        SendDatas[6] = (byte)((Var.MaterialRemainingThickness * Form1.ReferencePulseNumber) % 256); 
                        SendDatas[7] = (byte)(((Var.MaterialRemainingThickness * Form1.ReferencePulseNumber) / 65536) / 256);// 开槽剩余厚度
                        SendDatas[8] = (byte)(((Var.MaterialThickness  - Var.MaterialRemainingThickness) * Form1.ReferencePulseNumber) % 256); 
                        SendDatas[9] = (byte)((((Var.MaterialThickness - Var.MaterialRemainingThickness) * Form1.ReferencePulseNumber) / 65536) / 256);//开槽深度
                        SendDatas[10] = (byte)(Var.ZeroCalibrationPulseNumber % 256); 
                        SendDatas[11] = (byte)((Var.ZeroCalibrationPulseNumber / 65536) / 256);
                        SendDatas[12] = (byte)((Var.ZeroCalibrationPulseNumber % 16777216) / 65536);//零位校准脉冲数
                        if(Var.SheetLossOptimization)//使能了 角磨机 磨损优化
                        {
                            SendDatas[13] = (byte)Var.NumberofKnife;////对刀次数
                        }
                        else
                        {
                            SendDatas[13] = (byte)(Var.SlotNumberofRepetitions);//开槽重复次数
                        }
                            
                        SendDatas[14] = 0xEE;
                        SendDatas[15] = 0xEF;
                        Var1.Write(SendDatas, 0, 16);
                        break;
                    }
                case DataCommandType.SingleSlotCommand://单次开槽 命令
                    {
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.SingleSlotCommand;
                        SendDatas[2] = 0x00;
                        SendDatas[3] = 0xEE;
                        SendDatas[4] = 0xEF;
                        Var1.Write(SendDatas, 0, 5);
                        break;
                    }
                case DataCommandType.FeedSlottedCommand://送料开槽 命令
                    {
                        Length = (Int32)(Var3 * Var.FeedingPulseReference);//145.513
                        SendDatas[0] = 0xEE;
                        SendDatas[1] = (byte)DataCommandType.FeedSlottedCommand;
                        SendDatas[2] = 0x04;
                        SendDatas[3] = (byte)(Length % 256);
                        SendDatas[4] = (byte)((Length % 65536) / 256);
                        SendDatas[5] = (byte)((Length % 16777216) / 65536);
                        SendDatas[6] = (byte)(Length / 16777216);
                        SendDatas[7] = 0xEE;
                        SendDatas[8] = 0xEF;
                        Var1.Write(SendDatas, 0, 9);
                        break;
                    }
            }
        }

    }
}
