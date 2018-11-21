using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class CalculationBasicCtrl
    {
        void SetOptionScale()
        {
            //predefine scale amount
            SetPredefineProfitAmount();

            //setup price scales combobox if needed
            SetScaleCombobox();
        }

        void SetScaleCombobox()
        {
            //setup price scales combobox
            //if price scale more than 1
            if (_Model.GeneralSetting.PriceScale.Scale > 1)
            {
                List<ComboboxItemModel> oItems = new List<ComboboxItemModel>();

                //add basic calculation first
                oItems.Add(new ComboboxItemModel() { Value = 0, Caption = "Grundberechnung" });

                //setup price scale dropdown items
                for (int i = 1; i <= _Model.GeneralSetting.PriceScale.Scale; i++)
                {
                    oItems.Add(new ComboboxItemModel() { Value = i, Caption = String.Format("Staffel {0}", i) });
                }

                cboPriceScales.Properties.DataSource = oItems;
                cboPriceScales.Properties.ValueMember = "Value";
                cboPriceScales.Properties.DisplayMember = "Caption";
                cboPriceScales.ItemIndex = 0;
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;
            }
        }

        void SetPredefineProfitAmount()
        {
            //set predefined profit amount for each scale
            int iScaleCount = _Model.CalculationNotes.Count(item => item.ID > 0);

            if (iScaleCount > 1)
            {
                if (_Model.GeneralSetting.PriceScale.MinProfit > 0 && _Model.GeneralSetting.PriceScale.MaxProfit > 0)
                {
                    //everage amount excluded first item
                    decimal iEverageAmount = (_Model.GeneralSetting.PriceScale.MaxProfit -
                        _Model.GeneralSetting.PriceScale.MinProfit) / (iScaleCount - 1);

                    //scale item start from ID 1
                    for (int i = 0; i < iScaleCount; i++)
                    {
                        decimal iProfitAmount = 0;
                        if (i == 0)
                        {
                            // set first predefined
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MaxProfit;
                        }
                        else if (i == (iScaleCount - 1))
                        {
                            // set last predefined
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MinProfit;
                        }
                        else
                        {
                            // set rest predefined
                            //iProfitAmount = iEverageAmount * ((_Model.CalculationNotes.Count - 1) - i);
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MaxProfit - (iEverageAmount * i);
                        }

                        //update amount to calculation model
                        //if percent or fix predefined
                        CalculationItemModel oCalRow = _Model.CalculationNotes[i + 1].CalculationItems[0];
                        if (_Model.GeneralSetting.PriceScale.MarkUp == "P")
                        {
                            oCalRow.EditedField = "P";
                            _Calculation.UpdateRowAmountPercent(_Model, oCalRow, iProfitAmount, skipBaseGroupRows: true);
                        }
                        else
                        {
                            oCalRow.EditedField = "F";
                            _Calculation.UpdateRowAmountFix(_Model, oCalRow, iProfitAmount, skipBaseGroupRows: true);
                        }
                    }
                }
            }
        }
    }
}
