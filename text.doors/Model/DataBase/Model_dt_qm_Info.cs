using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model.DataBase
{
    public class Model_dt_qm_Info
    {
        public string dt_Code { get; set; }
        /// <summary>
        /// 樘号
        /// </summary>
        public string info_DangH { get; set; }
        /// <summary>
        /// 正压缝长
        /// </summary>
        public string qm_Z_FC { get; set; }
        /// <summary>
        /// 正压缝长
        /// </summary>
        public string qm_F_FC { get; set; }
        /// <summary>
        /// 正压面积
        /// </summary>
        public string qm_Z_MJ { get; set; }
        /// <summary>
        /// 负压面积
        /// </summary>
        public string qm_F_MJ { get; set; }



        /// <summary>
        /// 升-正压附加10
        /// </summary>
        public string qm_s_z_fj10 { get; set; }
        /// <summary>
        /// 升-正压附加30
        /// </summary>
        public string qm_s_z_fj30 { get; set; }
        /// <summary>
        /// 升-正压附加50
        /// </summary>
        public string qm_s_z_fj50 { get; set; }
        /// <summary>
        /// 升-正压附加70
        /// </summary>
        public string qm_s_z_fj70 { get; set; }
        /// <summary>
        /// 升-正压附加100
        /// </summary>
        public string qm_s_z_fj100 { get; set; }
        /// <summary>
        /// 升-正压附加150
        /// </summary>
        public string qm_s_z_fj150 { get; set; }
        /// <summary>
        /// 降-正压附加100
        /// </summary>
        public string qm_j_z_fj100 { get; set; }
        /// <summary>
        /// 降-正压附加70
        /// </summary>
        public string qm_j_z_fj70 { get; set; }
        /// <summary>
        /// 降-正压附加50
        /// </summary>
        public string qm_j_z_fj50 { get; set; }
        /// <summary>
        /// 降-正压附加30
        /// </summary>
        public string qm_j_z_fj30 { get; set; }
        /// <summary>
        /// 降-正压附加10
        /// </summary>
        public string qm_j_z_fj10 { get; set; }




        /// <summary>
        /// 升-正压总的10
        /// </summary>
        public string qm_s_z_zd10 { get; set; }
        /// <summary>
        /// 升-正压总的30
        /// </summary>
        public string qm_s_z_zd30 { get; set; }
        /// <summary>
        /// 升-正压总的50
        /// </summary>
        public string qm_s_z_zd50 { get; set; }
        /// <summary>
        /// 升-正压总的70
        /// </summary>
        public string qm_s_z_zd70 { get; set; }
        /// <summary>
        /// 升-正压总的100
        /// </summary>
        public string qm_s_z_zd100 { get; set; }
        /// <summary>
        /// 升-正压总的150
        /// </summary>
        public string qm_s_z_zd150 { get; set; }
        /// <summary>
        /// 降-正压总的100
        /// </summary>
        public string qm_j_z_zd100 { get; set; }
        /// <summary>
        /// 降-正压总的70
        /// </summary>
        public string qm_j_z_zd70 { get; set; }
        /// <summary>
        /// 降-正压总的50
        /// </summary>
        public string qm_j_z_zd50 { get; set; }
        /// <summary>
        /// 降-正压总的30
        /// </summary>
        public string qm_j_z_zd30 { get; set; }
        /// <summary>
        /// 降-正压总的10
        /// </summary>
        public string qm_j_z_zd10 { get; set; }



        /// <summary>
        /// 升-负压附加10
        /// </summary>
        public string qm_s_f_fj10 { get; set; }
        /// <summary>
        /// 升-负压附加30
        /// </summary>
        public string qm_s_f_fj30 { get; set; }
        /// <summary>
        /// 升-负压附加50
        /// </summary>
        public string qm_s_f_fj50 { get; set; }
        /// <summary>
        /// 升-负压附加70
        /// </summary>
        public string qm_s_f_fj70 { get; set; }
        /// <summary>
        /// 升-负压附加100
        /// </summary>
        public string qm_s_f_fj100 { get; set; }
        /// <summary>
        /// 升-负压附加150
        /// </summary>
        public string qm_s_f_fj150 { get; set; }
        /// <summary>
        /// 降-负压附加100
        /// </summary>
        public string qm_j_f_fj100 { get; set; }
        /// <summary>
        /// 降-负压附加70
        /// </summary>
        public string qm_j_f_fj70 { get; set; }
        /// <summary>
        /// 降-负压附加50
        /// </summary>
        public string qm_j_f_fj50 { get; set; }
        /// <summary>
        /// 降-负压附加30
        /// </summary>
        public string qm_j_f_fj30 { get; set; }
        /// <summary>
        /// 降-负压附加10
        /// </summary>
        public string qm_j_f_fj10 { get; set; }



        /// <summary>
        /// 升-负压总的10
        /// </summary>
        public string qm_s_f_zd10 { get; set; }
        /// <summary>
        /// 升-负压总的30
        /// </summary>
        public string qm_s_f_zd30 { get; set; }
        /// <summary>
        /// 升-负压总的50
        /// </summary>
        public string qm_s_f_zd50 { get; set; }
        /// <summary>
        /// 升-负压总的70
        /// </summary>
        public string qm_s_f_zd70 { get; set; }
        /// <summary>
        /// 升-负压总的100
        /// </summary>
        public string qm_s_f_zd100 { get; set; }
        /// <summary>
        /// 升-负压总的150
        /// </summary>
        public string qm_s_f_zd150 { get; set; }
        /// <summary>
        /// 降-负压总的100
        /// </summary>
        public string qm_j_f_zd100 { get; set; }
        /// <summary>
        /// 降-负压总的70
        /// </summary>
        public string qm_j_f_zd70 { get; set; }
        /// <summary>
        /// 降-负压总的50
        /// </summary>
        public string qm_j_f_zd50 { get; set; }
        /// <summary>
        /// 降-负压总的30
        /// </summary>
        public string qm_j_f_zd30 { get; set; }
        /// <summary>
        /// 降-负压总的10
        /// </summary>
        public string qm_j_f_zd10 { get; set; }


    }
}
