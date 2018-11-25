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
        static void SaveLAG_Dokumente(CalculationModel model)
        {
            //save or update
            string sCalculationID = model.ProffixModel.IsNew ? null : model.ID.ToString();
            ProffixLAGDokumente oProffixLAGDokumente = GetProffixLAGDokumente(
                model.ProffixModel.LAGDokumenteArtikelNrLAG, sCalculationID, model.ProffixConnection);

            if (oProffixLAGDokumente != null)
            {
                //update value 
                DataTable oDtLAGDokumente = new DataTable();
                oDtLAGDokumente.TableName = "LAG_Dokumente";
                oDtLAGDokumente.Columns.Add(new DataColumn("LaufNr", typeof(string)));
                oDtLAGDokumente.Columns.Add(new DataColumn("DateiName", typeof(string)));
                oDtLAGDokumente.Columns.Add(new DataColumn("Bezeichnung", typeof(string)));
                DataRow oDr = oDtLAGDokumente.NewRow();
                oDtLAGDokumente.Rows.Add(oDr);

                oDr["LaufNr"] = oProffixLAGDokumente.LaufNr;
                oDr["DateiName"] = String.Concat(oProffixLAGDokumente.DateiName, " ", model.ID);

                //Kalk. VP 01.12.2017 Aktiv Oldenburg Kunststoff-Te
                string sBezeichnung = String.Concat(
                    "Kalk. ",
                    model.GeneralSetting.CostType == "S" ? "VP " : "EP ",
                    DateTime.Now.ToString("dd.MM.yyyy", new CultureInfo("en-US")),
                    model.GeneralSetting.Options.Contains("A") ? " Aktiv" : " ",
                    model.GeneralSetting.Supplier);
                oDr["Bezeichnung"] = sBezeichnung.Length > 100 ? sBezeichnung.Substring(0, 100) : sBezeichnung;

                //update proffix
                UpdateRow(
                    oDr,
                    oDtLAGDokumente.Columns["LaufNr"],
                    new List<DataColumn>() {
                                oDtLAGDokumente.Columns["DateiName"],
                                oDtLAGDokumente.Columns["Bezeichnung"]
                    },
                    connectionString: model.ProffixConnection);
            }
        }
    }
}
