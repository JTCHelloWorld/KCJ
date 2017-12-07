#define WRITECOORDINATEPOINT

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
using System.Text.RegularExpressions;
using System.Threading;

namespace 开槽机
{

    public partial class Form1 : Form
    {//
       
        public SerialPort ComDevice = new SerialPort();
        Int32 serial_com_count = 0;//串口COM检测端口数 初始化从 配置文件中读取  若读取失败 默认 数据为20

        public byte[] ReDatas = new byte[150];

        public Int32 ReadDataNum = 0, ReadDataNum2 = 0, ReadDataNum3 = 0;
        // Int32 ReadDataNumFlag = 0;//接收 数据序列 累计数，例如送料 短时间会有 2个序列返回数据，所以需要接收到2个数组里，需要 标记为处理
        SerialSendData SerialSendDatas = new SerialSendData();
        public bool CheckDataFlag = false;//检测 返回 应答或数据

        public const Int32 ReferencePulseNumber = 1280;//上升下降 丝杠 丝杠 节距 一圈 1280脉冲 行程1mm
        #region

        //串口 属性 变量 及 初始值 
        public string serialBaudRate = "";
        public string serialDataBits = "";
        public int serialParitySelectedIndex = 0;//None = 0(SelectedIndex = 0),Odd = 1(SelectedIndex = 1),Even(偶数) = 2(SelectedIndex = 2)
        public string serialStopBits = "";

        //手动送料  送料长度
        public float ManualFeeding = 0;
        //自动开槽 重复加工次数
        public Int32 RepeatProcessingTimes = 0;
        //开槽 参数 
        public float MaterialThickness = 0;//材料厚度
        public float MaterialRemainingThickness = 0;//材料剩余厚度
        public Int32 SlotNumberofRepetitions = 0;//开槽重复次数
        //public float SlottedBenchmarks = 0;//开槽基准       
        public Int32 ZeroCalibrationPulseNumber = 0;//零位校准脉冲数
        public Int32 SawAccumulated = 0;//锯片累计开槽次数
        public Int32 FilterAngle = 0;//过滤角度，，可以在设置窗口设置，数据保存在 配置文件中
        public float FirstParagraphofCompensation = 0;//首段补偿
        public float LastParagraphofCompensation = 0;//末段补偿
        public Int32 PushTheKnifeTime = 0;//推刀时间
        public float FeedingCalibrationLength = 0;//送料校准  送料 默认长度
        public float FeedingPulseReference = 0;//送料 脉冲基准，单位mm对应的 脉冲数，电机3圈滚轮1圈，电机一圈6400脉冲，滚轮直径42mm

        public bool PulseReferenceCalibration = false;//用于 判断 送料 完成 标志位

        //缩放参数
        //外轮廓
        public bool OutLineZoomShrink = false;//外轮廓 放大 缩小
        //public bool OutLineShrink = true;//外轮廓 缩小
        public float OutLineZoomParameters = 0;//外轮廓 缩放参数
        //内轮廓
        public bool InnerContourZoomShrink = true;//内轮廓 放大 缩小 
        //public bool InnerContourShrink = true;//内轮廓 缩小
        public float InnerContourZoomParameters = 0;//内轮廓 缩放参数

        //末尾标记 
        public bool EndTagEnable = false;//末尾标记功能打开 标记
        public Int32 TailSectionDistance = 0;//距尾段距离
        public float EndTagMaterialRemainingThickness = 0;//末尾标记　材料剩余厚度

        //盘料 条料
        public bool ArticleTrayMaterial = true;//条料 盘料
        //public bool TrayMaterial = false;//盘料
        public float SlottedSpans = 0;//开槽跨距
        public float PlateLength = 0;//板材长度

        //单轮开槽对刀次数
        public bool SheetLossOptimization = false;//角膜片磨损优化 打开此功能
        public Int32 NumberofKnife = 0;//对刀次数

        //内外框 轮廓 识别 模式
        public bool AutomaticManualIdentification = false;//自动 手动 识别
       // public bool ManualIdentification = false;//手动识别

        //内外轮廓 顺逆时针 
        public bool OutLineCW_CCW = false;//外轮廓 顺时针 逆时针
        public bool InnerCW_CCW = false;//内轮廓 顺时针 逆时针
        //忽略段长  长度
        public Int32 IgnoreLngth = 0;//段长 小于此值 将下一段 合并过来 删除下一个 开槽点
        

        //显示用数组
        PointF[] PointXY_1 = new PointF[40960];//用于显示的 坐标数组  原始坐标点
        PointF[] PointXY_2 = new PointF[40960];//用于显示的 坐标数组  缩放以后的 坐标点

        PointF[] PointXY_1_1 = new PointF[40960];//
        public PointF[] PointXY_2_2 = new PointF[40960];//用于显示 中间值



        //文件处理 存储变量
        List<CoordinatePoints> coordinatepoints = new List<CoordinatePoints>();//原始坐标点 存储
        List<CoordinatePoints> coordinatepoints2 = new List<CoordinatePoints>();//原始坐标点 存储

        List<CoordinatePoints> dpListcoordinatepoints = new List<CoordinatePoints>();//缩放计算中间值
        List<CoordinatePoints> ndpListcoordinatepoints = new List<CoordinatePoints>();//缩放计算中间值
        public List<CoordinatePoints> newListcoordinatepoints = new List<CoordinatePoints>();//缩放计算 新的坐标值

       List<SlottedTaskInformation> SlottedTaskInformations = new List<SlottedTaskInformation>();//任务框图信息
        Int32 SelectContourCount = 0;//每次进入 字框拾取 时 拾取计数 用于撤销 和取消
      
        CoordinateMaxMin CoordinateHighVerysmall = new CoordinateMaxMin();//横纵 坐标 极值

        bool FlagCursorLeft = false;//鼠标 左按键 按下 true；没有按下 false；

   //     SlotManagementInformation SlotManagementInformations = new SlotManagementInformation();////轮廓 拾取  加工 顺序 管理 

        List<SlotManagementInformation> SlotManagementInformations = new List<SlotManagementInformation>();////轮廓 拾取  加工 顺序 管理 
        SlotManagementCalc SlotManagementCalcs = new SlotManagementCalc();
        public float ZoomSlottedSectionLengthSum = 0;//缩放后  字框  总长
        public float SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
        public float SlottedSectionNumSum = 0;//选择加工 总段数
        public float SlottedSelectBoxsNumSum = 0;//选择加工 总字框数

        bool SelectContour = false;//拾取字框 
        bool AllSelectContour = false;//全部拾取字框 
        bool AddNode = false;//增加节点 
        bool DeleteNode = false;//删除节点

        public bool SlotManagementChecked = false;//是否有拾取字框 的 动作
        public Int32 SlottedState = 0;//开槽 状态
        #endregion
        // Int32 CountTem = 0, CountTem2 = 0;

        #region 

        public Graphics g = null;

        List<GraphicsPath> UserGraphicsPaths= new List<GraphicsPath>();//任务框图信息

        List<GraphicsPath> UserGraphicsPaths2 = new List<GraphicsPath>();//任务框图信息  缩放后的 

        GraphicsPath UserGraphicsPaths_T = new GraphicsPath();//任务框图 局部显示 用于 字框 拾取时使用
        GraphicsPath UserGraphicsPaths_T2 = new GraphicsPath();//任务框图 局部显示 用于 字框 拾取时使用

        List<DisplayCalculationInformation> DisplayCalculationInformations = new List<DisplayCalculationInformation>();//存储每个任务 显示信息


        float FlagScale = 1.0f;//滚轮 缩放系数
        float AmplificationFactor = 0.0f;//适应屏幕的 缩放 系数
        
        private Bitmap buffer;

        Point Cursorstart; //画框的起始点
                                  // Point Cursorend;//画框的结束点<br>bool blnDraw;//判断是否绘制<br>Rectangel rect;

        public bool EndColorEnable = false;//颜色设置 
        public Color OriginalBlockDiagramColor = Color.LightSkyBlue;//原始框图 颜色        
        public Color ZoomOutlineBlockDiagramColor = Color.White;//缩放外框
        public Color ZoomInsideBlockDiagramColor = Color.Red;//缩放内框
        public Color CheckedBlockDiagramColor = Color.Red;//选中框图 颜色
        public Color YangAngleColor = Color.Yellow;//阳角颜色
        public Color YinAngleColor = Color.Cyan;//阴角颜色
        public Color SlottedColor = Color.Yellow;//开槽点  选中框图后 开槽点颜色
        public Color AddDeleteNodeColor = Color.LemonChiffon;//增加 删除 开槽点 临时 显示 颜色
        public Color NotSelectColor = Color.SlateGray;//没有被选择的  线段 和 开槽点 颜色

        Pen     pn1 = new Pen(Color.LightSkyBlue);   //原始字框  颜色    
        Pen     pn2 = new Pen(Color.Red);//缩放后的字框 及 字框拾取 后 没有选中的 颜色  
        Pen     pn3 = new Pen(Color.Red);//起始箭头 颜色
        Pen     pn4 = new Pen(Color.Red);//字框拾取 后的 颜色
        AdjustableArrowCap myArrow = new AdjustableArrowCap(4, 5);
        


        Point       Point_XY_T = new Point(0,0);//移动图形时 累计 移动的 数据存储
        PointF      ScreenCoordinates = new PointF(0, 0);//增加 节点 左键 按下 记录当前 屏幕坐标
        PointF      ScreenCoordinates2 = new PointF(0, 0);//增加 节点  记录当前 屏幕坐标
        PointF      ScreenCoordinates3 = new PointF(0, 0);//增加 节点 记录当前 屏幕坐标
        PointF      ScreenCoordinates4 = new PointF(0, 0);//增加 节点  记录当前 屏幕坐标
        const int     SizeSlottedWidth = 5, SizeSlottedHeight = 5;
        Size    SizeSlotted = new Size(SizeSlottedWidth, SizeSlottedHeight);//开槽点 矩形 大小 
        Pen     PenSlotted = new Pen(Color.Yellow);
        SolidBrush brush = new SolidBrush(Color.Yellow);
        Int32 SB1 = 0, SB2 = 0; //横 纵 坐标 相对原点 平移 数据，由于变量名很长 所以 使用中间变量
         Int32 Data_Num1 = 25;//计算 适应 显示区域 大小的  数据时不能正好显示到边框
        public bool MoveRefresh = false;//移动 标识  移动的时候 显示数据 不需要 重新 计算 更新 
        bool MoveLock = false;//默认 false :框图可以移动；true:框图不可以移动
       // bool FlagScale2 = false;//
        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        Form5 DebuggingForm = null;
        Form3 formCommonSettings = null;
        
        Form7 PulseRefForm = null;
        

        Thread thread = null;
        ParameterizedThreadStart ParStart = null; 
        Thread thread2 = null;
        ParameterizedThreadStart ParStart2 = null;

        private System.Timers.Timer timer = new System.Timers.Timer();//定义 多线程 定时器
        #region INI读写 声明区
        public string str = "";//该变量保存INI文件所在的具体物理位置
  //      public string strOne = "";
        //[DllImport("kernel32")]
        //private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        public string ContentReader(string area, string key, string def)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            GetPrivateProfileString(area, key, def, stringBuilder, 1024, str);
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        //private static extern long WritePrivateProfileString(
        //    string mpAppName,
        //    string mpKeyName,
        //    string mpDefault,
        //    string mpFileName);

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            this.pictureBox1.MouseWheel += new MouseEventHandler(PictureBox1_MouseWheel);//
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);//多线程 定时器  事件程序 绑定
            DebuggingForm = new Form5();
            

            ParStart = new ParameterizedThreadStart(CrossThreadFlush);//接收 串口 数据 检测 进程  带参数
            ParStart2 = new ParameterizedThreadStart(AutomaticSlottingProcess);//自动开槽进程 带参数
            Point_XY_T.X = 0;
            Point_XY_T.Y = 0;//初始化

            pn3.CustomEndCap = myArrow;

            this.button4.Enabled = false;
            this.button7.Enabled = false;
            this.button6.Enabled = false;

            AllSelectContour = false;//全部拾取字框 
            SelectContour = false;//拾取字框 
            AddNode = false;//增加节点 
            DeleteNode = false;//删除节点

            toolTip1.SetToolTip(button1, "点击发送送料命令");
            toolTip1.SetToolTip(textBox3, "输入大于0最多带2位小数的送退料数据");

            INI_Load();//INI文件读取，初始化配置变量

            button12.Enabled = EndColorEnable;//颜色设置按键 根据 使能 打开 关闭


            #region  寻找可用的出口COM端,COM属性变量初始化
            cbxCOMPort.Items.AddRange(SerialPort.GetPortNames());

            if (cbxCOMPort.Items.Count > 0)
            {
                cbxCOMPort.SelectedIndex = 0;
            }
            ComDevice.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived);//绑定事件

          //  serialBaudRate = "9600";
            serialDataBits = "8";
            serialParitySelectedIndex = 0;
            serialStopBits = "1";

            #endregion

            #region ListView 初始化数据

            
            //listView1.GridLines = true; //显示表格线
            //listView1.View = View.Details;//显示表格细节
            listView1.Columns.Add("序号", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("长度(mm)", 60);

            //for (int i = 0; i < 120; i++)
            //{
            //    listView1.Items.Add("A" + i);
            //    listView1.Items[i].SubItems.Add("AA" + i);
            //}

            //listView2.GridLines = true; //显示表格线
            //listView2.View = View.Details;//显示表格细节
            listView2.Columns.Add("名 称", 110, HorizontalAlignment.Center);
            listView2.Columns.Add("长度(mm)", 80);
                     
            listView2.Items.Add("路径总长");
            listView2.Items[0].SubItems.Add(Convert.ToString(ZoomSlottedSectionLengthSum));

            listView2.Items.Add("选取加工总长");
            listView2.Items[1].SubItems.Add(Convert.ToString(SlottedSectionLengthSum));

            listView2.Items.Add("加工用料总长");
            listView2.Items[2].SubItems.Add(Convert.ToString(SlottedSectionLengthSum + SlottedSpans));

            listView2.Items.Add("加工字段数");
            listView2.Items[3].SubItems.Add(Convert.ToString(SlotManagementInformations.Count));

            listView2.Items.Add("选择加工总段数");
            listView2.Items[4].SubItems.Add(Convert.ToString(SlotManagementInformations.Count));

            #endregion
        }
        #region 串口接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ReadDataNum3 = ComDevice.BytesToRead;
            ReadDataNum += ComDevice.BytesToRead;
            ComDevice.Read(ReDatas, ReadDataNum2, ReadDataNum3);//读取数据

            ReadDataNum2 = ReadDataNum;
        }
        #endregion
        #region 定时器  定时刷新 显示 日历
        /// <summary>
        /// 定时器  定时刷新 显示 日历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString();
        }
        #endregion
        #region   串口 相关
        #region  检测串口
        /// <summary>
        /// 检测串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckCOM_Click(object sender, EventArgs e)
        {
            bool comExistence = false;//有可用串口标志位
            cbxCOMPort.Items.Clear(); //清除当前串口号中的所有串口名称
            for (int i = 0; i < serial_com_count; i++)
            {
                try
                {
                    SerialPort sp = new SerialPort("COM" + (i + 1).ToString());
                    sp.Close();
                    sp.Open();
                    sp.Close();
                    cbxCOMPort.Items.Add("COM" + (i + 1).ToString());
                    comExistence = true;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (comExistence)
            {
                cbxCOMPort.SelectedIndex = 0;//使 ListBox 显示第 1 个添加的索引
            }
            else
            {
                /*
                    C#combobox1中设置dropdownstyle为dropdownlist，清空时写法： comboBox1.SelectedIndex = -1;
                    C#combobox1中设置dropdownstyle为dropdown，清空时写法： comboBox1.text= "";

                    C#combobox1中设置了items，清空items时写法： comboBox2.Items.Clear();
                 */
                cbxCOMPort.SelectedIndex = -1;
                MessageBox.Show("没有找到可用串口！");
            }
        }
        #endregion
        #region 打开关闭 串口端口
        /// <summary>
        /// 打开关闭 串口端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenCom_Click(object sender, EventArgs e)
        {
            if (cbxCOMPort.Items.Count <= 0)
            {
                MessageBox.Show("没有发现串口,请检查线路！","提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ComDevice.IsOpen == false)
            {
                ComDevice.PortName = cbxCOMPort.SelectedItem.ToString();
                ComDevice.BaudRate = Convert.ToInt32(serialBaudRate);
                ComDevice.Parity = (Parity)(serialParitySelectedIndex);// Convert.ToInt32(serialParity);
                ComDevice.DataBits = Convert.ToInt32(serialDataBits);
                ComDevice.StopBits = (StopBits)Convert.ToInt32(serialStopBits);

                Console.WriteLine(ComDevice.PortName);
                Console.WriteLine(ComDevice.BaudRate);
                Console.WriteLine(ComDevice.Parity);
                Console.WriteLine(ComDevice.DataBits);
                Console.WriteLine(ComDevice.StopBits);

                try
                {
                    ComDevice.Open();
                    //btnSend.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnOpenCom.Text = "关闭串口";
                btnOpenCom.ForeColor = System.Drawing.Color.Red;
                ReadDataNum = 0; ReadDataNum2 = 0;
            }
            else
            {
                try
                {
                    ComDevice.Close();
                    //btnSend.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnOpenCom.Text = "打开串口";
                btnOpenCom.ForeColor = System.Drawing.Color.Red;
            }

            cbxCOMPort.Enabled = !ComDevice.IsOpen;
            btnCheckCOM.Enabled = !ComDevice.IsOpen;
            btnSetCom.Enabled = !ComDevice.IsOpen;
        }
        #endregion
        #region 打开串口属性设置对话框
        /// <summary>
        /// 打开串口属性设置对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetCom_Click(object sender, EventArgs e)
        {
            Form2 formSerialSet = new Form2();
            formSerialSet.form1 = this;
            formSerialSet.ShowDialog();
        }
        #endregion
        #endregion
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region  打开文件
        private void 打开加工文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("我爱你 宝贝！！");
            openFileDialog1.InitialDirectory = "c://";

            //打开文件类型
            openFileDialog1.Filter = "PLT或DXF文件|*.plt;*.dxf|文本文件|*.txt|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fName = "", fName2 = "";

                    fName = openFileDialog1.FileName;
                    fName2 = openFileDialog1.SafeFileName;
                    string str1 = File.ReadAllText(fName);
                    //string str2 = "startPU3562 2568;PD-689,2100;PD-784 2580;PU568,2569;PD12 8954;PU256,589;";
                    
                    
                    //j 查找具体的字符 或 字符串具体的位置返回值；j2 k k2 k3 都是中间临时保存 j的值；
                    //count 任务文件 数量 记录 从 0开始 ，暂时还没有 他用；
                    //f：0：默认文件 刚开始处理文件；1：文件内此“PU”之后还有“PU”；２：文件内此“PU”之后没有“PU”；
                    //pointnum：坐标点数量 从 0 开始；
                    Int32 j = 0, j2 = 0,  k = 0, k2 = 0, k3 = 0, count = 0, f = 0, pointnum = 0;

                   // float PreviousDifferencePointX = 0.0f, PreviousDifferencePointY = 0.0f, CurrentDifferencePointX = 0.0f, CurrentDifferencePointY = 0.0f;

                    coordinatepoints.Clear();//原始坐标点 存储 清除
                    coordinatepoints2.Clear();

                    dpListcoordinatepoints.Clear();
                    ndpListcoordinatepoints.Clear();
                    newListcoordinatepoints.Clear();

                    SlottedTaskInformations.Clear();//任务框图信息 清除

                    UserGraphicsPaths.Clear();
                    UserGraphicsPaths2.Clear();
                    Point_XY_T.X = 0;
                    Point_XY_T.Y = 0;//初始化

                    AllSelectContour = false;//全部拾取字框 
                    SelectContour = false;//拾取字框 
                    AddNode = false;//增加节点 
                    DeleteNode = false;//删除节点
                    拾取字框toolStripMenuItem.Checked = false;
                    增加节点ToolStripMenuItem.Checked = false;
                    删除节点ToolStripMenuItem.Checked = false;
                    拾取全部ToolStripMenuItem.Checked = false;


                    SlotManagementChecked = false;//是否有拾取字框 的 动作
                    SlotManagementInformations.Clear();

                    FlagScale = 1.0f;//滚轮 缩放系数

                    ZoomSlottedSectionLengthSum = 0;//缩放后  字框  总长
                    SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
                    SlottedSectionNumSum = 0;//选择加工 总段数
                    SlottedSelectBoxsNumSum = 0;//选择加工 总字框数

                    SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                    SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                    if (fName2.Contains(".plt"))
                    {
                        if (str1.Length <= 0)
                        {
                           MessageBox.Show("文件没有任何数据\n请确认后再次导入", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        for (int i = 0; i < str1.Length;)
                        {
                            switch (f)
                            {
                                case 0:
                                    #region
                                    j = str1.IndexOf("PU");

                                    if (j == -1)
                                    {
                                        MessageBox.Show("文件没有有效数据\n请确认后再次导入", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;// break;
                                    }
                                    else
                                    {
                                        k = j; j2 = j;
                                        j = str1.IndexOf("PD", k);
                                        k3 = j;
                                        j = str1.IndexOf("PU", (k + 1));    //此处 做了一个 单点 排除算法，若只有一个点“PU X Y”忽略

                                        while (true)
                                        {
                                            if ((j != -1) && (k3 > j))          //通过对比 第一个PU后的第一个PD是否比第二个PU 靠后？
                                            {
                                                k = j;
                                                j2 = j;
                                                j = str1.IndexOf("PU", (k + 1));
                                            }
                                            else
                                            {
                                                k = j2;
                                                break;
                                            }
                                        }

                                        //if ((j != -1) && (k3 > j))          //通过对比 第一个PU后的第一个PD是否比第二个PU 靠后？
                                        //{
                                        //    k = j;
                                        //    j = str1.IndexOf("PU", (k + 1));
                                        //}
                                        //else
                                        //{
                                        //    k = j2;
                                        //}

                                        if (j == -1)
                                        {
                                            f = 2;//只有 一个 框图
                                        }
                                        else
                                        {
                                            f = 1;//2个以上 框图
                                            k2 = j;//保存当下的 下一个 PU起始地址索引
                                        }

                                        SlottedTaskInformation TemporaryTaskInformation = new SlottedTaskInformation();

                                        TemporaryTaskInformation.OriginalStartPoint = pointnum;//原始框图 坐标 起始点
                                        SlottedTaskInformations.Add(TemporaryTaskInformation);//框图默认起始点 记录保存

                                        j = str1.IndexOfAny(new char[] { ' ', ',' }, k);
                                        CoordinatePoints TemporaryPoint = new CoordinatePoints();
                                        TemporaryPoint.PointXY.X = Convert.ToInt32(str1.Substring((k + 2), (j - k - 2))) / 40.0f;
                                        k = j;
                                        j = str1.IndexOf(';', k);
                                        TemporaryPoint.PointXY.Y = Convert.ToInt32(str1.Substring((k + 1), (j - k - 1))) / 40.0f;

                                        coordinatepoints.Add(TemporaryPoint);     //坐标记录保存   第一个坐标   

                                        CoordinatePoints TemporaryPoint_1 = new CoordinatePoints();
                                        CoordinatePoints TemporaryPoint_2 = new CoordinatePoints();
                                        CoordinatePoints TemporaryPoint_3 = new CoordinatePoints();
                                        //CoordinatePoints TemporaryPoint_4 = new CoordinatePoints();

                                        //coordinatepoints2.Add(TemporaryPoint_4);
                                        dpListcoordinatepoints.Add(TemporaryPoint_1);//缩放数据
                                        ndpListcoordinatepoints.Add(TemporaryPoint_2);
                                        newListcoordinatepoints.Add(TemporaryPoint_3);
                                        
                                        //Console.WriteLine("NEW Task!{0}", count);
                                        //Console.WriteLine("X:{0},Y:{1} {2}", TemporaryPoint.PointXY.X, TemporaryPoint.PointXY.Y, pointnum);

                                        pointnum++;//坐标点数

                                        i = j;//j是';'地址索引
                                        k = j;
                                        count++;
                                    }
                                    break;
                                #endregion
                                case 1:
                                    #region
                                    j = str1.IndexOf("PD", k);//k 是';'地址索引

                                    if (j == -1)
                                    {
                                        SlottedTaskInformations[count - 1].OriginalEndPoint = pointnum - 1;//上一个任务 原始框图 坐标 终点
                                        goto Sun;//return;
                                    }
                                    else if (j > k2)//新的 任务 框图 起始点 PU
                                    {
                                        SlottedTaskInformation TemporaryTaskInformation = new SlottedTaskInformation();

                                        TemporaryTaskInformation.OriginalStartPoint = pointnum;//新任务 原始框图 坐标 起始点
                                        SlottedTaskInformations.Add(TemporaryTaskInformation);
                                        SlottedTaskInformations[count - 1].OriginalEndPoint = pointnum - 1;//上一个任务 原始框图 坐标 终点

                                       //Console.WriteLine("NEW Task!{0}", count);

                                        k3 = j;j2 = k2;
                                        j = str1.IndexOf("PU", (k2 + 1));    //此处 做了一个 单点 排除算法，若只有一个点“PU X Y”忽略
                                        while(true)
                                        {
                                            if ((j != -1) && (k3 > j))          //通过对比 第一个PU后的第一个PD是否比第二个PU 靠后？
                                            {
                                                k = j;
                                                j2 = j;
                                                j = str1.IndexOf("PU", (k + 1));
                                            }
                                            else
                                            {
                                                k = j2;
                                                break;
                                            }
                                        }




                                        //k3 = j;
                                        //j = str1.IndexOf("PU", (k2 + 1));    //此处 做了一个 单点 排除算法，若只有一个点“PU X Y”忽略

                                        //if ((j != -1) && (k3 > j))          //通过对比 第一个PU后的第一个PD是否比第二个PU 靠后？
                                        //{
                                        //    k = j;
                                        //    j = str1.IndexOf("PU", (k + 1));
                                        //}
                                        //else
                                        //{
                                        //    k = k2;
                                        //}


                                        //    j = str1.IndexOf("PU", (k2 + 1));
                                        if (j == -1)
                                        {
                                            f = 2;//只有 一个 框图
                                        }
                                        else
                                        {
                                            f = 1;//2个以上 框图
                                            k2 = j;//保存当下的 下一个 PU起始地址索引
                                        }

                              //          j = str1.IndexOf("PU", k);//k 是';'地址索引
                               //         k = j;
                                        j = str1.IndexOfAny(new char[] { ' ', ',' }, k);

                                        CoordinatePoints TemporaryPoint = new CoordinatePoints();
                                        TemporaryPoint.PointXY.X = Convert.ToInt32(str1.Substring((k + 2), (j - k - 2))) / 40.0f;
                                        k = j;
                                        j = str1.IndexOf(';', k);
                                        TemporaryPoint.PointXY.Y = Convert.ToInt32(str1.Substring((k + 1), (j - k - 1))) / 40.0f;

                                        coordinatepoints.Add(TemporaryPoint);

                                        CoordinatePoints TemporaryPoint_1 = new CoordinatePoints();
                                        CoordinatePoints TemporaryPoint_2 = new CoordinatePoints();
                                        CoordinatePoints TemporaryPoint_3 = new CoordinatePoints();
                                        //CoordinatePoints TemporaryPoint_4 = new CoordinatePoints();

                                        //coordinatepoints2.Add(TemporaryPoint_4);
                                        dpListcoordinatepoints.Add(TemporaryPoint_1);//缩放数据
                                        ndpListcoordinatepoints.Add(TemporaryPoint_2);
                                        newListcoordinatepoints.Add(TemporaryPoint_3);

                                        //Console.WriteLine("X:{0},Y:{1} {2}", TemporaryPoint.PointXY.X, TemporaryPoint.PointXY.Y, pointnum);

                                        pointnum++;//坐标点数

                                        i = j;//j是';'地址索引
                                        k = j;
                                        count++;
                                    }
                                    else//PD
                                    {
                                        k = j;//PD 起始点
                                        j = str1.IndexOfAny(new char[] { ' ', ',' }, k);

                                        CoordinatePoints TemporaryPoint = new CoordinatePoints();
                                        TemporaryPoint.PointXY.X = Convert.ToInt32(str1.Substring((k + 2), (j - k - 2))) / 40.0f;
                                        k = j;
                                        j = str1.IndexOf(';', k);
                                        TemporaryPoint.PointXY.Y = Convert.ToInt32(str1.Substring((k + 1), (j - k - 1))) / 40.0f;

                                        if (TemporaryPoint.PointXY != coordinatepoints[pointnum - 1].PointXY)//去除重复坐标点
                                        {                                         
                                            {
                                                coordinatepoints.Add(TemporaryPoint);

                                                CoordinatePoints TemporaryPoint_1 = new CoordinatePoints();
                                                CoordinatePoints TemporaryPoint_2 = new CoordinatePoints();
                                                CoordinatePoints TemporaryPoint_3 = new CoordinatePoints();
                                                //CoordinatePoints TemporaryPoint_4 = new CoordinatePoints();

                                                //coordinatepoints2.Add(TemporaryPoint_4);
                                                dpListcoordinatepoints.Add(TemporaryPoint_1);//缩放数据
                                                ndpListcoordinatepoints.Add(TemporaryPoint_2);
                                                newListcoordinatepoints.Add(TemporaryPoint_3);

                                  //               Console.WriteLine("X:{0},Y:{1} {2}", TemporaryPoint.PointXY.X, TemporaryPoint.PointXY.Y, pointnum);

                                                pointnum++;//坐标点数
                                            }
                                        }

                                        i = j;//j是';'地址索引
                                        k = j;
                                    }

                                    break;
                                #endregion
                                case 2:
                                    #region
                                    j = str1.IndexOf("PD", k);//k 是';'地址索引

                                    if (j == -1)
                                    {
                                        SlottedTaskInformations[count - 1].OriginalEndPoint = pointnum - 1;//上一个任务 原始框图 坐标 终点
                                        goto Sun;//return;
                                    }
                                    else//PD
                                    {
                                        k = j;//PD 起始点
                                        j = str1.IndexOfAny(new char[] { ' ', ',' }, k);

                                        CoordinatePoints TemporaryPoint = new CoordinatePoints();
                                        TemporaryPoint.PointXY.X = Convert.ToInt32(str1.Substring((k + 2), (j - k - 2))) / 40.0f;
                                        k = j;
                                        j = str1.IndexOf(';', k);
                                        TemporaryPoint.PointXY.Y = Convert.ToInt32(str1.Substring((k + 1), (j - k - 1))) / 40.0f;

                                        if (TemporaryPoint.PointXY != coordinatepoints[pointnum - 1].PointXY)//去除重复坐标点
                                        {
                                            {
                                                coordinatepoints.Add(TemporaryPoint);

                                                CoordinatePoints TemporaryPoint_1 = new CoordinatePoints();
                                                CoordinatePoints TemporaryPoint_2 = new CoordinatePoints();
                                                CoordinatePoints TemporaryPoint_3 = new CoordinatePoints();
                                                //CoordinatePoints TemporaryPoint_4 = new CoordinatePoints();

                                                //coordinatepoints2.Add(TemporaryPoint_4);
                                                dpListcoordinatepoints.Add(TemporaryPoint_1);//缩放数据
                                                ndpListcoordinatepoints.Add(TemporaryPoint_2);
                                                newListcoordinatepoints.Add(TemporaryPoint_3);

                                         //        Console.WriteLine("X:{0},Y:{1} {2}", TemporaryPoint.PointXY.X, TemporaryPoint.PointXY.Y, pointnum);

                                                pointnum++;//坐标点数
                                            }
                                        }

                                        i = j;//j是';'地址索引
                                        k = j;
                                    }
                                    break;
                                    #endregion
                            }
                        }
                        Sun:

                       // Console.WriteLine("总任务数：{0}", SlottedTaskInformations.Count);

                        //for (int i = 0; i < SlottedTaskInformations.Count; i++)
                        //{
                        //    Console.WriteLine("任务数：{0} 起始：{1} 终点：{2}", i, SlottedTaskInformations[i].OriginalStartPoint, SlottedTaskInformations[i].OriginalEndPoint);
                        //    for (j = SlottedTaskInformations[i].OriginalStartPoint; j <= SlottedTaskInformations[i].OriginalEndPoint; j++)
                        //    {
                        //        Console.WriteLine("X:{0},Y:{1} {2}", coordinatepoints[j].PointXY.X, coordinatepoints[j].PointXY.Y, j);
                        //    }
                        //}
                        RemoveRedundancyCoordinatePoints();//去除冗余的 斜率一样的一条线上的 多余点
                        IntegratedCall();
                        ConvertStorage();
                        float SSB1 = 0.0f, SSB2 = 0.0f;///横 纵 坐标 相对原点 平移 数据，由于变量名很长 所以 使用中间变量

                        SSB1 = (CoordinateHighVerysmall.AbscissaDifference / CoordinateHighVerysmall.OrdinateDifference);
                        SSB2 = ((float)pictureBox1.Width / (float)pictureBox1.Height);

                        // Console.WriteLine("{0}  {1}", SSB1, SSB2);
                        //适应 显示窗口 计算 显示 缩放 比例
                        if (SSB1 > SSB2)
                        {//判断 字框 外轮廓 横纵比 与 pictureBox 横纵比
                            AmplificationFactor = (float)(pictureBox1.Width - (pictureBox1.Width / Data_Num1)) / CoordinateHighVerysmall.AbscissaDifference;//适应屏幕的 缩放 系数 
                        }
                        else
                        {//(pictureBox1.Width - 10)  (pictureBox1.Height- 10) 减10的目的是 放大后的 图框 不会 靠着边框 显示不全
                            AmplificationFactor = (float)(pictureBox1.Height- (pictureBox1.Height / Data_Num1)) / CoordinateHighVerysmall.OrdinateDifference;//适应屏幕的 缩放 系数
                        }
                        //极值 * 显示系数 计算
                        CoordinateHighVerysmall.AbscissaMax_X_2 = CoordinateHighVerysmall.AbscissaMax_X * AmplificationFactor;
                        CoordinateHighVerysmall.AbscissaMin_X_2 = CoordinateHighVerysmall.AbscissaMin_X * AmplificationFactor;
                        CoordinateHighVerysmall.OrdinateMax_Y_2 = CoordinateHighVerysmall.OrdinateMax_Y * AmplificationFactor;
                        CoordinateHighVerysmall.OrdinateMin_Y_2 = CoordinateHighVerysmall.OrdinateMin_Y * AmplificationFactor;

                       // SB1 = Convert.ToInt32(CoordinateHighVerysmall.AbscissaMax_X_2 - (CoordinateHighVerysmall.AbscissaDifference_2 / 2));//最小 -80 最大 120  此时 偏右 20，需左移 20
                       // SB2 = Convert.ToInt32(CoordinateHighVerysmall.OrdinateMax_Y_2 - (CoordinateHighVerysmall.OrdinateDifference_2 / 2));//(CoordinateHighVerysmall.OrdinateDifference_2 / 2)横纵坐标 不是 都正好 贴合 显示边框 

                        for (int i = 0; i < SlottedTaskInformations.Count; i++)//所有的坐标点 需要 乘以 显示系数
                        {
                            for (j = SlottedTaskInformations[i].OriginalStartPoint2; j <= SlottedTaskInformations[i].OriginalEndPoint2; j++)
                            {
                                //PointXY_1_1[j].X = PointXY_1[j].X * AmplificationFactor;
                                //PointXY_1_1[j].Y = PointXY_1[j].Y * AmplificationFactor;

                                PointXY_2_2[j].X = PointXY_2[j].X * AmplificationFactor;//Convert.ToInt32(
                                PointXY_2_2[j].Y = PointXY_2[j].Y * AmplificationFactor;//Convert.ToInt32(
                            }
                        }

                        Int32 SA1 = 0, SA2 = 0, SA3 = 0;
                        for (int i = 0; i < SlottedTaskInformations.Count; i++)
                        {
                            SA1 = SlottedTaskInformations[i].OriginalStartPoint2; SA2 = SlottedTaskInformations[i].OriginalEndPoint2;
                            SA3 = SA2 - SA1;
                            //Console.WriteLine("{0} {1} {2}", SA1, SA2,SA3);

                         //   GraphicsPath gp = new GraphicsPath();
                            GraphicsPath gp2 = new GraphicsPath();

                         //   UserGraphicsPaths.Add(gp);
                            UserGraphicsPaths2.Add(gp2);

                          //  UserGraphicsPaths[i].AddCurve(PointXY_1_1, SA1, SA3, 0);
                            UserGraphicsPaths2[i].AddCurve(PointXY_2_2, SA1, SA3, 0);
                        }

                        buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        g = Graphics.FromImage(buffer);


                  //       Console.WriteLine("SB1  {0}  {1}  {2}  {3}", SB1, SB2, pictureBox1.Width, pictureBox1.Height);
                        g.TranslateTransform((pictureBox1.Width / 2 - SB1), (pictureBox1.Height / 2 + SB2));//移动 坐标 
                        g.ScaleTransform(1, -1);//y轴 方向 调换

                        g.Clear(Color.Black);

                        float KA1 = 0, KA2 = 0;

                      //  pn1.Color = OriginalBlockDiagramColor;

                        // 显示 缩放后的  框图 开槽点  根据阴阳角 显示 不同的颜色
                        for (int i = 0; i < SlottedTaskInformations.Count; i++)
                        {
                         //   g.DrawPath(pn1, UserGraphicsPaths[i]);
                            if (SlottedTaskInformations[i].InsideoutsideBox == InsideoutsideBoxType.OutsideBox)
                            {
                                pn2.Color = ZoomOutlineBlockDiagramColor;
                                pn3.Color = ZoomOutlineBlockDiagramColor;
                            }
                            else
                            {
                                pn2.Color = ZoomInsideBlockDiagramColor;
                                pn3.Color = ZoomInsideBlockDiagramColor;
                            }
                            g.DrawPath(pn2, UserGraphicsPaths2[i]);
                            ////显示 开槽点
                            for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                            {
                                //Console.WriteLine("开槽点 {0},{1}", SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint, j);
                                k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
                                RectangleF RectangleF1Slotted = new RectangleF((PointXY_2_2[k].X - (SizeSlottedWidth / 2)), (PointXY_2_2[k].Y - (SizeSlottedHeight / 2)), SizeSlotted.Width, SizeSlotted.Height);

                                if (SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus == CornerType.PositiveAngle)
                                {
                                    brush.Color = YangAngleColor;
                                }
                                else if (SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedCornerStatus == CornerType.NegativeAngle)
                                {
                                    brush.Color = YinAngleColor;
                                }


                                g.FillRectangle(brush, RectangleF1Slotted);
                            }
                            //在 起始段 加箭头
                            //AdjustableArrowCap myArrow = new AdjustableArrowCap(4, 5);
                            //pn3.CustomEndCap = myArrow;
                            if (SlottedTaskInformations[i].DefaultDirection == SlottedTaskInformations[i].SlottedDirection)
                            {
                                k2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                //箭头 所在直线 只有第一段线段的  一半
                                KA1 = PointXY_2_2[k2].X + ((PointXY_2_2[k2 + 1].X - PointXY_2_2[k2].X) * 2) / 3;
                                KA2 = PointXY_2_2[k2].Y + ((PointXY_2_2[k2 + 1].Y - PointXY_2_2[k2].Y) * 2) / 3;
                            }
                            else
                            {
                                if (SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint == SlottedTaskInformations[i].ZoomStartPoint)
                                {
                                    k2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                    k3 = SlottedTaskInformations[i].ZoomEndPoint - 1;
                                }
                                else
                                {
                                    k2 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;
                                    k3 = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint - 1;
                                }

                                //箭头 所在直线 只有第一段线段的  一半
                                KA1 = PointXY_2_2[k2].X + ((PointXY_2_2[k3].X - PointXY_2_2[k2].X) * 2) / 3;
                                KA2 = PointXY_2_2[k2].Y + ((PointXY_2_2[k3].Y - PointXY_2_2[k2].Y) * 2) / 3;
                            }
                            g.DrawLine(pn3, PointXY_2_2[k2].X, PointXY_2_2[k2].Y, KA1, KA2);
                        }//for end

                                             
                        pictureBox1.Image = buffer;
                      
                    }
                    else if (fName2.Contains(".dxf"))
                    {
                        //textBox2.Text = "PLT is NO";
                        //textBox3.Text = "DXF is OK";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion
        #region  鼠标按键 松开
        /// <summary>
        /// 鼠标按键 松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            //FlagCursor = false;
            FlagCursorLeft = false;
            this.Cursor = Cursors.Default;
        }
        #endregion
        #region  鼠标按键按下 检测，分左右 键
        /// <summary>
        /// 鼠标按键按下 检测，分左右 键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//鼠标 左击 单击
            {
                FlagCursorLeft = true;
                Cursorstart = e.Location;
             //   if((AddNode))
                {
                    ScreenCoordinates.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                    ScreenCoordinates.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;
                    Console.WriteLine("MouseDown {0} {1} {2} ", e.Location, pictureBox1.Size, ScreenCoordinates);
                }
                
            }
            if (e.Button == MouseButtons.Right)//鼠标 右击 单击
            {
                if(g == null)//没有导入文件 没有显示
                {
                    拾取字框toolStripMenuItem.Enabled = false;
                    拾取全部ToolStripMenuItem.Enabled = false;

                    撤销拾取ToolStripMenuItem.Enabled = false;
                    删除节点ToolStripMenuItem.Enabled = false;
                    增加节点ToolStripMenuItem.Enabled = false;
                    确定ToolStripMenuItem.Enabled = false;
                    取消ToolStripMenuItem.Enabled = false;
                }
                else
                {
                    if(AddNode)//增加节点 
                    {
                        拾取字框toolStripMenuItem.Enabled = false;
                        拾取全部ToolStripMenuItem.Enabled = false;

                        撤销拾取ToolStripMenuItem.Enabled = false;
                        删除节点ToolStripMenuItem.Enabled = false;
                        增加节点ToolStripMenuItem.Enabled = true;
                        确定ToolStripMenuItem.Enabled = true;
                        取消ToolStripMenuItem.Enabled = true;

                        return;
                    }
                    if (DeleteNode)//删除节点 
                    {
                        拾取字框toolStripMenuItem.Enabled = false;
                        拾取全部ToolStripMenuItem.Enabled = false;

                        撤销拾取ToolStripMenuItem.Enabled = false;
                        删除节点ToolStripMenuItem.Enabled = true;
                        增加节点ToolStripMenuItem.Enabled = false;
                        确定ToolStripMenuItem.Enabled = true;
                        取消ToolStripMenuItem.Enabled = true;

                        return;
                    }
                    if (SelectContour)//字框拾取
                    {
                        拾取字框toolStripMenuItem.Enabled = true;                        
                        拾取全部ToolStripMenuItem.Enabled = false;
                        if(SelectContourCount > 0)
                        {
                            撤销拾取ToolStripMenuItem.Enabled = true;
                        }
                        else
                        {
                            撤销拾取ToolStripMenuItem.Enabled = false;
                        }
                        
                        删除节点ToolStripMenuItem.Enabled = false;
                        增加节点ToolStripMenuItem.Enabled = false;
                        确定ToolStripMenuItem.Enabled = true;
                        取消ToolStripMenuItem.Enabled = true;

                        return;
                    }
                    if (AllSelectContour)//字框拾取
                    {
                        拾取字框toolStripMenuItem.Enabled = false;
                        拾取全部ToolStripMenuItem.Enabled = true;

                        撤销拾取ToolStripMenuItem.Enabled = false;

                        删除节点ToolStripMenuItem.Enabled = false;
                        增加节点ToolStripMenuItem.Enabled = false;
                        确定ToolStripMenuItem.Enabled = true;
                        取消ToolStripMenuItem.Enabled = true;

                        return;
                    }
                    //   else// if(!SelectContour)
                    {
                        拾取字框toolStripMenuItem.Enabled = true;
                        拾取全部ToolStripMenuItem.Enabled = true;

                        撤销拾取ToolStripMenuItem.Enabled = false;
                        删除节点ToolStripMenuItem.Enabled = true;
                        增加节点ToolStripMenuItem.Enabled = true;
                        确定ToolStripMenuItem.Enabled = false;
                        if(SlotManagementInformations.Count > 0)
                        {
                            取消ToolStripMenuItem.Enabled = true;
                        }
                        else
                        {
                            取消ToolStripMenuItem.Enabled = false;
                        }
                        
                    }


                }
            }
        }
        #endregion

        #region 框图移动
        /// <summary>
        /// 框图移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
             if(MoveLock == false)//
            {
                // Console.WriteLine("{0}", e.Location);
                if ((FlagCursorLeft))//(FlagCursor) && 
                {
                    this.Cursor = Cursors.Hand;
                    if (g != null)
                    {
                        //Console.WriteLine("{0}", e.Location);
                        g.TranslateTransform(e.X - Cursorstart.X, Cursorstart.Y - e.Y);//移动 坐标 
                        Point_XY_T.X += (e.X - Cursorstart.X);
                        Point_XY_T.Y += (e.Y - Cursorstart.Y);
                        //  Console.WriteLine("Point_XY_T {0}", Point_XY_T);
                       MoveRefresh = true;
                        BlockDiagramDrawing();
                        Cursorstart = e.Location;
                    }


                    //   Console.WriteLine("{0},{1},{2},{3}", e.X, e.Y, pictureBox1.Location.X, pictureBox1.Location.Y);
                }
            }           
        }
        #endregion
        #region  滚轮 事件
        /// <summary>
        /// 滚轮 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (g != null)
                {
                    FlagScale += 0.2f;

                    //if(FlagScale2 == false)
                    //{
                    //    FlagScale2 = true;
                        

                    //    ScreenCoordinates2.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                    //    ScreenCoordinates2.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;


                    //    ScreenCoordinates3.X = -ScreenCoordinates2.X * 0.2f;
                    //    ScreenCoordinates3.Y = -ScreenCoordinates2.Y * 0.2f;

                    //}
                    //else
                    //{
                    //   // if (((e.X <= (ScreenCoordinates4.X + 3)) && (e.X >= (ScreenCoordinates4.X - 3))) && ((e.Y <= (ScreenCoordinates4.Y + 3)) && (e.Y >= (ScreenCoordinates4.Y - 3))))
                    //    {
                    //        ScreenCoordinates3.X = -ScreenCoordinates2.X * 0.2f;
                    //        ScreenCoordinates3.Y = -ScreenCoordinates2.Y * 0.2f;
                    //    }
                    //    //else
                    //    //{
                    //    //    ScreenCoordinates2.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                    //    //    ScreenCoordinates2.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;


                    //    //    ScreenCoordinates3.X = -ScreenCoordinates2.X * 0.2f;
                    //    //    ScreenCoordinates3.Y = -ScreenCoordinates2.Y * 0.2f;
                    //    //}
                    //}

                    ////ScreenCoordinates2.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                    ////ScreenCoordinates2.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;
                    ////  if(FlagScale == 1.2f)
                    //{
                    ////    if(FlagScale2 == false)
                    //    {
                    //        ScreenCoordinates2.X = e.X - pictureBox1.Width / 2;
                    //        ScreenCoordinates2.Y = pictureBox1.Height / 2 - e.Y;

                    //        ScreenCoordinates3.X = -ScreenCoordinates2.X * 0.2f;
                    //        ScreenCoordinates3.Y = -ScreenCoordinates2.Y * 0.2f;
                    //   //     FlagScale2 = true;
                    //    }

                    //}


                    Console.WriteLine("{0} {1} {2} {3}", e.Location, pictureBox1.Size, ScreenCoordinates2, ScreenCoordinates3);
                  //  g.TranslateTransform(-ScreenCoordinates2.X * 0.2f, -ScreenCoordinates2.Y * 0.2f);//移动 坐标 
                   // g.TranslateTransform(ScreenCoordinates3.X, ScreenCoordinates3.Y);//移动 坐标 
                   // Point_XY_T.X += (int)ScreenCoordinates3.X;
                   // Point_XY_T.Y += (int)(-ScreenCoordinates3.Y);
                    BlockDiagramDrawing();

                   // ScreenCoordinates4 = e.Location;
                }
               //Console.WriteLine("{0}",e.Location);
            }
            else
            {
                if (g != null)
                {
                    if (FlagScale > 0.2)
                    {
                        FlagScale -= 0.2f;

                        //if (FlagScale2 == false)
                        //{
                        //    FlagScale2 = true;


                        //    ScreenCoordinates2.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                        //    ScreenCoordinates2.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;

                        //    ScreenCoordinates3.X = ScreenCoordinates2.X * 0.2f;
                        //    ScreenCoordinates3.Y = ScreenCoordinates2.Y * 0.2f;

                        //}
                        //else
                        //{
                        // //   if (((e.X <= (ScreenCoordinates4.X + 3)) && (e.X >= (ScreenCoordinates4.X - 3))) && ((e.Y <= (ScreenCoordinates4.Y + 3)) && (e.Y >= (ScreenCoordinates4.Y - 3))))
                        //    {
                        //        ScreenCoordinates3.X = ScreenCoordinates2.X * 0.2f;
                        //        ScreenCoordinates3.Y = ScreenCoordinates2.Y * 0.2f;
                        //    }
                        //    //else
                        //    //{
                        //    //    ScreenCoordinates2.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                        //    //    ScreenCoordinates2.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;


                        //    //    ScreenCoordinates3.X = ScreenCoordinates2.X * 0.2f;
                        //    //    ScreenCoordinates3.Y = ScreenCoordinates2.Y * 0.2f;
                        //    //}
                        //}

                        //   //ScreenCoordinates2.X = (e.X - (pictureBox1.Width / 2 - SB1)) - Point_XY_T.X;
                        //   //ScreenCoordinates2.Y = ((pictureBox1.Height / 2 + SB2) - e.Y) + Point_XY_T.Y;
                        ////   if(FlagScale == 0.8f)
                        //   {
                        //    //   if (FlagScale2 == false)
                        //       {
                        //           ScreenCoordinates2.X = e.X - pictureBox1.Width / 2;
                        //           ScreenCoordinates2.Y = pictureBox1.Height / 2 - e.Y;

                        //ScreenCoordinates3.X = ScreenCoordinates2.X * 0.2f;
                        //ScreenCoordinates3.Y = ScreenCoordinates2.Y * 0.2f;
                        //        //   FlagScale2 = true;
                        //       }

                        //   }

                        Console.WriteLine("{0} {1} {2} {3}", e.Location, pictureBox1.Size, ScreenCoordinates2, ScreenCoordinates3);
                       // g.TranslateTransform(-ScreenCoordinates2.X * -0.2f, -ScreenCoordinates2.Y * -0.2f);//移动 坐标 
                      //  g.TranslateTransform(ScreenCoordinates3.X, ScreenCoordinates3.Y);//移动 坐标
                      //  Point_XY_T.X += (int)ScreenCoordinates3.X;
                      //  Point_XY_T.Y += (int)(-ScreenCoordinates3.Y);
                        BlockDiagramDrawing();

                       // ScreenCoordinates4 = e.Location;
                    }

                    
                }
             //   Console.WriteLine("------");
            }
        }
        #endregion

        #region  窗体改变大小后 改变 ，根据新的窗体大小 重新绘制 框图
        /// <summary>
        /// 窗体改变大小后 改变 ，根据新的窗体大小 重新绘制 框图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
           //  Console.WriteLine("SizeChanged{0}", pictureBox1.Size);
            float  SSB1 = 0.0f, SSB2 = 0.0f;//横 纵 坐标 相对原点 平移 数据，由于变量名很长 所以 使用中间变量
            //SB1 = 0.0f, SB2 = 0.0f,
            if (g != null)
            {
                if ((pictureBox1.Width != 0) && (pictureBox1.Height != 0))//在最小化的 时候 数据为 0
                {
                    SSB1 = (CoordinateHighVerysmall.AbscissaDifference / CoordinateHighVerysmall.OrdinateDifference);
                    SSB2 = ((float)pictureBox1.Width / (float)pictureBox1.Height);
                    // Console.WriteLine("{0}  {1}", SSB1, SSB2);
                    //适应 显示窗口 计算 显示 缩放 比例
                    if (SSB1 > SSB2)
                    {//判断 字框 外轮廓 横纵比 与 pictureBox 横纵比
                        AmplificationFactor = (float)(pictureBox1.Width - (pictureBox1.Width / Data_Num1)) / CoordinateHighVerysmall.AbscissaDifference;//适应屏幕的 缩放 系数 
                                                                                                                                                        // Console.WriteLine("AmplificationFactor1:{0}", AmplificationFactor);
                    }
                    else
                    {//(pictureBox1.Width - 10)  (pictureBox1.Height- 10) 减10的目的是 放大后的 图框 不会 靠着边框 显示不全
                        AmplificationFactor = (float)(pictureBox1.Height - (pictureBox1.Height / Data_Num1)) / CoordinateHighVerysmall.OrdinateDifference;//适应屏幕的 缩放 系数
                                                                                                                                                          // Console.WriteLine("AmplificationFactor2:{0}", AmplificationFactor);
                    }
                    //极值 * 显示系数 计算
                    CoordinateHighVerysmall.AbscissaMax_X_2 = CoordinateHighVerysmall.AbscissaMax_X * AmplificationFactor;
                    CoordinateHighVerysmall.AbscissaMin_X_2 = CoordinateHighVerysmall.AbscissaMin_X * AmplificationFactor;
                    CoordinateHighVerysmall.OrdinateMax_Y_2 = CoordinateHighVerysmall.OrdinateMax_Y * AmplificationFactor;
                    CoordinateHighVerysmall.OrdinateMin_Y_2 = CoordinateHighVerysmall.OrdinateMin_Y * AmplificationFactor;

                   // SB1 = Convert.ToInt32(CoordinateHighVerysmall.AbscissaMax_X_2 - (CoordinateHighVerysmall.AbscissaDifference_2 / 2));
                    //SB2 = Convert.ToInt32(CoordinateHighVerysmall.OrdinateMax_Y_2 - (CoordinateHighVerysmall.OrdinateDifference_2 / 2));

                    buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    g = Graphics.FromImage(buffer);
                    //g.TranslateTransform((pictureBox1.Size.Width / 2), (pictureBox1.Size.Height / 2));//移动 坐标 
                    g.TranslateTransform((pictureBox1.Width / 2 - SB1), (pictureBox1.Height / 2 + SB2));//移动 坐标 
                    g.ScaleTransform(1, -1);//y轴 方向 调换
                    BlockDiagramDrawing();
                    Point_XY_T.X = 0;
                    Point_XY_T.Y = 0;//初始化
                }
            }
        }
        #endregion
        #region 常用设置界面
        /// <summary>
        /// 常用设置界面 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            formCommonSettings = new Form3();
            formCommonSettings.form1 = this;
            formCommonSettings.ShowDialog();
        }
        #endregion
        #region  加工参数
        /// <summary>
        ///  手动  送料 按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            bool Flag = false;//^([1-9]\d*\.\d{2})|(0\.[1-9]0)|(0\.0[1-9])$
            string str = textBox3.Text;
            float Length = 0;

               Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");
            if (Flag == false)
            {
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.00 10.123\"为无效数据",
                        "提示信息 手动送料长度", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if(ComDevice.IsOpen)
                {
   
                    CheckDataFlag = false;
                    button1.Enabled = false;//送料
                    button3.Enabled = false;//退料
                    button4.Enabled = true;//停止

                    Length = (float)Math.Round(Convert.ToSingle(str),2);
                    ShippingCost(Length);


                }
                else
                {
                    MessageBox.Show("请打开串口端口",
                    "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }


        }
        #endregion
        #region 手动 退料 按键
        /// <summary>
        /// 手动 退料 按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            bool Flag = false;//^([1-9]\d*\.\d{2})|(0\.[1-9]0)|(0\.0[1-9])$
            string str = textBox3.Text;
            float Length = 0;
            Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.[1-9][0-9]?$|^0\.0[1-9]$");
            if (Flag == false)
            {
                MessageBox.Show("请输入最多2位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过2位数\n" +
                    "例如:\"0\"\"0.\"\"0.0\"为无效数据",
                    "提示信息 手动送退料长度", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (ComDevice.IsOpen)
                {
                    button1.Enabled = false;//送料
                    button3.Enabled = false;//退料
                    button4.Enabled = true;//停止
 
                    Length = (float)Math.Round(Convert.ToSingle(str), 2);
                    ReturnMaterial(Length);
                }
                else
                {
                    MessageBox.Show("请打开串口端口",
                    "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }
        #endregion
        #region 送退料 停止
        /// <summary>
        /// 送退料 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            StopCost();
        }
        #endregion
        #region 手动送料长度
        /// <summary>
        /// 手动送料长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("{0 }Hello {1}", CountTem++, textBox3.Text);
            bool Flag = false;//^([1-9]\d*\.\d{2})|(0\.[1-9]0)|(0\.0[1-9])$
            string str = textBox3.Text;
            if(str.Length != 0)
            {
                  Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,2}$|^0\.?[0-9]?[1-9]?$|^0\.?[1-9]?[0-9]?$|^0\.?0?[1-9]?$");

                if (Flag == false)
                {
                    MessageBox.Show("请输入最多2位有效小数的正数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "小数点后不能超过2位数\n" +
                        "例如:\"0.00 10.123\"为无效数据",
                        "提示信息 手动送料长度", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    ManualFeeding = Convert.ToSingle(textBox3.Text);
                    WritePrivateProfileString("FeedingControl", "ManualFeeding", textBox3.Text, this.str);
                }
            }
            

            
        }
        #endregion
        #region 重复加工次数 数据
        /// <summary>
        /// 重复加工次数 数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;//^([1-9]\d*\.\d{2})|(0\.[1-9]0)|(0\.0[1-9])$
            string str = textBox4.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*$");
                if (Flag == false)
                {
                    MessageBox.Show("请输入大于 1 的正整数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "无小数点的正整数\n" +
                        "例如:\"0 1.2 -5\"为无效数据",
                        "提示信息 重复加工次数数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    RepeatProcessingTimes = Convert.ToInt32(textBox4.Text);
                    WritePrivateProfileString("SlottedControl", "RepeatProcessingTimes", textBox4.Text, this.str);
                }
            }               
        }
        #endregion
        # region 多线程定时器 倒计时 
        /// <summary>
        /// 多线程定时器 倒计时 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            CheckDataFlag = true;
        }
        #endregion
        #region 自动开槽 进程
        private void AutomaticSlottingProcess(object Var)
        {
            int i = 0,j = 0;
            SlottedState = 0;//开槽 状态
            SingleSlot();//单步开槽
            while(SlottedState == 0)
            {
                ;//等待 返回 状态
            }
            if(SlottedState != 1)//若 不是正常返回  恢复 界面 显示，结束 当前进程 退出
            {
                InterfaceDisplayProcessing();
                thread2.Abort();
                return;
            }
            for (i = 0;i < SlotManagementInformations.Count;i++)
            {
                for(j = 0;j < SlotManagementInformations[i].DisNumCoordinatePoint.Count;j++)
                {
                    SlottedState = 0;//开槽 状态
                    FeedSlotted(SlotManagementInformations[i].DisNumCoordinatePoint[j].SlottedLength);// 送料  开槽
                    while (SlottedState == 0)
                    {
                        ;//等待 返回 状态
                    }
                    if (SlottedState != 1)//若 不是正常返回  恢复 界面 显示，结束 当前进程 退出
                    {
                        InterfaceDisplayProcessing();
                        thread2.Abort();
                        return;
                    }
                }
            }
            SlottedMotorCancel();// 开槽电机 单独 停止
            InterfaceDisplayProcessing();
            thread2.Abort();
            timer.Stop();
            return;
        }
        #endregion

        #region 自动开槽控制 启动
        /// <summary>
        /// 自动开槽控制 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            #region 检测 数据有效性
            bool Flag = false;
            string str = textBox4.Text;// 重复加工次数数据 有效性 判断

             Flag = Regex.IsMatch(str, @"^[1-9]\d*$");
            if (Flag == false)
            {
                MessageBox.Show("请输入大于 1 的正整数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "无小数点的正整数\n" +
                    "例如:\"0 1.2 -5\"为无效数据",
                    "提示信息 重复加工次数数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Flag = false;
            str = textBox5.Text;// 材料厚度数据" 有效性 判断

            Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,1}$|^0\.[1-9]$");
            if (Flag == false)
            {
                MessageBox.Show("请输入最多1位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过1位数\n" +
                    "例如:\"0.0 0.12\"为无效数据",
                    "提示信息 材料厚度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Flag = false;
            str = textBox6.Text;// 材料剩余厚度数据" 有效性 判断

            Flag = Regex.IsMatch(str, @"^[1-9]\d*\.?\d{0,1}$|^0\.[1-9]$");
            if (Flag == false)
            {
                MessageBox.Show("请输入最多1位有效小数的正数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过1位数\n" +
                    "例如:\"0.0 0.12\"为无效数据",
                    "提示信息 材料剩余厚度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            if(MaterialThickness < MaterialRemainingThickness)//材料厚度  必须 大于等于 材料剩余厚度
            {
                MessageBox.Show("材料厚度 >= 材料剩余厚度\n" +
                    "请重新检查数据,重新输入数据\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "小数点后不能超过1位数\n" +
                    "例如:\"0.0 0.12\"为无效数据",
                    "提示信息 材料厚度 < 材料剩余厚度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Flag = false;
            str = textBox7.Text;// 开槽重复次数有效性 判断

            Flag = Regex.IsMatch(str, @"^[1-9]\d*$");
            if (Flag == false)
            {
                MessageBox.Show("请输入大于 1 的正整数\n" +
                    "输入数据必须为0-9的数字\n" +
                    "字母及\"+-*/\"都为无效数据\n" +
                    "无小数点的正整数\n" +
                    "例如:\"0 1.2 -5\"为无效数据",
                    "提示信息 开槽重复次数数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion
            if(SlotManagementChecked)
            {
                if (ComDevice.IsOpen)
                {
                    textBox3.Enabled = false;//送退料 长度输入 框
                    button1.Enabled = false;//手动送料  按键
                    button3.Enabled = false;//手动退料  按键
                    button4.Enabled = false;//手动送料 退料 停止  按键
                    button5.Enabled = false;//自动开槽  按键
                    button6.Enabled = true;//自动开槽  暂停 按键
                    button7.Enabled = true;//自动开槽  停止 按键
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;//开槽 数据 框
                    button8.Enabled = false;//设置  按键
                    button9.Enabled = false;//调试  按键
                    button10.Enabled = false;//开槽参数 按键
                    button11.Enabled = false;//送料 数据   按键

                    if (thread2 != null)
                    {
                        thread2.Abort();
                    }
                    
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread2 = new Thread(ParStart2);
                    //thread2.Priority = ThreadPriority.Lowest;
                    thread2.Start(0);
                }
                else
                {
                    MessageBox.Show("请打开串口端口",
                    "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("无加工文件",
                    "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            
   
        }
        #region 自动开槽 停止
        /// <summary>
        /// 自动开槽 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if (thread2 != null)
            {
                thread2.Abort();
            }
            InterfaceDisplayProcessing();
        }
        #endregion
        #endregion
        #region 材料厚度
        /// <summary>
        /// 材料厚度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox5.Text;
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
                        "提示信息 材料厚度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MaterialThickness = Convert.ToSingle(textBox5.Text);
                    WritePrivateProfileString("SlotParameters", "MaterialThickness", textBox5.Text, this.str);
                }
            }
        }
        #endregion
        #region 材料剩余厚度 数据
        /// <summary>
        /// 材料剩余厚度 数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox6.Text;
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
                        "提示信息 材料剩余厚度数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MaterialRemainingThickness = Convert.ToSingle(textBox6.Text);
                    WritePrivateProfileString("SlotParameters", "MaterialRemainingThickness", textBox6.Text, this.str);                   
                }
            }
        }
        #endregion
        #region 开槽重复次数
        /// <summary>
        /// 开槽重复次数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            bool Flag = false;
            string str = textBox7.Text;

            if (str.Length != 0)
            {
                Flag = Regex.IsMatch(str, @"^[1-9]\d*$");
                if (Flag == false)
                {
                    MessageBox.Show("请输入大于 1 的正整数\n" +
                        "输入数据必须为0-9的数字\n" +
                        "字母及\"+-*/\"都为无效数据\n" +
                        "无小数点的正整数\n" +
                        "例如:\"0 1.2 -5\"为无效数据",
                        "提示信息 开槽重复次数数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SlotNumberofRepetitions = Convert.ToInt32(textBox7.Text);
                    WritePrivateProfileString("SlotParameters", "SlotNumberofRepetitions", textBox7.Text, this.str);
                }
            }
        }
        #endregion
        #region 使当前 框图 适应  显示 区域
        /// <summary>
        /// 使当前 框图 适应  显示 区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
           // float SB1 = 0.0f, SB2 = 0.0f;//横 纵 坐标 相对原点 平移 数据，由于变量名很长 所以 使用中间变量
            if (g != null)
            {
                if ((pictureBox1.Width != 0) && (pictureBox1.Height != 0))//在最小化的 时候 数据为 0
                {
                    //适应 显示窗口 计算 显示 缩放 比例
                    if ((CoordinateHighVerysmall.AbscissaDifference / CoordinateHighVerysmall.OrdinateDifference) > (pictureBox1.Width / pictureBox1.Height))
                    {//判断 字框 外轮廓 横纵比 与 pictureBox 横纵比
                        AmplificationFactor = (pictureBox1.Width - (pictureBox1.Width / Data_Num1)) / CoordinateHighVerysmall.AbscissaDifference;//适应屏幕的 缩放 系数 
                    }
                    else
                    {//(pictureBox1.Width - 10)  (pictureBox1.Height- 10) 减10的目的是 放大后的 图框 不会 靠着边框 显示不全
                        AmplificationFactor = (pictureBox1.Height - (pictureBox1.Height / Data_Num1)) / CoordinateHighVerysmall.OrdinateDifference;//适应屏幕的 缩放 系数
                    }
                    //极值 * 显示系数 计算
                    CoordinateHighVerysmall.AbscissaMax_X_2 = CoordinateHighVerysmall.AbscissaMax_X * AmplificationFactor;
                    CoordinateHighVerysmall.AbscissaMin_X_2 = CoordinateHighVerysmall.AbscissaMin_X * AmplificationFactor;
                    CoordinateHighVerysmall.OrdinateMax_Y_2 = CoordinateHighVerysmall.OrdinateMax_Y * AmplificationFactor;
                    CoordinateHighVerysmall.OrdinateMin_Y_2 = CoordinateHighVerysmall.OrdinateMin_Y * AmplificationFactor;

                    //SB1 = Convert.ToInt32(CoordinateHighVerysmall.AbscissaMax_X_2 - (CoordinateHighVerysmall.AbscissaDifference_2 / 2));
                    //SB2 = Convert.ToInt32(CoordinateHighVerysmall.OrdinateMax_Y_2 - (CoordinateHighVerysmall.OrdinateDifference_2 / 2));

                    buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    g = Graphics.FromImage(buffer);
                    //g.TranslateTransform((pictureBox1.Size.Width / 2), (pictureBox1.Size.Height / 2));//移动 坐标 
                    g.TranslateTransform((pictureBox1.Width / 2 - SB1), (pictureBox1.Height / 2 + SB2));//移动 坐标 
                    g.ScaleTransform(1, -1);//y轴 方向 调换
                    
                    //上面的 是调整 横 纵 坐标的 适合 当前 显示 区域

                    FlagScale = 1.0f;//缩放系数 恢复 很重要 
                    //FlagScale2 = false;//
                    BlockDiagramDrawing();

                    Point_XY_T.X = 0;
                    Point_XY_T.Y = 0;//初始化
                }
            }
        }
        #endregion
        #region  右击菜单
        #region 拾取字框
        private void 拾取字框toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!SelectContour)//字框拾取
            {
                if(SlotManagementInformations.Count > 0)
                {
                    MessageBox.Show("已有拾取任务，请先取消拾取！",
                        "提示信息 字框拾取", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SelectContour = true;
                   
                    //SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
                    //SlottedSectionNumSum = 0;//选择加工 总段数
                    //SlottedSelectBoxsNumSum = 0;//选择加工 总字框数

                    //SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示



                    SelectContourCount = 0;//每次进入 字框拾取 时 拾取计数 用于撤销 和取消

                    拾取字框toolStripMenuItem.Checked = true;
                    SlotManagementChecked = false;
                    BlockDiagramDrawing();
                }
                
            }
        }
        #endregion
        #region 拾取全部
        private void 拾取全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 i = 0;

            if(!AllSelectContour)//拾取字框 
            {
                if (SlotManagementInformations.Count > 0)
                {
                    MessageBox.Show("已有拾取任务，请先取消拾取！",
                        "提示信息 拾取全部", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    AllSelectContour = true;

                    //SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
                    //SlottedSectionNumSum = 0;//选择加工 总段数
                    //SlottedSelectBoxsNumSum = 0;//选择加工 总字框数

                    //SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示

                    SelectContourCount = 0;
                    拾取全部ToolStripMenuItem.Checked = true;

                    for (i = 0; i < SlottedTaskInformations.Count; i++)
                    {
                        SlottedTaskInformations[i].StartSlottedPoint = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;// SlottedTaskInformations[i].ZoomStartPoint;
                        SlottedTaskInformations[i].EndSlottedPoint = SlottedTaskInformations[i].SlottedNodeInformations[0].SlottedSectionPoint;// SlottedTaskInformations[i].ZoomStartPoint;

                        SlottedTaskInformations[i].StartEndSlottedPointCount = 2;

                        SlotManagementInformation SlotManagementInformationTH = new SlotManagementInformation();
                        SlotManagementInformationTH.TaskNum = i;

                        SlotManagementInformations.Add(SlotManagementInformationTH);
                    }
                    SlotManagementCalcs.LengthCalculation(this, SlottedTaskInformations, SlotManagementInformations, 0, true);//从 头计算 完整 选取的 字框 数据
                    SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                    SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                    SlotManagementChecked = false;
                    BlockDiagramDrawing();
                }
            }
        }
        #endregion
        #region  撤销拾取
        private void 撤销拾取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 i = 0,k = 0;
            
            if (SelectContourCount > 0)
            {
                i = SlotManagementInformations.Count - 1;
                k = SlotManagementInformations[i].TaskNum;
                if (SlottedTaskInformations[k].StartEndSlottedPointCount == 2)
                {
                    SlottedTaskInformations[k].StartEndSlottedPointCount = 1;
                    SlotManagementInformations[i].DisNumCoordinatePoint.Clear();
                    SelectContourCount--;
                }
                else if (SlottedTaskInformations[k].StartEndSlottedPointCount == 1)
                {
                    SlottedTaskInformations[k].StartEndSlottedPointCount = 0;
                    SelectContourCount--;
                    SlotManagementInformations.RemoveAt(i);
                }

                
            }
            if (SelectContourCount > 0)
            {
                for (i = 0; i < SlotManagementInformations.Count; i++)
                {
                    k = SlotManagementInformations[i].TaskNum;
                    Console.WriteLine("撤销拾取*{0} {1} {2}", i, SlotManagementInformations[i].TaskNum, SlottedTaskInformations[k].StartEndSlottedPointCount);
                }
                SlotManagementCalcs.LengthCalculation(this, SlottedTaskInformations, SlotManagementInformations, 0, true);//从 头计算 完整 选取的 字框 数据
                SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
            }

            Console.WriteLine("撤销拾取 {0} {1}", SelectContourCount, SlotManagementInformations.Count);
                           
            BlockDiagramDrawing();
        }
        #endregion
        #region 删除节点
        private void 删除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!DeleteNode)
            {
                DeleteNode = true;//增加节点 
                BlockDiagramDrawing();
                删除节点ToolStripMenuItem.Checked = true;
            }
        }
        #endregion
        #region  增加节点
        private void 增加节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!AddNode)
            {
                AddNode = true;//增加节点 
                BlockDiagramDrawing();
                增加节点ToolStripMenuItem.Checked = true;
            }               
        }
        #endregion
        #region 确定
        private void 确定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 i = 0, j = 0,k = 0;

            if (AddNode)//增加节点 
            {
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                    {
                       if( SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode = false;
                        }                        
                    }
                }
                ZoomLengthCalculation();
                SlotManagementCalcs.LengthCalculation(this, SlottedTaskInformations, SlotManagementInformations, 0, true);//从 头计算 完整 选取的 字框 数据
                SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                AddNode = false;
                
                增加节点ToolStripMenuItem.Checked = false;
                BlockDiagramDrawing();
            }
            if(DeleteNode)//删除节点
            {
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                    {
                        if (SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations.RemoveAt(j);
                        }
                    }
                }
                ZoomLengthCalculation();
                SlotManagementCalcs.LengthCalculation(this, SlottedTaskInformations, SlotManagementInformations, 0, true);//从 头计算 完整 选取的 字框 数据
                SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                DeleteNode = false;
                BlockDiagramDrawing();
                删除节点ToolStripMenuItem.Checked = false;
            }
            if (SelectContour)//字框拾取
            {
                for(i = 0; i < SlotManagementInformations.Count;)
                {
                    k = SlotManagementInformations[i].TaskNum;
                    if (SlottedTaskInformations[k].StartEndSlottedPointCount == 1)
                    {
                        SlotManagementInformations.RemoveAt(i);
                        SlottedTaskInformations[k].StartEndSlottedPointCount = 0;
                        SelectContourCount--;
                       // Console.WriteLine("确定#{0} {1} {2} {3}", i, SelectContourCount, SlotManagementInformations.Count, SlottedTaskInformations[k].StartEndSlottedPointCount);
                        continue;
                    }
                    i++;
                }
                if(SlotManagementInformations.Count > 0)
                {
                    SlotManagementChecked = true;
                    //for (i = 0; i < SlotManagementInformations.Count; i++)
                    //{
                    //    Console.WriteLine("确定*{0} {1}", i, SlotManagementInformations[i].TaskNum);
                    //}
                    
                }
                else
                {
                    SlotManagementChecked = false;
                }
              //  Console.WriteLine("确定{0} {1}", SelectContourCount, SlotManagementInformations.Count);

                SelectContour = false;
                拾取字框toolStripMenuItem.Checked = false;
                BlockDiagramDrawing();
            }
            if (AllSelectContour)//全部 拾取 字框 
            {
                AllSelectContour = false;
                拾取全部ToolStripMenuItem.Checked = false;
                SlotManagementChecked = true;
                BlockDiagramDrawing();
            }
        }
        #endregion
        #region  取消
        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 i = 0, j = 0;

            if (AddNode)//增加节点 
            {
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                    {
                        if (SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations.RemoveAt(j);
                        }
                    }
                }

                AddNode = false;
                BlockDiagramDrawing();
                增加节点ToolStripMenuItem.Checked = false;
                return;
            }
            if (DeleteNode)//删除节点
            {
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                    {
                        if (SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode)
                        {
                            SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode = false;
                        }
                    }
                }

                DeleteNode = false;
                BlockDiagramDrawing();
                删除节点ToolStripMenuItem.Checked = false;
                return;
            }

            if((SlotManagementInformations.Count > 0) || SelectContour)
            {
                for (i = 0; i < SlottedTaskInformations.Count; i++)
                {
                    SlottedTaskInformations[i].StartEndSlottedPointCount = 0;
                }
                SlotManagementInformations.Clear();

                SlottedSectionLengthSum = 0;//开槽选取后  字框  总长
                SlottedSectionNumSum = 0;//选择加工 总段数
                SlottedSelectBoxsNumSum = 0;//选择加工 总字框数

                SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                AllSelectContour = false;
                拾取全部ToolStripMenuItem.Checked = false;
                SlotManagementChecked = false;

                SelectContourCount = 0;
                SelectContour = false;
                拾取字框toolStripMenuItem.Checked = false;
                BlockDiagramDrawing();
            }
            
        }
        #endregion
        #region 图款显示  鼠标选择 
        /// <summary>
        /// 图款显示  鼠标选择 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Int32 i = 0, j = 0, k = 0, k2 = 0, i2 = 0;
            bool Flag = false,Flag2 = false;

            if (FlagCursorLeft)
            {
                //Console.WriteLine("Hello{0}",(int)Math.Round((double)(SizeSlottedWidth/2)));
                if (AddNode)//增加节点 
                {
                    for (i = 0; i < SlottedTaskInformations.Count; i++)
                    {
                     //   k = 0;
                        for (j = SlottedTaskInformations[i].ZoomStartPoint; j < SlottedTaskInformations[i].ZoomEndPoint; j++)
                        {
                           // Console.WriteLine("任务 {0} 节点{1} {2}", i,j,PointXY_2_2[j]);
                            //if (k < SlottedTaskInformations[i].SlottedNodeInformations.Count)
                            //{
                            //    k2 = SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionPoint; 
                            //    if (j == k2)
                            //    {

                            //        if ((ScreenCoordinates.X <= (PointXY_2_2[j].X + 3) && (ScreenCoordinates.X >= (PointXY_2_2[j].X - 3))) && (ScreenCoordinates.Y <= (PointXY_2_2[j].Y + 3) && (ScreenCoordinates.Y >= (PointXY_2_2[j].Y - 3))))
                            //        {
                            //            k++;
                            //            Console.WriteLine("break");
                            //            break;
                            //        }
                            //    }                              
                            //}
                            Flag2 = false;
                            for (k = 0; k < SlottedTaskInformations[i].SlottedNodeInformations.Count;k++)
                            {
                                k2 = SlottedTaskInformations[i].SlottedNodeInformations[k].SlottedSectionPoint;
                                if (j == k2)
                                {
                                    if ((ScreenCoordinates.X <= (PointXY_2_2[j].X + 3) && (ScreenCoordinates.X >= (PointXY_2_2[j].X - 3))) && (ScreenCoordinates.Y <= (PointXY_2_2[j].Y + 3) && (ScreenCoordinates.Y >= (PointXY_2_2[j].Y - 3))))
                                    {
                                      //  Console.WriteLine("break");
                                        Flag2 = true;
                                        break;
                                    }
                                }
                                
                            }
                            if (Flag2)
                                break;
                            if ((ScreenCoordinates.X <= (PointXY_2_2[j].X + 3) && (ScreenCoordinates.X >= (PointXY_2_2[j].X - 3))) && (ScreenCoordinates.Y <= (PointXY_2_2[j].Y + 3) && (ScreenCoordinates.Y >= (PointXY_2_2[j].Y - 3))))
                            {
                            //    Console.WriteLine("break2");
                                SlottedNodeInformation TemporaryNodeInformation = new SlottedNodeInformation();

                                TemporaryNodeInformation.SlottedSectionPoint = j;
                                TemporaryNodeInformation.NewDeleteNode = true;//新增的 开槽点
                                SlottedTaskInformations[i].SlottedNodeInformations.Insert(k, TemporaryNodeInformation);
                                BlockDiagramDrawing();

                                Flag2 = true;
                                break;
                            }
                        }
                        if (Flag2)
                            break;
                    }
                }
                if (DeleteNode)//删除节点
                {
                    for (i = 0; i < SlottedTaskInformations.Count; i++)
                    {
                        for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                        {
                            k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
                            if ((ScreenCoordinates.X <= (PointXY_2_2[k].X + 3) && (ScreenCoordinates.X >= (PointXY_2_2[k].X - 3))) && (ScreenCoordinates.Y <= (PointXY_2_2[k].Y + 3) && (ScreenCoordinates.Y >= (PointXY_2_2[k].Y - 3))))
                            {
                              //   Console.WriteLine("break3");
                                SlottedTaskInformations[i].SlottedNodeInformations[j].NewDeleteNode = true;//删除 开槽点
                                BlockDiagramDrawing();
                                break;
                            }
                        }
                    }
                }
                if (SelectContour)//拾取字框 
                {
                    for (i = 0; i < SlottedTaskInformations.Count; i++)
                    {
                        for (j = 0; j < SlottedTaskInformations[i].SlottedNodeInformations.Count; j++)
                        {
                            k = SlottedTaskInformations[i].SlottedNodeInformations[j].SlottedSectionPoint;
                            if ((ScreenCoordinates.X <= (PointXY_2_2[k].X + 3) && (ScreenCoordinates.X >= (PointXY_2_2[k].X - 3))) && (ScreenCoordinates.Y <= (PointXY_2_2[k].Y + 3) && (ScreenCoordinates.Y >= (PointXY_2_2[k].Y - 3))))
                            {
                                switch (SlottedTaskInformations[i].StartEndSlottedPointCount)
                                {
                                    case 0:

                                    case 2:
                                     //   Console.WriteLine("#{0} {1} {2}", i, SelectContourCount, SlotManagementInformations.Count);
                                        SlottedTaskInformations[i].StartSlottedPoint = k;
                                        if (SlottedTaskInformations[i].StartEndSlottedPointCount == 2)
                                        {
                                            for (i2 = 0; i2 < SlotManagementInformations.Count; i2++)
                                            {
                                                if (i == SlotManagementInformations[i2].TaskNum)
                                                {
                                                    SlotManagementInformations.RemoveAt(i2);
                                                    SelectContourCount -= 2;
                                                    Flag = true;
                                                    break;
                                                }
                                            }
                                        }
                                        SlottedTaskInformations[i].StartEndSlottedPointCount = 1;
                                        SelectContourCount++;
                                        SlotManagementInformation SlotManagementInformationTH = new SlotManagementInformation();
                                        SlotManagementInformationTH.TaskNum = i;
                                        SlotManagementInformations.Add(SlotManagementInformationTH);
                                      //  Console.WriteLine("*{0} {1} {2}", i, SelectContourCount, SlotManagementInformations.Count);
                                        if(Flag)
                                        {
                                            SlotManagementCalcs.LengthCalculation(this, SlottedTaskInformations, SlotManagementInformations, 0, true);//从 头计算 完整 选取的 字框 数据
                                            SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                                            SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                                            Flag = false;
                                        }
                                        
                                        BlockDiagramDrawing();
                                        goto BREAK;

                                    case 1:
                                        SlottedTaskInformations[i].EndSlottedPoint = k;
                                        SlottedTaskInformations[i].StartEndSlottedPointCount = 2;

                                        //  SlotManagementInformations.SlottedOrder.Add(i);
                                        SelectContourCount++;
                                      //  Console.WriteLine("**{0} {1} {2}", i, SelectContourCount, SlotManagementInformations.Count);
                                        // SlotManagementInformations.LengthCalculation(this, SlottedTaskInformations, i);//计算 显示 当前任务选择  数据
                                        SlotManagementCalcs.LengthCalculation(this, SlottedTaskInformations, SlotManagementInformations, i,false);
                                        SlotManagementCalcs.ListView_1_Display(this, SlotManagementInformations);//选取段数数据
                                        SlotManagementCalcs.ListView_2_Display(this);//轮廓数据显示
                                        BlockDiagramDrawing();
                                        goto BREAK;
                                }
                                BREAK:
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #endregion
        #region 调试界面
        /// <summary>
        /// 调试界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            if (ComDevice.IsOpen)
            {
                DebuggingForm.form1 = this;
                DebuggingForm.ShowDialog();
            }
            else
            {

                MessageBox.Show("请打开串口端口",
                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 送料 脉冲基准
        /// <summary>
        /// 送料 脉冲基准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            PulseRefForm = new Form7();
            PulseRefForm.form1 = this;
            PulseRefForm.ShowDialog();
        }


        #endregion
        #region 主界面关闭 窗口 关闭 开的进程
        /// <summary>
        /// 主界面关闭 窗口 关闭 开的进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(thread != null)
            {
                thread.Abort();
            }
            if (thread2 != null)
            {
                thread2.Abort();
            }
          //  DebuggingForm.Dispose();
            this.Dispose();

        }
        #endregion
        #region 框图移动 锁定
        /// <summary>
        /// 框图移动 锁定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MoveLock == false)
            {
                MoveLock = true;
                toolStripButton3.Image = global::开槽机.Properties.Resources.dagoukuang;
            }
            else
            {
                MoveLock = false;
                toolStripButton3.Image = global::开槽机.Properties.Resources.没打勾框;
            }
                
        }
        #endregion
        #region 显示区域 进入 捕获 鼠标
        /// <summary>
        /// 显示区域 进入 捕获 鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
        #endregion
        #region 颜色设置 界面
        /// <summary>
        /// 颜色设置 界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            Form4 ColorSet = new Form4();
            ColorSet.form1 = this;
            ColorSet.ShowDialog();
        }
        #endregion
    }
}
