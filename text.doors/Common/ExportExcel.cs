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

            ws.GetRow(2).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(3).GetCell(3).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(4).GetCell(3).SetCellValue(dt_Settings.Rows[0]["weituodanwei"].ToString());
            ws.GetRow(5).GetCell(3).SetCellValue(dt_Settings.Rows[0]["dizhi"].ToString());
            ws.GetRow(5).GetCell(8).SetCellValue(dt_Settings.Rows[0]["dianhua"].ToString());
            ws.GetRow(6).GetCell(3).SetCellValue(dt_Settings.Rows[0]["chouyangriqi"].ToString());
            ws.GetRow(7).GetCell(3).SetCellValue(dt_Settings.Rows[0]["chouyangdidian"].ToString());
            ws.GetRow(8).GetCell(3).SetCellValue(dt_Settings.Rows[0]["gongchengmingcheng"].ToString());
            ws.GetRow(9).GetCell(3).SetCellValue(dt_Settings.Rows[0]["shengchandanwei"].ToString());
            //样品
            ws.GetRow(10).GetCell(3).SetCellValue(dt_Settings.Rows[0]["yangpinmingcheng"].ToString());
            ws.GetRow(10).GetCell(8).SetCellValue(dt_Settings.Rows[0]["yangpinzhuangtai"].ToString());
            //ws.GetRow(11).GetCell(3).SetCellValue(dt_Settings.Rows[0]["shengchandanwei"].ToString());
            ws.GetRow(11).GetCell(8).SetCellValue(dt_Settings.Rows[0]["guigexinghao"].ToString());
            //检测
            ws.GetRow(12).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jiancexiangmu"].ToString());
            ws.GetRow(12).GetCell(8).SetCellValue(dt_Settings.Rows[0]["jianceshuliang"].ToString());
            ws.GetRow(13).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jiancedidian"].ToString());
            ws.GetRow(13).GetCell(8).SetCellValue(dt_Settings.Rows[0]["jianceriqi"].ToString());
            ws.GetRow(14).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceyiju"].ToString());
            ws.GetRow(15).GetCell(3).SetCellValue(dt_Settings.Rows[0]["jianceshebei"].ToString());
            //结论
            ws.GetRow(18).GetCell(8).SetCellValue(0);
            ws.GetRow(19).GetCell(8).SetCellValue(0);
            ws.GetRow(20).GetCell(8).SetCellValue(0);
            ws.GetRow(21).GetCell(4).SetCellValue("(采用稳定加压方法检测)");
            ws.GetRow(22).GetCell(8).SetCellValue(0);
            #endregion

            #region 质量检测报告
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("质量检测报告");
            ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(2).GetCell(9).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(3).GetCell(2).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());
            ws.GetRow(3).GetCell(9).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
            ws.GetRow(4).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanpinzhong"].ToString());
            ws.GetRow(4).GetCell(9).SetCellValue(dt_Settings.Rows[0]["kaiqifangshi"].ToString());
            ws.GetRow(5).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanxiangqian"].ToString());
            ws.GetRow(5).GetCell(9).SetCellValue(dt_Settings.Rows[0]["kuangshanmifeng"].ToString());
            ws.GetRow(6).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
            ws.GetRow(6).GetCell(9).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
            #endregion

            #region 质量检测报告
            ws = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.GetSheet("质量检测报告");
            ws.GetRow(2).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dt_Code"].ToString());
            ws.GetRow(2).GetCell(9).SetCellValue(dt_Settings.Rows[0]["weituobianhao"].ToString());
            ws.GetRow(3).GetCell(2).SetCellValue(dt_Settings.Rows[0]["kaiqifengchang"].ToString());
            ws.GetRow(3).GetCell(9).SetCellValue(dt_Settings.Rows[0]["shijianmianji"].ToString());
            ws.GetRow(4).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanpinzhong"].ToString());
            ws.GetRow(4).GetCell(9).SetCellValue(dt_Settings.Rows[0]["kaiqifangshi"].ToString());
            ws.GetRow(5).GetCell(2).SetCellValue(dt_Settings.Rows[0]["mianbanxiangqian"].ToString());
            ws.GetRow(5).GetCell(9).SetCellValue(dt_Settings.Rows[0]["kuangshanmifeng"].ToString());
            ws.GetRow(6).GetCell(2).SetCellValue(dt_Settings.Rows[0]["dangqianwendu"].ToString());
            ws.GetRow(6).GetCell(9).SetCellValue(dt_Settings.Rows[0]["daqiyali"].ToString());
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