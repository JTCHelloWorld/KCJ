using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace 开槽机
{
    enum InsideoutsideBoxType   //内外框 枚举
    {
        None,               //初始化
        InsideBox,          //内框 内轮廓
        OutsideBox          //外框 外轮廓
    }
    enum DirectionType   //顺逆时针 枚举 
    {
        None,               //初始化
        Clockwise,          //顺时针
        Counterclockwise    //逆时针Corners   Yang angle
    }

    enum CornerType    //阴阳 角 枚举
    {
        None,                   //初始化
        NegativeAngle,           //阴角
        PositiveAngle           //阳角
    }
    public class CoordinatePoints//坐标点
    {
        public PointF PointXY       = new PointF();//任务框图 坐标点
        public bool SlottedVisible  = false;//坐标是否可见
    }

    class DisNumCoordinatePoints
    {
  //      public PointF PointNum = new PointF();//显示 线段数字  坐标
        public float SlottedLength = 0;//字框拾取  每一段 线段的  长度

    }
    class SlottedTaskInformation//
    {
        public int OriginalStartPoint = 0;//任务框图 起始点  坐标转换过来（除去重复点）原始坐标信息
        public int OriginalEndPoint = 0;//任务框图 结束点

        public int OriginalStartPoint2 = 0;//任务框图 起始点  坐标转换过来（除去重复点）原始坐标信息  去除冗余点 斜率一致的
        public int OriginalEndPoint2 = 0;//任务框图 结束点

        public int ZoomStartPoint = 0;//任务框图 起始点  缩放后的 坐标数据 信息
        public int ZoomEndPoint = 0;//任务框图 结束点

        public int StartSlottedPoint = 0;//任务框图 开槽 起始点
        public int EndSlottedPoint = 0;//任务框图 开槽 结束点
        public Int32 StartEndSlottedPointCount = 0;//设置 起始 终点 开槽点时  标记 起始 终点  是否标记，标记到 哪一个点

        public DirectionType DefaultDirection = DirectionType.None;//默认 框图 方向
        public DirectionType SlottedDirection = DirectionType.None;//开槽 方向
        public float ZoomSlottedSectionTotalLength = 0.0f;//缩放后 单个 字框 总长度
       
        public InsideoutsideBoxType InsideoutsideBox = InsideoutsideBoxType.None;//框图  内外框  内外 轮廓
        public List<SlottedNodeInformation> SlottedNodeInformations = new List<SlottedNodeInformation>();

        public Int32 SlotOrderIndex = 0;//如果有任务拾取，此任务在 任务 拾取 顺序的 第几个 索引里，数据 从 1开始 因为默认是0，以防混淆；

    }

    class SlottedNodeInformation//
    {
        public bool NewDeleteNode = false;//新增的开槽点 删除开槽点

        public float SlottedSectionLength = 0.0f;//每一段的 长度

        public float SlottedSectionLength2 = 0.0f;//每一段的 长度 缩放后
        public Int32 SlottedSectionPoint = 0;//每一段的起始 开槽点
        public PointF PointNum = new PointF();//显示 线段数字  坐标
        public CornerType SlottedCornerStatus = CornerType.None;//开槽点的 阴阳角  状态
    }
    /// <summary>
    /// 字框拾取 保存 加工文件 顺序对应的字框任务序号；每个任务加工线段的长度；
    /// </summary>
    partial class SlotManagementInformation//轮廓 拾取  加工 顺序 管理 
    {
        public Int32 TaskNum = 0;//记录 选取的任务 序号
        public Int32 SlottedSectionAccumulation = 0;//开槽进程  送料开槽 的 累加
        public float SlottedTotalLength = 0.0f;//字框 拾取 后 线段 总长度 单任务
        public List<DisNumCoordinatePoints> DisNumCoordinatePoint = new List<DisNumCoordinatePoints>();//字框 拾取  后 每一段 字段 长度 及 线段 序号 显示 坐标        
    }
    /// <summary>
    /// 段 显示计算信息
    /// </summary>
    class DisplayCalculationInformation
    {
        public float KA1 = 0, KA2 = 0;
        public Int32 Kk2 = 0;
        public List<Pen> UserPens = new List<Pen>();
        public List<GraphicsPath> UserGraphicsPaths = new List<GraphicsPath>();        
    }
    #region 极值记录计算
    /// <summary>
    /// 极值记录计算
    /// </summary>
    class CoordinateMaxMin
    {
        public float AbscissaMax_X = 0.0f;//横坐标 最大
        public float AbscissaMin_X = 0.0f;//横坐标 最小

        public float OrdinateMax_Y = 0.0f;//纵坐标 最大
        public float OrdinateMin_Y = 0.0f;//纵坐标 最小

        public float AbscissaMax_X_2 = 0.0f;//横坐标 最大 * 显示缩放系数
        public float AbscissaMin_X_2 = 0.0f;//横坐标 最小  * 显示缩放系数


        public float OrdinateMin_Y_2 = 0.0f;//纵坐标 最小  * 显示缩放系数
        public float OrdinateMax_Y_2 = 0.0f;//纵坐标 最大  * 显示缩放系数



        //横坐标 差值 计算
        public float AbscissaDifference
        {
            get
            {
                return (AbscissaMax_X - AbscissaMin_X);
            }           
        }

        //纵坐标 差值 计算
        public float OrdinateDifference
        {
            get
            {
                return (OrdinateMax_Y - OrdinateMin_Y);
            }           
        }

        ////横坐标   * 显示缩放系数 差值 计算
        //public float AbscissaDifference_2
        //{
        //    get
        //    {
        //        return (AbscissaMax_X_2 - AbscissaMin_X_2);
        //    }
        //}

        ////纵坐标   * 显示缩放系数 差值 计算
        //public float OrdinateDifference_2
        //{
        //    get
        //    {
        //        return (OrdinateMax_Y_2 - OrdinateMin_Y_2);
        //    }
        //}
    }
    #endregion
}
