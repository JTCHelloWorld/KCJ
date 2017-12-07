using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 开槽机
{
    class CRC16
    {
        /*计算CRC码的步骤为：
(1).预置16位寄存器为FFFFH。称此寄存器为CRC寄存器；
(2).把第一个8位数据与CRC寄存器的低位相异或，把结果放于CRC寄存器；
(3).把寄存器的内容右移一位(朝低位)，用0填补最高位，检查最低位；//此最低位是右移之前的最低位，即右移出来的一位
(4).如果最低位为0：重复第3步(再次移位)如果最低位为1：CRC寄存器与多项式A001（1010 0000 0000 0001）进行异或；
(5).重复步骤3和4，直到右移8次，这样整个8位数据全部进行了处理；
(6).重复步骤2到步骤5，进行下一个8位数据的处理；
(7).最后得到的CRC寄存器即为CRC码。(CRC码 = CRC_L +CRC_H)
*/
        /*******************************************************************************
        * Function Name :u16 CRC16(u8 *framea,u16 DATA_Len)
        * Description   :CRC16 校验 生成 和 校验 程序
        * Input         :数据开始指针 参与 运算的 数据长度
        * Output        :CRC16 校验 数据
        * Other         :数据长度 DATA_Len >=2
        * Date          :2014年5月8日13:25:47
        *******************************************************************************/
        public UInt16 CRC16_Calc(byte[] framea, UInt16 DATA_Len)
        {
            byte CRC16Lo, CRC16Hi;     //CRC寄存器
            byte SaveLo, SaveHi;       //CRC缓存寄存器
            byte GLo, GHi;             //生成多项式
            UInt16 i;                    //需校验的字节位数
            UInt16 Flag;                 //移位次数

            CRC16Lo = 0xff;
            CRC16Hi = 0xff;           //寄存器赋初值

            GLo = 0x01;
            GHi = 0xa0;               //多项式a001(HEX)=x15+x13+1

            for (i = 0; i < DATA_Len; i++)
            {
                CRC16Lo = (byte)(CRC16Lo ^ framea[i]);//按位与CRC寄存器异或

                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveLo = CRC16Lo;
                    SaveHi = CRC16Hi;
                    CRC16Hi = (byte)(CRC16Hi >> 1);     //高字节右移一位
                    CRC16Lo = (byte)(CRC16Lo >> 1);     //低字节右移一位
                    if ((SaveHi & 0x01) == 0x01)    //刚刚高位右移了，若高位移位前 最低位为1 则 低8位有以后高位 就是 1
                    {
                        CRC16Lo = (byte)(CRC16Lo | 0x80);
                    }
                    if ((SaveLo & 0x01) == 0x01)        //低8位 右移 前最低位为 1 则 移位后 需 异或 多项式
                    {
                        CRC16Hi = (byte)(CRC16Hi ^ GHi);
                        CRC16Lo = (byte)(CRC16Lo ^ GLo);
                    }
                }
            }
            return (byte)(((CRC16Hi << 8) | CRC16Lo));//注意modbus是输出的字为CRCL在前CRCH在后
        }
    }
}
