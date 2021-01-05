using NPOI.SS.Formula.Functions;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.dal;
using text.doors.Model;
using text.doors.Model.DataBase;
using text.doors.Service;

namespace text.doors.Common
{
    public class ExportExcel
    {

        private string _tempCode = "";
        /// <summary>
        /// 模板文件
        /// </summary>
        private string templetFileName = "";
        public ExportExcel(string tempCode)
        {
            this._tempCode = tempCode;
            this.templetFileName = System.Windows.Forms.Application.StartupPath + "\\template\\basedata.xls";
        }


        public bool ExportData(string outFilePath)
        {
            FileStream file = new FileStream(this.templetFileName, FileMode.Open, FileAccess.Read);
            NPOI.HSSF.UserModel.HSSFWorkbook hssfworkbook = new NPOI.HSSF.UserModel.HSSFWorkbook(file);
            NPOI.HSSF.UserModel.HSSFSheet ws = new NPOI.HSSF.UserModel.HSSFSheet(hssfworkbook);

            DataTable dt_Settings = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);
            if (dt_Settings == null || dt_Settings.Rows.Count == 0)
            {
                return false;
            }

            #region   --性能检测报告--
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("性能检测报告");
            ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(1).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(2).GetCell(3).SetCellValue(dt_Settings.Rows[0]["weituodanwei"].ToString());
            ws.GetRow(3).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dizhi"].ToString());
            ws.GetRow(3).GetCell(8).SetCellValue(dt_Settings.Rows[0]["dianhua"].ToString());

            ws.GetRow(4).GetCell(3).SetCellValue(dt_Settings.Rows[0]["chouyangriqi"].ToString());
            ws.GetRow(5).GetCell(3).SetCellValue(dt_Settings.Rows[0]["chouyangdidian"].ToString());
            ws.GetRow(6).GetCell(3).SetCellValue(dt_Settings.Rows[0]["gongchengmingcheng"].ToString());
            ws.GetRow(7).GetCell(3).SetCellValue(dt_Settings.Rows[0]["shengchandanwei"].ToString());
            //样品
            ws.GetRow(8).GetCell(3).SetCellValue(dt_Settings.Rows[0]["yangpinmingcheng"].ToString());
            ws.GetRow(8).GetCell(8).SetCellValue(dt_Settings.Rows[0]["yangpinzhuangtai"].ToString());
            ws.GetRow(9).GetCell(8).SetCellValue(dt_Settings.Rows[0]["guigexinghao"].ToString());
            //检测
            ws.GetRow(10).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jiancexiangmu"].ToString());
            ws.GetRow(10).GetCell(8).SetCellValue(dt_Settings.Rows[0]["jianceshuliang"].ToString());
            ws.GetRow(11).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jiancedidian"].ToString());
            ws.GetRow(11).GetCell(8).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(12).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceyiju"].ToString());
            ws.GetRow(13).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceshebei"].ToString());
            //结论
            ws.GetRow(16).GetCell(8).SetCellValue(0);
            ws.GetRow(17).GetCell(8).SetCellValue(0);
            ws.GetRow(18).GetCell(8).SetCellValue(0);
            ws.GetRow(19).GetCell(4).SetCellValue("(采用稳定加压方法检测)");
            ws.GetRow(20).GetCell(8).SetCellValue(0);
            #endregion

            #region 质量检测报告
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("质量检测报告");
            ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(2).GetCell(6).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(3).GetCell(2).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());
            ws.GetRow(3).GetCell(9).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
            ws.GetRow(4).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanpinzhong"].ToString());
            ws.GetRow(5).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanxiangqian"].ToString());
            ws.GetRow(5).GetCell(9).SetCellValue(dt_Settings.Rows[0]["kuangshanmifeng"].ToString());
            ws.GetRow(6).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
            ws.GetRow(6).GetCell(9).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());

            var cc = dt_Settings.Rows[0]["zuidamianban"].ToString();
            var ccArr = cc.Split('*');
            if (ccArr.Length > 0)
                ws.GetRow(7).GetCell(3).SetCellValue(ccArr[0]); //宽
            if (ccArr.Length > 1)
                ws.GetRow(7).GetCell(7).SetCellValue(ccArr[1]);//长
            if (ccArr.Length > 2)
                ws.GetRow(7).GetCell(11).SetCellValue(ccArr[2]);// 厚


            //工程设计值
            ws.GetRow(7).GetCell(3).SetCellValue(dt_Settings.Rows[0]["qimidangweifengchangshejizhi"].ToString());
            ws.GetRow(7).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shuimijingyashejizhi"].ToString());
            ws.GetRow(7).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengyashejizhi"].ToString());

            ws.GetRow(8).GetCell(3).SetCellValue(dt_Settings.Rows[0]["qimidanweimianjishejizhi"].ToString());
            ws.GetRow(8).GetCell(8).SetCellValue(dt_Settings.Rows[0]["shuimidongyashejizhi"].ToString());
            ws.GetRow(8).GetCell(12).SetCellValue(dt_Settings.Rows[0]["kangfengyafuyashejizhi"].ToString());
            //气密性能
            ws.GetRow(11).GetCell(6).SetCellValue("");
            ws.GetRow(11).GetCell(9).SetCellValue("");
            ws.GetRow(12).GetCell(6).SetCellValue("");
            ws.GetRow(12).GetCell(9).SetCellValue("");
            //水密性能
            ws.GetRow(13).GetCell(3).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
            //稳定加压
            ws.GetRow(14).GetCell(5).SetCellValue("");
            ws.GetRow(15).GetCell(5).SetCellValue("");
            //波动
            ws.GetRow(16).GetCell(3).SetCellValue("");
            ws.GetRow(17).GetCell(3).SetCellValue("");
            //抗风压
            ws.GetRow(18).GetCell(6).SetCellValue("");
            ws.GetRow(19).GetCell(6).SetCellValue("");
            ws.GetRow(20).GetCell(6).SetCellValue("");
            ws.GetRow(21).GetCell(6).SetCellValue("");

            ws.GetRow(22).GetCell(8).SetCellValue("");
            //风荷载
            ws.GetRow(23).GetCell(6).SetCellValue("");
            ws.GetRow(24).GetCell(6).SetCellValue("");
            ws.GetRow(25).GetCell(6).SetCellValue("");
            ws.GetRow(26).GetCell(6).SetCellValue("");

            //重复
            //气密性能
            ws.GetRow(27).GetCell(6).SetCellValue("");
            ws.GetRow(27).GetCell(9).SetCellValue("");
            ws.GetRow(28).GetCell(6).SetCellValue("");
            ws.GetRow(28).GetCell(9).SetCellValue("");
            //水密性能
            ws.GetRow(29).GetCell(3).SetCellValue("");
            //稳定加压
            ws.GetRow(30).GetCell(5).SetCellValue("");
            ws.GetRow(31).GetCell(5).SetCellValue("");
            //波动
            ws.GetRow(32).GetCell(3).SetCellValue("");
            ws.GetRow(33).GetCell(3).SetCellValue("");

            //检测结果
            ws.GetRow(35).GetCell(6).SetCellValue("");
            ws.GetRow(36).GetCell(6).SetCellValue("");
            ws.GetRow(37).GetCell(6).SetCellValue("");
            ws.GetRow(38).GetCell(6).SetCellValue("");
            #endregion

            #region 气密性
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("气密性");

            ws.GetRow(2).GetCell(3).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(2).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
            ws.GetRow(2).GetCell(7).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());

            ws.GetRow(3).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(3).GetCell(5).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
            ws.GetRow(3).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());

            List<Model_dt_qm_Info> qm_Info = new DAL_dt_Settings().GetQMListByCode(_tempCode);

            if (qm_Info != null && qm_Info.Count > 0)
            {
                #region  气密性第一次
                var qm = qm_Info.FindAll(t => t.testcount == 1).OrderBy(t => t.info_DangH).ToList();
                if ((qm != null && qm.Count() > 0))
                {
                    //1检测 2.工程检测
                    var testtype = qm[0].testtype;
                    var index = 0;

                    #region 赋值
                    foreach (var item in qm)
                    {
                        index++;
                        if (index == 1)
                        {
                            //正压
                            ws.GetRow(7).GetCell(3).SetCellValue(item.qm_s_z_zd10);
                            ws.GetRow(7).GetCell(4).SetCellValue(item.qm_s_z_fj10);
                            ws.GetRow(8).GetCell(3).SetCellValue(item.qm_s_z_zd30);
                            ws.GetRow(8).GetCell(4).SetCellValue(item.qm_s_z_fj30);
                            ws.GetRow(9).GetCell(3).SetCellValue(item.qm_s_z_zd50);
                            ws.GetRow(9).GetCell(4).SetCellValue(item.qm_s_z_fj50);
                            ws.GetRow(10).GetCell(3).SetCellValue(item.qm_s_z_zd70);
                            ws.GetRow(10).GetCell(4).SetCellValue(item.qm_s_z_fj70);
                            ws.GetRow(11).GetCell(3).SetCellValue(item.qm_s_z_zd100);
                            ws.GetRow(11).GetCell(4).SetCellValue(item.qm_s_z_fj100);
                            ws.GetRow(12).GetCell(3).SetCellValue(item.qm_s_z_zd150);
                            ws.GetRow(12).GetCell(4).SetCellValue(item.qm_s_z_fj150);
                            ws.GetRow(13).GetCell(3).SetCellValue(item.qm_j_z_zd100);
                            ws.GetRow(13).GetCell(4).SetCellValue(item.qm_j_z_fj100);
                            ws.GetRow(14).GetCell(3).SetCellValue(item.qm_j_z_zd70);
                            ws.GetRow(14).GetCell(4).SetCellValue(item.qm_j_z_fj70);
                            ws.GetRow(15).GetCell(3).SetCellValue(item.qm_j_z_zd50);
                            ws.GetRow(15).GetCell(4).SetCellValue(item.qm_j_z_fj50);
                            ws.GetRow(16).GetCell(3).SetCellValue(item.qm_j_z_zd30);
                            ws.GetRow(16).GetCell(4).SetCellValue(item.qm_j_z_fj30);
                            ws.GetRow(17).GetCell(3).SetCellValue(item.qm_j_z_zd10);
                            ws.GetRow(17).GetCell(4).SetCellValue(item.qm_j_z_fj10);

                            if (item.sjz_value == "0")
                            {
                                ws.GetRow(18).GetCell(2).SetCellValue("设计值");
                            }
                            else
                            {
                                ws.GetRow(18).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj);

                            ws.GetRow(19).GetCell(3).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(3).SetCellValue(item.qm_Z_MJ);


                            //负压
                            ws.GetRow(23).GetCell(3).SetCellValue(item.qm_s_f_zd10);
                            ws.GetRow(23).GetCell(4).SetCellValue(item.qm_s_f_fj10);
                            ws.GetRow(24).GetCell(3).SetCellValue(item.qm_s_f_zd30);
                            ws.GetRow(24).GetCell(4).SetCellValue(item.qm_s_f_fj30);
                            ws.GetRow(25).GetCell(3).SetCellValue(item.qm_s_f_zd50);
                            ws.GetRow(25).GetCell(4).SetCellValue(item.qm_s_f_fj50);
                            ws.GetRow(26).GetCell(3).SetCellValue(item.qm_s_f_zd70);
                            ws.GetRow(26).GetCell(4).SetCellValue(item.qm_s_f_fj70);
                            ws.GetRow(27).GetCell(3).SetCellValue(item.qm_s_f_zd100);
                            ws.GetRow(27).GetCell(4).SetCellValue(item.qm_s_f_fj100);
                            ws.GetRow(28).GetCell(3).SetCellValue(item.qm_s_f_zd150);
                            ws.GetRow(28).GetCell(4).SetCellValue(item.qm_s_f_fj150);
                            ws.GetRow(29).GetCell(3).SetCellValue(item.qm_j_f_zd100);
                            ws.GetRow(29).GetCell(4).SetCellValue(item.qm_j_f_fj100);
                            ws.GetRow(30).GetCell(3).SetCellValue(item.qm_j_f_zd70);
                            ws.GetRow(30).GetCell(4).SetCellValue(item.qm_j_f_fj70);
                            ws.GetRow(31).GetCell(3).SetCellValue(item.qm_j_f_zd50);
                            ws.GetRow(31).GetCell(4).SetCellValue(item.qm_j_f_fj50);
                            ws.GetRow(32).GetCell(3).SetCellValue(item.qm_j_f_zd30);
                            ws.GetRow(32).GetCell(4).SetCellValue(item.qm_j_f_fj30);
                            ws.GetRow(33).GetCell(3).SetCellValue(item.qm_j_f_zd10);
                            ws.GetRow(33).GetCell(4).SetCellValue(item.qm_j_f_fj10);

                            if (item.sjz_value == "0")
                            {
                                ws.GetRow(34).GetCell(2).SetCellValue("设计值");
                            }
                            else
                            {
                                ws.GetRow(34).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj);

                            ws.GetRow(35).GetCell(3).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(3).SetCellValue(item.qm_F_MJ);

                        }
                        else if (index == 2)
                        {
                            //正压
                            ws.GetRow(7).GetCell(5).SetCellValue(item.qm_s_z_zd10);
                            ws.GetRow(7).GetCell(6).SetCellValue(item.qm_s_z_fj10);
                            ws.GetRow(8).GetCell(5).SetCellValue(item.qm_s_z_zd30);
                            ws.GetRow(8).GetCell(6).SetCellValue(item.qm_s_z_fj30);
                            ws.GetRow(9).GetCell(5).SetCellValue(item.qm_s_z_zd50);
                            ws.GetRow(9).GetCell(6).SetCellValue(item.qm_s_z_fj50);
                            ws.GetRow(10).GetCell(5).SetCellValue(item.qm_s_z_zd70);
                            ws.GetRow(10).GetCell(6).SetCellValue(item.qm_s_z_fj70);
                            ws.GetRow(11).GetCell(5).SetCellValue(item.qm_s_z_zd100);
                            ws.GetRow(11).GetCell(6).SetCellValue(item.qm_s_z_fj100);
                            ws.GetRow(12).GetCell(5).SetCellValue(item.qm_s_z_zd150);
                            ws.GetRow(12).GetCell(6).SetCellValue(item.qm_s_z_fj150);
                            ws.GetRow(13).GetCell(5).SetCellValue(item.qm_j_z_zd100);
                            ws.GetRow(13).GetCell(6).SetCellValue(item.qm_j_z_fj100);
                            ws.GetRow(14).GetCell(5).SetCellValue(item.qm_j_z_zd70);
                            ws.GetRow(14).GetCell(6).SetCellValue(item.qm_j_z_fj70);
                            ws.GetRow(15).GetCell(5).SetCellValue(item.qm_j_z_zd50);
                            ws.GetRow(15).GetCell(6).SetCellValue(item.qm_j_z_fj50);
                            ws.GetRow(16).GetCell(5).SetCellValue(item.qm_j_z_zd30);
                            ws.GetRow(16).GetCell(6).SetCellValue(item.qm_j_z_fj30);
                            ws.GetRow(17).GetCell(5).SetCellValue(item.qm_j_z_zd10);
                            ws.GetRow(17).GetCell(6).SetCellValue(item.qm_j_z_fj10);

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj);

                            ws.GetRow(19).GetCell(3).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(3).SetCellValue(item.qm_Z_MJ);
                            //负压
                            ws.GetRow(23).GetCell(5).SetCellValue(item.qm_s_f_zd10);
                            ws.GetRow(23).GetCell(6).SetCellValue(item.qm_s_f_fj10);
                            ws.GetRow(24).GetCell(5).SetCellValue(item.qm_s_f_zd30);
                            ws.GetRow(24).GetCell(6).SetCellValue(item.qm_s_f_fj30);
                            ws.GetRow(25).GetCell(5).SetCellValue(item.qm_s_f_zd50);
                            ws.GetRow(25).GetCell(6).SetCellValue(item.qm_s_f_fj50);
                            ws.GetRow(26).GetCell(5).SetCellValue(item.qm_s_f_zd70);
                            ws.GetRow(26).GetCell(6).SetCellValue(item.qm_s_f_fj70);
                            ws.GetRow(27).GetCell(5).SetCellValue(item.qm_s_f_zd100);
                            ws.GetRow(27).GetCell(6).SetCellValue(item.qm_s_f_fj100);
                            ws.GetRow(28).GetCell(5).SetCellValue(item.qm_s_f_zd150);
                            ws.GetRow(28).GetCell(6).SetCellValue(item.qm_s_f_fj150);
                            ws.GetRow(29).GetCell(5).SetCellValue(item.qm_j_f_zd100);
                            ws.GetRow(29).GetCell(6).SetCellValue(item.qm_j_f_fj100);
                            ws.GetRow(30).GetCell(5).SetCellValue(item.qm_j_f_zd70);
                            ws.GetRow(30).GetCell(6).SetCellValue(item.qm_j_f_fj70);
                            ws.GetRow(31).GetCell(5).SetCellValue(item.qm_j_f_zd50);
                            ws.GetRow(31).GetCell(6).SetCellValue(item.qm_j_f_fj50);
                            ws.GetRow(32).GetCell(5).SetCellValue(item.qm_j_f_zd30);
                            ws.GetRow(32).GetCell(6).SetCellValue(item.qm_j_f_fj30);
                            ws.GetRow(33).GetCell(5).SetCellValue(item.qm_j_f_zd10);
                            ws.GetRow(33).GetCell(6).SetCellValue(item.qm_j_f_fj10);

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj);

                            ws.GetRow(35).GetCell(3).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(3).SetCellValue(item.qm_F_MJ);
                        }
                        else if (index == 3)
                        {
                            //正压
                            ws.GetRow(7).GetCell(7).SetCellValue(item.qm_s_z_zd10);
                            ws.GetRow(7).GetCell(8).SetCellValue(item.qm_s_z_fj10);
                            ws.GetRow(8).GetCell(7).SetCellValue(item.qm_s_z_zd30);
                            ws.GetRow(8).GetCell(8).SetCellValue(item.qm_s_z_fj30);
                            ws.GetRow(9).GetCell(7).SetCellValue(item.qm_s_z_zd50);
                            ws.GetRow(9).GetCell(8).SetCellValue(item.qm_s_z_fj50);
                            ws.GetRow(10).GetCell(7).SetCellValue(item.qm_s_z_zd70);
                            ws.GetRow(10).GetCell(8).SetCellValue(item.qm_s_z_fj70);
                            ws.GetRow(11).GetCell(7).SetCellValue(item.qm_s_z_zd100);
                            ws.GetRow(11).GetCell(8).SetCellValue(item.qm_s_z_fj100);
                            ws.GetRow(12).GetCell(7).SetCellValue(item.qm_s_z_zd150);
                            ws.GetRow(12).GetCell(8).SetCellValue(item.qm_s_z_fj150);
                            ws.GetRow(13).GetCell(7).SetCellValue(item.qm_j_z_zd100);
                            ws.GetRow(13).GetCell(8).SetCellValue(item.qm_j_z_fj100);
                            ws.GetRow(14).GetCell(7).SetCellValue(item.qm_j_z_zd70);
                            ws.GetRow(14).GetCell(8).SetCellValue(item.qm_j_z_fj70);
                            ws.GetRow(15).GetCell(7).SetCellValue(item.qm_j_z_zd50);
                            ws.GetRow(15).GetCell(8).SetCellValue(item.qm_j_z_fj50);
                            ws.GetRow(16).GetCell(7).SetCellValue(item.qm_j_z_zd30);
                            ws.GetRow(16).GetCell(8).SetCellValue(item.qm_j_z_fj30);
                            ws.GetRow(17).GetCell(7).SetCellValue(item.qm_j_z_zd10);
                            ws.GetRow(17).GetCell(8).SetCellValue(item.qm_j_z_fj10);

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj);

                            ws.GetRow(19).GetCell(3).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(3).SetCellValue(item.qm_Z_MJ);
                            //负压
                            ws.GetRow(23).GetCell(7).SetCellValue(item.qm_s_f_zd10);
                            ws.GetRow(23).GetCell(8).SetCellValue(item.qm_s_f_fj10);
                            ws.GetRow(24).GetCell(7).SetCellValue(item.qm_s_f_zd30);
                            ws.GetRow(24).GetCell(8).SetCellValue(item.qm_s_f_fj30);
                            ws.GetRow(25).GetCell(7).SetCellValue(item.qm_s_f_zd50);
                            ws.GetRow(25).GetCell(8).SetCellValue(item.qm_s_f_fj50);
                            ws.GetRow(26).GetCell(7).SetCellValue(item.qm_s_f_zd70);
                            ws.GetRow(26).GetCell(8).SetCellValue(item.qm_s_f_fj70);
                            ws.GetRow(27).GetCell(7).SetCellValue(item.qm_s_f_zd100);
                            ws.GetRow(27).GetCell(8).SetCellValue(item.qm_s_f_fj100);
                            ws.GetRow(28).GetCell(7).SetCellValue(item.qm_s_f_zd150);
                            ws.GetRow(28).GetCell(8).SetCellValue(item.qm_s_f_fj150);
                            ws.GetRow(29).GetCell(7).SetCellValue(item.qm_j_f_zd100);
                            ws.GetRow(29).GetCell(8).SetCellValue(item.qm_j_f_fj100);
                            ws.GetRow(30).GetCell(7).SetCellValue(item.qm_j_f_zd70);
                            ws.GetRow(30).GetCell(8).SetCellValue(item.qm_j_f_fj70);
                            ws.GetRow(31).GetCell(7).SetCellValue(item.qm_j_f_zd50);
                            ws.GetRow(31).GetCell(8).SetCellValue(item.qm_j_f_fj50);
                            ws.GetRow(32).GetCell(7).SetCellValue(item.qm_j_f_zd30);
                            ws.GetRow(32).GetCell(8).SetCellValue(item.qm_j_f_fj30);
                            ws.GetRow(33).GetCell(7).SetCellValue(item.qm_j_f_zd10);
                            ws.GetRow(33).GetCell(8).SetCellValue(item.qm_j_f_fj10);

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj);

                            ws.GetRow(35).GetCell(3).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(3).SetCellValue(item.qm_F_MJ);
                        }
                    }
                    #endregion
                }
                #endregion

                #region 重复气密性

                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复气密性");
                qm = qm_Info.FindAll(t => t.testcount == 2).OrderBy(t => t.info_DangH).ToList();
                if ((qm != null && qm.Count() > 0))
                {
                    //1检测 2.工程检测
                    var testtype = qm[0].testtype;
                    var index = 0;

                    #region 赋值
                    foreach (var item in qm)
                    {
                        index++;
                        if (index == 1)
                        {
                            //正压
                            ws.GetRow(7).GetCell(3).SetCellValue(item.qm_s_z_zd10);
                            ws.GetRow(7).GetCell(4).SetCellValue(item.qm_s_z_fj10);
                            ws.GetRow(8).GetCell(3).SetCellValue(item.qm_s_z_zd30);
                            ws.GetRow(8).GetCell(4).SetCellValue(item.qm_s_z_fj30);
                            ws.GetRow(9).GetCell(3).SetCellValue(item.qm_s_z_zd50);
                            ws.GetRow(9).GetCell(4).SetCellValue(item.qm_s_z_fj50);
                            ws.GetRow(10).GetCell(3).SetCellValue(item.qm_s_z_zd70);
                            ws.GetRow(10).GetCell(4).SetCellValue(item.qm_s_z_fj70);
                            ws.GetRow(11).GetCell(3).SetCellValue(item.qm_s_z_zd100);
                            ws.GetRow(11).GetCell(4).SetCellValue(item.qm_s_z_fj100);
                            ws.GetRow(12).GetCell(3).SetCellValue(item.qm_s_z_zd150);
                            ws.GetRow(12).GetCell(4).SetCellValue(item.qm_s_z_fj150);
                            ws.GetRow(13).GetCell(3).SetCellValue(item.qm_j_z_zd100);
                            ws.GetRow(13).GetCell(4).SetCellValue(item.qm_j_z_fj100);
                            ws.GetRow(14).GetCell(3).SetCellValue(item.qm_j_z_zd70);
                            ws.GetRow(14).GetCell(4).SetCellValue(item.qm_j_z_fj70);
                            ws.GetRow(15).GetCell(3).SetCellValue(item.qm_j_z_zd50);
                            ws.GetRow(15).GetCell(4).SetCellValue(item.qm_j_z_fj50);
                            ws.GetRow(16).GetCell(3).SetCellValue(item.qm_j_z_zd30);
                            ws.GetRow(16).GetCell(4).SetCellValue(item.qm_j_z_fj30);
                            ws.GetRow(17).GetCell(3).SetCellValue(item.qm_j_z_zd10);
                            ws.GetRow(17).GetCell(4).SetCellValue(item.qm_j_z_fj10);

                            if (item.sjz_value == "0")
                            {
                                ws.GetRow(18).GetCell(2).SetCellValue("设计值");
                            }
                            else
                            {
                                ws.GetRow(18).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj);

                            ws.GetRow(19).GetCell(3).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(3).SetCellValue(item.qm_Z_MJ);


                            //负压
                            ws.GetRow(23).GetCell(3).SetCellValue(item.qm_s_f_zd10);
                            ws.GetRow(23).GetCell(4).SetCellValue(item.qm_s_f_fj10);
                            ws.GetRow(24).GetCell(3).SetCellValue(item.qm_s_f_zd30);
                            ws.GetRow(24).GetCell(4).SetCellValue(item.qm_s_f_fj30);
                            ws.GetRow(25).GetCell(3).SetCellValue(item.qm_s_f_zd50);
                            ws.GetRow(25).GetCell(4).SetCellValue(item.qm_s_f_fj50);
                            ws.GetRow(26).GetCell(3).SetCellValue(item.qm_s_f_zd70);
                            ws.GetRow(26).GetCell(4).SetCellValue(item.qm_s_f_fj70);
                            ws.GetRow(27).GetCell(3).SetCellValue(item.qm_s_f_zd100);
                            ws.GetRow(27).GetCell(4).SetCellValue(item.qm_s_f_fj100);
                            ws.GetRow(28).GetCell(3).SetCellValue(item.qm_s_f_zd150);
                            ws.GetRow(28).GetCell(4).SetCellValue(item.qm_s_f_fj150);
                            ws.GetRow(29).GetCell(3).SetCellValue(item.qm_j_f_zd100);
                            ws.GetRow(29).GetCell(4).SetCellValue(item.qm_j_f_fj100);
                            ws.GetRow(30).GetCell(3).SetCellValue(item.qm_j_f_zd70);
                            ws.GetRow(30).GetCell(4).SetCellValue(item.qm_j_f_fj70);
                            ws.GetRow(31).GetCell(3).SetCellValue(item.qm_j_f_zd50);
                            ws.GetRow(31).GetCell(4).SetCellValue(item.qm_j_f_fj50);
                            ws.GetRow(32).GetCell(3).SetCellValue(item.qm_j_f_zd30);
                            ws.GetRow(32).GetCell(4).SetCellValue(item.qm_j_f_fj30);
                            ws.GetRow(33).GetCell(3).SetCellValue(item.qm_j_f_zd10);
                            ws.GetRow(33).GetCell(4).SetCellValue(item.qm_j_f_fj10);

                            if (item.sjz_value == "0")
                            {
                                ws.GetRow(34).GetCell(2).SetCellValue("设计值");
                            }
                            else
                            {
                                ws.GetRow(34).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj);

                            ws.GetRow(35).GetCell(3).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(3).SetCellValue(item.qm_F_MJ);

                        }
                        else if (index == 2)
                        {
                            //正压
                            ws.GetRow(7).GetCell(5).SetCellValue(item.qm_s_z_zd10);
                            ws.GetRow(7).GetCell(6).SetCellValue(item.qm_s_z_fj10);
                            ws.GetRow(8).GetCell(5).SetCellValue(item.qm_s_z_zd30);
                            ws.GetRow(8).GetCell(6).SetCellValue(item.qm_s_z_fj30);
                            ws.GetRow(9).GetCell(5).SetCellValue(item.qm_s_z_zd50);
                            ws.GetRow(9).GetCell(6).SetCellValue(item.qm_s_z_fj50);
                            ws.GetRow(10).GetCell(5).SetCellValue(item.qm_s_z_zd70);
                            ws.GetRow(10).GetCell(6).SetCellValue(item.qm_s_z_fj70);
                            ws.GetRow(11).GetCell(5).SetCellValue(item.qm_s_z_zd100);
                            ws.GetRow(11).GetCell(6).SetCellValue(item.qm_s_z_fj100);
                            ws.GetRow(12).GetCell(5).SetCellValue(item.qm_s_z_zd150);
                            ws.GetRow(12).GetCell(6).SetCellValue(item.qm_s_z_fj150);
                            ws.GetRow(13).GetCell(5).SetCellValue(item.qm_j_z_zd100);
                            ws.GetRow(13).GetCell(6).SetCellValue(item.qm_j_z_fj100);
                            ws.GetRow(14).GetCell(5).SetCellValue(item.qm_j_z_zd70);
                            ws.GetRow(14).GetCell(6).SetCellValue(item.qm_j_z_fj70);
                            ws.GetRow(15).GetCell(5).SetCellValue(item.qm_j_z_zd50);
                            ws.GetRow(15).GetCell(6).SetCellValue(item.qm_j_z_fj50);
                            ws.GetRow(16).GetCell(5).SetCellValue(item.qm_j_z_zd30);
                            ws.GetRow(16).GetCell(6).SetCellValue(item.qm_j_z_fj30);
                            ws.GetRow(17).GetCell(5).SetCellValue(item.qm_j_z_zd10);
                            ws.GetRow(17).GetCell(6).SetCellValue(item.qm_j_z_fj10);

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj);

                            ws.GetRow(19).GetCell(3).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(3).SetCellValue(item.qm_Z_MJ);
                            //负压
                            ws.GetRow(23).GetCell(5).SetCellValue(item.qm_s_f_zd10);
                            ws.GetRow(23).GetCell(6).SetCellValue(item.qm_s_f_fj10);
                            ws.GetRow(24).GetCell(5).SetCellValue(item.qm_s_f_zd30);
                            ws.GetRow(24).GetCell(6).SetCellValue(item.qm_s_f_fj30);
                            ws.GetRow(25).GetCell(5).SetCellValue(item.qm_s_f_zd50);
                            ws.GetRow(25).GetCell(6).SetCellValue(item.qm_s_f_fj50);
                            ws.GetRow(26).GetCell(5).SetCellValue(item.qm_s_f_zd70);
                            ws.GetRow(26).GetCell(6).SetCellValue(item.qm_s_f_fj70);
                            ws.GetRow(27).GetCell(5).SetCellValue(item.qm_s_f_zd100);
                            ws.GetRow(27).GetCell(6).SetCellValue(item.qm_s_f_fj100);
                            ws.GetRow(28).GetCell(5).SetCellValue(item.qm_s_f_zd150);
                            ws.GetRow(28).GetCell(6).SetCellValue(item.qm_s_f_fj150);
                            ws.GetRow(29).GetCell(5).SetCellValue(item.qm_j_f_zd100);
                            ws.GetRow(29).GetCell(6).SetCellValue(item.qm_j_f_fj100);
                            ws.GetRow(30).GetCell(5).SetCellValue(item.qm_j_f_zd70);
                            ws.GetRow(30).GetCell(6).SetCellValue(item.qm_j_f_fj70);
                            ws.GetRow(31).GetCell(5).SetCellValue(item.qm_j_f_zd50);
                            ws.GetRow(31).GetCell(6).SetCellValue(item.qm_j_f_fj50);
                            ws.GetRow(32).GetCell(5).SetCellValue(item.qm_j_f_zd30);
                            ws.GetRow(32).GetCell(6).SetCellValue(item.qm_j_f_fj30);
                            ws.GetRow(33).GetCell(5).SetCellValue(item.qm_j_f_zd10);
                            ws.GetRow(33).GetCell(6).SetCellValue(item.qm_j_f_fj10);

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj);

                            ws.GetRow(35).GetCell(3).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(3).SetCellValue(item.qm_F_MJ);
                        }
                        else if (index == 3)
                        {
                            //正压
                            ws.GetRow(7).GetCell(7).SetCellValue(item.qm_s_z_zd10);
                            ws.GetRow(7).GetCell(8).SetCellValue(item.qm_s_z_fj10);
                            ws.GetRow(8).GetCell(7).SetCellValue(item.qm_s_z_zd30);
                            ws.GetRow(8).GetCell(8).SetCellValue(item.qm_s_z_fj30);
                            ws.GetRow(9).GetCell(7).SetCellValue(item.qm_s_z_zd50);
                            ws.GetRow(9).GetCell(8).SetCellValue(item.qm_s_z_fj50);
                            ws.GetRow(10).GetCell(7).SetCellValue(item.qm_s_z_zd70);
                            ws.GetRow(10).GetCell(8).SetCellValue(item.qm_s_z_fj70);
                            ws.GetRow(11).GetCell(7).SetCellValue(item.qm_s_z_zd100);
                            ws.GetRow(11).GetCell(8).SetCellValue(item.qm_s_z_fj100);
                            ws.GetRow(12).GetCell(7).SetCellValue(item.qm_s_z_zd150);
                            ws.GetRow(12).GetCell(8).SetCellValue(item.qm_s_z_fj150);
                            ws.GetRow(13).GetCell(7).SetCellValue(item.qm_j_z_zd100);
                            ws.GetRow(13).GetCell(8).SetCellValue(item.qm_j_z_fj100);
                            ws.GetRow(14).GetCell(7).SetCellValue(item.qm_j_z_zd70);
                            ws.GetRow(14).GetCell(8).SetCellValue(item.qm_j_z_fj70);
                            ws.GetRow(15).GetCell(7).SetCellValue(item.qm_j_z_zd50);
                            ws.GetRow(15).GetCell(8).SetCellValue(item.qm_j_z_fj50);
                            ws.GetRow(16).GetCell(7).SetCellValue(item.qm_j_z_zd30);
                            ws.GetRow(16).GetCell(8).SetCellValue(item.qm_j_z_fj30);
                            ws.GetRow(17).GetCell(7).SetCellValue(item.qm_j_z_zd10);
                            ws.GetRow(17).GetCell(8).SetCellValue(item.qm_j_z_fj10);

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj);

                            ws.GetRow(19).GetCell(3).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(3).SetCellValue(item.qm_Z_MJ);
                            //负压
                            ws.GetRow(23).GetCell(7).SetCellValue(item.qm_s_f_zd10);
                            ws.GetRow(23).GetCell(8).SetCellValue(item.qm_s_f_fj10);
                            ws.GetRow(24).GetCell(7).SetCellValue(item.qm_s_f_zd30);
                            ws.GetRow(24).GetCell(8).SetCellValue(item.qm_s_f_fj30);
                            ws.GetRow(25).GetCell(7).SetCellValue(item.qm_s_f_zd50);
                            ws.GetRow(25).GetCell(8).SetCellValue(item.qm_s_f_fj50);
                            ws.GetRow(26).GetCell(7).SetCellValue(item.qm_s_f_zd70);
                            ws.GetRow(26).GetCell(8).SetCellValue(item.qm_s_f_fj70);
                            ws.GetRow(27).GetCell(7).SetCellValue(item.qm_s_f_zd100);
                            ws.GetRow(27).GetCell(8).SetCellValue(item.qm_s_f_fj100);
                            ws.GetRow(28).GetCell(7).SetCellValue(item.qm_s_f_zd150);
                            ws.GetRow(28).GetCell(8).SetCellValue(item.qm_s_f_fj150);
                            ws.GetRow(29).GetCell(7).SetCellValue(item.qm_j_f_zd100);
                            ws.GetRow(29).GetCell(8).SetCellValue(item.qm_j_f_fj100);
                            ws.GetRow(30).GetCell(7).SetCellValue(item.qm_j_f_zd70);
                            ws.GetRow(30).GetCell(8).SetCellValue(item.qm_j_f_fj70);
                            ws.GetRow(31).GetCell(7).SetCellValue(item.qm_j_f_zd50);
                            ws.GetRow(31).GetCell(8).SetCellValue(item.qm_j_f_fj50);
                            ws.GetRow(32).GetCell(7).SetCellValue(item.qm_j_f_zd30);
                            ws.GetRow(32).GetCell(8).SetCellValue(item.qm_j_f_fj30);
                            ws.GetRow(33).GetCell(7).SetCellValue(item.qm_j_f_zd10);
                            ws.GetRow(33).GetCell(8).SetCellValue(item.qm_j_f_fj10);

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj);

                            ws.GetRow(35).GetCell(3).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(3).SetCellValue(item.qm_F_MJ);
                        }
                    }
                    #endregion
                }
                #endregion

            }
            #endregion

            #region  水密性
            List<Model_dt_sm_Info> sm_Info = new DAL_dt_Settings().GetSMListByCode(_tempCode);
            if (sm_Info != null && sm_Info.Count > 0)
            {
                var isBoDong = false;
                if (sm_Info[0].Method == "波动加压")
                {
                    isBoDong = true;
                }
                if (isBoDong)
                {
                    var sm = sm_Info.FindAll(t => t.testcount == 1).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        #region 水密性（波动）
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性（波动）");
                        ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;
                            var res = "否";
                            if (item.sm_PaDesc.Contains("〇"))
                            {
                                res = "是";
                            }
                            if (index == 1)
                            {
                                if (item.xxyl == "0" && item.sxyl == "0")
                                {
                                    ws.GetRow(5).GetCell(4).SetCellValue(res);
                                    ws.GetRow(5).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "50" && item.sxyl == "150")
                                {
                                    ws.GetRow(6).GetCell(4).SetCellValue(res);
                                    ws.GetRow(6).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "75" && item.sxyl == "225")
                                {
                                    ws.GetRow(7).GetCell(4).SetCellValue(res);
                                    ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "100" && item.sxyl == "300")
                                {
                                    ws.GetRow(8).GetCell(4).SetCellValue(res);
                                    ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "125" && item.sxyl == "375")
                                {
                                    ws.GetRow(9).GetCell(4).SetCellValue(res);
                                    ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "150" && item.sxyl == "450")
                                {
                                    ws.GetRow(10).GetCell(4).SetCellValue(res);
                                    ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "175" && item.sxyl == "525")
                                {
                                    ws.GetRow(11).GetCell(4).SetCellValue(res);
                                    ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "200" && item.sxyl == "600")
                                {
                                    ws.GetRow(12).GetCell(4).SetCellValue(res);
                                    ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "250" && item.sxyl == "750")
                                {
                                    ws.GetRow(13).GetCell(4).SetCellValue(res);
                                    ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);

                                }
                                else if (item.xxyl == "300" && item.sxyl == "900")
                                {
                                    ws.GetRow(14).GetCell(4).SetCellValue(res);
                                    ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "350" && item.sxyl == "1050")
                                {
                                    ws.GetRow(15).GetCell(4).SetCellValue(res);
                                    ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(16).GetCell(1).SetCellValue(item.xxyl);
                                ws.GetRow(16).GetCell(2).SetCellValue("");
                                ws.GetRow(16).GetCell(3).SetCellValue(item.sxyl);
                            }
                            else if (index == 2)
                            {
                                if (item.xxyl == "0" && item.sxyl == "0")
                                {
                                    ws.GetRow(5).GetCell(7).SetCellValue(res);
                                    ws.GetRow(5).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "50" && item.sxyl == "150")
                                {
                                    ws.GetRow(6).GetCell(7).SetCellValue(res);
                                    ws.GetRow(6).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "75" && item.sxyl == "225")
                                {
                                    ws.GetRow(7).GetCell(7).SetCellValue(res);
                                    ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "100" && item.sxyl == "300")
                                {
                                    ws.GetRow(8).GetCell(7).SetCellValue(res);
                                    ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "125" && item.sxyl == "375")
                                {
                                    ws.GetRow(9).GetCell(7).SetCellValue(res);
                                    ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "150" && item.sxyl == "450")
                                {
                                    ws.GetRow(10).GetCell(7).SetCellValue(res);
                                    ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "175" && item.sxyl == "525")
                                {
                                    ws.GetRow(11).GetCell(7).SetCellValue(res);
                                    ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "200" && item.sxyl == "600")
                                {
                                    ws.GetRow(12).GetCell(7).SetCellValue(res);
                                    ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "250" && item.sxyl == "750")
                                {
                                    ws.GetRow(13).GetCell(7).SetCellValue(res);
                                    ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);

                                }
                                else if (item.xxyl == "300" && item.sxyl == "900")
                                {
                                    ws.GetRow(14).GetCell(7).SetCellValue(res);
                                    ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "350" && item.sxyl == "1050")
                                {
                                    ws.GetRow(15).GetCell(7).SetCellValue(res);
                                    ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            else if (index == 3)
                            {
                                if (item.xxyl == "0" && item.sxyl == "0")
                                {
                                    ws.GetRow(5).GetCell(10).SetCellValue(res);
                                    ws.GetRow(5).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "50" && item.sxyl == "150")
                                {
                                    ws.GetRow(6).GetCell(10).SetCellValue(res);
                                    ws.GetRow(6).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "75" && item.sxyl == "225")
                                {
                                    ws.GetRow(7).GetCell(10).SetCellValue(res);
                                    ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "100" && item.sxyl == "300")
                                {
                                    ws.GetRow(8).GetCell(10).SetCellValue(res);
                                    ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "125" && item.sxyl == "375")
                                {
                                    ws.GetRow(9).GetCell(10).SetCellValue(res);
                                    ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "150" && item.sxyl == "450")
                                {
                                    ws.GetRow(10).GetCell(10).SetCellValue(res);
                                    ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "175" && item.sxyl == "525")
                                {
                                    ws.GetRow(11).GetCell(10).SetCellValue(res);
                                    ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "200" && item.sxyl == "600")
                                {
                                    ws.GetRow(12).GetCell(10).SetCellValue(res);
                                    ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "250" && item.sxyl == "750")
                                {
                                    ws.GetRow(13).GetCell(10).SetCellValue(res);
                                    ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);

                                }
                                else if (item.xxyl == "300" && item.sxyl == "900")
                                {
                                    ws.GetRow(14).GetCell(10).SetCellValue(res);
                                    ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "350" && item.sxyl == "1050")
                                {
                                    ws.GetRow(15).GetCell(10).SetCellValue(res);
                                    ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                            }
                        }
                        #endregion

                    }
                    sm = sm_Info.FindAll(t => t.testcount == 2).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        #region 重复水密性（波动）
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性（波动）");
                        ws.GetRow(1).GetCell(1).SetCellValue("");
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        #region 赋值
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;
                            var res = "否";
                            if (item.sm_PaDesc.Contains("〇"))
                            {
                                res = "是";
                            }
                            if (index == 1)
                            {
                                if (item.xxyl == "0" && item.sxyl == "0")
                                {
                                    ws.GetRow(5).GetCell(4).SetCellValue(res);
                                    ws.GetRow(5).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "50" && item.sxyl == "150")
                                {
                                    ws.GetRow(6).GetCell(4).SetCellValue(res);
                                    ws.GetRow(6).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "75" && item.sxyl == "225")
                                {
                                    ws.GetRow(7).GetCell(4).SetCellValue(res);
                                    ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "100" && item.sxyl == "300")
                                {
                                    ws.GetRow(8).GetCell(4).SetCellValue(res);
                                    ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "125" && item.sxyl == "375")
                                {
                                    ws.GetRow(9).GetCell(4).SetCellValue(res);
                                    ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "150" && item.sxyl == "450")
                                {
                                    ws.GetRow(10).GetCell(4).SetCellValue(res);
                                    ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "175" && item.sxyl == "525")
                                {
                                    ws.GetRow(11).GetCell(4).SetCellValue(res);
                                    ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "200" && item.sxyl == "600")
                                {
                                    ws.GetRow(12).GetCell(4).SetCellValue(res);
                                    ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "250" && item.sxyl == "750")
                                {
                                    ws.GetRow(13).GetCell(4).SetCellValue(res);
                                    ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);

                                }
                                else if (item.xxyl == "300" && item.sxyl == "900")
                                {
                                    ws.GetRow(14).GetCell(4).SetCellValue(res);
                                    ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "350" && item.sxyl == "1050")
                                {
                                    ws.GetRow(15).GetCell(4).SetCellValue(res);
                                    ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(16).GetCell(1).SetCellValue(item.xxyl);
                                ws.GetRow(16).GetCell(2).SetCellValue("");
                                ws.GetRow(16).GetCell(3).SetCellValue(item.sxyl);
                            }
                            else if (index == 2)
                            {
                                if (item.xxyl == "0" && item.sxyl == "0")
                                {
                                    ws.GetRow(5).GetCell(7).SetCellValue(res);
                                    ws.GetRow(5).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "50" && item.sxyl == "150")
                                {
                                    ws.GetRow(6).GetCell(7).SetCellValue(res);
                                    ws.GetRow(6).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "75" && item.sxyl == "225")
                                {
                                    ws.GetRow(7).GetCell(7).SetCellValue(res);
                                    ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "100" && item.sxyl == "300")
                                {
                                    ws.GetRow(8).GetCell(7).SetCellValue(res);
                                    ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "125" && item.sxyl == "375")
                                {
                                    ws.GetRow(9).GetCell(7).SetCellValue(res);
                                    ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "150" && item.sxyl == "450")
                                {
                                    ws.GetRow(10).GetCell(7).SetCellValue(res);
                                    ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "175" && item.sxyl == "525")
                                {
                                    ws.GetRow(11).GetCell(7).SetCellValue(res);
                                    ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "200" && item.sxyl == "600")
                                {
                                    ws.GetRow(12).GetCell(7).SetCellValue(res);
                                    ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "250" && item.sxyl == "750")
                                {
                                    ws.GetRow(13).GetCell(7).SetCellValue(res);
                                    ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);

                                }
                                else if (item.xxyl == "300" && item.sxyl == "900")
                                {
                                    ws.GetRow(14).GetCell(7).SetCellValue(res);
                                    ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "350" && item.sxyl == "1050")
                                {
                                    ws.GetRow(15).GetCell(7).SetCellValue(res);
                                    ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            else if (index == 3)
                            {
                                if (item.xxyl == "0" && item.sxyl == "0")
                                {
                                    ws.GetRow(5).GetCell(10).SetCellValue(res);
                                    ws.GetRow(5).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "50" && item.sxyl == "150")
                                {
                                    ws.GetRow(6).GetCell(10).SetCellValue(res);
                                    ws.GetRow(6).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "75" && item.sxyl == "225")
                                {
                                    ws.GetRow(7).GetCell(10).SetCellValue(res);
                                    ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "100" && item.sxyl == "300")
                                {
                                    ws.GetRow(8).GetCell(10).SetCellValue(res);
                                    ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "125" && item.sxyl == "375")
                                {
                                    ws.GetRow(9).GetCell(10).SetCellValue(res);
                                    ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "150" && item.sxyl == "450")
                                {
                                    ws.GetRow(10).GetCell(10).SetCellValue(res);
                                    ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "175" && item.sxyl == "525")
                                {
                                    ws.GetRow(11).GetCell(10).SetCellValue(res);
                                    ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "200" && item.sxyl == "600")
                                {
                                    ws.GetRow(12).GetCell(10).SetCellValue(res);
                                    ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "250" && item.sxyl == "750")
                                {
                                    ws.GetRow(13).GetCell(10).SetCellValue(res);
                                    ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);

                                }
                                else if (item.xxyl == "300" && item.sxyl == "900")
                                {
                                    ws.GetRow(14).GetCell(10).SetCellValue(res);
                                    ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                else if (item.xxyl == "350" && item.sxyl == "1050")
                                {
                                    ws.GetRow(15).GetCell(10).SetCellValue(res);
                                    ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                            }
                        }
                        #endregion 
                        #endregion
                    }
                }
                else
                {
                    var sm = sm_Info.FindAll(t => t.testcount == 1).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性（稳定）");
                        ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        #region 水密性（稳定）
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;
                            var res = "否";
                            if (item.sm_PaDesc.Contains("〇"))
                            {
                                res = "是";
                            }
                            #region  赋值
                            if (index == 1)
                            {
                                if (item.gongchengjiance == "0")
                                {
                                    ws.GetRow(16).GetCell(1).SetCellValue("设计值");
                                }
                                else
                                {
                                    ws.GetRow(16).GetCell(1).SetCellValue(item.gongchengjiance);
                                }

                                ws.GetRow(17).GetCell(4).SetCellValue(item.sm_Pa);

                                if (item.sm_Pa == "0")
                                {
                                    ws.GetRow(5).GetCell(4).SetCellValue(res);
                                    ws.GetRow(5).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "100")
                                {
                                    ws.GetRow(6).GetCell(4).SetCellValue(res);
                                    ws.GetRow(6).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "150")
                                {
                                    ws.GetRow(7).GetCell(4).SetCellValue(res);
                                    ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "200")
                                {
                                    ws.GetRow(8).GetCell(4).SetCellValue(res);
                                    ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "250")
                                {
                                    ws.GetRow(9).GetCell(4).SetCellValue(res);
                                    ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "300")
                                {
                                    ws.GetRow(10).GetCell(4).SetCellValue(res);
                                    ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "350")
                                {
                                    ws.GetRow(11).GetCell(4).SetCellValue(res);
                                    ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "400")
                                {
                                    ws.GetRow(12).GetCell(4).SetCellValue(res);
                                    ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "500")
                                {
                                    ws.GetRow(13).GetCell(4).SetCellValue(res);
                                    ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "600")
                                {
                                    ws.GetRow(14).GetCell(4).SetCellValue(res);
                                    ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "700")
                                {
                                    ws.GetRow(15).GetCell(4).SetCellValue(res);
                                    ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            else if (index == 2)
                            {
                                ws.GetRow(17).GetCell(7).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    ws.GetRow(5).GetCell(7).SetCellValue(res);
                                    ws.GetRow(5).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "100")
                                {
                                    ws.GetRow(6).GetCell(7).SetCellValue(res);
                                    ws.GetRow(6).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "150")
                                {
                                    ws.GetRow(7).GetCell(7).SetCellValue(res);
                                    ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "200")
                                {
                                    ws.GetRow(8).GetCell(7).SetCellValue(res);
                                    ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "250")
                                {
                                    ws.GetRow(9).GetCell(7).SetCellValue(res);
                                    ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "300")
                                {
                                    ws.GetRow(10).GetCell(7).SetCellValue(res);
                                    ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "350")
                                {
                                    ws.GetRow(11).GetCell(7).SetCellValue(res);
                                    ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "400")
                                {
                                    ws.GetRow(12).GetCell(7).SetCellValue(res);
                                    ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "500")
                                {
                                    ws.GetRow(13).GetCell(7).SetCellValue(res);
                                    ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "600")
                                {
                                    ws.GetRow(14).GetCell(7).SetCellValue(res);
                                    ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "700")
                                {
                                    ws.GetRow(15).GetCell(7).SetCellValue(res);
                                    ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            else if (index == 3)
                            {
                                ws.GetRow(17).GetCell(10).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    ws.GetRow(5).GetCell(10).SetCellValue(res);
                                    ws.GetRow(5).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "100")
                                {
                                    ws.GetRow(6).GetCell(10).SetCellValue(res);
                                    ws.GetRow(6).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "150")
                                {
                                    ws.GetRow(7).GetCell(10).SetCellValue(res);
                                    ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "200")
                                {
                                    ws.GetRow(8).GetCell(10).SetCellValue(res);
                                    ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "250")
                                {
                                    ws.GetRow(9).GetCell(10).SetCellValue(res);
                                    ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "300")
                                {
                                    ws.GetRow(10).GetCell(10).SetCellValue(res);
                                    ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "350")
                                {
                                    ws.GetRow(11).GetCell(10).SetCellValue(res);
                                    ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "400")
                                {
                                    ws.GetRow(12).GetCell(10).SetCellValue(res);
                                    ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "500")
                                {
                                    ws.GetRow(13).GetCell(10).SetCellValue(res);
                                    ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "600")
                                {
                                    ws.GetRow(14).GetCell(10).SetCellValue(res);
                                    ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "700")
                                {
                                    ws.GetRow(15).GetCell(10).SetCellValue(res);
                                    ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }

                    sm = sm_Info.FindAll(t => t.testcount == 2).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性（稳定）");
                        ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        #region 重复水密性（稳定）
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;
                            var res = "否";
                            if (item.sm_PaDesc.Contains("〇"))
                            {
                                res = "是";
                            }
                            #region  赋值
                            if (index == 1)
                            {
                                if (item.gongchengjiance == "0")
                                {
                                    ws.GetRow(16).GetCell(1).SetCellValue("设计值");
                                }
                                else
                                {
                                    ws.GetRow(16).GetCell(1).SetCellValue(item.gongchengjiance);
                                }
                                ws.GetRow(17).GetCell(4).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    ws.GetRow(5).GetCell(4).SetCellValue(res);
                                    ws.GetRow(5).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "100")
                                {
                                    ws.GetRow(6).GetCell(4).SetCellValue(res);
                                    ws.GetRow(6).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "150")
                                {
                                    ws.GetRow(7).GetCell(4).SetCellValue(res);
                                    ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "200")
                                {
                                    ws.GetRow(8).GetCell(4).SetCellValue(res);
                                    ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "250")
                                {
                                    ws.GetRow(9).GetCell(4).SetCellValue(res);
                                    ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "300")
                                {
                                    ws.GetRow(10).GetCell(4).SetCellValue(res);
                                    ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "350")
                                {
                                    ws.GetRow(11).GetCell(4).SetCellValue(res);
                                    ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "400")
                                {
                                    ws.GetRow(12).GetCell(4).SetCellValue(res);
                                    ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "500")
                                {
                                    ws.GetRow(13).GetCell(4).SetCellValue(res);
                                    ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "600")
                                {
                                    ws.GetRow(14).GetCell(4).SetCellValue(res);
                                    ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "700")
                                {
                                    ws.GetRow(15).GetCell(4).SetCellValue(res);
                                    ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            else if (index == 2)
                            {
                                ws.GetRow(17).GetCell(7).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    ws.GetRow(5).GetCell(7).SetCellValue(res);
                                    ws.GetRow(5).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "100")
                                {
                                    ws.GetRow(6).GetCell(7).SetCellValue(res);
                                    ws.GetRow(6).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "150")
                                {
                                    ws.GetRow(7).GetCell(7).SetCellValue(res);
                                    ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "200")
                                {
                                    ws.GetRow(8).GetCell(7).SetCellValue(res);
                                    ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "250")
                                {
                                    ws.GetRow(9).GetCell(7).SetCellValue(res);
                                    ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "300")
                                {
                                    ws.GetRow(10).GetCell(7).SetCellValue(res);
                                    ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "350")
                                {
                                    ws.GetRow(11).GetCell(7).SetCellValue(res);
                                    ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "400")
                                {
                                    ws.GetRow(12).GetCell(7).SetCellValue(res);
                                    ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "500")
                                {
                                    ws.GetRow(13).GetCell(7).SetCellValue(res);
                                    ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "600")
                                {
                                    ws.GetRow(14).GetCell(7).SetCellValue(res);
                                    ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "700")
                                {
                                    ws.GetRow(15).GetCell(7).SetCellValue(res);
                                    ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            else if (index == 3)
                            {
                                ws.GetRow(17).GetCell(10).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    ws.GetRow(5).GetCell(10).SetCellValue(res);
                                    ws.GetRow(5).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "100")
                                {
                                    ws.GetRow(6).GetCell(10).SetCellValue(res);
                                    ws.GetRow(6).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "150")
                                {
                                    ws.GetRow(7).GetCell(10).SetCellValue(res);
                                    ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "200")
                                {
                                    ws.GetRow(8).GetCell(10).SetCellValue(res);
                                    ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "250")
                                {
                                    ws.GetRow(9).GetCell(10).SetCellValue(res);
                                    ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "300")
                                {
                                    ws.GetRow(10).GetCell(10).SetCellValue(res);
                                    ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "350")
                                {
                                    ws.GetRow(11).GetCell(10).SetCellValue(res);
                                    ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "400")
                                {
                                    ws.GetRow(12).GetCell(10).SetCellValue(res);
                                    ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "500")
                                {
                                    ws.GetRow(13).GetCell(10).SetCellValue(res);
                                    ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "600")
                                {
                                    ws.GetRow(14).GetCell(10).SetCellValue(res);
                                    ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                if (item.sm_Pa == "700")
                                {
                                    ws.GetRow(15).GetCell(10).SetCellValue(res);
                                    ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region 抗风压
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("抗风压");
            ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(2).GetCell(4).SetCellValue(dt_Settings.Rows[0][""].ToString());
            ws.GetRow(2).GetCell(11).SetCellValue("");
            ws.GetRow(3).GetCell(1).SetCellValue("");
            ws.GetRow(3).GetCell(4).SetCellValue("");
            ws.GetRow(3).GetCell(11).SetCellValue("");

            var databaseDefPa = 250;

            DataTable kfy_Info1 = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, "第1樘");

            if (kfy_Info1 != null && kfy_Info1.Rows.Count > 0)
            {
                var dr1 = kfy_Info1.Rows[0];
                var jc = int.Parse(dr1["defJC"].ToString());

                for (int i = 1; i < 9; i++)
                {
                    ws.GetRow(i + 6).GetCell(0).SetCellValue(jc * i);
                    ws.GetRow(i + 6).GetCell(1).SetCellValue(dr1["z_one_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(2).SetCellValue(dr1["z_two_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(3).SetCellValue(dr1["z_three_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(4).SetCellValue(dr1["z_nd_" + (databaseDefPa * i)].ToString());

                    ws.GetRow(i + 33).GetCell(0).SetCellValue(-(jc * i));
                    ws.GetRow(i + 33).GetCell(1).SetCellValue(dr1["f_one_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(2).SetCellValue(dr1["f_two_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(3).SetCellValue(dr1["f_three_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(4).SetCellValue(dr1["f_nd_" + (databaseDefPa * i)].ToString());
                }
            }

            DataTable kfy_Info2 = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, "第2樘");
            if (kfy_Info2 != null && kfy_Info2.Rows.Count > 0)
            {
                var dr2 = kfy_Info2.Rows[0];
                for (int i = 1; i < 9; i++)
                {
                    ws.GetRow(i + 6).GetCell(5).SetCellValue(dr2["z_one_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(6).SetCellValue(dr2["z_two_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(7).SetCellValue(dr2["z_three_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(8).SetCellValue(dr2["z_nd_" + (databaseDefPa * i)].ToString());

                    ws.GetRow(i + 33).GetCell(5).SetCellValue(dr2["f_one_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(6).SetCellValue(dr2["f_two_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(7).SetCellValue(dr2["f_three_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(8).SetCellValue(dr2["f_nd_" + (databaseDefPa * i)].ToString());
                }
            }

            DataTable kfy_Info3 = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, "第3樘");
            if (kfy_Info3 != null && kfy_Info3.Rows.Count > 0)
            {
                var dr3 = kfy_Info3.Rows[0];
                for (int i = 1; i < 9; i++)
                {
                    ws.GetRow(i + 6).GetCell(9).SetCellValue(dr3["z_one_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(10).SetCellValue(dr3["z_two_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(11).SetCellValue(dr3["z_three_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 6).GetCell(12).SetCellValue(dr3["z_nd_" + (databaseDefPa * i)].ToString());

                    ws.GetRow(i + 33).GetCell(9).SetCellValue(dr3["f_one_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(10).SetCellValue(dr3["f_two_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(11).SetCellValue(dr3["f_three_" + (databaseDefPa * i)].ToString());
                    ws.GetRow(i + 33).GetCell(12).SetCellValue(dr3["f_nd_" + (databaseDefPa * i)].ToString());
                }
            }

            #endregion

            ws.ForceFormulaRecalculation = true;

            using (FileStream filess = File.OpenWrite(outFilePath))
            {
                hssfworkbook.Write(filess);
            }

            return true;
        }


        //private GetSMIndex()
        //{
        //    List<AirtightCalculation> airtightCalculation = new List<AirtightCalculation>();
        //    airtightCalculation.Add(new AirtightCalculation()
        //    {
        //        PaValue = 10,
        //        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_Z"].Value.ToString()),
        //        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_Z"].Value.ToString()),

        //        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_F"].Value.ToString()),
        //        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_F"].Value.ToString()),

        //        // _Z_Q_SJ_P=17.478,
        //        kPa = kPa,
        //        CurrentTemperature = tempTemperature
        //    });
        //    airtightCalculation.Add(new AirtightCalculation()
        //    {
        //        PaValue = 30,
        //        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_F_Z"].Value.ToString()),
        //        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_F_Z"].Value.ToString()),
        //        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_F"].Value.ToString()),
        //        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_F"].Value.ToString()),

        //        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_F"].Value.ToString()),
        //        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_F"].Value.ToString()),
        //        //  _Z_Q_SJ_P = 22.062,
        //        kPa = kPa,
        //        CurrentTemperature = tempTemperature
        //    });
        //    airtightCalculation.Add(new AirtightCalculation()
        //    {
        //        PaValue = 50,
        //        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_Z"].Value.ToString()),
        //        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_Z"].Value.ToString()),

        //        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_F"].Value.ToString()),
        //        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_F"].Value.ToString()),
        //        kPa = kPa,
        //        // _Z_Q_SJ_P = 25.786,
        //        CurrentTemperature = tempTemperature
        //    });
        //    airtightCalculation.Add(new AirtightCalculation()
        //    {
        //        PaValue = 70,
        //        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_Z"].Value.ToString()),
        //        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_Z"].Value.ToString()),

        //        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_F"].Value.ToString()),
        //        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_F"].Value.ToString()),
        //        kPa = kPa,
        //        //  _Z_Q_SJ_P = 35.815,
        //        CurrentTemperature = tempTemperature
        //    });
        //    airtightCalculation.Add(new AirtightCalculation()
        //    {
        //        PaValue = 100,
        //        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
        //        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z"].Value.ToString()),
        //        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z"].Value.ToString()),

        //        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
        //        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F"].Value.ToString()),
        //        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F"].Value.ToString()),
        //        kPa = kPa,
        //        // _Z_Q_SJ_P = 63.893,
        //        CurrentTemperature = tempTemperature
        //    });

        //    //获取分级指标
        //    var indexStitchLengthAndArea = Formula.GetJK_IndexStitchLengthAndArea(airtightCalculation, stitchLength, sumArea);
        //    if (indexStitchLengthAndArea != null)
        //    {
        //        zFc = indexStitchLengthAndArea.ZY_FC;
        //        fFc = indexStitchLengthAndArea.FY_FC;
        //        zMj = indexStitchLengthAndArea.ZY_MJ;
        //        fMj = indexStitchLengthAndArea.FY_MJ;
        //    }
        //}


    }

}