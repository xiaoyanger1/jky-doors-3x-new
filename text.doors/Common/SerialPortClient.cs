using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Default;

namespace text.doors.Common
{
    public class SerialPortClient
    {
        public static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();

        public SerialPort sp = new SerialPort();

        public static string _protName = System.Configuration.ConfigurationSettings.AppSettings["ProtName"].ToString();
        public static string _baudRate = System.Configuration.ConfigurationSettings.AppSettings["BaudRate"].ToString();
        public static string _dataBits = System.Configuration.ConfigurationSettings.AppSettings["DataBits"].ToString();
        public static string _stopBits = System.Configuration.ConfigurationSettings.AppSettings["StopBits"].ToString();
        public static string _parity = System.Configuration.ConfigurationSettings.AppSettings["Parity"].ToString();
        public static string _slaveID = System.Configuration.ConfigurationSettings.AppSettings["SlaveID"].ToString();

        public ModbusSerialMaster _MASTER;
        private static readonly Object syncLock = new Object();

        private ushort _StartAddress = 0;
        private ushort _NumOfPoints = 1;
        private byte _SlaveID = byte.Parse(_slaveID);

        public SerialPortClient()
        {
            sp.PortName = _protName;//串口号
            sp.BaudRate = int.Parse(_baudRate);//波特率
            sp.DataBits = int.Parse(_dataBits);//数据位
            sp.StopBits = (StopBits)int.Parse(_stopBits);//停止位
            if (_parity == "None")
            {
                sp.Parity = Parity.None;//校验位
            }
            else if (_parity == "Even")
            {
                sp.Parity = Parity.Even;//校验位
            }

            sp.ReadTimeout = 1500;//读取数据的超时时间，引发ReadExisting异常
        }


        public void Init()
        {
            string[] strPortNames = SerialPort.GetPortNames();

            if (strPortNames == null || strPortNames.Length == 0)
            {
                MessageBox.Show("未找到串口！", "串口", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }
        public void SerialPortOpen()
        {
            if (_MASTER != null)
                _MASTER.Dispose();
            try
            {
                if (sp.IsOpen)
                {
                    sp.Close();
                    sp.Open();//打开串口
                }
                else
                {
                    sp.Open();//打开串口
                }
                //由客户端创建Modbus TCP的主
                _MASTER = ModbusSerialMaster.CreateAscii(sp);
                _MASTER.Transport.Retries = 0;   //不必调试
                _MASTER.Transport.ReadTimeout = 1500;//读取超时

            }
            catch (Exception ex)
            {
                sp.Close();
            }
        }


        #region 气密

        /// <summary>
        /// 设置正压预备
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SetZYYB()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压预备);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取正压预备结束
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetZYYBJS()
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压预备结束);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        /// <summary>
        /// 发送正压开始
        /// </summary>
        public bool SendZYKS()
        {
            try
            {

                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压开始);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 读取正压开始结束
        /// </summary>
        /// <param name="IsSuccess"></param>
        public double GetZYKSJS()
        {
            double res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压开始结束);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = double.Parse(holding_register[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        /// <summary>
        /// 发送负压预备
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SendFYYB()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压预备);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// 发送负压开始
        /// </summary>
        public bool SendFYKS()
        {
            if (sp.IsOpen)
            {
                try
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压开始);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 读取正压预备结束
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetFYYBJS()
        {
            int res = 0;

            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压预备结束);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        /// <summary>
        /// 读取负压开始结束
        /// </summary>
        /// <param name="IsSuccess"></param>
        public double GetFYKSJS()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                lock (syncLock)
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压开始结束);
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    res = double.Parse(holding_register[0].ToString());
                }
            }
            return res;
        }



        /// <summary>
        /// 获取正压预备时，设定压力值
        /// </summary>
        public double GetZYYBYLZ(string type)
        {
            double res = 0;

            if (sp.IsOpen)
            {
                lock (syncLock)
                {
                    if (type == "ZYYB")
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压预备_设定值);
                    }
                    else if (type == "ZYKS")
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压开始_设定值);
                    }
                    else if (type == "FYYB")
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压预备_设定值);
                    }
                    else if (type == "FYKS")
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压开始_设定值);
                    }

                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    res = double.Parse((double.Parse(holding_register[0].ToString())).ToString());
                }
            }

            return res;
        }





        /// <summary>
        /// 获取正压是否开始计时
        /// </summary>
        public bool GetQiMiTimeStart(string key)
        {
            if (sp.IsOpen)
            {
                lock (syncLock)
                {
                    _StartAddress = BFMCommand.GetCommandDict(key);
                    ushort[] t = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (Convert.ToInt32(t[0]) > 5)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }


        #endregion

        #region 水密

        /// <summary>
        /// 设置水密预备
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SetSMYB()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密性预备加压);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


        /// <summary>
        /// 读取水密预备结束
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetSMYBJS(ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密预备结束);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        /// <summary>
        /// 读取水密预备结束——波动
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetSMXYJJS_BD(ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密预备结束);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        /// <summary>
        /// 读取水密波动加压结束
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetSMJS_BD(ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密开始波动结束);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }


        /// <summary>
        /// 读取水密预备设定压力-稳定加压
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetSMYBSDYL(ref bool IsSuccess, string type)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (type == "SMYB")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密预备_设定值);
                        }
                        else if (type == "SMKS")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密开始_设定值);
                        }
                        else if (type == "YCJY")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密依次加压_设定值);
                        }
                        else if (type == "XYJ")
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密开始_设定值);
                        }

                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                    }
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        /// <summary>
        /// 发送水密开始-稳定加压
        /// </summary>
        public bool SendSMXKS()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水密性开始);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 发送水密性下一级 -稳定加压
        /// </summary>
        public bool SendSMXXYJ()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.下一级);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 设置水密依次加压
        /// </summary>
        public bool SendSMYCJY(double value)
        {
            try
            {

                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.依次加压数值);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.依次加压);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        /// <summary>
        /// 急停
        /// </summary>
        public bool Stop()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.急停);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion


        #region 波动加压

        public bool SendSMXKS_波动()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.国标检测波动加压开始);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 停止波动
        /// </summary>
        public bool StopBoDong()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.工程检测水密性停止加压);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 设置水密-工程检测波动开始波动
        /// </summary>
        public bool SendBoDongksjy(double maxValue, double minValue)
        {
            try
            {

                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.上限压力设定);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(maxValue));

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.下限压力设定);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(minValue));


                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.工程检测水密性波动开始);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 切换波动
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool qiehuanTab(bool type)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.国标检测波动加压开始);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, type);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


        #endregion


        #region 位移

        /// <summary>
        /// 获取位移传感器1
        /// </summary>
        public double GetDisplace1()
        {
            double res = 0;

            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.位移1);
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        res = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.位移传感器1, float.Parse(res.ToString()));
                    }
                }
                catch (Exception) { }
            }
            return res;
        }

        /// <summary>
        /// 获取位移传感器2
        /// </summary>
        public double GetDisplace2()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.位移2);
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        res = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.位移传感器2, float.Parse(res.ToString()));
                    }
                }
                catch (Exception)
                { }
            }
            return res;
        }


        /// <summary>
        /// 获取位移传感器3
        /// </summary>
        public double GetDisplace3()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.位移3);
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        res = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.位移传感器3, float.Parse(res.ToString()));
                    }
                }
                catch (Exception) { }
            }
            return res;
        }

        /// <summary>
        /// 设置位移归零
        /// </summary>
        public bool SendWYGL()
        {
            try
            {
                if (sp.IsOpen)
                {
                    //lock (syncLock)
                    //{
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.位移1标零);
                    bool[] readCoils = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                    if (readCoils[0])
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    else
                    {
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
            return true;
        }



        /// <summary>
        /// 设置位移归零
        /// </summary>
        public bool SendWYGL2()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.位移2标零);
                        bool[] readCoils2 = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils2[0])
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


        /// <summary>
        /// 设置位移归零
        /// </summary>
        public bool SendWYGL3()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.位移3标零);
                        bool[] readCoils3 = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils3[0])
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }




        /// <summary>
        /// 设置抗风压正压预备
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SetKFYZYYB()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压正压预备);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送正压开始
        /// </summary>
        public bool SendKFYZYKS()
        {
            try
            {

                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压正压开始);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送抗风压负压预备
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SendKFYFYYB()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {

                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压负压预备);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// 发送抗风压负压开始
        /// </summary>
        public bool SendKFYFYKS()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压负压开始);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(_SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置风压安全反复数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set_FY_Value(string commandValue, string commandStr, double value, bool isZ = true)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(commandValue);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));

                        _StartAddress = BFMCommand.GetCommandDict(commandStr);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }




        /// <summary>
        /// 获取风压是否开始计时
        /// </summary>
        public bool Read_FY_Static_IsStart(string commandStr)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(commandStr);
                        ushort[] t = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (commandStr == BFMCommand.风压安全_负压是否计时 || commandStr == BFMCommand.风压安全_正压是否计时)
                        {
                            if (Convert.ToInt32(t[0]) >= 15)
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            if (Convert.ToInt32(t[0]) >= 40)
                                return true;
                            else
                                return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Info(ex);
                return false;
            }

        }


        /// <summary>
        /// 读取风压按钮状态
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int Read_FY_BtnType(string commandStr, ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(commandStr);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }



        /// <summary>
        /// 获取正压预备时，设定压力值
        /// </summary>
        public double Read_FY_Btn_SetValue(string type)
        {
            double res = 0;

            if (type == "ZYYB")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压_正压预备_设定值);
            }
            else if (type == "ZYKS")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压_正压开始_设定值);
            }
            else if (type == "FYYB")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压_负压预备_设定值);
            }
            else if (type == "FYKS")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风压_负压开始_设定值);
            }

            else if (type == "ZAQ")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正安全数值);
            }
            else if (type == "FAQ")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负安全数值);
            }
            else if (type == "ZFF")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正反复数值);
            }
            else if (type == "FFF")
            {
                _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负反复数值);
            }
            if (sp.IsOpen)
            {
                lock (syncLock)
                {
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    res = double.Parse((double.Parse(holding_register[0].ToString())).ToString());
                }
            }
            return res;
        }


        /// <summary>
        /// 发送风压按钮
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <param name="isZ">是否正压</param>
        public bool Send_FY_Btn(string commandStr, bool isZ = true)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(commandStr);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 首页


        /// <summary>
        /// 设置高压标0
        /// </summary>
        public bool SendGYBD(ref bool _GaoYaGuiLing, bool logon = false)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.高压标0_交替型按钮);
                        bool[] readCoils = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils[0])
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _GaoYaGuiLing = false;
                        }
                        else
                        {
                            if (logon == false)
                                _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _GaoYaGuiLing = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置低压标0
        /// </summary>
        public bool SendDYBD(ref bool _DiyaGuiLing, bool logon = false)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.低压标0_交替型按钮);
                        bool[] readCoils = _MASTER.ReadCoils(_SlaveID, _StartAddress, _NumOfPoints);
                        if (readCoils[0])
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _DiyaGuiLing = false;
                        }
                        else
                        {
                            if (logon == false)
                                _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _DiyaGuiLing = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 开关量控制
        /// </summary>
        public bool Sendkglkz()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.开关量控制);

                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取温度显示
        /// </summary>
        public double GetWDXS()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.温度显示);

                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        res = double.Parse((double.Parse(holding_register[0].ToString()) / 10).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.温度传感器, float.Parse(res.ToString()));
                    }
                }
                catch (Exception ex)
                { }

            }

            return res;
        }

        /// <summary>
        /// 获取大气压力显示
        /// </summary>
        public double GetDQYLXS()
        {
            double res = 0;

            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.大气压力显示);
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        res = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.大气压力传感器, float.Parse(res.ToString()));
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return res;
        }

        /// <summary>
        /// 获取风速显示
        /// </summary>
        public double GetFSXS()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风速显示);
                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        var f = double.Parse((double.Parse(holding_register[0].ToString()) / 100).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.风速传感器, float.Parse(f.ToString()));
                    }
                }
                catch (Exception ex) { }
            }
            return res;
        }


        /// <summary>
        /// 读取差压高显示
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int GetCY_High()
        {
            double res = 0;

            if (sp.IsOpen)
            {
                try
                {
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.差压高显示);

                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        var f = double.Parse(holding_register[0].ToString()); ;

                        if (int.Parse(holding_register[0].ToString()) > 10000)
                            f = -(65535 - int.Parse(holding_register[0].ToString()));
                        else
                            f = int.Parse(holding_register[0].ToString());

                        res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器高, float.Parse(f.ToString()));

                        return int.Parse(Math.Round(res, 0).ToString());
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return 0;
        }


        /// <summary>
        /// 读取抗风压极差
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int GetKFYjC()
        {
            int res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.改变级差);

                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (holding_register.Length > 0)
                        {
                            res = int.Parse(holding_register[0].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return res;
        }

        /// <summary>
        /// 读取差压低传感器显示
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int GetCY_Low()
        {
            double res = 0;
            if (sp.IsOpen)
            {
                try
                {
                    //lock (syncLock)
                    //{
                    _StartAddress = BFMCommand.GetCommandDict(BFMCommand.差压低显示);

                    ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                    if (holding_register.Length > 0)
                    {
                        var f = double.Parse(holding_register[0].ToString());

                        if (int.Parse(holding_register[0].ToString()) > 10000)
                            f = -(65535 - int.Parse(holding_register[0].ToString()));
                        else
                            f = int.Parse(holding_register[0].ToString());

                        f = double.Parse((f / 10).ToString());
                        res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器低, float.Parse(f.ToString()));
                        return int.Parse(Math.Round(res, 0).ToString());
                        //}
                    }
                }
                catch (Exception ex) { }
            }
            return 0;
        }
        /// <summary>
        /// 读取波动差压显示（min ,max ）
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public void GetCYXS_BODONG(ref bool IsSuccess, ref int minVal, ref int maxVal)
        {
            if (sp.IsOpen)
            {
                try
                {
                    lock (syncLock)
                    {
                        //最小
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.读取设定波动加压Min);

                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (holding_register.Length > 0)
                        {
                            var f = double.Parse(holding_register[0].ToString());

                            if (int.Parse(holding_register[0].ToString()) > 10000)
                                f = -(65535 - int.Parse(holding_register[0].ToString()));
                            else
                                f = int.Parse(holding_register[0].ToString());

                            var res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器高, float.Parse(f.ToString()));
                            minVal = int.Parse(Math.Round(res, 0).ToString());
                        }

                        //最大
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.读取设定波动加压Max);

                        ushort[] holding_register1 = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        if (holding_register1.Length > 0)
                        {
                            var f = double.Parse(holding_register1[0].ToString());

                            if (int.Parse(holding_register1[0].ToString()) > 10000)
                                f = -(65535 - int.Parse(holding_register1[0].ToString()));
                            else
                                f = int.Parse(holding_register1[0].ToString());

                            var res = Formula.GetValues(PublicEnum.DemarcateType.差压传感器高, float.Parse(f.ToString()));
                            maxVal = int.Parse(Math.Round(res, 0).ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsSuccess = false;
                }
            }
        }



        /// <summary>
        /// 设置风机控制
        /// </summary>
        public bool SendFJKZ(double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风机控制);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// 获取风机显示
        /// </summary>
        public double ReadFJSD(ref bool IsSuccess)
        {
            double res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风机设定值);
                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;

            }
            return res;
        }

        /// <summary>
        /// 设置正压阀
        /// </summary>
        public bool SendZYF()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压阀);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压阀);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置负压阀
        /// </summary>
        public bool SendFYF()
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.正压阀);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.负压阀);
                        _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }



        /// <summary>
        /// 风机启动
        /// </summary>
        public bool SendFengJiQiDong(ref bool _FengJiQiDongStat)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.风机启动);
                        if (_FengJiQiDongStat)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _FengJiQiDongStat = false;
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _FengJiQiDongStat = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 水泵启动
        /// </summary>
        public bool SendShuiBengQiDong(ref bool _ShuiBengQiDong)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.水泵启动);
                        if (_ShuiBengQiDong)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _ShuiBengQiDong = false;
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _ShuiBengQiDong = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 保护阀通
        /// </summary>
        public bool SendBaoHuFaTong(ref bool _ShuiBengQiDong)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.保护阀通);
                        if (_ShuiBengQiDong)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _ShuiBengQiDong = false;
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _ShuiBengQiDong = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 四通阀开
        /// </summary>
        public bool SendSiTongFaKai(ref bool _SiTongFaKai)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.四通阀开);
                        if (_SiTongFaKai)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _SiTongFaKai = false;
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _SiTongFaKai = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 点动开
        /// </summary>
        public bool SendDianDongKai(bool isdown)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (isdown)
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动开);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动开);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// 点动关
        /// </summary>
        public bool SendDianDongGuan(bool isdown)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (isdown)
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动关);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                        }
                        else
                        {
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动关);
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关到头
        /// </summary>
        public bool SendGuanDaoTou(ref bool _GuanDaoTou)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.点动关);
                        if (_GuanDaoTou)
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, false);
                            _GuanDaoTou = false;
                        }
                        else
                        {
                            _MASTER.WriteSingleCoil(this._SlaveID, _StartAddress, true);
                            _GuanDaoTou = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 写入PID
        /// </summary>
        /// <param name="IsSuccess"></param>
        public bool SendPid(string type, double value)
        {
            try
            {
                lock (syncLock)
                {
                    if (sp.IsOpen)
                    {
                        if (type == "P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.P);
                        else if (type == "I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.I);
                        else if (type == "D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.D);
                        else if (type == "_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._P);
                        else if (type == "_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._I);
                        else if (type == "_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._D);
                        else if (type == "B_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_P);
                        else if (type == "B_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_I);
                        else if (type == "B_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_D);

                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取PID
        /// </summary>
        /// <param name="IsSuccess"></param>
        public int GetPID(string type, ref bool IsSuccess)
        {
            int res = 0;
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        if (type == "P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.P);
                        else if (type == "I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.I);
                        else if (type == "D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.D);

                        else if (type == "_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._P);
                        else if (type == "_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._I);
                        else if (type == "_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand._D);

                        else if (type == "B_P")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_P);
                        else if (type == "B_I")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_I);
                        else if (type == "B_D")
                            _StartAddress = BFMCommand.GetCommandDict(BFMCommand.B_D);


                        ushort[] holding_register = _MASTER.ReadHoldingRegisters(_SlaveID, _StartAddress, _NumOfPoints);
                        res = int.Parse(holding_register[0].ToString());
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            return res;
        }

        #endregion



        /// <summary>
        /// 设置改变级差
        /// </summary>
        public bool SendGBJC(double value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    lock (syncLock)
                    {
                        _StartAddress = BFMCommand.GetCommandDict(BFMCommand.改变级差);
                        _MASTER.WriteSingleRegister(_SlaveID, _StartAddress, (ushort)(value));
                    }
                }
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }


    }
}
