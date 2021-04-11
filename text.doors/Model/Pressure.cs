using text.doors.Common;
using text.doors.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Default;

namespace text.doors.Model
{
    public class Pressure
    {
        public Pressure()
        {
            ZYFJ_10 = new List<double>();
            ZYFJ_30 = new List<double>();
            ZYFJ_50 = new List<double>();
            ZYFJ_70 = new List<double>();
            ZYFJ_100 = new List<double>();
            ZYFJ150 = new List<double>();
            ZYFJ100 = new List<double>();
            ZYFJ70 = new List<double>();
            ZYFJ50 = new List<double>();
            ZYFJ30 = new List<double>();
            ZYFJ10 = new List<double>();
            ZYFJ_YCJY = new List<double>();


            ZYZD_10 = new List<double>();
            ZYZD_30 = new List<double>();
            ZYZD_50 = new List<double>();
            ZYZD_70 = new List<double>();
            ZYZD_100 = new List<double>();
            ZYZD150 = new List<double>();
            ZYZD100 = new List<double>();
            ZYZD70 = new List<double>();
            ZYZD50 = new List<double>();
            ZYZD30 = new List<double>();
            ZYZD10 = new List<double>();
            ZYZD_YCJY = new List<double>();

            FYFJ_10 = new List<double>();
            FYFJ_30 = new List<double>();
            FYFJ_50 = new List<double>();
            FYFJ_70 = new List<double>();
            FYFJ_100 = new List<double>();
            FYFJ150 = new List<double>();
            FYFJ100 = new List<double>();
            FYFJ70 = new List<double>();
            FYFJ50 = new List<double>();
            FYFJ30 = new List<double>();
            FYFJ10 = new List<double>();
            FYFJ_YCJY = new List<double>();

            FYZD_10 = new List<double>();
            FYZD_30 = new List<double>();
            FYZD_50 = new List<double>();
            FYZD_70 = new List<double>();
            FYZD_100 = new List<double>();
            FYZD150 = new List<double>();
            FYZD100 = new List<double>();
            FYZD70 = new List<double>();
            FYZD50 = new List<double>();
            FYZD30 = new List<double>();
            FYZD10 = new List<double>();
            FYZD_YCJY = new List<double>();

            PressurePa = "0";
            Pressure_Z = 0.00;
            Pressure_Z_Z = 0.00;
            Pressure_F = 0.00;
            Pressure_F_Z = 0.00;



        }

        //public List<Pressure> ClearZ_F()
        //{
        //    ZYFJ_10 = new List<double>();
        //    ZYFJ_30 = new List<double>();
        //    ZYFJ_50 = new List<double>();
        //    ZYFJ_70 = new List<double>();
        //    ZYFJ_100 = new List<double>();
        //    ZYFJ150 = new List<double>();
        //    ZYFJ100 = new List<double>();
        //    ZYFJ70 = new List<double>();
        //    ZYFJ50 = new List<double>();
        //    ZYFJ30 = new List<double>();
        //    ZYFJ10 = new List<double>();
        //    ZYFJ_YCJY = new List<double>();


        //    Pressure_Z = 0;
        //    return GetPressure();
        //}


        //public List<Pressure> ClearZ_Z()
        //{
        //    ZYZD_10 = new List<double>();
        //    ZYZD_30 = new List<double>();
        //    ZYZD_50 = new List<double>();
        //    ZYZD_70 = new List<double>();
        //    ZYZD_100 = new List<double>();
        //    ZYZD150 = new List<double>();
        //    ZYZD100 = new List<double>();
        //    ZYZD70 = new List<double>();
        //    ZYZD50 = new List<double>();
        //    ZYZD30 = new List<double>();
        //    ZYZD10 = new List<double>();
        //    ZYZD_YCJY = new List<double>();

        //    Pressure_Z_Z = 0.00;
        //    return GetPressure();
        //}
        //public List<Pressure> ClearF_F()
        //{
        //    FYFJ_10 = new List<double>();
        //    FYFJ_30 = new List<double>();
        //    FYFJ_50 = new List<double>();
        //    FYFJ_70 = new List<double>();
        //    FYFJ_100 = new List<double>();
        //    FYFJ150 = new List<double>();
        //    FYFJ100 = new List<double>();
        //    FYFJ70 = new List<double>();
        //    FYFJ50 = new List<double>();
        //    FYFJ30 = new List<double>();
        //    FYFJ10 = new List<double>();
        //    FYFJ_YCJY = new List<double>();

        //    Pressure_F = 0.00;
        //    return GetPressure();
        //}

        //public List<Pressure> ClearF_Z()
        //{
        //    FYZD_10 = new List<double>();
        //    FYZD_30 = new List<double>();
        //    FYZD_50 = new List<double>();
        //    FYZD_70 = new List<double>();
        //    FYZD_100 = new List<double>();
        //    FYZD150 = new List<double>();
        //    FYZD100 = new List<double>();
        //    FYZD70 = new List<double>();
        //    FYZD50 = new List<double>();
        //    FYZD30 = new List<double>();
        //    FYZD10 = new List<double>();
        //    FYZD_YCJY = new List<double>();

        //    Pressure_F_Z = 0.00;
        //    return GetPressure();
        //}

        ////实验次数
        //public int TestCount { get; set; }


        //压力pa
        public string PressurePa { get; set; }
        //正压附加
        public double Pressure_Z { get; set; }
        //正压总的
        public double Pressure_Z_Z { get; set; }
        //负压附加
        public double Pressure_F { get; set; }
        //负压总的
        public double Pressure_F_Z { get; set; }

        /// <summary>
        /// 正压附加
        /// </summary>


        private List<double> ZYFJ_10 = new List<double>();
        private List<double> ZYFJ_30 = new List<double>();
        private List<double> ZYFJ_50 = new List<double>();
        private List<double> ZYFJ_70 = new List<double>();
        private List<double> ZYFJ_100 = new List<double>();
        private List<double> ZYFJ150 = new List<double>();
        private List<double> ZYFJ100 = new List<double>();
        private List<double> ZYFJ70 = new List<double>();
        private List<double> ZYFJ50 = new List<double>();
        private List<double> ZYFJ30 = new List<double>();
        private List<double> ZYFJ10 = new List<double>();
        private List<double> ZYFJ_YCJY = new List<double>();

        /// <summary>
        /// 正压总的
        /// </summary>

        private List<double> ZYZD_10 = new List<double>();
        private List<double> ZYZD_30 = new List<double>();
        private List<double> ZYZD_50 = new List<double>();
        private List<double> ZYZD_70 = new List<double>();
        private List<double> ZYZD_100 = new List<double>();
        private List<double> ZYZD150 = new List<double>();
        private List<double> ZYZD100 = new List<double>();
        private List<double> ZYZD70 = new List<double>();
        private List<double> ZYZD50 = new List<double>();
        private List<double> ZYZD30 = new List<double>();
        private List<double> ZYZD10 = new List<double>();
        private List<double> ZYZD_YCJY = new List<double>();

        /// <summary>
        /// 负压附加
        /// </summary>

        private List<double> FYFJ_10 = new List<double>();
        private List<double> FYFJ_30 = new List<double>();
        private List<double> FYFJ_50 = new List<double>();
        private List<double> FYFJ_70 = new List<double>();
        private List<double> FYFJ_100 = new List<double>();
        private List<double> FYFJ150 = new List<double>();
        private List<double> FYFJ100 = new List<double>();
        private List<double> FYFJ70 = new List<double>();
        private List<double> FYFJ50 = new List<double>();
        private List<double> FYFJ30 = new List<double>();
        private List<double> FYFJ10 = new List<double>();
        private List<double> FYFJ_YCJY = new List<double>();
        /// <summary>
        /// 负压总的
        /// </summary>

        private List<double> FYZD_10 = new List<double>();
        private List<double> FYZD_30 = new List<double>();
        private List<double> FYZD_50 = new List<double>();
        private List<double> FYZD_70 = new List<double>();
        private List<double> FYZD_100 = new List<double>();
        private List<double> FYZD150 = new List<double>();
        private List<double> FYZD100 = new List<double>();
        private List<double> FYZD70 = new List<double>();
        private List<double> FYZD50 = new List<double>();
        private List<double> FYZD30 = new List<double>();
        private List<double> FYZD10 = new List<double>();
        private List<double> FYZD_YCJY = new List<double>();




        /// <summary>
        /// 获取风速数据
        /// </summary>
        /// <returns></returns>
        public List<Pressure> GetPressure()
        {
            List<Pressure> list = new List<Pressure>();
            AddYL_10(list);
            AddYL_30(list);
            AddYL_50(list);
            AddYL_70(list);
            AddYL_100(list);
            AddYL150(list);
            AddYL100(list);
            AddYL70(list);
            AddYL50(list);
            AddYL30(list);
            AddYL10(list);
            AddYCJY(list);
            return list;
        }


        /// <summary>
        /// 增加风速数据(正压附加)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddZYFJ(double data, PublicEnum.Kpa_Level fs)
        {
            if (fs == PublicEnum.Kpa_Level.liter10)
                ZYFJ_10.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter30)
                ZYFJ_30.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter50)
                ZYFJ_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter70)
                ZYFJ_70.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                ZYFJ_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                ZYFJ150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                ZYFJ100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop70)
                ZYFJ70.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                ZYFJ50.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop30)
                ZYFJ30.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop10)
                ZYFJ10.Add(data);
            if (fs == PublicEnum.Kpa_Level.YCJY)
                ZYFJ_YCJY.Add(data);
        }

        /// <summary>
        /// 增加风速数据(正压总的)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddZYZD(double data, PublicEnum.Kpa_Level fs)
        {

            data = double.Parse(DefaultBase.Z_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter10)
                ZYZD_10.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter30)
                ZYZD_30.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter50)
                ZYZD_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter70)
                ZYZD_70.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                ZYZD_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                ZYZD150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                ZYZD100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop70)
                ZYZD70.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                ZYZD50.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop30)
                ZYZD30.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop10)
                ZYZD10.Add(data);
            if (fs == PublicEnum.Kpa_Level.YCJY)
                ZYZD_YCJY.Add(data);
        }

        /// <summary>
        /// 增加风速数据(负压附加)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddFYFJ(double data, PublicEnum.Kpa_Level fs)
        {
            if (fs == PublicEnum.Kpa_Level.liter10)
                FYFJ_10.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter30)
                FYFJ_30.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter50)
                FYFJ_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter70)
                FYFJ_70.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                FYFJ_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                FYFJ150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                FYFJ100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop70)
                FYFJ70.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                FYFJ50.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop30)
                FYFJ30.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop10)
                FYFJ10.Add(data);
            if (fs == PublicEnum.Kpa_Level.YCJY)
                FYFJ_YCJY.Add(data);
        }


        /// <summary>
        /// 增加风速数据(负压总的)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fs">风速枚举</param>
        public void AddFYZD(double data, PublicEnum.Kpa_Level fs)
        {
            data = double.Parse(DefaultBase.F_Factor) * data;

            if (fs == PublicEnum.Kpa_Level.liter10)
                FYZD_10.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter30)
                FYZD_30.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter50)
                FYZD_50.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter70)
                FYZD_70.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter100)
                FYZD_100.Add(data);
            if (fs == PublicEnum.Kpa_Level.liter150)
                FYZD150.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop100)
                FYZD100.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop70)
                FYZD70.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop50)
                FYZD50.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop30)
                FYZD30.Add(data);
            if (fs == PublicEnum.Kpa_Level.drop10)
                FYZD10.Add(data);
            if (fs == PublicEnum.Kpa_Level.YCJY)
                FYZD_YCJY.Add(data);
        }

        /// <summary>
        /// 增加一级10压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL_10(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "10";
            model.Pressure_Z = ZYFJ_10.Count() == 0 ? 0 : Math.Round(ZYFJ_10.Sum(t => t) / ZYFJ_10.Count(), 2);
            model.Pressure_Z_Z = ZYZD_10.Count() == 0 ? 0 : Math.Round(ZYZD_10.Sum(t => t) / ZYZD_10.Count(), 2);
            model.Pressure_F = FYFJ_10.Count() == 0 ? 0 : Math.Round(FYFJ_10.Sum(t => t) / FYFJ_10.Count(), 2);
            model.Pressure_F_Z = FYZD_10.Count() == 0 ? 0 : Math.Round(FYZD_10.Sum(t => t) / FYZD_10.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加一级30压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL_30(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "30";
            model.Pressure_Z = ZYFJ_30.Count() == 0 ? 0 : Math.Round(ZYFJ_30.Sum(t => t) / ZYFJ_30.Count(), 2);
            model.Pressure_Z_Z = ZYZD_30.Count() == 0 ? 0 : Math.Round(ZYZD_30.Sum(t => t) / ZYZD_30.Count(), 2);
            model.Pressure_F = FYFJ_30.Count() == 0 ? 0 : Math.Round(FYFJ_30.Sum(t => t) / FYFJ_30.Count(), 2);
            model.Pressure_F_Z = FYZD_30.Count() == 0 ? 0 : Math.Round(FYZD_30.Sum(t => t) / FYZD_30.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加一级50压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL_50(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "50";
            model.Pressure_Z = ZYFJ_50.Count() == 0 ? 0 : Math.Round(ZYFJ_50.Sum(t => t) / ZYFJ_50.Count(), 2);
            model.Pressure_Z_Z = ZYZD_50.Count() == 0 ? 0 : Math.Round(ZYZD_50.Sum(t => t) / ZYZD_50.Count(), 2);
            model.Pressure_F = FYFJ_50.Count() == 0 ? 0 : Math.Round(FYFJ_50.Sum(t => t) / FYFJ_50.Count(), 2);
            model.Pressure_F_Z = FYZD_50.Count() == 0 ? 0 : Math.Round(FYZD_50.Sum(t => t) / FYZD_50.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加一级70压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL_70(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "70";
            model.Pressure_Z = ZYFJ_70.Count() == 0 ? 0 : Math.Round(ZYFJ_70.Sum(t => t) / ZYFJ_70.Count(), 2);
            model.Pressure_Z_Z = ZYZD_70.Count() == 0 ? 0 : Math.Round(ZYZD_70.Sum(t => t) / ZYZD_70.Count(), 2);
            model.Pressure_F = FYFJ_70.Count() == 0 ? 0 : Math.Round(FYFJ_70.Sum(t => t) / FYFJ_70.Count(), 2);
            model.Pressure_F_Z = FYZD_70.Count() == 0 ? 0 : Math.Round(FYZD_70.Sum(t => t) / FYZD_70.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加一级100压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL_100(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "100";
            model.Pressure_Z = ZYFJ_100.Count() == 0 ? 0 : Math.Round(ZYFJ_100.Sum(t => t) / ZYFJ_100.Count(), 2);
            model.Pressure_Z_Z = ZYZD_100.Count() == 0 ? 0 : Math.Round(ZYZD_100.Sum(t => t) / ZYZD_100.Count(), 2);
            model.Pressure_F = FYFJ_100.Count() == 0 ? 0 : Math.Round(FYFJ_100.Sum(t => t) / FYFJ_100.Count(), 2);
            model.Pressure_F_Z = FYZD_100.Count() == 0 ? 0 : Math.Round(FYZD_100.Sum(t => t) / FYZD_100.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 获取150压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL150(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "150";
            model.Pressure_Z = ZYFJ150.Count() == 0 ? 0 : Math.Round(ZYFJ150.Sum(t => t) / ZYFJ150.Count(), 2);
            model.Pressure_Z_Z = ZYZD150.Count() == 0 ? 0 : Math.Round(ZYZD150.Sum(t => t) / ZYZD150.Count(), 2);
            model.Pressure_F = FYFJ150.Count() == 0 ? 0 : Math.Round(FYFJ150.Sum(t => t) / FYFJ150.Count(), 2);
            model.Pressure_F_Z = FYZD150.Count() == 0 ? 0 : Math.Round(FYZD150.Sum(t => t) / FYZD150.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加二级100压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL100(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "100";
            model.Pressure_Z = ZYFJ100.Count() == 0 ? 0 : Math.Round(ZYFJ100.Sum(t => t) / ZYFJ100.Count(), 2);
            model.Pressure_Z_Z = ZYZD100.Count() == 0 ? 0 : Math.Round(ZYZD100.Sum(t => t) / ZYZD100.Count(), 2);
            model.Pressure_F = FYFJ100.Count() == 0 ? 0 : Math.Round(FYFJ100.Sum(t => t) / FYFJ100.Count(), 2);
            model.Pressure_F_Z = FYZD100.Count() == 0 ? 0 : Math.Round(FYZD100.Sum(t => t) / FYZD100.Count(), 2);
            list.Add(model);
        }
        /// <summary>
        /// 增加二级70压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL70(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "70";
            model.Pressure_Z = ZYFJ70.Count() == 0 ? 0 : Math.Round(ZYFJ70.Sum(t => t) / ZYFJ70.Count(), 2);
            model.Pressure_Z_Z = ZYZD70.Count() == 0 ? 0 : Math.Round(ZYZD70.Sum(t => t) / ZYZD70.Count(), 2);
            model.Pressure_F = FYFJ70.Count() == 0 ? 0 : Math.Round(FYFJ70.Sum(t => t) / FYFJ70.Count(), 2);
            model.Pressure_F_Z = FYZD70.Count() == 0 ? 0 : Math.Round(FYZD70.Sum(t => t) / FYZD70.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加二级50压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL50(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "50";
            model.Pressure_Z = ZYFJ50.Count() == 0 ? 0 : Math.Round(ZYFJ50.Sum(t => t) / ZYFJ50.Count(), 2);
            model.Pressure_Z_Z = ZYZD50.Count() == 0 ? 0 : Math.Round(ZYZD50.Sum(t => t) / ZYZD50.Count(), 2);
            model.Pressure_F = FYFJ50.Count() == 0 ? 0 : Math.Round(FYFJ50.Sum(t => t) / FYFJ50.Count(), 2);
            model.Pressure_F_Z = FYZD50.Count() == 0 ? 0 : Math.Round(FYZD50.Sum(t => t) / FYZD50.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加二级30压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL30(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "30";
            model.Pressure_Z = ZYFJ30.Count() == 0 ? 0 : Math.Round(ZYFJ30.Sum(t => t) / ZYFJ30.Count(), 2);
            model.Pressure_Z_Z = ZYZD30.Count() == 0 ? 0 : Math.Round(ZYZD30.Sum(t => t) / ZYZD30.Count(), 2);
            model.Pressure_F = FYFJ30.Count() == 0 ? 0 : Math.Round(FYFJ30.Sum(t => t) / FYFJ30.Count(), 2);
            model.Pressure_F_Z = FYZD30.Count() == 0 ? 0 : Math.Round(FYZD30.Sum(t => t) / FYZD30.Count(), 2);
            list.Add(model);
        }

        /// <summary>
        /// 增加二级10压力
        /// </summary>
        /// <param name="list"></param>
        private void AddYL10(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "10";
            model.Pressure_Z = ZYFJ10.Count() == 0 ? 0 : Math.Round(ZYFJ10.Sum(t => t) / ZYFJ10.Count(), 2);
            model.Pressure_Z_Z = ZYZD10.Count() == 0 ? 0 : Math.Round(ZYZD10.Sum(t => t) / ZYZD10.Count(), 2);
            model.Pressure_F = FYFJ10.Count() == 0 ? 0 : Math.Round(FYFJ10.Sum(t => t) / FYFJ10.Count(), 2);
            model.Pressure_F_Z = FYZD10.Count() == 0 ? 0 : Math.Round(FYZD10.Sum(t => t) / FYZD10.Count(), 2);
            list.Add(model);
        }



        /// <summary>
        /// 增加设计值
        /// </summary>
        /// <param name="list"></param>
        private void AddYCJY(List<Pressure> list)
        {
            Pressure model = new Pressure();
            model.PressurePa = "设计值";
            model.Pressure_Z = ZYFJ_YCJY.Count() == 0 ? 0 : Math.Round(ZYFJ_YCJY.Sum(t => t) / ZYFJ_YCJY.Count(), 2);
            model.Pressure_Z_Z = ZYZD_YCJY.Count() == 0 ? 0 : Math.Round(ZYZD_YCJY.Sum(t => t) / ZYZD_YCJY.Count(), 2);
            model.Pressure_F = FYFJ_YCJY.Count() == 0 ? 0 : Math.Round(FYFJ_YCJY.Sum(t => t) / FYFJ_YCJY.Count(), 2);
            model.Pressure_F_Z = FYZD_YCJY.Count() == 0 ? 0 : Math.Round(FYZD_YCJY.Sum(t => t) / FYZD_YCJY.Count(), 2);
            list.Add(model);
        }
    }
}
