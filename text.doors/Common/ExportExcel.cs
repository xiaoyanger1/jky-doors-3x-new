using NPOI.SS.Formula.Functions;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Converters;
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
            //try
            //{

            Model_dt_Settings _settings = new DAL_dt_Settings().GetInfoByCode(_tempCode);

            FileStream file = new FileStream(this.templetFileName, FileMode.Open, FileAccess.Read);
            NPOI.HSSF.UserModel.HSSFWorkbook hssfworkbook = new NPOI.HSSF.UserModel.HSSFWorkbook(file);
            NPOI.HSSF.UserModel.HSSFSheet ws = new NPOI.HSSF.UserModel.HSSFSheet(hssfworkbook);

            DataTable dt_Settings = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);

            List<Model_dt_qm_Info> qm_Info = new DAL_dt_Settings().GetQMListByCode(_tempCode);

            List<Model_dt_sm_Info> sm_Info = new DAL_dt_Settings().GetSMListByCode(_tempCode);

            if (dt_Settings == null || dt_Settings.Rows.Count == 0)
                return false;

            #region   --性能检测报告--
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("性能检测报告");
            ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
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
            ws.GetRow(9).GetCell(3).SetCellValue(dt_Settings.Rows[0]["yangpinshangbiao"].ToString());
            ws.GetRow(9).GetCell(8).SetCellValue(dt_Settings.Rows[0]["guigexinghao"].ToString());
            //检测
            ws.GetRow(10).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jiancexiangmu"].ToString());
            ws.GetRow(10).GetCell(8).SetCellValue(dt_Settings.Rows[0]["jianceshuliang"].ToString());
            ws.GetRow(11).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jiancedidian"].ToString());
            ws.GetRow(11).GetCell(8).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(12).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceyiju"].ToString());
            ws.GetRow(13).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceshebei"].ToString());


            if (sm_Info != null && sm_Info.Count > 0)
            {
                if (sm_Info[0].Method == "波动加压")
                    ws.GetRow(19).GetCell(4).SetCellValue("(采用波动加压方法检测)");
                else
                    ws.GetRow(19).GetCell(4).SetCellValue("(采用稳定加压方法检测)");
            }
            //结论
            Formula formula = new Formula();

            var zLevel = formula.Get_Z_AirTightLevel(qm_Info).ToString();
            var fLevel = formula.Get_F_AirTightLevel(qm_Info).ToString();

            ws.GetRow(16).GetCell(8).SetCellValue(zLevel);
            ws.GetRow(17).GetCell(8).SetCellValue(fLevel);

            var smLevel = formula.GetWaterTightLevel(sm_Info).ToString();
            ws.GetRow(18).GetCell(8).SetCellValue(smLevel);

            List<int> kfyValue = new List<int>();

            foreach (var item in _settings.dt_kfy_Info)
            {
                kfyValue.Add(int.Parse(item.p3));
                kfyValue.Add(int.Parse(item._p3));
            }
            if (kfyValue != null && kfyValue.Count() > 0)
            {
                var minValue = kfyValue.Min(t => t);
                var kfyLevel = Formula.GetWindPressureLevel(minValue).ToString();

                ws.GetRow(20).GetCell(8).SetCellValue(kfyLevel);
            }
            else
            {
                ws.GetRow(20).GetCell(8).SetCellValue("-");
            }
            #endregion

            #region 质量检测报告
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("质量检测报告");
            ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(2).GetCell(6).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(3).GetCell(2).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());
            ws.GetRow(3).GetCell(9).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
            ws.GetRow(4).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanpinzhong"].ToString());
            ws.GetRow(4).GetCell(9).SetCellValue(dt_Settings.Rows[0]["anzhuangfangshi"].ToString());
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
            ws.GetRow(8).GetCell(3).SetCellValue(dt_Settings.Rows[0]["qimidanweifengchangshejizhi"].ToString());
            ws.GetRow(8).GetCell(8).SetCellValue(dt_Settings.Rows[0]["shuimijingyashejizhi"].ToString());
            ws.GetRow(8).GetCell(12).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengyashejizhi"].ToString());

            ws.GetRow(9).GetCell(3).SetCellValue(dt_Settings.Rows[0]["qimidanweimianjishejizhi"].ToString());
            ws.GetRow(9).GetCell(8).SetCellValue(dt_Settings.Rows[0]["shuimidongyashejizhi"].ToString());
            ws.GetRow(9).GetCell(12).SetCellValue(dt_Settings.Rows[0]["kangfengyafuyashejizhi"].ToString());
            //气密性能
            double zfc = 0;
            double zmj = 0;
            double ffc = 0;
            double fmj = 0;
            if (qm_Info != null && qm_Info.Count > 0)
            {
                zfc = double.Parse(qm_Info.FindAll(t => t.testcount == 1).Max(t => t.qm_Z_FC));
                zmj = double.Parse(qm_Info.FindAll(t => t.testcount == 1).Max(t => t.qm_Z_MJ));
                ffc = double.Parse(qm_Info.FindAll(t => t.testcount == 1).Max(t => t.qm_F_FC));
                fmj = double.Parse(qm_Info.FindAll(t => t.testcount == 1).Max(t => t.qm_F_MJ));
            }
            ws.GetRow(11).GetCell(6).SetCellValue(zfc);
            ws.GetRow(11).GetCell(9).SetCellValue(ffc);
            ws.GetRow(12).GetCell(6).SetCellValue(zmj);
            ws.GetRow(12).GetCell(9).SetCellValue(fmj);
            //水密性能
            ws.GetRow(13).GetCell(3).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());

            if (sm_Info?.FindAll(t => t.testcount == 1) != null && sm_Info.FindAll(t => t.testcount == 1).Count > 0)
            {
                if (sm_Info.FindAll(t => t.testcount == 1)[0].Method == "波动加压")
                {
                    //稳定
                    ws.GetRow(14).GetCell(5).SetCellValue("-");
                    ws.GetRow(15).GetCell(5).SetCellValue("-");

                    //波动
                    var level1 = sm_Info.FindAll(t => t.testcount == 1);
                    var wdPa = 999;
                    var wdPaDesc = "";
                    if (level1 != null && level1.Count > 0)
                    {
                        foreach (var item in level1)
                        {
                            if ((!item.sm_PaDesc.Contains("〇") && !item.sm_PaDesc.Contains("□")) && int.Parse(item.sm_Pa) < 999)
                            {
                                wdPa = int.Parse(item.sm_Pa);
                                wdPaDesc = item.sm_PaDesc;
                            }
                        }
                        if (wdPa == 999)
                        {
                            ws.GetRow(16).GetCell(5).SetCellValue("-");
                            ws.GetRow(17).GetCell(5).SetCellValue("-");
                        }
                        else
                        {
                            ws.GetRow(16).GetCell(5).SetCellValue(wdPa);
                            if (wdPa == 0) { ws.GetRow(17).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 100) { ws.GetRow(17).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 150) { ws.GetRow(17).GetCell(5).SetCellValue("100"); }
                            else if (wdPa == 200) { ws.GetRow(17).GetCell(5).SetCellValue("150"); }
                            else if (wdPa == 250) { ws.GetRow(17).GetCell(5).SetCellValue("200"); }
                            else if (wdPa == 300) { ws.GetRow(17).GetCell(5).SetCellValue("250"); }
                            else if (wdPa == 350) { ws.GetRow(17).GetCell(5).SetCellValue("300"); }
                            else if (wdPa == 400) { ws.GetRow(17).GetCell(5).SetCellValue("350"); }
                            else if (wdPa == 500) { ws.GetRow(17).GetCell(5).SetCellValue("400"); }
                            else if (wdPa == 600) { ws.GetRow(17).GetCell(5).SetCellValue("500"); }
                            else if (wdPa == 700) { ws.GetRow(17).GetCell(5).SetCellValue("600"); }
                        }
                    }
                }
                else
                {
                    //稳定加压
                    var level1 = sm_Info.FindAll(t => t.testcount == 1);
                    var wdPa = 999;
                    var wdPaDesc = "";
                    if (level1 != null && level1.Count > 0)
                    {
                        foreach (var item in level1)
                        {
                            if ((!item.sm_PaDesc.Contains("〇") && !item.sm_PaDesc.Contains("□")) && int.Parse(item.sm_Pa) < 999)
                            {
                                wdPa = int.Parse(item.sm_Pa);
                                wdPaDesc = item.sm_PaDesc;
                            }
                        }
                        if (wdPa == 999)
                        {
                            ws.GetRow(14).GetCell(5).SetCellValue("-");
                            ws.GetRow(15).GetCell(5).SetCellValue("-");
                        }
                        else
                        {
                            ws.GetRow(14).GetCell(5).SetCellValue(wdPa);
                            if (wdPa == 0) { ws.GetRow(15).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 100) { ws.GetRow(15).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 150) { ws.GetRow(15).GetCell(5).SetCellValue("100"); }
                            else if (wdPa == 200) { ws.GetRow(15).GetCell(5).SetCellValue("150"); }
                            else if (wdPa == 250) { ws.GetRow(15).GetCell(5).SetCellValue("200"); }
                            else if (wdPa == 300) { ws.GetRow(15).GetCell(5).SetCellValue("250"); }
                            else if (wdPa == 350) { ws.GetRow(15).GetCell(5).SetCellValue("300"); }
                            else if (wdPa == 400) { ws.GetRow(15).GetCell(5).SetCellValue("350"); }
                            else if (wdPa == 500) { ws.GetRow(15).GetCell(5).SetCellValue("400"); }
                            else if (wdPa == 600) { ws.GetRow(15).GetCell(5).SetCellValue("500"); }
                            else if (wdPa == 700) { ws.GetRow(15).GetCell(5).SetCellValue("600"); }
                        }
                    }

                    ws.GetRow(16).GetCell(5).SetCellValue("-");
                    ws.GetRow(17).GetCell(5).SetCellValue("-");
                }
            }

            //抗风压
            var kfyList = _settings.dt_kfy_Info;
            if (kfyList != null && kfyList.Count > 0)
            {
                if (kfyList[0].testtype == 2)
                {
                    //风荷载
                    ws.GetRow(35).GetCell(6).SetCellValue(kfyList.Min(t => t.p3));
                    ws.GetRow(36).GetCell(6).SetCellValue(kfyList.Min(t => t._p3));
                    ws.GetRow(37).GetCell(6).SetCellValue(kfyList.Min(t => t.pMax));
                    ws.GetRow(38).GetCell(6).SetCellValue(kfyList.Min(t => t._pMax));

                    ws.GetRow(18).GetCell(6).SetCellValue("-");
                    ws.GetRow(19).GetCell(6).SetCellValue("-");
                    ws.GetRow(20).GetCell(6).SetCellValue("-");
                    ws.GetRow(21).GetCell(6).SetCellValue("-");
                    //风荷载
                    ws.GetRow(23).GetCell(6).SetCellValue("-");
                    ws.GetRow(24).GetCell(6).SetCellValue("-");
                    ws.GetRow(25).GetCell(6).SetCellValue("-");
                    ws.GetRow(26).GetCell(6).SetCellValue("-");
                }
                else
                {
                    ws.GetRow(18).GetCell(6).SetCellValue(kfyList.Min(t => t.p1));
                    ws.GetRow(19).GetCell(6).SetCellValue(kfyList.Min(t => t._p1));
                    ws.GetRow(20).GetCell(6).SetCellValue(kfyList.Min(t => t.p2));
                    ws.GetRow(21).GetCell(6).SetCellValue(kfyList.Min(t => t._p2));
                    //风荷载
                    ws.GetRow(23).GetCell(6).SetCellValue(kfyList.Min(t => t.p3));
                    ws.GetRow(24).GetCell(6).SetCellValue(kfyList.Min(t => t._p3));
                    ws.GetRow(25).GetCell(6).SetCellValue(kfyList.Min(t => t.pMax));
                    ws.GetRow(26).GetCell(6).SetCellValue(kfyList.Min(t => t._pMax));
                    //风荷载
                    ws.GetRow(35).GetCell(6).SetCellValue("-");
                    ws.GetRow(36).GetCell(6).SetCellValue("-");
                    ws.GetRow(37).GetCell(6).SetCellValue("-");
                    ws.GetRow(38).GetCell(6).SetCellValue("-");
                }



            }

            //重复气密性能
            double zfc1 = 0;
            double zmj1 = 0;
            double ffc1 = 0;
            double fmj1 = 0;
            if (qm_Info.FindAll(t => t.testcount == 2) != null && qm_Info.FindAll(t => t.testcount == 2).Count > 0)
            {
                zfc1 = double.Parse(qm_Info.FindAll(t => t.testcount == 2).Max(t => t.qm_Z_FC));
                zmj1 = double.Parse(qm_Info.FindAll(t => t.testcount == 2).Max(t => t.qm_Z_MJ));
                ffc1 = double.Parse(qm_Info.FindAll(t => t.testcount == 2).Max(t => t.qm_F_FC));
                fmj1 = double.Parse(qm_Info.FindAll(t => t.testcount == 2).Max(t => t.qm_F_MJ));
            }

            ws.GetRow(27).GetCell(6).SetCellValue(zfc1);
            ws.GetRow(27).GetCell(9).SetCellValue(ffc1);
            ws.GetRow(28).GetCell(6).SetCellValue(zmj1);
            ws.GetRow(28).GetCell(9).SetCellValue(fmj1);
            //水密性能
            ws.GetRow(29).GetCell(3).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());

            if (sm_Info?.FindAll(t => t.testcount == 2) != null && sm_Info.FindAll(t => t.testcount == 1).Count > 0)
            {
                if (sm_Info.FindAll(t => t.testcount == 1)[0].Method == "波动加压")
                {
                    #region  波动加压
                    //稳定
                    ws.GetRow(30).GetCell(5).SetCellValue("-");
                    ws.GetRow(31).GetCell(5).SetCellValue("-");

                    //波动
                    var level1 = sm_Info.FindAll(t => t.testcount == 1);
                    var wdPa = 999;
                    var wdPaDesc = "";
                    if (level1 != null && level1.Count > 0)
                    {
                        foreach (var item in level1)
                        {
                            if ((!item.sm_PaDesc.Contains("〇") && !item.sm_PaDesc.Contains("□")) && int.Parse(item.sm_Pa) < 999)
                            {
                                wdPa = int.Parse(item.sm_Pa);
                                wdPaDesc = item.sm_PaDesc;
                            }
                        }
                        if (wdPa == 999)
                        {
                            ws.GetRow(32).GetCell(5).SetCellValue("-");
                            ws.GetRow(33).GetCell(5).SetCellValue("-");
                        }
                        else
                        {
                            ws.GetRow(32).GetCell(5).SetCellValue(wdPa);
                            if (wdPa == 0) { ws.GetRow(33).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 100) { ws.GetRow(33).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 150) { ws.GetRow(33).GetCell(5).SetCellValue("100"); }
                            else if (wdPa == 200) { ws.GetRow(33).GetCell(5).SetCellValue("150"); }
                            else if (wdPa == 250) { ws.GetRow(33).GetCell(5).SetCellValue("200"); }
                            else if (wdPa == 300) { ws.GetRow(33).GetCell(5).SetCellValue("250"); }
                            else if (wdPa == 350) { ws.GetRow(33).GetCell(5).SetCellValue("300"); }
                            else if (wdPa == 400) { ws.GetRow(33).GetCell(5).SetCellValue("350"); }
                            else if (wdPa == 500) { ws.GetRow(33).GetCell(5).SetCellValue("400"); }
                            else if (wdPa == 600) { ws.GetRow(33).GetCell(5).SetCellValue("500"); }
                            else if (wdPa == 700) { ws.GetRow(33).GetCell(5).SetCellValue("600"); }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region  稳定加压
                    //稳定加压
                    var level1 = sm_Info.FindAll(t => t.testcount == 2);
                    var wdPa = 999;
                    var wdPaDesc = "";
                    if (level1 != null && level1.Count > 0)
                    {
                        foreach (var item in level1)
                        {
                            if ((!item.sm_PaDesc.Contains("〇") && !item.sm_PaDesc.Contains("□")) && int.Parse(item.sm_Pa) < 999)
                            {
                                wdPa = int.Parse(item.sm_Pa);
                                wdPaDesc = item.sm_PaDesc;
                            }
                        }
                        if (wdPa == 999)
                        {
                            ws.GetRow(30).GetCell(5).SetCellValue("-");
                            ws.GetRow(31).GetCell(5).SetCellValue("-");
                        }
                        else
                        {
                            ws.GetRow(30).GetCell(5).SetCellValue(wdPa);
                            if (wdPa == 0) { ws.GetRow(31).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 100) { ws.GetRow(31).GetCell(5).SetCellValue("0"); }
                            else if (wdPa == 150) { ws.GetRow(31).GetCell(5).SetCellValue("100"); }
                            else if (wdPa == 200) { ws.GetRow(31).GetCell(5).SetCellValue("150"); }
                            else if (wdPa == 250) { ws.GetRow(31).GetCell(5).SetCellValue("200"); }
                            else if (wdPa == 300) { ws.GetRow(31).GetCell(5).SetCellValue("250"); }
                            else if (wdPa == 350) { ws.GetRow(31).GetCell(5).SetCellValue("300"); }
                            else if (wdPa == 400) { ws.GetRow(31).GetCell(5).SetCellValue("350"); }
                            else if (wdPa == 500) { ws.GetRow(31).GetCell(5).SetCellValue("400"); }
                            else if (wdPa == 600) { ws.GetRow(31).GetCell(5).SetCellValue("500"); }
                            else if (wdPa == 700) { ws.GetRow(31).GetCell(5).SetCellValue("600"); }
                        }
                    }

                    ws.GetRow(32).GetCell(5).SetCellValue("-");
                    ws.GetRow(33).GetCell(5).SetCellValue("-");
                    #endregion
                }
            }


            #endregion

            #region 气密性

            if (qm_Info != null && qm_Info.Count > 0)
            {
                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("气密性");
                ws.ForceFormulaRecalculation = true;
                ws.GetRow(2).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(2).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
                ws.GetRow(2).GetCell(7).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());

                ws.GetRow(3).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(3).GetCell(5).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
                ws.GetRow(3).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());

                #region  气密性数据
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
                                ws.GetRow(18).GetCell(2).SetCellValue("-");
                            }
                            else
                            {
                                ws.GetRow(18).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd == "0.00" ? "-" : item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj == "0.00" ? "-" : item.sjz_z_fj);

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
                                ws.GetRow(34).GetCell(2).SetCellValue("-");
                            }
                            else
                            {
                                ws.GetRow(34).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd == "0.00" ? "-" : item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj == "0.00" ? "-" : item.sjz_f_fj);

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

                            ws.GetRow(18).GetCell(5).SetCellValue(item.sjz_z_zd == "0.00" ? "-" : item.sjz_z_zd);
                            ws.GetRow(18).GetCell(6).SetCellValue(item.sjz_z_fj == "0.00" ? "-" : item.sjz_z_fj);

                            ws.GetRow(19).GetCell(5).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(5).SetCellValue(item.qm_Z_MJ);
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

                            ws.GetRow(34).GetCell(5).SetCellValue(item.sjz_f_zd == "0.00" ? "-" : item.sjz_f_zd);
                            ws.GetRow(34).GetCell(6).SetCellValue(item.sjz_f_fj == "0.00" ? "-" : item.sjz_f_fj);

                            ws.GetRow(35).GetCell(5).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(5).SetCellValue(item.qm_F_MJ);
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

                            ws.GetRow(18).GetCell(7).SetCellValue(item.sjz_z_zd == "0.00" ? "-" : item.sjz_z_zd);
                            ws.GetRow(18).GetCell(8).SetCellValue(item.sjz_z_fj == "0.00" ? "-" : item.sjz_z_fj);

                            ws.GetRow(19).GetCell(7).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(7).SetCellValue(item.qm_Z_MJ);
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

                            ws.GetRow(34).GetCell(7).SetCellValue(item.sjz_f_zd == "0.00" ? "-" : item.sjz_f_zd);
                            ws.GetRow(34).GetCell(8).SetCellValue(item.sjz_f_fj == "0.00" ? "-" : item.sjz_f_fj);

                            ws.GetRow(35).GetCell(7).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(7).SetCellValue(item.qm_F_MJ);
                        }
                    }
                    #endregion
                }
                #endregion

                ws.GetRow(44).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(44).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
                ws.GetRow(44).GetCell(7).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());

                ws.GetRow(45).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(45).GetCell(5).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
                ws.GetRow(45).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
            }
            #endregion

            #region 重复气密性
            if (qm_Info != null && qm_Info.Count > 0)
            {
                #region 重复气密性

                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复气密性");
                ws.ForceFormulaRecalculation = true;
                ws.GetRow(2).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(2).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
                ws.GetRow(2).GetCell(7).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());

                ws.GetRow(3).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(3).GetCell(5).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
                ws.GetRow(3).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());


                var qm = qm_Info.FindAll(t => t.testcount == 2).OrderBy(t => t.info_DangH).ToList();
                if ((qm != null && qm.Count() > 0))
                {
                    ws.ForceFormulaRecalculation = true;
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
                                ws.GetRow(18).GetCell(2).SetCellValue("-");
                            }
                            else
                            {
                                ws.GetRow(18).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(18).GetCell(3).SetCellValue(item.sjz_z_zd == "0" ? "-" : item.sjz_z_zd);
                            ws.GetRow(18).GetCell(4).SetCellValue(item.sjz_z_fj == "0" ? "-" : item.sjz_z_fj);

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
                                ws.GetRow(34).GetCell(2).SetCellValue("-");
                            }
                            else
                            {
                                ws.GetRow(34).GetCell(2).SetCellValue(item.sjz_value);
                            }

                            ws.GetRow(34).GetCell(3).SetCellValue(item.sjz_f_zd == "0" ? "-" : item.sjz_f_zd);
                            ws.GetRow(34).GetCell(4).SetCellValue(item.sjz_f_fj == "0" ? "-" : item.sjz_f_fj);

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

                            ws.GetRow(18).GetCell(5).SetCellValue(item.sjz_z_zd == "0" ? "-" : item.sjz_z_zd);
                            ws.GetRow(18).GetCell(6).SetCellValue(item.sjz_z_fj == "0" ? "-" : item.sjz_z_fj);

                            ws.GetRow(19).GetCell(5).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(5).SetCellValue(item.qm_Z_MJ);
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

                            ws.GetRow(34).GetCell(5).SetCellValue(item.sjz_f_zd == "0" ? "-" : item.sjz_f_zd);
                            ws.GetRow(34).GetCell(6).SetCellValue(item.sjz_f_fj == "0" ? "-" : item.sjz_f_fj);

                            ws.GetRow(35).GetCell(5).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(5).SetCellValue(item.qm_F_MJ);
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

                            ws.GetRow(18).GetCell(7).SetCellValue(item.sjz_z_zd == "0" ? "-" : item.sjz_z_zd);
                            ws.GetRow(18).GetCell(8).SetCellValue(item.sjz_z_fj == "0" ? "-" : item.sjz_z_fj);

                            ws.GetRow(19).GetCell(7).SetCellValue(item.qm_Z_FC);
                            ws.GetRow(20).GetCell(7).SetCellValue(item.qm_Z_MJ);
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

                            ws.GetRow(34).GetCell(7).SetCellValue(item.sjz_f_zd == "0" ? "-" : item.sjz_f_zd);
                            ws.GetRow(34).GetCell(8).SetCellValue(item.sjz_f_fj == "0" ? "-" : item.sjz_f_fj);

                            ws.GetRow(35).GetCell(7).SetCellValue(item.qm_F_FC);
                            ws.GetRow(36).GetCell(7).SetCellValue(item.qm_F_MJ);
                        }
                    }
                    #endregion
                }

                ws.GetRow(44).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(44).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
                ws.GetRow(44).GetCell(7).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());

                ws.GetRow(45).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(45).GetCell(5).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
                ws.GetRow(45).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                #endregion
            }
            #endregion

            #region  水密性

            if (sm_Info != null && sm_Info.Count > 0)
            {
                var isBoDong = false;
                if (sm_Info[0].Method == "波动加压")
                {
                    isBoDong = true;
                }

                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性(稳定)");
                ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());

                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性(稳定)");
                ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());

                #region 水密性（波动）
                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性(波动)");
                ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                #endregion




                ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性(波动)");
                ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());


                if (isBoDong)
                {
                    var sm = sm_Info.FindAll(t => t.testcount == 1).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        #region 水密性（波动）
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性(波动)");
                        ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;
                            var res = "是";
                            var isLeakage = true; //是否渗漏
                            if (item.sm_PaDesc.Contains("〇") || item.sm_PaDesc.Contains("□"))
                            {
                                res = "否";
                                isLeakage = false;
                            }
                            if (index == 1)
                            {
                                #region 赋值
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

                                #endregion
                            }
                            else if (index == 2)
                            {
                                #region 赋值
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
                                #endregion
                            }
                            else if (index == 3)
                            {
                                #region 赋值
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
                                #endregion
                            }
                        }
                        #endregion
                    }

                    sm = sm_Info.FindAll(t => t.testcount == 2).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        #region 重复水密性（波动）
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性(波动)");
                        ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        #region 赋值
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;
                            var res = "否";
                            if (item.sm_PaDesc.Contains("〇") || item.sm_PaDesc.Contains("□"))
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
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性(稳定)");
                        ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        #region 水密性（稳定）
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;

                            var res = "是";
                            var isLeakage = true; //是否渗漏
                            if (item.sm_PaDesc.Contains("〇") || item.sm_PaDesc.Contains("□"))
                            {
                                res = "否";
                                isLeakage = false;
                            }

                            if (index == 1)
                            {
                                #region  赋值
                                if (item.gongchengjiance == "0")
                                    ws.GetRow(16).GetCell(1).SetCellValue("-");
                                else
                                {
                                    ws.GetRow(16).GetCell(1).SetCellValue(item.gongchengjiance);
                                    ws.GetRow(16).GetCell(4).SetCellValue(res);
                                    ws.GetRow(16).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(17).GetCell(4).SetCellValue(item.sm_Pa);

                                if (item.sm_Pa == "0")
                                {
                                    if (isLeakage)
                                    {
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue(res);
                                        ws.GetRow(5).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "100")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue(res);
                                        ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue(res);
                                        ws.GetRow(6).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "150")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue(res);
                                        ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue(res);
                                        ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "200")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue(res);
                                        ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue(res);
                                        ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "250")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue(res);
                                        ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue(res);
                                        ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "300")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue(res);
                                        ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue(res);
                                        ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "350")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue(res);
                                        ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);

                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue(res);
                                        ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "400")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue(res);
                                        ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue(res);
                                        ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "500")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue(res);
                                        ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue(res);
                                        ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "600")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue("否");
                                        ws.GetRow(14).GetCell(5).SetCellValue("-");
                                        ws.GetRow(15).GetCell(4).SetCellValue(res);
                                        ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue(res);
                                        ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "700")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue("否");
                                        ws.GetRow(14).GetCell(5).SetCellValue("-");
                                        ws.GetRow(15).GetCell(4).SetCellValue("否");
                                        ws.GetRow(15).GetCell(5).SetCellValue("-");
                                        ws.GetRow(16).GetCell(4).SetCellValue(res);
                                        ws.GetRow(16).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue("否");
                                        ws.GetRow(14).GetCell(5).SetCellValue("-");
                                        ws.GetRow(15).GetCell(4).SetCellValue(res);
                                        ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }

                                #endregion
                            }
                            else if (index == 2)
                            {
                                #region 赋值
                                if (item.gongchengjiance == "0") { }
                                else
                                {
                                    ws.GetRow(16).GetCell(7).SetCellValue(res);
                                    ws.GetRow(16).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(17).GetCell(7).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    if (isLeakage)
                                    {
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue(res);
                                        ws.GetRow(5).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }

                                if (item.sm_Pa == "100")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue(res);
                                        ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue(res);
                                        ws.GetRow(6).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "150")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue(res);
                                        ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue(res);
                                        ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "200")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue(res);
                                        ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue(res);
                                        ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "250")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue(res);
                                        ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue(res);
                                        ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "300")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue(res);
                                        ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue(res);
                                        ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "350")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue(res);
                                        ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);

                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue(res);
                                        ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "400")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue(res);
                                        ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue(res);
                                        ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "500")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue(res);
                                        ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue(res);
                                        ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "600")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue("否");
                                        ws.GetRow(14).GetCell(8).SetCellValue("-");
                                        ws.GetRow(15).GetCell(7).SetCellValue(res);
                                        ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue(res);
                                        ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "700")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue("否");
                                        ws.GetRow(14).GetCell(8).SetCellValue("-");
                                        ws.GetRow(15).GetCell(7).SetCellValue("否");
                                        ws.GetRow(15).GetCell(8).SetCellValue("-");
                                        ws.GetRow(16).GetCell(7).SetCellValue(res);
                                        ws.GetRow(16).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue("否");
                                        ws.GetRow(14).GetCell(8).SetCellValue("-");
                                        ws.GetRow(15).GetCell(7).SetCellValue(res);
                                        ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                #endregion
                            }
                            else if (index == 3)
                            {
                                #region 赋值
                                if (item.gongchengjiance == "0") { }
                                else
                                {
                                    ws.GetRow(16).GetCell(10).SetCellValue(res);
                                    ws.GetRow(16).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(17).GetCell(10).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    if (isLeakage)
                                    {
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue(res);
                                        ws.GetRow(5).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }

                                if (item.sm_Pa == "100")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue(res);
                                        ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue(res);
                                        ws.GetRow(6).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "150")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue(res);
                                        ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue(res);
                                        ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "200")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue(res);
                                        ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue(res);
                                        ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "250")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue(res);
                                        ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue(res);
                                        ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "300")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue(res);
                                        ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue(res);
                                        ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "350")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue(res);
                                        ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);

                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue(res);
                                        ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "400")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue(res);
                                        ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue(res);
                                        ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "500")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue(res);
                                        ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue(res);
                                        ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "600")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue("否");
                                        ws.GetRow(14).GetCell(11).SetCellValue("-");
                                        ws.GetRow(15).GetCell(10).SetCellValue(res);
                                        ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue(res);
                                        ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "700")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue("否");
                                        ws.GetRow(14).GetCell(11).SetCellValue("-");
                                        ws.GetRow(15).GetCell(10).SetCellValue("否");
                                        ws.GetRow(15).GetCell(11).SetCellValue("-");
                                        ws.GetRow(16).GetCell(10).SetCellValue(res);
                                        ws.GetRow(16).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue("否");
                                        ws.GetRow(14).GetCell(11).SetCellValue("-");
                                        ws.GetRow(15).GetCell(10).SetCellValue(res);
                                        ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                #endregion
                            }

                        }
                        #endregion
                    }


                    sm = sm_Info.FindAll(t => t.testcount == 2).OrderBy(t => t.info_DangH).ToList();
                    if ((sm != null && sm.Count() > 0))
                    {
                        ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性(稳定)");
                        ws.GetRow(1).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
                        ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
                        ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
                        ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
                        #region 重复水密性（稳定）
                        var index = 0;
                        foreach (var item in sm)
                        {
                            index++;

                            var res = "是";
                            var isLeakage = true; //是否渗漏
                            if (item.sm_PaDesc.Contains("〇") || item.sm_PaDesc.Contains("□"))
                            {
                                res = "否";
                                isLeakage = false;
                            }

                            if (index == 1)
                            {
                                #region  赋值
                                if (item.gongchengjiance == "0")
                                    ws.GetRow(16).GetCell(1).SetCellValue("-");
                                else
                                {
                                    ws.GetRow(16).GetCell(1).SetCellValue(item.gongchengjiance);
                                    ws.GetRow(16).GetCell(4).SetCellValue(res);
                                    ws.GetRow(16).GetCell(5).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(17).GetCell(4).SetCellValue(item.sm_Pa);

                                if (item.sm_Pa == "0")
                                {
                                    if (isLeakage)
                                    {
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue(res);
                                        ws.GetRow(5).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "100")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue(res);
                                        ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue(res);
                                        ws.GetRow(6).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "150")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue(res);
                                        ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue(res);
                                        ws.GetRow(7).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "200")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue(res);
                                        ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue(res);
                                        ws.GetRow(8).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "250")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue(res);
                                        ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue(res);
                                        ws.GetRow(9).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "300")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue(res);
                                        ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue(res);
                                        ws.GetRow(10).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "350")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue(res);
                                        ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);

                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue(res);
                                        ws.GetRow(11).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "400")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue(res);
                                        ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue(res);
                                        ws.GetRow(12).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "500")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue(res);
                                        ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue(res);
                                        ws.GetRow(13).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "600")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue("否");
                                        ws.GetRow(14).GetCell(5).SetCellValue("-");
                                        ws.GetRow(15).GetCell(4).SetCellValue(res);
                                        ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue(res);
                                        ws.GetRow(14).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "700")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue("否");
                                        ws.GetRow(14).GetCell(5).SetCellValue("-");
                                        ws.GetRow(15).GetCell(4).SetCellValue("否");
                                        ws.GetRow(15).GetCell(5).SetCellValue("-");
                                        ws.GetRow(16).GetCell(4).SetCellValue(res);
                                        ws.GetRow(16).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(4).SetCellValue("否");
                                        ws.GetRow(5).GetCell(5).SetCellValue("-");
                                        ws.GetRow(6).GetCell(4).SetCellValue("否");
                                        ws.GetRow(6).GetCell(5).SetCellValue("-");
                                        ws.GetRow(7).GetCell(4).SetCellValue("否");
                                        ws.GetRow(7).GetCell(5).SetCellValue("-");
                                        ws.GetRow(8).GetCell(4).SetCellValue("否");
                                        ws.GetRow(8).GetCell(5).SetCellValue("-");
                                        ws.GetRow(9).GetCell(4).SetCellValue("否");
                                        ws.GetRow(9).GetCell(5).SetCellValue("-");
                                        ws.GetRow(10).GetCell(4).SetCellValue("否");
                                        ws.GetRow(10).GetCell(5).SetCellValue("-");
                                        ws.GetRow(11).GetCell(4).SetCellValue("否");
                                        ws.GetRow(11).GetCell(5).SetCellValue("-");
                                        ws.GetRow(12).GetCell(4).SetCellValue("否");
                                        ws.GetRow(12).GetCell(5).SetCellValue("-");
                                        ws.GetRow(13).GetCell(4).SetCellValue("否");
                                        ws.GetRow(13).GetCell(5).SetCellValue("-");
                                        ws.GetRow(14).GetCell(4).SetCellValue("否");
                                        ws.GetRow(14).GetCell(5).SetCellValue("-");
                                        ws.GetRow(15).GetCell(4).SetCellValue(res);
                                        ws.GetRow(15).GetCell(5).SetCellValue(item.sm_PaDesc);
                                    }
                                }

                                #endregion
                            }
                            else if (index == 2)
                            {
                                #region 赋值
                                if (item.gongchengjiance == "0") { }
                                else
                                {
                                    ws.GetRow(16).GetCell(7).SetCellValue(res);
                                    ws.GetRow(16).GetCell(8).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(17).GetCell(7).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    if (isLeakage)
                                    {
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue(res);
                                        ws.GetRow(5).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }

                                if (item.sm_Pa == "100")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue(res);
                                        ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue(res);
                                        ws.GetRow(6).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "150")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue(res);
                                        ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue(res);
                                        ws.GetRow(7).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "200")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue(res);
                                        ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue(res);
                                        ws.GetRow(8).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "250")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue(res);
                                        ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue(res);
                                        ws.GetRow(9).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "300")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue(res);
                                        ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue(res);
                                        ws.GetRow(10).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "350")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue(res);
                                        ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);

                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue(res);
                                        ws.GetRow(11).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "400")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue(res);
                                        ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue(res);
                                        ws.GetRow(12).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "500")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue(res);
                                        ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue(res);
                                        ws.GetRow(13).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "600")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue("否");
                                        ws.GetRow(14).GetCell(8).SetCellValue("-");
                                        ws.GetRow(15).GetCell(7).SetCellValue(res);
                                        ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue(res);
                                        ws.GetRow(14).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "700")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue("否");
                                        ws.GetRow(14).GetCell(8).SetCellValue("-");
                                        ws.GetRow(15).GetCell(7).SetCellValue("否");
                                        ws.GetRow(15).GetCell(8).SetCellValue("-");
                                        ws.GetRow(16).GetCell(7).SetCellValue(res);
                                        ws.GetRow(16).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(7).SetCellValue("否");
                                        ws.GetRow(5).GetCell(8).SetCellValue("-");
                                        ws.GetRow(6).GetCell(7).SetCellValue("否");
                                        ws.GetRow(6).GetCell(8).SetCellValue("-");
                                        ws.GetRow(7).GetCell(7).SetCellValue("否");
                                        ws.GetRow(7).GetCell(8).SetCellValue("-");
                                        ws.GetRow(8).GetCell(7).SetCellValue("否");
                                        ws.GetRow(8).GetCell(8).SetCellValue("-");
                                        ws.GetRow(9).GetCell(7).SetCellValue("否");
                                        ws.GetRow(9).GetCell(8).SetCellValue("-");
                                        ws.GetRow(10).GetCell(7).SetCellValue("否");
                                        ws.GetRow(10).GetCell(8).SetCellValue("-");
                                        ws.GetRow(11).GetCell(7).SetCellValue("否");
                                        ws.GetRow(11).GetCell(8).SetCellValue("-");
                                        ws.GetRow(12).GetCell(7).SetCellValue("否");
                                        ws.GetRow(12).GetCell(8).SetCellValue("-");
                                        ws.GetRow(13).GetCell(7).SetCellValue("否");
                                        ws.GetRow(13).GetCell(8).SetCellValue("-");
                                        ws.GetRow(14).GetCell(7).SetCellValue("否");
                                        ws.GetRow(14).GetCell(8).SetCellValue("-");
                                        ws.GetRow(15).GetCell(7).SetCellValue(res);
                                        ws.GetRow(15).GetCell(8).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                #endregion
                            }
                            else if (index == 3)
                            {
                                #region 赋值
                                if (item.gongchengjiance == "0") { }
                                else
                                {
                                    ws.GetRow(16).GetCell(10).SetCellValue(res);
                                    ws.GetRow(16).GetCell(11).SetCellValue(item.sm_PaDesc);
                                }
                                ws.GetRow(17).GetCell(10).SetCellValue(item.sm_Pa);
                                if (item.sm_Pa == "0")
                                {
                                    if (isLeakage)
                                    {
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue(res);
                                        ws.GetRow(5).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }

                                if (item.sm_Pa == "100")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue(res);
                                        ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue(res);
                                        ws.GetRow(6).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "150")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue(res);
                                        ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue(res);
                                        ws.GetRow(7).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "200")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue(res);
                                        ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue(res);
                                        ws.GetRow(8).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "250")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue(res);
                                        ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue(res);
                                        ws.GetRow(9).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "300")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue(res);
                                        ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue(res);
                                        ws.GetRow(10).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "350")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue(res);
                                        ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);

                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue(res);
                                        ws.GetRow(11).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "400")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue(res);
                                        ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue(res);
                                        ws.GetRow(12).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "500")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue(res);
                                        ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue(res);
                                        ws.GetRow(13).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "600")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue("否");
                                        ws.GetRow(14).GetCell(11).SetCellValue("-");
                                        ws.GetRow(15).GetCell(10).SetCellValue(res);
                                        ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue(res);
                                        ws.GetRow(14).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                if (item.sm_Pa == "700")
                                {
                                    if (isLeakage)
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue("否");
                                        ws.GetRow(14).GetCell(11).SetCellValue("-");
                                        ws.GetRow(15).GetCell(10).SetCellValue("否");
                                        ws.GetRow(15).GetCell(11).SetCellValue("-");
                                        ws.GetRow(16).GetCell(10).SetCellValue(res);
                                        ws.GetRow(16).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                    else
                                    {
                                        ws.GetRow(5).GetCell(10).SetCellValue("否");
                                        ws.GetRow(5).GetCell(11).SetCellValue("-");
                                        ws.GetRow(6).GetCell(10).SetCellValue("否");
                                        ws.GetRow(6).GetCell(11).SetCellValue("-");
                                        ws.GetRow(7).GetCell(10).SetCellValue("否");
                                        ws.GetRow(7).GetCell(11).SetCellValue("-");
                                        ws.GetRow(8).GetCell(10).SetCellValue("否");
                                        ws.GetRow(8).GetCell(11).SetCellValue("-");
                                        ws.GetRow(9).GetCell(10).SetCellValue("否");
                                        ws.GetRow(9).GetCell(11).SetCellValue("-");
                                        ws.GetRow(10).GetCell(10).SetCellValue("否");
                                        ws.GetRow(10).GetCell(11).SetCellValue("-");
                                        ws.GetRow(11).GetCell(10).SetCellValue("否");
                                        ws.GetRow(11).GetCell(11).SetCellValue("-");
                                        ws.GetRow(12).GetCell(10).SetCellValue("否");
                                        ws.GetRow(12).GetCell(11).SetCellValue("-");
                                        ws.GetRow(13).GetCell(10).SetCellValue("否");
                                        ws.GetRow(13).GetCell(11).SetCellValue("-");
                                        ws.GetRow(14).GetCell(10).SetCellValue("否");
                                        ws.GetRow(14).GetCell(11).SetCellValue("-");
                                        ws.GetRow(15).GetCell(10).SetCellValue(res);
                                        ws.GetRow(15).GetCell(11).SetCellValue(item.sm_PaDesc);
                                    }
                                }
                                #endregion
                            }

                        }
                        #endregion
                    }

                }
            }
            #endregion

            #region 抗风压
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("抗风压");

            ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());


            ws.GetRow(2).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengyashejizhi"].ToString());
            ws.GetRow(3).GetCell(1).SetCellValue(dt_Settings.Rows[0]["ganjianchangdu"].ToString());

            ws.GetRow(3).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengp3shejizhi"].ToString());


            ws.GetRow(28).GetCell(1).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(29).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());

            ws.GetRow(29).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengyashejizhi"].ToString());
            ws.GetRow(30).GetCell(1).SetCellValue(dt_Settings.Rows[0]["ganjianchangdu"].ToString());

            ws.GetRow(30).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengp3shejizhi"].ToString());


            ws.GetRow(55).GetCell(1).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(56).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());

            ws.GetRow(56).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengyashejizhi"].ToString());
            ws.GetRow(57).GetCell(1).SetCellValue(dt_Settings.Rows[0]["ganjianchangdu"].ToString());

            ws.GetRow(57).GetCell(11).SetCellValue(dt_Settings.Rows[0]["kangfengyazhengp3shejizhi"].ToString());

            var databaseDefPa = 250;

            DataTable kfy_Info1 = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, "第1樘");

            if (kfy_Info1 != null && kfy_Info1.Rows.Count > 0)
            {
                ws.ForceFormulaRecalculation = true;
                var dr1 = kfy_Info1.Rows[0];
                var ganjianchangdu = double.Parse(dt_Settings.Rows[0]["ganjianchangdu"].ToString());
                var lx = double.Parse(dr1["lx"].ToString());

                //最大面法线挠度;
                var zdmfxnd = Math.Round((ganjianchangdu / lx), 2);

                ws.GetRow(3).GetCell(6).SetCellValue(zdmfxnd);
                ws.GetRow(30).GetCell(6).SetCellValue(zdmfxnd);
                ws.GetRow(57).GetCell(6).SetCellValue(zdmfxnd);

                ws.GetRow(2).GetCell(6).SetCellValue(Math.Round(zdmfxnd * 2.5, 2));
                ws.GetRow(29).GetCell(6).SetCellValue(Math.Round(zdmfxnd * 2.5, 2));
                ws.GetRow(56).GetCell(6).SetCellValue(Math.Round(zdmfxnd * 2.5, 2));

                var jc = int.Parse(dr1["defJC"].ToString());

                for (int i = 1; i < 12; i++)
                {
                    if (i == 9)
                    {
                        ws.GetRow(15).GetCell(0).SetCellValue("P3阶段");
                        ws.GetRow(15).GetCell(1).SetCellValue(dr1["z_one_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(2).SetCellValue(dr1["z_two_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(3).SetCellValue(dr1["z_three_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(4).SetCellValue(dr1["z_nd_p3jieduan"].ToString());

                        ws.GetRow(42).GetCell(0).SetCellValue("-P3阶段");
                        ws.GetRow(42).GetCell(1).SetCellValue("-" + dr1["f_one_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(2).SetCellValue("-" + dr1["f_two_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(3).SetCellValue("-" + dr1["f_three_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(4).SetCellValue("-" + dr1["f_nd_p3jieduan"].ToString());
                    }
                    else if (i == 10)
                    {
                        ws.GetRow(16).GetCell(0).SetCellValue("P3残余变形");
                        ws.GetRow(16).GetCell(1).SetCellValue(dr1["z_one_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(2).SetCellValue(dr1["z_two_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(3).SetCellValue(dr1["z_three_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(4).SetCellValue(dr1["z_nd_p3canyubianxing"].ToString());

                        ws.GetRow(43).GetCell(0).SetCellValue("-P3残余变形");
                        ws.GetRow(43).GetCell(1).SetCellValue("-" + dr1["f_one_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(2).SetCellValue("-" + dr1["f_two_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(3).SetCellValue("-" + dr1["f_three_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(4).SetCellValue("-" + dr1["f_nd_p3canyubianxing"].ToString());
                    }
                    else if (i == 11)
                    {
                        ws.GetRow(17).GetCell(0).SetCellValue("PMax/残余变形");
                        ws.GetRow(17).GetCell(1).SetCellValue(dr1["z_one_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(2).SetCellValue(dr1["z_two_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(3).SetCellValue(dr1["z_three_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(4).SetCellValue(dr1["z_nd_pMaxcanyubianxing"].ToString());

                        ws.GetRow(44).GetCell(0).SetCellValue("-PMax/残余变形");
                        ws.GetRow(44).GetCell(1).SetCellValue("-" + dr1["f_one_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(2).SetCellValue("-" + dr1["f_two_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(3).SetCellValue("-" + dr1["f_three_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(4).SetCellValue("-" + dr1["f_nd_pMaxcanyubianxing"].ToString());
                    }
                    else
                    {
                        if (dr1["z_nd_" + (databaseDefPa * i)].ToString() != "0.00")
                        {
                            ws.GetRow(i + 6).GetCell(0).SetCellValue(jc * i);
                            ws.GetRow(i + 6).GetCell(1).SetCellValue(dr1["z_one_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(2).SetCellValue(dr1["z_two_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(3).SetCellValue(dr1["z_three_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(4).SetCellValue(double.Parse(dr1["z_nd_" + (databaseDefPa * i)].ToString()));

                            //  ws.GetRow(12 + i).GetCell(18).SetCellValue(dr1["z_nd_" + (databaseDefPa * i)].ToString());

                        }

                        if (dr1["f_nd_" + (databaseDefPa * i)].ToString() != "0.00")
                        {
                            ws.GetRow(i + 33).GetCell(0).SetCellValue(-(jc * i));
                            ws.GetRow(i + 33).GetCell(1).SetCellValue("-" + dr1["f_one_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(2).SetCellValue("-" + dr1["f_two_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(3).SetCellValue("-" + dr1["f_three_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(4).SetCellValue(double.Parse("-" + dr1["f_nd_" + (databaseDefPa * i)].ToString()));

                            //  ws.GetRow(12 - i).GetCell(18).SetCellValue("-" + dr1["f_nd_" + (databaseDefPa * i)].ToString());
                        }

                    }
                }

                var testytpe = int.Parse(dr1["testtype"].ToString());
                if (testytpe == 2)
                {
                    ws.GetRow(18).GetCell(4).SetCellValue(dr1["p1"].ToString());
                    ws.GetRow(19).GetCell(4).SetCellValue(dr1["p2"].ToString());
                    ws.GetRow(20).GetCell(4).SetCellValue(dr1["p3"].ToString());
                    ws.GetRow(21).GetCell(4).SetCellValue(dr1["z_pmax"].ToString());

                    ws.GetRow(22).GetCell(4).SetCellValue(dr1["desc"].ToString());

                    ws.GetRow(45).GetCell(4).SetCellValue("-" + dr1["_p1"].ToString());
                    ws.GetRow(46).GetCell(4).SetCellValue("-" + dr1["_p2"].ToString());
                    ws.GetRow(47).GetCell(4).SetCellValue("-" + dr1["_p3"].ToString());
                    ws.GetRow(48).GetCell(4).SetCellValue("-" + dr1["f_pmax"].ToString());

                    ws.GetRow(49).GetCell(4).SetCellValue(dr1["desc"].ToString());
                }
                else
                {
                    ws.GetRow(18).GetCell(2).SetCellValue(dr1["p1"].ToString());
                    ws.GetRow(19).GetCell(2).SetCellValue(dr1["p2"].ToString());
                    ws.GetRow(20).GetCell(2).SetCellValue(dr1["p3"].ToString());
                    ws.GetRow(21).GetCell(2).SetCellValue(dr1["z_pmax"].ToString());
                    ws.GetRow(22).GetCell(2).SetCellValue(dr1["desc"].ToString());


                    ws.GetRow(45).GetCell(2).SetCellValue("-" + dr1["_p1"].ToString());
                    ws.GetRow(46).GetCell(2).SetCellValue("-" + dr1["_p2"].ToString());
                    ws.GetRow(47).GetCell(2).SetCellValue("-" + dr1["_p3"].ToString());
                    ws.GetRow(48).GetCell(2).SetCellValue("-" + dr1["f_pmax"].ToString());


                    ws.GetRow(49).GetCell(2).SetCellValue(dr1["desc"].ToString());
                }

            }

            DataTable kfy_Info2 = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, "第2樘");
            if (kfy_Info2 != null && kfy_Info2.Rows.Count > 0)
            {
                var dr2 = kfy_Info2.Rows[0];
                for (int i = 1; i < 12; i++)
                {
                    if (i == 9)
                    {
                        ws.GetRow(15).GetCell(0).SetCellValue("P3阶段");
                        ws.GetRow(15).GetCell(5).SetCellValue(dr2["z_one_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(6).SetCellValue(dr2["z_two_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(7).SetCellValue(dr2["z_three_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(8).SetCellValue(dr2["z_nd_p3jieduan"].ToString());

                        ws.GetRow(42).GetCell(0).SetCellValue("-P3阶段");
                        ws.GetRow(42).GetCell(5).SetCellValue("-" + dr2["f_one_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(6).SetCellValue("-" + dr2["f_two_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(7).SetCellValue("-" + dr2["f_three_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(8).SetCellValue("-" + dr2["f_nd_p3jieduan"].ToString());
                    }
                    else if (i == 10)
                    {

                        ws.GetRow(16).GetCell(5).SetCellValue(dr2["z_one_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(6).SetCellValue(dr2["z_two_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(7).SetCellValue(dr2["z_three_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(8).SetCellValue(dr2["z_nd_p3canyubianxing"].ToString());

                        ws.GetRow(43).GetCell(5).SetCellValue("-" + dr2["f_one_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(6).SetCellValue("-" + dr2["f_two_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(7).SetCellValue("-" + dr2["f_three_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(8).SetCellValue("-" + dr2["f_nd_p3canyubianxing"].ToString());
                    }
                    else if (i == 11)
                    {
                        ws.GetRow(17).GetCell(5).SetCellValue(dr2["z_one_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(6).SetCellValue(dr2["z_two_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(7).SetCellValue(dr2["z_three_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(8).SetCellValue(dr2["z_nd_pMaxcanyubianxing"].ToString());

                        ws.GetRow(44).GetCell(5).SetCellValue("-" + dr2["f_one_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(6).SetCellValue("-" + dr2["f_two_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(7).SetCellValue("-" + dr2["f_three_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(8).SetCellValue("-" + dr2["f_nd_pMaxcanyubianxing"].ToString());
                    }
                    else
                    {
                        if (dr2["z_nd_" + (databaseDefPa * i)].ToString() != "0.00")
                        {
                            ws.GetRow(i + 6).GetCell(5).SetCellValue(dr2["z_one_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(6).SetCellValue(dr2["z_two_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(7).SetCellValue(dr2["z_three_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(8).SetCellValue(double.Parse(dr2["z_nd_" + (databaseDefPa * i)].ToString()));

                            // ws.GetRow(12 + i).GetCell(19).SetCellValue(dr2["z_nd_" + (databaseDefPa * i)].ToString());

                        }
                        if (dr2["f_nd_" + (databaseDefPa * i)].ToString() != "0.00")
                        {
                            ws.GetRow(i + 33).GetCell(5).SetCellValue("-" + dr2["f_one_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(6).SetCellValue("-" + dr2["f_two_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(7).SetCellValue("-" + dr2["f_three_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(8).SetCellValue(double.Parse("-" + dr2["f_nd_" + (databaseDefPa * i)].ToString()));

                            //ws.GetRow(12 + i).GetCell(19).SetCellValue("-" + dr2["f_nd_" + (databaseDefPa * i)].ToString());

                        }

                    }
                }

                var testytpe = int.Parse(dr2["testtype"].ToString());
                if (testytpe == 2)
                {
                    ws.GetRow(18).GetCell(8).SetCellValue(dr2["p1"].ToString());
                    ws.GetRow(19).GetCell(8).SetCellValue(dr2["p2"].ToString());
                    ws.GetRow(20).GetCell(8).SetCellValue(dr2["p3"].ToString());
                    ws.GetRow(21).GetCell(8).SetCellValue(dr2["z_pmax"].ToString());
                    ws.GetRow(22).GetCell(8).SetCellValue(dr2["desc"].ToString());


                    ws.GetRow(45).GetCell(8).SetCellValue("-" + dr2["_p1"].ToString());
                    ws.GetRow(46).GetCell(8).SetCellValue("-" + dr2["_p2"].ToString());
                    ws.GetRow(47).GetCell(8).SetCellValue("-" + dr2["_p3"].ToString());
                    ws.GetRow(48).GetCell(8).SetCellValue("-" + dr2["f_pmax"].ToString());

                    ws.GetRow(49).GetCell(8).SetCellValue(dr2["desc"].ToString());
                }
                else
                {
                    ws.GetRow(18).GetCell(6).SetCellValue(dr2["p1"].ToString());
                    ws.GetRow(19).GetCell(6).SetCellValue(dr2["p2"].ToString());
                    ws.GetRow(20).GetCell(6).SetCellValue(dr2["p3"].ToString());
                    ws.GetRow(21).GetCell(6).SetCellValue(dr2["z_pmax"].ToString());
                    ws.GetRow(22).GetCell(6).SetCellValue(dr2["desc"].ToString());


                    ws.GetRow(45).GetCell(6).SetCellValue("-" + dr2["_p1"].ToString());
                    ws.GetRow(46).GetCell(6).SetCellValue("-" + dr2["_p2"].ToString());
                    ws.GetRow(47).GetCell(6).SetCellValue("-" + dr2["_p3"].ToString());
                    ws.GetRow(48).GetCell(6).SetCellValue("-" + dr2["f_pmax"].ToString());

                    ws.GetRow(49).GetCell(6).SetCellValue(dr2["desc"].ToString());
                }
            }

            DataTable kfy_Info3 = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, "第3樘");
            if (kfy_Info3 != null && kfy_Info3.Rows.Count > 0)
            {
                var dr3 = kfy_Info3.Rows[0];
                for (int i = 1; i < 12; i++)
                {
                    if (i == 9)
                    {
                        ws.GetRow(15).GetCell(9).SetCellValue(dr3["z_one_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(10).SetCellValue(dr3["z_two_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(11).SetCellValue(dr3["z_three_p3jieduan"].ToString());
                        ws.GetRow(15).GetCell(12).SetCellValue(dr3["z_nd_p3jieduan"].ToString());

                        ws.GetRow(42).GetCell(9).SetCellValue("-" + dr3["f_one_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(10).SetCellValue("-" + dr3["f_two_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(11).SetCellValue("-" + dr3["f_three_p3jieduan"].ToString());
                        ws.GetRow(42).GetCell(12).SetCellValue("-" + dr3["f_nd_p3jieduan"].ToString());
                    }
                    else if (i == 10)
                    {
                        ws.GetRow(16).GetCell(9).SetCellValue(dr3["z_one_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(10).SetCellValue(dr3["z_two_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(11).SetCellValue(dr3["z_three_p3canyubianxing"].ToString());
                        ws.GetRow(16).GetCell(12).SetCellValue(dr3["z_nd_p3canyubianxing"].ToString());

                        ws.GetRow(43).GetCell(9).SetCellValue("-" + dr3["f_one_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(10).SetCellValue("-" + dr3["f_two_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(11).SetCellValue("-" + dr3["f_three_p3canyubianxing"].ToString());
                        ws.GetRow(43).GetCell(12).SetCellValue("-" + dr3["f_nd_p3canyubianxing"].ToString());
                    }
                    else if (i == 11)
                    {
                        ws.GetRow(17).GetCell(9).SetCellValue(dr3["z_one_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(10).SetCellValue(dr3["z_two_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(11).SetCellValue(dr3["z_three_pMaxcanyubianxing"].ToString());
                        ws.GetRow(17).GetCell(12).SetCellValue(dr3["z_nd_pMaxcanyubianxing"].ToString());

                        ws.GetRow(44).GetCell(9).SetCellValue("-" + dr3["f_one_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(10).SetCellValue("-" + dr3["f_two_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(11).SetCellValue("-" + dr3["f_three_pMaxcanyubianxing"].ToString());
                        ws.GetRow(44).GetCell(12).SetCellValue("-" + dr3["f_nd_pMaxcanyubianxing"].ToString());
                    }
                    else
                    {

                        if (dr3["z_nd_" + (databaseDefPa * i)].ToString() != "0.00")
                        {
                            ws.GetRow(i + 6).GetCell(9).SetCellValue(dr3["z_one_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(10).SetCellValue(dr3["z_two_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(11).SetCellValue(dr3["z_three_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 6).GetCell(12).SetCellValue(double.Parse(dr3["z_nd_" + (databaseDefPa * i)].ToString()));

                            //ws.GetRow(12 + i).GetCell(20).SetCellValue(dr3["z_nd_" + (databaseDefPa * i)].ToString());
                        }

                        if (dr3["f_nd_" + (databaseDefPa * i)].ToString() != "0.00")
                        {
                            ws.GetRow(i + 33).GetCell(9).SetCellValue("-" + dr3["f_one_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(10).SetCellValue("-" + dr3["f_two_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(11).SetCellValue("-" + dr3["f_three_" + (databaseDefPa * i)].ToString());
                            ws.GetRow(i + 33).GetCell(12).SetCellValue(double.Parse("-" + dr3["f_nd_" + (databaseDefPa * i)].ToString()));

                            //  ws.GetRow(12 + i).GetCell(20).SetCellValue("-" + dr3["f_nd_" + (databaseDefPa * i)].ToString());

                        }

                    }
                }

                var testytpe = int.Parse(dr3["testtype"].ToString());
                if (testytpe == 2)
                {
                    ws.GetRow(18).GetCell(12).SetCellValue(dr3["p1"].ToString());
                    ws.GetRow(19).GetCell(12).SetCellValue(dr3["p2"].ToString());
                    ws.GetRow(20).GetCell(12).SetCellValue(dr3["p3"].ToString());
                    ws.GetRow(21).GetCell(12).SetCellValue(dr3["z_pmax"].ToString());
                    ws.GetRow(22).GetCell(12).SetCellValue(dr3["desc"].ToString());

                    ws.GetRow(45).GetCell(12).SetCellValue("-" + dr3["_p1"].ToString());
                    ws.GetRow(46).GetCell(12).SetCellValue("-" + dr3["_p2"].ToString());
                    ws.GetRow(47).GetCell(12).SetCellValue("-" + dr3["_p3"].ToString());
                    ws.GetRow(48).GetCell(12).SetCellValue("-" + dr3["f_pmax"].ToString());

                    ws.GetRow(49).GetCell(12).SetCellValue(dr3["desc"].ToString());
                }
                else
                {
                    ws.GetRow(18).GetCell(10).SetCellValue(dr3["p1"].ToString());
                    ws.GetRow(19).GetCell(10).SetCellValue(dr3["p2"].ToString());
                    ws.GetRow(20).GetCell(10).SetCellValue(dr3["p3"].ToString());
                    ws.GetRow(21).GetCell(10).SetCellValue(dr3["z_pmax"].ToString());
                    ws.GetRow(21).GetCell(10).SetCellValue(dr3["z_pmax"].ToString());

                    ws.GetRow(45).GetCell(10).SetCellValue("-" + dr3["_p1"].ToString());
                    ws.GetRow(46).GetCell(10).SetCellValue("-" + dr3["_p2"].ToString());
                    ws.GetRow(47).GetCell(10).SetCellValue("-" + dr3["_p3"].ToString());
                    ws.GetRow(48).GetCell(10).SetCellValue("-" + dr3["f_pmax"].ToString());


                    ws.GetRow(49).GetCell(10).SetCellValue(dr3["desc"].ToString());
                }
            }

            #endregion


            using (FileStream filess = File.OpenWrite(outFilePath))
            {
                hssfworkbook.Write(filess);

            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("导出异常", "导出", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            return true;
        }
    }

}