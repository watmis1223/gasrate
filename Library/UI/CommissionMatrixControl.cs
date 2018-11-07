using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CalculationOilPrice.Library.Entity;

namespace CalculationOilPrice.Library.UI
{
    public partial class CommissionMatrixControl : UserControl
    {
        DataTable commissionDt;
        DataTable commissionMatrixDt;
        Order currentOrder;

        enum TempColumnNames
        {
            CommissionList,
            Total1,
            Total2,
            OilQuantityFor100KG,
            MarginAmount,
            MarginPercent,
            Sign1,
            Sign2,
            Sign3
        }

        public CommissionMatrixControl()
        {
            InitializeComponent();
        }

        void Reload()
        {
            commissionMatrixDt = Storage.StorageOperator.LoadTable("CommissionMatrix", null, null, null);
            InitCommissionRows();
            commissionGridControl.DataSource = commissionDt;
            commissionGridView.RefreshData();

            commissionGridView.Columns[TempColumnNames.CommissionList.ToString()].Width = 170;
            commissionGridView.Columns[TempColumnNames.Sign1.ToString()].Width = 20;
            commissionGridView.Columns[TempColumnNames.Sign2.ToString()].Width = 20;
            commissionGridView.Columns[TempColumnNames.Sign3.ToString()].Width = 20;
            commissionGridView.Columns[TempColumnNames.Total1.ToString()].Width = 70;
            commissionGridView.Columns[TempColumnNames.Total2.ToString()].Width = 70;
            commissionGridView.Columns[TempColumnNames.OilQuantityFor100KG.ToString()].Width = 60;
            commissionGridView.Columns[TempColumnNames.MarginAmount.ToString()].Width = 200;
            commissionGridView.Columns[TempColumnNames.MarginPercent.ToString()].Width = 90;

            ///Setup columns
            commissionGridView.Columns[TempColumnNames.CommissionList.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.Total1.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.Total1.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            commissionGridView.Columns[TempColumnNames.Total1.ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            commissionGridView.Columns[TempColumnNames.Total1.ToString()].DisplayFormat.FormatString = "n2";


            commissionGridView.Columns[TempColumnNames.Total2.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.Total2.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            commissionGridView.Columns[TempColumnNames.Total2.ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            commissionGridView.Columns[TempColumnNames.Total2.ToString()].DisplayFormat.FormatString = "n2";

            commissionGridView.Columns[TempColumnNames.OilQuantityFor100KG.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.OilQuantityFor100KG.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            commissionGridView.Columns[TempColumnNames.OilQuantityFor100KG.ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            commissionGridView.Columns[TempColumnNames.OilQuantityFor100KG.ToString()].DisplayFormat.FormatString = "n2";

            commissionGridView.Columns[TempColumnNames.MarginAmount.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.MarginAmount.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            commissionGridView.Columns[TempColumnNames.MarginAmount.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;            
            commissionGridView.Columns[TempColumnNames.MarginAmount.ToString()].DisplayFormat.FormatString = "n2";

            commissionGridView.Columns[TempColumnNames.MarginPercent.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.MarginPercent.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            commissionGridView.Columns[TempColumnNames.MarginPercent.ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            commissionGridView.Columns[TempColumnNames.MarginPercent.ToString()].DisplayFormat.FormatString = "n2";

            commissionGridView.Columns[TempColumnNames.Sign1.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.Sign1.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            
            commissionGridView.Columns[TempColumnNames.Sign2.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.Sign2.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            
            commissionGridView.Columns[TempColumnNames.Sign3.ToString()].OptionsColumn.AllowEdit = false;
            commissionGridView.Columns[TempColumnNames.Sign3.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        void InitCommissionRows()
        {
            commissionDt = new DataTable("Commission");
            DataColumn dc = null;

            dc = new DataColumn(TempColumnNames.CommissionList.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Total1.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "per 100 KG";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Sign1.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Total2.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "per 100 KG";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Sign2.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.OilQuantityFor100KG.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "Menge";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Sign3.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.MarginAmount.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "Marge als Betrag";
            commissionDt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.MarginPercent.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "Marge in Prozent";
            commissionDt.Columns.Add(dc);

            DataRow dr = null;

            decimal vpPer100KgInWIR = currentOrder.OilPrice * currentOrder.WIRFactor;

            decimal oilQuantityPer100KG = currentOrder.OilQuantity / 100;
            oilQuantityPer100KG = Math.Round(oilQuantityPer100KG, 2, MidpointRounding.AwayFromZero);
            
            decimal marginAmount = GetMarginAmount(vpPer100KgInWIR, currentOrder.OilPrice, oilQuantityPer100KG);
            decimal marginPercent = GetMarginPercent(currentOrder.Total1Price, marginAmount);
            

            decimal wirCorrectionTotal1 = marginAmount * (currentOrder.PartOfWIR/100);
            wirCorrectionTotal1 = Math.Round(wirCorrectionTotal1, 2, MidpointRounding.AwayFromZero);

            decimal wirCorrectionMarginAmount = wirCorrectionTotal1 * currentOrder.CommissionFactor;
            wirCorrectionMarginAmount = Math.Round(wirCorrectionMarginAmount, 2, MidpointRounding.AwayFromZero);

            decimal commissionLegitimateMarginAmount = marginAmount - wirCorrectionMarginAmount;
            commissionLegitimateMarginAmount = Math.Round(commissionLegitimateMarginAmount, 2, MidpointRounding.AwayFromZero);

            decimal commissionLegitimateMarginPercent = GetMarginPercent(currentOrder.Total1Price, commissionLegitimateMarginAmount);
            decimal commissionFactor = GetComissionFactor(commissionLegitimateMarginAmount, commissionLegitimateMarginPercent);
            
            decimal totalBonus = commissionLegitimateMarginAmount * commissionFactor;
            totalBonus = Math.Round(totalBonus, 2, MidpointRounding.AwayFromZero);


            ///========== row1
            dr = commissionDt.NewRow();
            dr[TempColumnNames.CommissionList.ToString()] = "Kalk. Verkaufspreis";
            //dr[TempColumnNames.Total1.ToString()] = currentOrder.TotalPricePer100KG;
            
            dr[TempColumnNames.Total1.ToString()] = vpPer100KgInWIR;
            //
            dr[TempColumnNames.Sign1.ToString()] = "-";
            dr[TempColumnNames.Total2.ToString()] = currentOrder.OilPrice;
            dr[TempColumnNames.Sign2.ToString()] = "x";
            dr[TempColumnNames.OilQuantityFor100KG.ToString()] = oilQuantityPer100KG;
            dr[TempColumnNames.Sign3.ToString()] = "=";
            dr[TempColumnNames.MarginAmount.ToString()] = string.Format("{0:n2}", marginAmount);
            dr[TempColumnNames.MarginPercent.ToString()] = string.Format("{0:n2}", marginPercent);
            commissionDt.Rows.Add(dr);
            currentOrder.Commission.CalculationSalesPriceAmount = marginAmount;
            currentOrder.Commission.CalculationSalesPricePercent = marginPercent;

            ///========== row2
            dr = commissionDt.NewRow();
            dr[TempColumnNames.CommissionList.ToString()] = "WIR Anteil %";
            dr[TempColumnNames.Total1.ToString()] = currentOrder.PartOfWIR;
            commissionDt.Rows.Add(dr);


            ///========== row3
            dr = commissionDt.NewRow();
            dr[TempColumnNames.CommissionList.ToString()] = "WIR Korrektur";
            dr[TempColumnNames.Total1.ToString()] = wirCorrectionTotal1;
            dr[TempColumnNames.Sign1.ToString()] = "x";
            dr[TempColumnNames.Total2.ToString()] = currentOrder.CommissionFactor;
            dr[TempColumnNames.Sign3.ToString()] = "=";
            dr[TempColumnNames.MarginAmount.ToString()] = string.Format("{0:n2}", wirCorrectionMarginAmount);
            commissionDt.Rows.Add(dr);
            currentOrder.Commission.WIRCorrection = wirCorrectionTotal1;
            currentOrder.Commission.WIRCorrectionAmount = wirCorrectionMarginAmount;

            ///========== row4
            dr = commissionDt.NewRow();
            dr[TempColumnNames.CommissionList.ToString()] = "Provisionsberechtigte Marge";
            dr[TempColumnNames.MarginAmount.ToString()] =
                string.Format("{0:n2} - {1:n2} = {2:n2}", marginAmount, wirCorrectionMarginAmount, commissionLegitimateMarginAmount);

            ///* check solution if it correct again.
            dr[TempColumnNames.MarginPercent.ToString()] = string.Format("{0:n2}", commissionLegitimateMarginPercent);
            commissionDt.Rows.Add(dr);
            currentOrder.Commission.CommissionLegitimateAmount = commissionLegitimateMarginAmount;
            currentOrder.Commission.CommissionLegitimatePercent = commissionLegitimateMarginPercent;


            dr = commissionDt.NewRow();
            commissionDt.Rows.Add(dr);

            ///========== row5
            dr = commissionDt.NewRow();
            dr[TempColumnNames.CommissionList.ToString()] = "Provision gemäss Matrix in Prozent";
            dr[TempColumnNames.Total1.ToString()] = commissionFactor * 100;
            commissionDt.Rows.Add(dr);
            currentOrder.Commission.TotalCommissionPercent = commissionFactor * 100;            

            ///========== row6
            dr = commissionDt.NewRow();
            dr[TempColumnNames.CommissionList.ToString()] = "zu zahlende Provision";
            dr[TempColumnNames.Total1.ToString()] = totalBonus;
            commissionDt.Rows.Add(dr);
            currentOrder.Commission.TotalCommissionAmount = totalBonus; 
        }

        decimal GetMarginAmount(decimal totalPricePer100KG, decimal oilPrice, decimal oilQuantityFor100KG)
        {
            decimal amount = 0;
            amount = (totalPricePer100KG - oilPrice) * oilQuantityFor100KG;
            amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            return amount;
        }

        decimal GetMarginPercent(decimal totalPrice, decimal marginAmount)
        {
            decimal amount = 0;

            try
            {
                amount = (marginAmount / totalPrice) * 100;
                amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            }
            catch { }
            
            return amount;
        }

        decimal GetComissionFactor(decimal commisionLegitimateMarginAmount, decimal commisionLegitimateMarginPercent)
        {
            decimal factor = 0;
            decimal min = 0;
            decimal max = 0;
            decimal roundCommisionLegitimateMarginPercent = Math.Round(commisionLegitimateMarginPercent, 0, MidpointRounding.AwayFromZero);

            if (commissionMatrixDt != null)
            {
                foreach (DataRow dr in commissionMatrixDt.Rows)
                {
                    min = Convert.ToDecimal(dr["MarginMin"]);
                    max = Convert.ToDecimal(dr["MarginMax"]);

                    if (roundCommisionLegitimateMarginPercent >= min && roundCommisionLegitimateMarginPercent <= max)
                    {
                        factor = Convert.ToDecimal(dr["PercentOfCommission"]);
                        break;
                    }
                }
            }           

            factor = factor / 100;
            factor = Math.Round(factor, 2, MidpointRounding.AwayFromZero);

            return factor;
        }

        public void CalculateCommission(Order order)
        {
            currentOrder = order;
            Reload();
        }
    }
}
