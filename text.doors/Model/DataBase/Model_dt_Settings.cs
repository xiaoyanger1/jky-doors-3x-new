using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text.doors.Model.DataBase
{
    public class Model_dt_Settings
    {
        public Model_dt_Settings()
        {
            dt_InfoList = new List<Model_dt_Info>();
            dt_qm_Info = new List<Model_dt_qm_Info>();
            dt_sm_Info = new List<Model_dt_sm_Info>();
            dt_kfy_Info = new List<Model_dt_kfy_Info>();
        }

        public string dt_Code { get; set; }//编号
        public DateTime dt_Create { get; set; }


        public string weituobianhao { get; set; }
        public string weituodanwei { get; set; }
        public string dizhi { get; set; }
        public string dianhua { get; set; }
        public string chouyangriqi { get; set; }
        public string chouyangdidian { get; set; }
        public string gongchengmingcheng { get; set; }
        public string gongchengdidian { get; set; }
        public string shengchandanwei { get; set; }
        public string jiancexiangmu { get; set; }
        public string jiancedidian { get; set; }
        public string jianceriqi { get; set; }
        public string jianceshebei { get; set; }
        public string jianceyiju { get; set; }


        public string yangpinmingcheng { get; set; }
        public string yangpinshangbiao { get; set; }
        public string yangpinzhuangtai { get; set; }
        public string guigexinghao { get; set; }
        public string kaiqifangshi { get; set; }
        public string mianbanpinzhong { get; set; }
        public string zuidamianban { get; set; }
        public string mianbanhoudu { get; set; }
        public string anzhuangfangshi { get; set; }
        public string mianbanxiangqian { get; set; }
        public string kuangshanmifeng { get; set; }
        public string wujinpeijian { get; set; }
        public string jianceshuliang { get; set; }
        public string dangqiandanghao { get; set; }


        public string dangqianwendu { get; set; }
        public string daqiyali { get; set; }
        public string kaiqifengchang { get; set; }
        public string shijianmianji { get; set; }
        public string ganjianchangdu { get; set; }
        public string penlinshuiliang { get; set; }
        public string qimidanweifengchangshejizhi { get; set; }
        public string qimidanweimianjishejizhi { get; set; }
        public string shuimijingyashejizhi { get; set; }
        public string shuimidongyashejizhi { get; set; }
        public string kangfengyazhengyashejizhi { get; set; }
        public string kangfengyafuyashejizhi { get; set; }
        public string danshandansuodian { get; set; }

        public string kangfengyazhengp3shejizhi { get; set; }
        public string kangfengyazhengpmaxshejizhi { get; set; }


        public List<Model_dt_Info> dt_InfoList { get; set; }
        public List<Model_dt_qm_Info> dt_qm_Info { get; set; }
        public List<Model_dt_sm_Info> dt_sm_Info { get; set; }

        public List<Model_dt_kfy_Info> dt_kfy_Info { get; set; }
    }


}
