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
        static void SavePRE_PreisStaffel(CalculationModel model)
        {
            //update value 
            DataTable dt = new DataTable();
            dt.TableName = "PRE_PreisStaffel";
            DataColumn[] cols = {
                new DataColumn("LaufNr", typeof(Int32)), //id
                new DataColumn("ANummer", typeof(string)), //artikel
                new DataColumn("ArtikelTyp", typeof(Int16)), //1
                new DataColumn("AssortiertPRE", typeof(Int32)), //0

                new DataColumn("KNummer", typeof(Int32)), //0
                new DataColumn("KundenTyp", typeof(Int16)), //0

                new DataColumn("MengeVon", typeof(float)), //scale
                new DataColumn("PreisTypPRE", typeof(string)), //ART
                new DataColumn("Prozent", typeof(Int16)), //0

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
            dt.Columns.AddRange(cols);

           
            //value columns
            List<DataColumn> valueCols = new List<DataColumn>();


            //PRE_PreisStaffel            
            List<ProffixPREPreisStaffelModel> oList = GetProffixPREPreisStaffelModelList(model.ProffixModel.LAGDokumenteArtikelNrLAG, model.ProffixConnection);

            //clear existing            
            if (oList != null && oList.Count > 0)
            {
                //overwrite amount
                //scale calculation note id start from 1
                foreach (CalculationNoteModel note in model.CalculationNotes)
                {
                    ProffixPREPreisStaffelModel oModel = oList.Where(item => item.MengeVon == note.Quantity).FirstOrDefault();
                    if (oModel != null)
                    {
                        oModel.Wert = note.CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault().Total;
                    }
                }

                //set value columns
                valueCols.Add(dt.Columns["Wert"]);

                foreach (ProffixPREPreisStaffelModel item in oList)
                {
                    DataRow dr = dt.NewRow();
                    dr["LaufNr"] = item.LaufNr;
                    dr["Wert"] = item.Wert;
                    dt.Rows.Add(dr);

                    UpdateRow(
                        dr,
                        dt.Columns["LaufNr"],
                        valueCols,
                        connectionString: model.ProffixConnection
                        );
                }
            }
            else
            {
                //set value columns
                valueCols.AddRange(dt.Columns.Cast<DataColumn>().ToList());

                //insert new
                DateTime oNow = DateTime.Now;
                foreach (CalculationNoteModel note in model.CalculationNotes)
                {
                    if (note.ID == 0)
                    {
                        return;
                    }

                    DataRow dr = dt.NewRow();
                    dr["LaufNr"] = 0; //new row
                    dr["ANummer"] = model.ProffixModel.LAGDokumenteArtikelNrLAG;
                    dr["ArtikelTyp"] = 1;
                    dr["AssortiertPRE"] = 0;

                    dr["KNummer"] = 0;
                    dr["KundenTyp"] = 0;

                    dr["MengeVon"] = note.Quantity;
                    dr["PreisTypPRE"] = "ART";
                    dr["Prozent"] = 0;

                    dr["StaffelCode"] = String.Concat("", ".", model.ProffixModel.LAGDokumenteArtikelNrLAG, ".CHF"); ////10.0.1.221100200.CHF
                    dr["Verkauf"] = 1;
                    dr["WaehrungPRO"] = "CHF";
                    dr["Wert"] = note.CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault().Total;

                    dr["ImportNr"] = 0;

                    dr["ErstelltAm"] = oNow;
                    dr["ErstelltVon"] = model.GeneralSetting.Employee; //"cs";
                    dr["GeaendertAm"] = oNow;
                    dr["GeaendertVon"] = model.GeneralSetting.Employee; //"cs";

                    dr["Geaendert"] = 1;
                    dr["Exportiert"] = 0;

                    dt.Rows.Add(dr);

                    InsertRowManualIncreaseID(dr, dt.Columns["LaufNr"], null, connectionString: model.ProffixConnection);
                }
            }
        }
    }
}
