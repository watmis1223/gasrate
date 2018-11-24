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
            DataTable dt = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("cal_CalPriceGetByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PriceID", id));
                    conn.Open();

                    try
                    {
                        dt.Load(cmd.ExecuteReader());
                    }
                    catch { }

                    conn.Close();
                }
            }

            if (dt != null)
            {
                string sEncodeJson = String.Concat(
                        dt.Rows[0]["JsonData1"],
                        dt.Rows[0]["JsonData2"],
                        dt.Rows[0]["JsonData3"],
                        dt.Rows[0]["JsonData4"],
                        dt.Rows[0]["JsonData5"]);

                model = Utility.JsonToObject<CalculationModel>(Zipper.Unzip(Convert.FromBase64String(sEncodeJson)));
            }

            return model;
        }

        public static ProffixLAGArtikelModel GetProffixLAGArtikelModel(string artikelNrLAG, string connectionString)
        {
            ProffixLAGArtikelModel model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("Bezeichnung1", typeof(string)),
                new DataColumn("Bezeichnung2", typeof(string)),
                new DataColumn("Bezeichnung3", typeof(string)),
                new DataColumn("Bezeichnung4", typeof(string)),
                new DataColumn("Bezeichnung5", typeof(string)),
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("ArtikelNrLAG", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            DataTable dt = LoadTable("LAG_Artikel", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixLAGArtikelModel()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    ArtikelNrLAG = dt.Rows[0]["ArtikelNrLAG"].ToString(),
                    Bezeichnung1 = dt.Rows[0]["Bezeichnung1"].ToString(),
                    Bezeichnung2 = dt.Rows[0]["Bezeichnung2"].ToString(),
                    Bezeichnung3 = dt.Rows[0]["Bezeichnung3"].ToString(),
                    Bezeichnung4 = dt.Rows[0]["Bezeichnung4"].ToString(),
                    Bezeichnung5 = dt.Rows[0]["Bezeichnung5"].ToString(),
                };
            }

            return model;
        }

        public static List<ProffixLAGLieferantenModel> GetProffixLAGLieferantenModelList(string artikelNrLAG, string connectionString)
        {
            List<ProffixLAGLieferantenModel> modelList = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("[Name]", typeof(string))
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("ArtikelNrLAG", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            DataTable dt = LoadTable("LAG_Lieferanten", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                modelList = new List<ProffixLAGLieferantenModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    modelList.Add(new ProffixLAGLieferantenModel()
                    {
                        LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                        ArtikelNrLAG = dr["ArtikelNrLAG"].ToString(),
                        Name = dr["Name"].ToString()
                    });
                }
            }

            return modelList;
        }

        public static ProffixLAGDokumente GetProffixLAGDokumente(string artikelNrLAG, string calculationID, string connectionString)
        {
            ProffixLAGDokumente model = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)),
                new DataColumn("ArtikelNrLAG", typeof(string)),
                new DataColumn("Bemerkungen", typeof(string)),
                new DataColumn("DateiName", typeof(DateTime))
            };

            DataColumn[] oCondition = new DataColumn[2];
            DataColumn col = new DataColumn("ArtikelNrLAG", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            col = new DataColumn("rtrim(ltrim([DateiName]))", typeof(string));
            col.DefaultValue = String.Concat("opencal", "%", artikelNrLAG,
                String.IsNullOrWhiteSpace(calculationID) ? "" : String.Concat(" ", calculationID));
            oCondition[1] = col;

            DataTable dt = LoadTable("LAG_Dokumente", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new ProffixLAGDokumente()
                {
                    LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                    ArtikelNrLAG = dt.Rows[0]["ArtikelNrLAG"].ToString(),
                    Bemerkungen = dt.Rows[0]["Bemerkungen"].ToString(),
                    DateiName = dt.Rows[0]["DateiName"].ToString()
                };
            }

            return model;
        }


        public static void SaveProffix()
        {
        }

        public static void SaveModel(CalculationModel model)
        {
            bool isNew = false;

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
                isNew = true;
                //model.ID = CalPriceGetLatestID() + 1;
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


            //extract encode json to JsonData1 to JsonData5
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

            //insert new row
            if (isNew)
            {
                //update proffix data if needed
                if (!String.IsNullOrWhiteSpace(model.ArtikelNrLAG))
                {
                    ProffixLAGDokumente oProffixLAGDokumente = GetProffixLAGDokumente(model.ArtikelNrLAG, null, model.ProffixConnection);                    

                    if (oProffixLAGDokumente != null)
                    {
                        //insert or update calculation
                        long iIdentity = InsertRowReturnIdentity(dt.Rows[0], dt.Columns["PriceID"], oIgnoreSave.ToArray());
                        
                        //update value 
                        DataTable oDtLAGDokumente = new DataTable();
                        oDtLAGDokumente.TableName = "LAG_Dokumente";
                        oDtLAGDokumente.Columns.Add(new DataColumn("LaufNr", typeof(string)));                        
                        oDtLAGDokumente.Columns.Add(new DataColumn("DateiName", typeof(string)));
                        DataRow oDr = oDtLAGDokumente.NewRow();
                        oDtLAGDokumente.Rows.Add(oDr);

                        oDr["LaufNr"] = oProffixLAGDokumente.LaufNr;
                        oDr["DateiName"] = String.Concat(oProffixLAGDokumente.DateiName, " ", iIdentity);
                        UpdateRow(
                            oDr,
                            oDtLAGDokumente.Columns["LaufNr"],
                            new List<DataColumn>() { oDtLAGDokumente.Columns["DateiName"] },
                            connectionString: model.ProffixConnection);
                    }
                }
                else
                {
                    //insert or update calculation
                    SaveTable(dt, dt.Columns["PriceID"], oIgnoreSave.ToArray());
                }
            }
        }


        //static void GetCAL_EinkaufskalkulationData(CalculationModel model)
        //{
        //    // #1 table to save
        //    if (model == null)
        //    {
        //        return;
        //    }

        //    DataTable dt = new DataTable("CAL_EinkaufskalkulationData");
        //    dt.Columns.Add(new DataColumn("[CalcNrCAL]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[TypVP]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[TypEP]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[TypDeck]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Schreibschutz]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Aktiv]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Bemerkungen]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[StaffAnzahl]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[StaffAngabe]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[MinAufschlag]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[MaxAufschlag]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[BenutzerzeileAnzahl]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[LieferantNb]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[Grundlage]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[Mitarbeiter]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[Datum]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[Einheit]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[EinheitEinkauf]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[EinheitVerkauf]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[EinheitDivisor]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[Waehrung]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[WaehrungText]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[WaehrungDivisor]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[DokumentNrLAG]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel1]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel2]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel3]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel4]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel5]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel6]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel7]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel8]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel9]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[MengeStaffel10]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[ArtikelNrLAG]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[AdressNrADR]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[SK2]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[Gueltig_Von]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[Gueltig_Bis]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[Staffelnr]", typeof(string)));
        //    dt.Columns.Add(new DataColumn("[SK2minusVGK]", typeof(float)));
        //}

        //static void GetCAL_ZeileData(CalculationModel model)
        //{
        //    // #2 table to save
        //    if (model == null)
        //    {
        //        return;
        //    }

        //    DataTable dt = new DataTable("CAL_Zeile");
        //    dt.Columns.Add(new DataColumn("[CalcNrCAL]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[PositionNr]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Typ]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[EingabeWert]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[EingabeFeld]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Waehrung]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Einheit]", typeof(Int16)));            
        //}

        //static void GetCAL_ZeileBenutzerdefiniertData(CalculationModel model)
        //{
        //    // #3 table to save
        //    if (model == null)
        //    {
        //        return;
        //    }

        //    DataTable dt = new DataTable("CAL_ZeileBenutzerdefiniert");
        //    dt.Columns.Add(new DataColumn("[CalcNrCAL]", typeof(Int32)));
        //    dt.Columns.Add(new DataColumn("[PositionNr]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Typ]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[EingabeWert]", typeof(float)));
        //    dt.Columns.Add(new DataColumn("[EingabeFeld]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Waehrung]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[Einheit]", typeof(Int16)));
        //    dt.Columns.Add(new DataColumn("[ZeilenText]", typeof(string)));
        //}



    }
}
