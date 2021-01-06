using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model
{
    public class AirtightCalculation
    {
        //正压升压总的最小
        public double Z_S_ZZ_Value { get; set; }
        //正压降压总的最大
        public double Z_J_ZZ_Value { get; set; }

        //正压升压附加最小
        public double Z_S_FJ_Value { get; set; }
        //正压降压附加最大
        public double Z_J_FJ_Value { get; set; }



        //负压升压总的最小
        public double F_S_ZZ_Value { get; set; }
        //负压降压总的最大
        public double F_J_ZZ_Value { get; set; }

        //负压升压附加最小
        public double F_S_FJ_Value { get; set; }
        //负压降压附加最大
        public double F_J_FJ_Value { get; set; }

        public int PaValue { get; set; }

        //当前温度
        public double CurrentTemperature { get; set; }

        //大气压力
        public double kPa { get; set; }

        /// <summary>
        /// 正压Q
        /// </summary>
        public double _Z_Q
        {
            get
            {
                return (this.Z_S_ZZ_Value + this.Z_J_ZZ_Value) / 2 - (this.Z_S_FJ_Value + this.Z_J_FJ_Value) / 2;
            }
        }

        /// <summary>
        /// 正压Q计算
        /// </summary>
        public double _Z_Q_SJ_P
        {
            get
            {
                //todo:更改为三位小数
                return 293 / 101.3 * (this.kPa / (273 + this.CurrentTemperature)) * _Z_Q;
            }
        }
        /// <summary>
        /// 负压Q
        /// </summary>

        public double _F_Q
        {
            get
            {
                return (this.F_S_ZZ_Value + this.F_J_ZZ_Value) / 2 - (this.F_S_FJ_Value + this.F_J_FJ_Value) / 2;
            }
        }

        /// <summary>
        /// 负压Q计算
        /// </summary>
        public double _F_Q_SJ_P
        {
            get
            {
                //todo:更改为三位小数
                return 293 / 101.3 * (this.kPa / (273 + this.CurrentTemperature)) * _F_Q;
            }
        }

    }

    public class Point
    {
        public double X;
        public double Y;
        public Point(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }
    }


    public class IndexStitchLengthAndArea
    {
        public double ZY_FC { get; set; }
        public double ZY_MJ { get; set; }
        public double FY_FC { get; set; }
        public double FY_MJ { get; set; }

    }

}
