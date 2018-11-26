using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using CalculationOilPrice.Library.Util;
using System.Globalization;

namespace CalculationOilPrice.Library.Storage
{
    static partial class StorageOperator
    {
        public static long CalPriceGetLatestID()
        {
            long iID = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("cal_CalPriceGetLatestID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    iID = Convert.ToInt64(cmd.ExecuteScalar());
                    conn.Close();
                }
            }

            return iID;
        }

        public static CalculationModel CalPriceLoadByID(long id)
        {
            CalculationModel model = null;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("cal_CalPriceGetByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PriceID", id));
                    conn.Open();

                    try
                    {
                        SqlDataReader drd = cmd.ExecuteReader();
                        dt.Load(drd);
                    }
                    catch { }

                    conn.Close();
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                string sEncodeJson = String.Concat(
                        dt.Rows[0]["JsonData1"],
                        dt.Rows[0]["JsonData2"],
                        dt.Rows[0]["JsonData3"],
                        dt.Rows[0]["JsonData4"],
                        dt.Rows[0]["JsonData5"],
                        dt.Rows[0]["JsonData6"],
                        dt.Rows[0]["JsonData7"],
                        dt.Rows[0]["JsonData8"],
                        dt.Rows[0]["JsonData9"],
                        dt.Rows[0]["JsonData10"]
                        );

                model = Utility.JsonToObject<CalculationModel>(Zipper.Unzip(Convert.FromBase64String(sEncodeJson)));
            }

            return model;
        }

        public static void SaveModel(CalculationModel model)
        {
            DataTable dt = new DataTable("CalPrice");
            dt.Columns.Add(new DataColumn("PriceID", typeof(long)));
            dt.Columns.Add(new DataColumn("JsonData1", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData2", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData3", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData4", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData5", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData6", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData7", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData8", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData9", typeof(string)));
            dt.Columns.Add(new DataColumn("JsonData10", typeof(string)));
            dt.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));

            //create new calculation row
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //set insert columns
            List<DataColumn> oIgnoreSave = new List<DataColumn>();
            oIgnoreSave.Add(dt.Columns["PriceID"]);
            if (model.ID == 0)
            {
                //model.ID = CalPriceGetLatestID() + 1;
                dr["CreatedDate"] = DateTime.Now;
                oIgnoreSave.Add(dt.Columns["ModifiedDate"]);

                //insert new row first to get id
                model.ID = InsertRowReturnIdentity(dt.Rows[0], dt.Columns["PriceID"], oIgnoreSave.ToArray());
            }
            else
            {
                dr["ModifiedDate"] = DateTime.Now;
                oIgnoreSave.Add(dt.Columns["CreatedDate"]);
            }

            dr["PriceID"] = model.ID;

            string sJson = Utility.ObjectToJson(model);
            string sCompress = Zipper.Zip(sJson);

            //extract encode json to JsonData1 to JsonData10
            int i = 1;
            while (!String.IsNullOrWhiteSpace(sCompress))
            {
                if (sCompress.Length >= 3000)
                {
                    dr[String.Concat("JsonData", i)] = sCompress.Substring(0, 4000);
                    sCompress = sCompress.Remove(0, 3000);
                }
                else
                {
                    dr[String.Concat("JsonData", i)] = sCompress.Substring(0);
                    sCompress = sCompress.Remove(0, sCompress.Length);
                }

                i += 1;
            }

            //update jsondata by particular row
            SaveTable(dt, dt.Columns["PriceID"], oIgnoreSave.ToArray());

            //save proffix if needed
            SaveProffix(model);
        }

        public static void SaveProffix(CalculationModel model)
        {
            if (model.ProffixModel == null)
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(model.ProffixModel.LAGDokumenteArtikelNrLAG))
            {
                return;
            }

            //1. save LAG_Dokumente
            SaveLAG_Dokumente(model);

            //2. save LAG_Artikel
            if (model.GeneralSetting.PriceScale.Scale == 1 && model.GeneralSetting.Options.Contains("A"))
            {
                SaveLAG_Artikel(model);
            }

            //3. save scale if needed
            if (model.GeneralSetting.PriceScale.Scale > 1 && model.GeneralSetting.Options.Contains("A"))
            {
                SavePRE_PreisStaffel(model);                
            }
        }
    }
}
