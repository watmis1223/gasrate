using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models;
using CalculationOilPrice.Library.Util;

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

        public static void SaveModel(CalculationModel model)
        {
            DataTable dt = new DataTable("CalPrice");
            dt.Columns.Add(new DataColumn("PriceID", typeof(long)));
            dt.Columns.Add(new DataColumn("JsonData", typeof(string)));
            dt.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));

            DataRow dr = dt.NewRow();
            List<DataColumn> oIgnoreSave = new List<DataColumn>();
            oIgnoreSave.Add(dt.Columns["PriceID"]);
            if (model.ID == 0)
            {
                model.ID = CalPriceGetLatestID() + 1;
                dr["CreatedDate"] = DateTime.Now;
                oIgnoreSave.Add(dt.Columns["ModifiedDate"]);
            }
            else
            {
                dr["ModifiedDate"] = DateTime.Now;
                oIgnoreSave.Add(dt.Columns["CreatedDate"]);
            }

            dr["PriceID"] = model.ID;

            string sJson = Utility.ObjectToJson(model);
            string sCompress = Zipper.Zip(sJson);
            //string sCompressAgain = Zipper.Zip(sCompress);

            dr["JsonData"] = sCompress;
            dt.Rows.Add(dr);

            SaveTable(dt, dt.Columns["PriceID"], oIgnoreSave.ToArray());
        }
    }
}
