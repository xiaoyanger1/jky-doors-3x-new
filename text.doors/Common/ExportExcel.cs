using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.dal;

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
            //ws.GetRow(11).GetCell(3).SetCellValue(dt_Settings.Rows[0]["shengchandanwei"].ToString());
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
            ws.GetRow(7).GetCell(3).SetCellValue("");
            ws.GetRow(7).GetCell(7).SetCellValue("");
            ws.GetRow(7).GetCell(11).SetCellValue("");

            ws.GetRow(8).GetCell(3).SetCellValue("");
            ws.GetRow(8).GetCell(8).SetCellValue("");
            ws.GetRow(8).GetCell(12).SetCellValue("");
            //气密性能
            ws.GetRow(11).GetCell(6).SetCellValue("");
            ws.GetRow(11).GetCell(9).SetCellValue("");
            ws.GetRow(12).GetCell(6).SetCellValue("");
            ws.GetRow(12).GetCell(9).SetCellValue("");
            //水密性能
            ws.GetRow(13).GetCell(3).SetCellValue("");
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


            ws.GetRow(7).GetCell(3).SetCellValue("");
            ws.GetRow(7).GetCell(4).SetCellValue("");
            ws.GetRow(7).GetCell(5).SetCellValue("");
            ws.GetRow(7).GetCell(6).SetCellValue("");
            ws.GetRow(7).GetCell(7).SetCellValue("");
            ws.GetRow(7).GetCell(8).SetCellValue("");

            ws.GetRow(8).GetCell(3).SetCellValue("");
            ws.GetRow(8).GetCell(4).SetCellValue("");
            ws.GetRow(8).GetCell(5).SetCellValue("");
            ws.GetRow(8).GetCell(6).SetCellValue("");
            ws.GetRow(8).GetCell(7).SetCellValue("");
            ws.GetRow(8).GetCell(8).SetCellValue("");

            ws.GetRow(9).GetCell(3).SetCellValue("");
            ws.GetRow(9).GetCell(4).SetCellValue("");
            ws.GetRow(9).GetCell(5).SetCellValue("");
            ws.GetRow(9).GetCell(6).SetCellValue("");
            ws.GetRow(9).GetCell(7).SetCellValue("");
            ws.GetRow(9).GetCell(8).SetCellValue("");

            ws.GetRow(10).GetCell(3).SetCellValue("");
            ws.GetRow(10).GetCell(4).SetCellValue("");
            ws.GetRow(10).GetCell(5).SetCellValue("");
            ws.GetRow(10).GetCell(6).SetCellValue("");
            ws.GetRow(10).GetCell(7).SetCellValue("");
            ws.GetRow(10).GetCell(8).SetCellValue("");

            ws.GetRow(11).GetCell(3).SetCellValue("");
            ws.GetRow(11).GetCell(4).SetCellValue("");
            ws.GetRow(11).GetCell(5).SetCellValue("");
            ws.GetRow(11).GetCell(6).SetCellValue("");
            ws.GetRow(11).GetCell(7).SetCellValue("");
            ws.GetRow(11).GetCell(8).SetCellValue("");

            ws.GetRow(12).GetCell(3).SetCellValue("");
            ws.GetRow(12).GetCell(4).SetCellValue("");
            ws.GetRow(12).GetCell(5).SetCellValue("");
            ws.GetRow(12).GetCell(6).SetCellValue("");
            ws.GetRow(12).GetCell(7).SetCellValue("");
            ws.GetRow(12).GetCell(8).SetCellValue("");

            ws.GetRow(13).GetCell(3).SetCellValue("");
            ws.GetRow(13).GetCell(4).SetCellValue("");
            ws.GetRow(13).GetCell(5).SetCellValue("");
            ws.GetRow(13).GetCell(6).SetCellValue("");
            ws.GetRow(13).GetCell(7).SetCellValue("");
            ws.GetRow(13).GetCell(8).SetCellValue("");

            ws.GetRow(14).GetCell(3).SetCellValue("");
            ws.GetRow(14).GetCell(4).SetCellValue("");
            ws.GetRow(14).GetCell(5).SetCellValue("");
            ws.GetRow(14).GetCell(6).SetCellValue("");
            ws.GetRow(14).GetCell(7).SetCellValue("");
            ws.GetRow(14).GetCell(8).SetCellValue("");

            ws.GetRow(15).GetCell(3).SetCellValue("");
            ws.GetRow(15).GetCell(4).SetCellValue("");
            ws.GetRow(15).GetCell(5).SetCellValue("");
            ws.GetRow(15).GetCell(6).SetCellValue("");
            ws.GetRow(15).GetCell(7).SetCellValue("");
            ws.GetRow(15).GetCell(8).SetCellValue("");

            ws.GetRow(16).GetCell(3).SetCellValue("");
            ws.GetRow(16).GetCell(4).SetCellValue("");
            ws.GetRow(16).GetCell(5).SetCellValue("");
            ws.GetRow(16).GetCell(6).SetCellValue("");
            ws.GetRow(16).GetCell(7).SetCellValue("");
            ws.GetRow(16).GetCell(8).SetCellValue("");

            ws.GetRow(17).GetCell(3).SetCellValue("");
            ws.GetRow(17).GetCell(4).SetCellValue("");
            ws.GetRow(17).GetCell(5).SetCellValue("");
            ws.GetRow(17).GetCell(6).SetCellValue("");
            ws.GetRow(17).GetCell(7).SetCellValue("");
            ws.GetRow(17).GetCell(8).SetCellValue("");

            ws.GetRow(18).GetCell(3).SetCellValue("");
            ws.GetRow(18).GetCell(4).SetCellValue("");
            ws.GetRow(18).GetCell(5).SetCellValue("");
            ws.GetRow(18).GetCell(6).SetCellValue("");
            ws.GetRow(18).GetCell(7).SetCellValue("");
            ws.GetRow(18).GetCell(8).SetCellValue("");

            ws.GetRow(19).GetCell(3).SetCellValue("");
            ws.GetRow(19).GetCell(4).SetCellValue("");
            ws.GetRow(19).GetCell(5).SetCellValue("");
            ws.GetRow(19).GetCell(6).SetCellValue("");
            ws.GetRow(19).GetCell(7).SetCellValue("");
            ws.GetRow(19).GetCell(8).SetCellValue("");

            ws.GetRow(20).GetCell(3).SetCellValue("");
            ws.GetRow(20).GetCell(4).SetCellValue("");
            ws.GetRow(20).GetCell(5).SetCellValue("");
            ws.GetRow(20).GetCell(6).SetCellValue("");
            ws.GetRow(20).GetCell(7).SetCellValue("");
            ws.GetRow(20).GetCell(8).SetCellValue("");


            //负压
            ws.GetRow(23).GetCell(3).SetCellValue("");
            ws.GetRow(23).GetCell(4).SetCellValue("");
            ws.GetRow(23).GetCell(5).SetCellValue("");
            ws.GetRow(23).GetCell(6).SetCellValue("");
            ws.GetRow(23).GetCell(7).SetCellValue("");
            ws.GetRow(23).GetCell(8).SetCellValue("");

            ws.GetRow(24).GetCell(3).SetCellValue("");
            ws.GetRow(24).GetCell(4).SetCellValue("");
            ws.GetRow(24).GetCell(5).SetCellValue("");
            ws.GetRow(24).GetCell(6).SetCellValue("");
            ws.GetRow(24).GetCell(7).SetCellValue("");
            ws.GetRow(24).GetCell(8).SetCellValue("");

            ws.GetRow(25).GetCell(3).SetCellValue("");
            ws.GetRow(25).GetCell(4).SetCellValue("");
            ws.GetRow(25).GetCell(5).SetCellValue("");
            ws.GetRow(25).GetCell(6).SetCellValue("");
            ws.GetRow(25).GetCell(7).SetCellValue("");
            ws.GetRow(25).GetCell(8).SetCellValue("");

            ws.GetRow(26).GetCell(3).SetCellValue("");
            ws.GetRow(26).GetCell(4).SetCellValue("");
            ws.GetRow(26).GetCell(5).SetCellValue("");
            ws.GetRow(26).GetCell(6).SetCellValue("");
            ws.GetRow(26).GetCell(7).SetCellValue("");
            ws.GetRow(26).GetCell(8).SetCellValue("");

            ws.GetRow(27).GetCell(3).SetCellValue("");
            ws.GetRow(27).GetCell(4).SetCellValue("");
            ws.GetRow(27).GetCell(5).SetCellValue("");
            ws.GetRow(27).GetCell(6).SetCellValue("");
            ws.GetRow(27).GetCell(7).SetCellValue("");
            ws.GetRow(27).GetCell(8).SetCellValue("");

            ws.GetRow(28).GetCell(3).SetCellValue("");
            ws.GetRow(28).GetCell(4).SetCellValue("");
            ws.GetRow(28).GetCell(5).SetCellValue("");
            ws.GetRow(28).GetCell(6).SetCellValue("");
            ws.GetRow(28).GetCell(7).SetCellValue("");
            ws.GetRow(28).GetCell(8).SetCellValue("");

            ws.GetRow(29).GetCell(3).SetCellValue("");
            ws.GetRow(29).GetCell(4).SetCellValue("");
            ws.GetRow(29).GetCell(5).SetCellValue("");
            ws.GetRow(29).GetCell(6).SetCellValue("");
            ws.GetRow(29).GetCell(7).SetCellValue("");
            ws.GetRow(29).GetCell(8).SetCellValue("");

            ws.GetRow(30).GetCell(3).SetCellValue("");
            ws.GetRow(30).GetCell(4).SetCellValue("");
            ws.GetRow(30).GetCell(5).SetCellValue("");
            ws.GetRow(30).GetCell(6).SetCellValue("");
            ws.GetRow(30).GetCell(7).SetCellValue("");
            ws.GetRow(30).GetCell(8).SetCellValue("");

            ws.GetRow(31).GetCell(3).SetCellValue("");
            ws.GetRow(31).GetCell(4).SetCellValue("");
            ws.GetRow(31).GetCell(5).SetCellValue("");
            ws.GetRow(31).GetCell(6).SetCellValue("");
            ws.GetRow(31).GetCell(7).SetCellValue("");
            ws.GetRow(31).GetCell(8).SetCellValue("");

            ws.GetRow(32).GetCell(3).SetCellValue("");
            ws.GetRow(32).GetCell(4).SetCellValue("");
            ws.GetRow(32).GetCell(5).SetCellValue("");
            ws.GetRow(32).GetCell(6).SetCellValue("");
            ws.GetRow(32).GetCell(7).SetCellValue("");
            ws.GetRow(32).GetCell(8).SetCellValue("");

            ws.GetRow(33).GetCell(3).SetCellValue("");
            ws.GetRow(33).GetCell(4).SetCellValue("");
            ws.GetRow(33).GetCell(5).SetCellValue("");
            ws.GetRow(33).GetCell(6).SetCellValue("");
            ws.GetRow(33).GetCell(7).SetCellValue("");
            ws.GetRow(33).GetCell(8).SetCellValue("");

            ws.GetRow(34).GetCell(3).SetCellValue("");
            ws.GetRow(34).GetCell(4).SetCellValue("");
            ws.GetRow(34).GetCell(5).SetCellValue("");
            ws.GetRow(34).GetCell(6).SetCellValue("");
            ws.GetRow(34).GetCell(7).SetCellValue("");
            ws.GetRow(34).GetCell(8).SetCellValue("");

            ws.GetRow(35).GetCell(3).SetCellValue("");
            ws.GetRow(35).GetCell(4).SetCellValue("");
            ws.GetRow(35).GetCell(5).SetCellValue("");
            ws.GetRow(35).GetCell(6).SetCellValue("");
            ws.GetRow(35).GetCell(7).SetCellValue("");
            ws.GetRow(35).GetCell(8).SetCellValue("");

            ws.GetRow(36).GetCell(3).SetCellValue("");
            ws.GetRow(36).GetCell(4).SetCellValue("");
            ws.GetRow(36).GetCell(5).SetCellValue("");
            ws.GetRow(36).GetCell(6).SetCellValue("");
            ws.GetRow(36).GetCell(7).SetCellValue("");
            ws.GetRow(36).GetCell(8).SetCellValue("");

            //画图赋值

            #endregion


            #region 水密性（稳定）

            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性（稳定）");
            ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
            ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());


            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            #endregion

            #region 水密性（波动）
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("水密性（波动）");
            ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
            ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
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
            #endregion

            #region 重复气密性
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复气密性");
            ws.GetRow(2).GetCell(3).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(2).GetCell(5).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
            ws.GetRow(2).GetCell(7).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());

            ws.GetRow(3).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(3).GetCell(5).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
            ws.GetRow(3).GetCell(7).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());


            ws.GetRow(7).GetCell(3).SetCellValue("");
            ws.GetRow(7).GetCell(4).SetCellValue("");
            ws.GetRow(7).GetCell(5).SetCellValue("");
            ws.GetRow(7).GetCell(6).SetCellValue("");
            ws.GetRow(7).GetCell(7).SetCellValue("");
            ws.GetRow(7).GetCell(8).SetCellValue("");

            ws.GetRow(8).GetCell(3).SetCellValue("");
            ws.GetRow(8).GetCell(4).SetCellValue("");
            ws.GetRow(8).GetCell(5).SetCellValue("");
            ws.GetRow(8).GetCell(6).SetCellValue("");
            ws.GetRow(8).GetCell(7).SetCellValue("");
            ws.GetRow(8).GetCell(8).SetCellValue("");

            ws.GetRow(9).GetCell(3).SetCellValue("");
            ws.GetRow(9).GetCell(4).SetCellValue("");
            ws.GetRow(9).GetCell(5).SetCellValue("");
            ws.GetRow(9).GetCell(6).SetCellValue("");
            ws.GetRow(9).GetCell(7).SetCellValue("");
            ws.GetRow(9).GetCell(8).SetCellValue("");

            ws.GetRow(10).GetCell(3).SetCellValue("");
            ws.GetRow(10).GetCell(4).SetCellValue("");
            ws.GetRow(10).GetCell(5).SetCellValue("");
            ws.GetRow(10).GetCell(6).SetCellValue("");
            ws.GetRow(10).GetCell(7).SetCellValue("");
            ws.GetRow(10).GetCell(8).SetCellValue("");

            ws.GetRow(11).GetCell(3).SetCellValue("");
            ws.GetRow(11).GetCell(4).SetCellValue("");
            ws.GetRow(11).GetCell(5).SetCellValue("");
            ws.GetRow(11).GetCell(6).SetCellValue("");
            ws.GetRow(11).GetCell(7).SetCellValue("");
            ws.GetRow(11).GetCell(8).SetCellValue("");

            ws.GetRow(12).GetCell(3).SetCellValue("");
            ws.GetRow(12).GetCell(4).SetCellValue("");
            ws.GetRow(12).GetCell(5).SetCellValue("");
            ws.GetRow(12).GetCell(6).SetCellValue("");
            ws.GetRow(12).GetCell(7).SetCellValue("");
            ws.GetRow(12).GetCell(8).SetCellValue("");

            ws.GetRow(13).GetCell(3).SetCellValue("");
            ws.GetRow(13).GetCell(4).SetCellValue("");
            ws.GetRow(13).GetCell(5).SetCellValue("");
            ws.GetRow(13).GetCell(6).SetCellValue("");
            ws.GetRow(13).GetCell(7).SetCellValue("");
            ws.GetRow(13).GetCell(8).SetCellValue("");

            ws.GetRow(14).GetCell(3).SetCellValue("");
            ws.GetRow(14).GetCell(4).SetCellValue("");
            ws.GetRow(14).GetCell(5).SetCellValue("");
            ws.GetRow(14).GetCell(6).SetCellValue("");
            ws.GetRow(14).GetCell(7).SetCellValue("");
            ws.GetRow(14).GetCell(8).SetCellValue("");

            ws.GetRow(15).GetCell(3).SetCellValue("");
            ws.GetRow(15).GetCell(4).SetCellValue("");
            ws.GetRow(15).GetCell(5).SetCellValue("");
            ws.GetRow(15).GetCell(6).SetCellValue("");
            ws.GetRow(15).GetCell(7).SetCellValue("");
            ws.GetRow(15).GetCell(8).SetCellValue("");

            ws.GetRow(16).GetCell(3).SetCellValue("");
            ws.GetRow(16).GetCell(4).SetCellValue("");
            ws.GetRow(16).GetCell(5).SetCellValue("");
            ws.GetRow(16).GetCell(6).SetCellValue("");
            ws.GetRow(16).GetCell(7).SetCellValue("");
            ws.GetRow(16).GetCell(8).SetCellValue("");

            ws.GetRow(17).GetCell(3).SetCellValue("");
            ws.GetRow(17).GetCell(4).SetCellValue("");
            ws.GetRow(17).GetCell(5).SetCellValue("");
            ws.GetRow(17).GetCell(6).SetCellValue("");
            ws.GetRow(17).GetCell(7).SetCellValue("");
            ws.GetRow(17).GetCell(8).SetCellValue("");

            ws.GetRow(18).GetCell(3).SetCellValue("");
            ws.GetRow(18).GetCell(4).SetCellValue("");
            ws.GetRow(18).GetCell(5).SetCellValue("");
            ws.GetRow(18).GetCell(6).SetCellValue("");
            ws.GetRow(18).GetCell(7).SetCellValue("");
            ws.GetRow(18).GetCell(8).SetCellValue("");

            ws.GetRow(19).GetCell(3).SetCellValue("");
            ws.GetRow(19).GetCell(4).SetCellValue("");
            ws.GetRow(19).GetCell(5).SetCellValue("");
            ws.GetRow(19).GetCell(6).SetCellValue("");
            ws.GetRow(19).GetCell(7).SetCellValue("");
            ws.GetRow(19).GetCell(8).SetCellValue("");

            ws.GetRow(20).GetCell(3).SetCellValue("");
            ws.GetRow(20).GetCell(4).SetCellValue("");
            ws.GetRow(20).GetCell(5).SetCellValue("");
            ws.GetRow(20).GetCell(6).SetCellValue("");
            ws.GetRow(20).GetCell(7).SetCellValue("");
            ws.GetRow(20).GetCell(8).SetCellValue("");


            //负压
            ws.GetRow(23).GetCell(3).SetCellValue("");
            ws.GetRow(23).GetCell(4).SetCellValue("");
            ws.GetRow(23).GetCell(5).SetCellValue("");
            ws.GetRow(23).GetCell(6).SetCellValue("");
            ws.GetRow(23).GetCell(7).SetCellValue("");
            ws.GetRow(23).GetCell(8).SetCellValue("");

            ws.GetRow(24).GetCell(3).SetCellValue("");
            ws.GetRow(24).GetCell(4).SetCellValue("");
            ws.GetRow(24).GetCell(5).SetCellValue("");
            ws.GetRow(24).GetCell(6).SetCellValue("");
            ws.GetRow(24).GetCell(7).SetCellValue("");
            ws.GetRow(24).GetCell(8).SetCellValue("");

            ws.GetRow(25).GetCell(3).SetCellValue("");
            ws.GetRow(25).GetCell(4).SetCellValue("");
            ws.GetRow(25).GetCell(5).SetCellValue("");
            ws.GetRow(25).GetCell(6).SetCellValue("");
            ws.GetRow(25).GetCell(7).SetCellValue("");
            ws.GetRow(25).GetCell(8).SetCellValue("");

            ws.GetRow(26).GetCell(3).SetCellValue("");
            ws.GetRow(26).GetCell(4).SetCellValue("");
            ws.GetRow(26).GetCell(5).SetCellValue("");
            ws.GetRow(26).GetCell(6).SetCellValue("");
            ws.GetRow(26).GetCell(7).SetCellValue("");
            ws.GetRow(26).GetCell(8).SetCellValue("");

            ws.GetRow(27).GetCell(3).SetCellValue("");
            ws.GetRow(27).GetCell(4).SetCellValue("");
            ws.GetRow(27).GetCell(5).SetCellValue("");
            ws.GetRow(27).GetCell(6).SetCellValue("");
            ws.GetRow(27).GetCell(7).SetCellValue("");
            ws.GetRow(27).GetCell(8).SetCellValue("");

            ws.GetRow(28).GetCell(3).SetCellValue("");
            ws.GetRow(28).GetCell(4).SetCellValue("");
            ws.GetRow(28).GetCell(5).SetCellValue("");
            ws.GetRow(28).GetCell(6).SetCellValue("");
            ws.GetRow(28).GetCell(7).SetCellValue("");
            ws.GetRow(28).GetCell(8).SetCellValue("");

            ws.GetRow(29).GetCell(3).SetCellValue("");
            ws.GetRow(29).GetCell(4).SetCellValue("");
            ws.GetRow(29).GetCell(5).SetCellValue("");
            ws.GetRow(29).GetCell(6).SetCellValue("");
            ws.GetRow(29).GetCell(7).SetCellValue("");
            ws.GetRow(29).GetCell(8).SetCellValue("");

            ws.GetRow(30).GetCell(3).SetCellValue("");
            ws.GetRow(30).GetCell(4).SetCellValue("");
            ws.GetRow(30).GetCell(5).SetCellValue("");
            ws.GetRow(30).GetCell(6).SetCellValue("");
            ws.GetRow(30).GetCell(7).SetCellValue("");
            ws.GetRow(30).GetCell(8).SetCellValue("");

            ws.GetRow(31).GetCell(3).SetCellValue("");
            ws.GetRow(31).GetCell(4).SetCellValue("");
            ws.GetRow(31).GetCell(5).SetCellValue("");
            ws.GetRow(31).GetCell(6).SetCellValue("");
            ws.GetRow(31).GetCell(7).SetCellValue("");
            ws.GetRow(31).GetCell(8).SetCellValue("");

            ws.GetRow(32).GetCell(3).SetCellValue("");
            ws.GetRow(32).GetCell(4).SetCellValue("");
            ws.GetRow(32).GetCell(5).SetCellValue("");
            ws.GetRow(32).GetCell(6).SetCellValue("");
            ws.GetRow(32).GetCell(7).SetCellValue("");
            ws.GetRow(32).GetCell(8).SetCellValue("");

            ws.GetRow(33).GetCell(3).SetCellValue("");
            ws.GetRow(33).GetCell(4).SetCellValue("");
            ws.GetRow(33).GetCell(5).SetCellValue("");
            ws.GetRow(33).GetCell(6).SetCellValue("");
            ws.GetRow(33).GetCell(7).SetCellValue("");
            ws.GetRow(33).GetCell(8).SetCellValue("");

            ws.GetRow(34).GetCell(3).SetCellValue("");
            ws.GetRow(34).GetCell(4).SetCellValue("");
            ws.GetRow(34).GetCell(5).SetCellValue("");
            ws.GetRow(34).GetCell(6).SetCellValue("");
            ws.GetRow(34).GetCell(7).SetCellValue("");
            ws.GetRow(34).GetCell(8).SetCellValue("");

            ws.GetRow(35).GetCell(3).SetCellValue("");
            ws.GetRow(35).GetCell(4).SetCellValue("");
            ws.GetRow(35).GetCell(5).SetCellValue("");
            ws.GetRow(35).GetCell(6).SetCellValue("");
            ws.GetRow(35).GetCell(7).SetCellValue("");
            ws.GetRow(35).GetCell(8).SetCellValue("");

            ws.GetRow(36).GetCell(3).SetCellValue("");
            ws.GetRow(36).GetCell(4).SetCellValue("");
            ws.GetRow(36).GetCell(5).SetCellValue("");
            ws.GetRow(36).GetCell(6).SetCellValue("");
            ws.GetRow(36).GetCell(7).SetCellValue("");
            ws.GetRow(36).GetCell(8).SetCellValue("");

            //画图赋值

            #endregion

            #region 重复水密性（稳定）

            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性（稳定）");
            ws.GetRow(1).GetCell(1).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
            ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());


            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            ws.GetRow(2).GetCell(10).SetCellValue("");
            ws.GetRow(2).GetCell(10).SetCellValue("");

            #endregion

            #region 重复水密性（波动）
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("重复水密性（波动）");
            ws.GetRow(1).GetCell(1).SetCellValue("");
            ws.GetRow(1).GetCell(10).SetCellValue(dt_Settings.Rows[0]["penlinshuiliang"].ToString());
            ws.GetRow(2).GetCell(1).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(2).GetCell(10).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
            #endregion

            ws.ForceFormulaRecalculation = true;

            using (FileStream filess = File.OpenWrite(outFilePath))
            {
                hssfworkbook.Write(filess);
            }

            return true;
        }
    }

}