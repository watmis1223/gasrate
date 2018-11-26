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
        static void SaveLAG_Artikel(CalculationModel model)
        {
            //save or update
            //update value 
            DataTable dt = new DataTable();
            dt.TableName = "LAG_Artikel";
            dt.Columns.Add(new DataColumn("ArtikelNrLAG", typeof(string)));
            dt.Columns.Add(new DataColumn("Verkauf5", typeof(float)));
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //scale calculation note id start from 1
            dr["ArtikelNrLAG"] = model.ProffixModel.LAGDokumenteArtikelNrLAG;
            dr["Verkauf5"] = model.CalculationNotes[1].CalculationItems.Where(item => item.Tag == "VK(liste)").FirstOrDefault().Total;

            //update proffix
            UpdateRow(
                dr,
                dt.Columns["ArtikelNrLAG"],
                new List<DataColumn>() {
                    dt.Columns["Verkauf5"]
                },
                connectionString: model.ProffixConnection);
        }
    }
}
