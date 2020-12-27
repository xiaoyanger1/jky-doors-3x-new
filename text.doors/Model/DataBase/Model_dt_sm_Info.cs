using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model.DataBase
{
    public class Model_dt_sm_Info
    {
        public string dt_Code { get; set; }

        /// <summary>
        /// 樘号描述
        /// </summary>
        public int testcount { get; set; }
        /// <summary>
        /// 樘号描述
        /// </summary>
        public string info_DangH { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string sm_PaDesc { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        public string sm_Pa { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string sm_Remark { get; set; }

        public string Method{ get; set; }
        /// <summary>
        /// 上线压力
        /// </summary>
        public string sxyl { get; set; }

        /// <summary>
        /// 下线压力
        /// </summary>
        public string xxyl{ get; set; }

        public string gongchengjiance { get; set; }
    }
}
