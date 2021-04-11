using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Service;

namespace text.doors.Default
{
    /// <summary>
    /// 默认配置
    /// </summary>
    public static class DefaultBase
    {
        #region --系统默认

        public static bool isRelease = false;
        /// <summary>
        /// 是否打开审核页面
        /// </summary>
        public static bool IsOpenComplexAssessment { get; set; }

        /// <summary>
        /// 当前设置测试的项目
        /// </summary>
        public static string base_TestItem = "";
        /// <summary>
        /// 规格数量
        /// </summary>
        public static int base_SpecCount = 0;
        /// <summary>
        /// 确定是否设置樘号
        /// </summary>
        public static bool IsSetTong = false;

        /// <summary>
        /// 是否锁点
        /// </summary>
        public static bool LockPoint = false;

        /// <summary>
        /// 杆件长度
        /// </summary>
        public static int BarLength = 0;


        /// <summary>
        /// 正压系数
        /// </summary>
        public static string Z_Factor = System.Configuration.ConfigurationSettings.AppSettings["Z_Factor"].ToString();
        /// <summary>
        /// 负压系数
        /// </summary>
        public static string F_Factor = System.Configuration.ConfigurationSettings.AppSettings["F_Factor"].ToString();



        public static PublicEnum.DetectionItem? _TestItem
        {
            get
            {
                #region
                PublicEnum.DetectionItem? res = null;
                switch (base_TestItem)
                {
                    case "气密性能检测":
                        res = PublicEnum.DetectionItem.气密性能检测;
                        break;
                    case "水密性能检测":
                        res = PublicEnum.DetectionItem.水密性能检测;
                        break;
                    case "抗风压性能检测":
                        res = PublicEnum.DetectionItem.抗风压性能检测;
                        break;
                    case "气密性能及水密性能检测":
                        res = PublicEnum.DetectionItem.气密性能及水密性能检测;
                        break;
                    case "气密、水密、抗风压性能检测":
                        res = PublicEnum.DetectionItem.气密水密抗风压性能检测;
                        break;
                    case "气密性能及抗风压性能检测":
                        res = PublicEnum.DetectionItem.气密性能及抗风压性能检测;
                        break;
                    case "水密性能及抗风压性能检测":
                        res = PublicEnum.DetectionItem.水密性能及抗风压性能检测;
                        break;
                }
                #endregion
                return res;
            }
        }

        /// <summary>
        /// 气密、水密等级字典
        /// </summary>
        public static Dictionary<int, int> AirtightLevel = new Dictionary<int, int>()
        {
            {1,0 },{2,100},{3,150},{4,200},{5,250},{6,300},{7,350},{8,400},{9,500},{10,600},{11,700}
        };
        #endregion

        /// <summary>
        /// 导入图片名称
        /// </summary>
        public static string ImagesName = "";

        #region  IP相关
        /* /// <summary>
         /// IP端口
         /// </summary>
         public static int TCPPort
         {
             get
             {
                 var res = 502;
                 var dt = new DAL_dt_BaseSet().GetSystemBaseSet();
                 if (dt != null)
                     res = int.Parse(dt.Rows[0]["PROT"].ToString());
                 return res;
             }
         }
         /// <summary>
         /// IP地址
         /// </summary>
         public static string IPAddress
         {
             get
             {
                 var res = "192.168.2.5";

                 var dt = new DAL_dt_BaseSet().GetSystemBaseSet();
                 if (dt != null)
                     res = dt.Rows[0]["IP"].ToString();
                 return res;
             }
         }

         /// <summary>
         /// IP地址
         /// </summary>
         public static double _D
         {
             get
             {
                 var res = 0.08;
                 var dt = new DAL_dt_BaseSet().GetSystemBaseSet();
                 if (dt != null)
                     res = Convert.ToDouble(dt.Rows[0]["D"].ToString());
                 return res;
             }
         }*/

        #endregion
    }


    //public static class RegisterData
    //{
    //    public static double Displace1 = 0;
    //    public static double Displace2 = 0;
    //    public static double Displace3 = 0;
    //    //差压高
    //    public static int CY_High_Value = 0;
    //    //差压低        
    //    public static int CY_Low_Value = 0;
    //    //风速
    //    public static double WindSpeed_Value = 0;
    //    //大气压力
    //    public static double AtmospherePa_Value = 0;
    //    //温度
    //    public static double Temperature_Value = 0;
    //}
}
