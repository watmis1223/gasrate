using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationOilPrice.Library.Storage
{
    static partial class StorageOperator
    {
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

        public static List<ProffixPREPreisStaffelModel> GetProffixPREPreisStaffelModelList(string artikelNrLAG, string connectionString)
        {
            List<ProffixPREPreisStaffelModel> modelList = null;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            DataColumn[] oSelect = {
                new DataColumn("LaufNr", typeof(Int32)), //id
                new DataColumn("ANummer", typeof(string)), //artikel
                new DataColumn("ArtikelTyp", typeof(Int16)), //1
                new DataColumn("AssortiertPRE", typeof(Int32)), //0

                new DataColumn("KNummer", typeof(Int32)), //0
                new DataColumn("KundenTyp", typeof(Int16)), //0

                new DataColumn("MengeVon", typeof(float)), //scale
                new DataColumn("PreisTypPRE", typeof(string)), //ART
                new DataColumn("Prozent", typeof(Int16)),

                new DataColumn("StaffelCode", typeof(string)), //10.0.1.221100200.CHF
                new DataColumn("Verkauf", typeof(Int16)), //1
                new DataColumn("WaehrungPRO", typeof(string)), //CHF
                new DataColumn("Wert", typeof(float)), //amount
                
                new DataColumn("ImportNr", typeof(Int32)), //0

                new DataColumn("ErstelltAm", typeof(DateTime)),
                new DataColumn("ErstelltVon", typeof(string)), //cs
                new DataColumn("GeaendertAm", typeof(DateTime)),
                new DataColumn("GeaendertVon", typeof(string)), //cs

                new DataColumn("Geaendert", typeof(Int16)), //1
                new DataColumn("Exportiert", typeof(Int16)), //0
            };

            DataColumn[] oCondition = new DataColumn[1];
            DataColumn col = new DataColumn("ANummer", typeof(string));
            col.DefaultValue = artikelNrLAG;
            oCondition[0] = col;

            DataTable dt = LoadTable("PRE_PreisStaffel", oSelect, oCondition, null, connectionString: connectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                modelList = new List<ProffixPREPreisStaffelModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    modelList.Add(new ProffixPREPreisStaffelModel()
                    {
                        LaufNr = Convert.ToInt32(dt.Rows[0]["LaufNr"]),
                        ANummer = dr["ArtikelNrLAG"].ToString(),
                        ArtikelTyp = Convert.ToInt32(dr["ArtikelTyp"]),
                        AssortiertPRE = Convert.ToInt32(dr["AssortiertPRE"]),

                        KNummer = Convert.ToInt32(dr["KNummer"]),
                        KundenTyp = Convert.ToInt32(dr["KundenTyp"]),

                        MengeVon = Convert.ToDecimal(dr["MengeVon"]),
                        PreisTypPRE = dr["PreisTypPRE"].ToString(),
                        Prozent = Convert.ToInt32(dr["Prozent"]),

                        StaffelCode = dr["StaffelCode"].ToString(),
                        Verkauf = Convert.ToInt32(dr["Verkauf"]),
                        WaehrungPRO = dr["WaehrungPRO"].ToString(),
                        Wert = Convert.ToDecimal(dr["Wert"]),

                        ImportNr = Convert.ToInt32(dr["ImportNr"]),

                        ErstelltAm = Convert.ToDateTime(dr["ErstelltAm"]),
                        ErstelltVon = dr["ErstelltVon"].ToString(),
                        GeaendertAm = Convert.ToDateTime(dr["GeaendertAm"]),
                        GeaendertVon = dr["GeaendertVon"].ToString(),

                        Geaendert = Convert.ToInt32(dr["Geaendert"]),
                        Exportiert = dr["Exportiert"].ToString(),
                    });
                }
            }

            return modelList;
        }
    }
}
