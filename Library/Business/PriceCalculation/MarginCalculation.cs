using CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Business.PriceCalculation
{
    public class MarginCalculation
    {
        public void UpdateBaseAmountAll(CalculationModel model)
        {
            //if margin not enabled
            if (!model.GeneralSetting.Options.Contains("M"))
            {
                return;
            }

            //get margin item by tag
            var oModels = model.CalculationMarginViewItems.FindAll(item => !item.IsSummary);

            foreach (CalculationItemModel oMargin in oModels)
            {
                CalculationItemModel oBasic = model.CalculationViewItems.Where(o => o.Tag == oMargin.Tag).FirstOrDefault();

                if (oBasic != null)
                {
                    oMargin.Total = oBasic.Total;
                }
                else
                {
                    //if BEN tag
                    oMargin.Total = model.CalculationViewItems.Where(o => o.Tag.StartsWith("BEN")).Sum(o => o.Total);
                }

                //set variable total
                if (oMargin.Tag == "BEK" || oMargin.Tag == "VK(bar)" || oMargin.Tag == "VK(brutto)")
                {
                    //if base amount row
                    oMargin.VariableTotal = oMargin.Total;
                }
                else
                {
                    if (oMargin.EditedField == "P")
                    {
                        //calculate amount fix
                        oMargin.AmountFix = oMargin.Total * (oMargin.AmountPercent / 100);

                        //calculate variable total
                        oMargin.VariableTotal = oMargin.Total - oMargin.AmountFix;
                    }
                    else if (oMargin.EditedField == "F")
                    {
                        //calculate amount fix
                        oMargin.AmountPercent = (oMargin.AmountFix / oMargin.Total) * 100;

                        //calculate variable total
                        oMargin.VariableTotal = oMargin.Total - oMargin.AmountFix;
                    }
                    else if (oMargin.EditedField == "V")
                    {
                        //calculate amount fix
                        oMargin.AmountFix = oMargin.Total - oMargin.VariableTotal;

                        //calculate amount percent
                        oMargin.AmountPercent = (oMargin.AmountFix / oMargin.Total) * 100;
                    }
                    else
                    {
                        //calculate amount fix
                        oMargin.AmountFix = oMargin.Total * (oMargin.AmountPercent / 100);

                        //calculate variable total
                        oMargin.VariableTotal = oMargin.Total - oMargin.AmountFix;
                    }
                }
            }

            //update group amount
            var oGroups = model.CalculationMarginViewItems.FindAll(item => item.IsSummary);
            foreach (CalculationItemModel item in oGroups)
            {
                UpdateGroupAmount(model, item.Group, item.Order);
            }
        }

        void UpdateGroupAmount(CalculationModel model, int group, int groupID)
        {
            CalculationItemModel oGroup = model.CalculationMarginViewItems.Find(item => item.Group == group && item.Order == groupID && item.IsSummary);

            if (oGroup != null)
            {
                oGroup.VariableTotal = 0;
                oGroup.AmountFix = 0;

                //set Verwaltungs- und Vertriebskosten
                if (oGroup.Tag == "VVK")
                {
                    oGroup.Total = 0;
                }

                //update items in group list first
                foreach (int i in oGroup.SummaryGroups)
                {
                    //get items per group
                    var oModels = model.CalculationMarginViewItems.FindAll(item => item.Group == i && !item.IsSummary);

                    //update group's amout fix
                    oGroup.AmountFix += (from item in oModels select item.AmountFix).Sum();

                    //update group's variable total
                    oGroup.VariableTotal += (from item in oModels select item.VariableTotal).Sum();

                    if (oGroup.Tag == "VVK")
                    {
                        oGroup.Total += (from item in oModels select item.Total).Sum();
                    }
                }

                //set Deckungsbeitrag
                if (oGroup.Tag == "VK")
                {
                    oGroup.VariableTotal = oGroup.AmountFix;
                }
            }
        }

        public decimal GetBaseTotal(CalculationModel model, int rowID)
        {
            return model.CalculationMarginViewItems[rowID].Total;
        }

        public void UpdateMarginRowAmountFix(CalculationModel model, int rowID, decimal value)
        {
            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.AmountFix = value;
            }
        }

        public void UpdateMarginRowAmountPercent(CalculationModel model, int rowID, decimal value)
        {
            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.AmountPercent = value;
            }
        }

        public void UpdateMarginRowAmountVariable(CalculationModel model, int rowID, decimal value)
        {
            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.VariableTotal = value;
            }
        }

        public void UpdateMarginRowEditedField(CalculationModel model, int rowID, string field)
        {
            //P percent, F fix, V variable total, T total

            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.EditedField = field;
            }
        }

        public decimal GetMarginSummarize(CalculationModel model)
        {
            decimal iSummary = 0;

            CalculationItemModel oVK = model.CalculationMarginViewItems.Find(item => item.Tag == "VK");
            CalculationItemModel oVKbrutto = model.CalculationMarginViewItems.Find(item => item.Tag == "VK(brutto)");

            if (oVK != null && (oVKbrutto != null && oVKbrutto.VariableTotal > 0))
            {
                iSummary = (oVK.VariableTotal / oVKbrutto.VariableTotal) * 100;
            }

            return iSummary;
        }

        //public void UpdateCalculationRowAmount(CalculationModel model, int basicItemRowID, decimal value, bool isPercent, bool specialCalculation, bool isCellEdit)
        //{
        //    CalculationItemModel oCalRow = model.CalculationViewItems[rowID];

        //    //if convert needed
        //    if (isCellEdit)
        //    {
        //        if (oCalRow.Convert != null)
        //        {
        //            //if edited P then convert F
        //            oCalRow.Convert.ConvertAmountField = isPercent ? "F" : "P";
        //        }
        //    }

        //    //UpdateCalculationRowAmount(model, oCalRow, value, isPercent, specialCalculation);
        //}

        //public void UpdateGroupAmountAll(CalculationModel model, bool updateGroupOnly)
        //{
        //    var oModels = model.CalculationViewItems.FindAll(item => item.IsSummary);

        //    foreach (CalculationItemModel item in oModels)
        //    {
        //        UpdateGroupAmount(model, item.Group, item.Order, updateGroupOnly);
        //    }
        //}

        //void UpdateGroupAmount(CalculationModel model, int group, int groupID, bool updateGroupOnly)
        //{
        //    CalculationItemModel oGroup = model.CalculationViewItems.Find(item => item.Group == group && item.Order == groupID && item.IsSummary);

        //    if (oGroup != null)
        //    {
        //        oGroup.Total = 0;

        //        //update items in group list first
        //        foreach (int i in oGroup.SummaryGroups)
        //        {
        //            //get items per group
        //            var oModels = model.CalculationViewItems.FindAll(item => item.Group == i && !item.IsSummary);

        //            if (!updateGroupOnly)
        //            {
        //                foreach (CalculationItemModel item in oModels)
        //                {
        //                    bool isSpecial = false;
        //                    if (item.Group == 0)
        //                    {
        //                        //if (item.Currency.Currency != "CHF")
        //                        //{
        //                        //    //if use custom currency
        //                        //    UpdateCalculationRowAmount(model, item.Order, item.Total, false, isSpecial, false);
        //                        //}
        //                        //else
        //                        //{
        //                        //    UpdateCalculationRowAmount(model, item.Order, item.AmountFix, false, isSpecial, false);
        //                        //}

        //                        UpdateCalculationRowAmount(model, item.Order, item.AmountFix, false, isSpecial, false);
        //                    }
        //                    else
        //                    {
        //                        //if special row
        //                        if (item.Tag == "SKT" || item.Tag == "PV" || item.Tag == "RBT")
        //                        {
        //                            isSpecial = true;
        //                        }

        //                        UpdateCalculationRowAmount(model, item.Order, item.AmountPercent, true, isSpecial, false);

        //                        ////if use scale
        //                        //if (model.ScaleCalculationItems.Count > 1 && item.Tag == "GA")
        //                        //{
        //                        //    if (model.GeneralSetting.PriceScale.MarkUp == "F")
        //                        //    {
        //                        //        UpdateCalculationRowAmount(model, item.Order, item.AmountFix, false, isSpecial, false);
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        UpdateCalculationRowAmount(model, item.Order, item.AmountPercent, true, isSpecial, false);
        //                        //    }
        //                        //}
        //                        //else
        //                        //{
        //                        //    //if not use scale
        //                        //    UpdateCalculationRowAmount(model, item.Order, item.AmountPercent, true, isSpecial, false);
        //                        //}
        //                    }

        //                    //if special field
        //                    if (isSpecial)
        //                    {
        //                        //if convert needed for group
        //                        if (item.Convert != null && item.Convert.Unit == "VE")
        //                        {
        //                            if (model.GeneralSetting.Convert.UnitNumber > 0)
        //                            {
        //                                item.Total = item.Total / model.GeneralSetting.Convert.UnitNumber;
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            //update group's total only
        //            oGroup.Total += (from item in oModels select item.Total).Sum();
        //        }

        //        //if convert needed for group
        //        if (oGroup.Convert != null && oGroup.Convert.Unit == "VE")
        //        {
        //            if (//_Model.GeneralSetting.Convert.VEUnitNumber > 0 &&
        //                //_Model.GeneralSetting.Convert.EEUnitNumber > 0 &&
        //                model.GeneralSetting.Convert.UnitNumber > 0)
        //            {
        //                oGroup.Total = oGroup.Total / model.GeneralSetting.Convert.UnitNumber;
        //            }
        //        }
        //    }
        //}

        //public void UpdateCalculationRowAmount(CalculationModel model, int rowID, decimal value, bool isPercent, bool specialCalculation, bool isCellEdit)
        //{
        //    CalculationItemModel oCalRow = model.CalculationViewItems[rowID];

        //    //if convert needed
        //    if (isCellEdit)
        //    {
        //        if (oCalRow.Convert != null)
        //        {
        //            //if edited P then convert F
        //            oCalRow.Convert.ConvertAmountField = isPercent ? "F" : "P";
        //        }
        //    }

        //    UpdateCalculationRowAmount(model, oCalRow, value, isPercent, specialCalculation);
        //}

        //void UpdateCalculationRowAmount(CalculationModel model, CalculationItemModel calRow, decimal value, bool isPercent, bool specialCalculation)
        //{
        //    if (calRow != null)
        //    {
        //        //if not master amount row
        //        //master row's group is 0
        //        if (calRow.Group != 0)
        //        {
        //            if (isPercent)
        //            {
        //                if (specialCalculation)
        //                {
        //                    UpdateRowAmountPercentSpecial(model, calRow, value);
        //                }
        //                else
        //                {
        //                    UpdateRowAmountPercent(model, calRow, value);
        //                }
        //            }
        //            else
        //            {
        //                UpdateRowAmountFix(model, calRow, value);
        //            }
        //        }
        //        else
        //        {
        //            //set row's total amount
        //            //master row
        //            calRow.Total = value;

        //            //if currency needed
        //            //if master amount
        //            if (calRow.Currency != null && calRow.Currency.Currency != "CHF" && calRow.Currency.CurrencyBaseAmountField == "F")
        //            {
        //                if (model.GeneralSetting.Currency.Rate > 0)
        //                {
        //                    calRow.Total = (value * model.GeneralSetting.Currency.Rate);
        //                }
        //            }
        //        }
        //    }
        //}

        //void UpdateRowAmountPercentSpecial(CalculationModel model, CalculationItemModel calRow, decimal value)
        //{
        //    //decimal iBaseAmount = model.MasterAmount;
        //    decimal iBaseAmount = model.CalculationViewItems[0].AmountFix;
        //    if (model.CalculationViewItems[0].Currency.Currency != "CHF")
        //    {
        //        iBaseAmount = model.CalculationViewItems[0].Total;
        //    }

        //    if (calRow.CalculationBaseGroupRows != null)
        //    {
        //        iBaseAmount = GetCalculationBaseSummaryGroups(model, calRow.CalculationBaseGroupRows);
        //    }

        //    //formular for SKT, PV, RBT(maximum input is 99.99 %
        //    // if edit one cell only (SKT or PV or RBT)
        //    // [90 % +10 % from input = 100 %]    [10 % from input]
        //    // 108.131 from (VK(bar))
        //    // (((100 / 90) * 108.131) *          (10 / 100))        = 12.0145555556

        //    //if edit multiple cell (SKT, PV)
        //    // get multiple summary percent first           
        //    // if SKT = 4, PV = 6 so sum is 10
        //    // so if SKT = 1
        //    // [90 % +10 % from input = 100 %]    [10 % from input]
        //    // 108.131 from (VK(bar))
        //    // (((100 / 90) * 108.131) *          (10 / 100))        = 12.0145555556
        //    // SKT = 12.0145555556 * (4/10) = 4.80582222224 (40%)
        //    // PV = 12.0145555556 * (6/10) = 7.20873333336 (60%)

        //    decimal iDiffer = 100 - value;
        //    calRow.AmountPercent = value;
        //    calRow.AmountFix = (((100 / iDiffer) * iBaseAmount) * (value / 100));

        //    if (calRow.Tag == "SKT" || calRow.Tag == "PV")
        //    {
        //        decimal iSummaryPercent = value;
        //        var oCalculationRows = model.CalculationViewItems.FindAll(item => !item.IsSummary && item.Group == calRow.Group);
        //        iSummaryPercent = oCalculationRows.Sum(item => item.AmountPercent);

        //        if (iSummaryPercent > 0 && iSummaryPercent < 100)
        //        {
        //            iDiffer = 100 - iSummaryPercent;
        //            calRow.AmountFix = (((100 / iDiffer) * iBaseAmount) * (iSummaryPercent / 100)) * (value / iSummaryPercent);
        //        }
        //    }

        //    calRow.Total = calRow.AmountFix;
        //}

        //public void UpdateRowAmountPercent(CalculationModel model, CalculationItemModel calRow, decimal value, bool skipBaseGroupRows = false)
        //{
        //    //decimal iBaseAmount = model.MasterAmount;
        //    decimal iBaseAmount = model.CalculationViewItems[0].AmountFix;
        //    if (model.CalculationViewItems[0].Currency.Currency != "CHF")
        //    {
        //        iBaseAmount = model.CalculationViewItems[0].Total;
        //    }

        //    if (!skipBaseGroupRows)
        //    {
        //        //get summary from above rows in gridview
        //        if (calRow.CalculationBaseGroupRows != null)
        //        {
        //            iBaseAmount = GetCalculationBaseSummaryGroups(model, calRow.CalculationBaseGroupRows);
        //        }
        //    }

        //    //original row's fix-amount
        //    decimal iOriginalFixAmount = calRow.AmountFix;

        //    calRow.AmountPercent = value;
        //    calRow.AmountFix = iBaseAmount * (calRow.AmountPercent / 100);
        //    calRow.Total = calRow.AmountFix;

        //    //if currency needed
        //    //calRow.Currency.CurrencyBaseAmountField == "F"
        //    if (calRow.Currency != null && calRow.Currency.CurrencyBaseAmountField == "F")
        //    {
        //        //calRow.Currency.OriginalAmount = calRow.AmountPercent;
        //        if (model.GeneralSetting.Currency.Rate > 0)
        //        {
        //            if (calRow.Currency.Currency != "CHF")
        //            {
        //                //calRow.AmountPercent = calRow.AmountPercent * model.GeneralSetting.Currency.Rate;
        //                calRow.AmountPercent = ((iOriginalFixAmount / iBaseAmount) * 100) * model.GeneralSetting.Currency.Rate;
        //                calRow.AmountFix = iOriginalFixAmount;
        //                calRow.Total = iBaseAmount * (calRow.AmountPercent / 100);
        //            }
        //            else
        //            {
        //                calRow.AmountPercent = (iOriginalFixAmount / iBaseAmount) * 100;
        //                calRow.AmountFix = iOriginalFixAmount;
        //                calRow.Total = calRow.AmountFix;
        //            }
        //        }
        //    }


        //    //if convert needed
        //    //!String.IsNullOrWhiteSpace(calRow.Convert.ConvertAmountField)
        //    //if edit P convert to F            
        //    if (calRow.Convert != null && !String.IsNullOrWhiteSpace(calRow.Convert.ConvertAmountField))
        //    {
        //        if (calRow.Convert.ConvertAmountField == "F")
        //        {
        //            if (calRow.Convert.Unit == "VE" && model.GeneralSetting.Convert.UnitNumber > 0)
        //            {
        //                calRow.AmountFix = calRow.AmountFix / model.GeneralSetting.Convert.UnitNumber;
        //                calRow.Total = calRow.AmountFix;
        //            }
        //        }
        //        else if (calRow.Convert.ConvertAmountField == "P")
        //        {
        //            //if currency with unit
        //            if (calRow.Currency != null && calRow.Currency.CurrencyBaseAmountField == "F")
        //            {
        //                if (calRow.Convert.Unit == "VE" && model.GeneralSetting.Convert.UnitNumber > 0)
        //                {
        //                    calRow.Convert.OriginalAmount = calRow.AmountPercent;

        //                    calRow.AmountPercent = calRow.AmountPercent / model.GeneralSetting.Convert.UnitNumber;
        //                    calRow.Total = iBaseAmount * (calRow.AmountPercent / 100);
        //                }
        //            }
        //            else
        //            {
        //                //only unit
        //                //calling from summary-all functionals
        //                if (calRow.Convert.Unit == "EE")
        //                {
        //                    //calRow.Convert.OriginalAmount = calRow.AmountPercent;
        //                    calRow.AmountPercent = calRow.Convert.OriginalAmount;
        //                    calRow.AmountFix = iBaseAmount * (calRow.AmountPercent / 100);
        //                    calRow.Total = calRow.AmountFix;
        //                }
        //                else if (calRow.Convert.Unit == "VE" && model.GeneralSetting.Convert.UnitNumber > 0)
        //                {
        //                    calRow.AmountPercent = calRow.Convert.OriginalAmount;
        //                    calRow.AmountFix = iBaseAmount * (calRow.AmountPercent / 100);

        //                    calRow.AmountPercent = calRow.AmountPercent / model.GeneralSetting.Convert.UnitNumber;
        //                    calRow.Total = (iBaseAmount * (calRow.AmountPercent / 100));
        //                }
        //            }
        //        }
        //    }
        //}

        //public void UpdateRowAmountFix(CalculationModel model, CalculationItemModel calRow, decimal value, bool skipBaseGroupRows = false)
        //{
        //    //decimal iBaseAmount = model.MasterAmount;
        //    decimal iBaseAmount = model.CalculationViewItems[0].AmountFix;
        //    if (model.CalculationViewItems[0].Currency.Currency != "CHF")
        //    {
        //        iBaseAmount = model.CalculationViewItems[0].Total;
        //    }

        //    if (!skipBaseGroupRows)
        //    {
        //        //get summary from above rows in gridview
        //        if (calRow.CalculationBaseGroupRows != null)
        //        {
        //            iBaseAmount = GetCalculationBaseSummaryGroups(model, calRow.CalculationBaseGroupRows);
        //        }
        //    }

        //    //calculate
        //    calRow.AmountFix = value;
        //    if (iBaseAmount > 0)
        //    {
        //        calRow.AmountPercent = (value / iBaseAmount) * 100;
        //    }
        //    calRow.Total = calRow.AmountFix;


        //    //if convert needed
        //    //if edit F convert to P
        //    if (calRow.Convert != null && !String.IsNullOrWhiteSpace(calRow.Convert.ConvertAmountField))
        //    {
        //        if (calRow.Convert.ConvertAmountField == "P")
        //        {
        //            if (calRow.Convert.Unit == "EE")
        //            {
        //                calRow.Convert.OriginalAmount = calRow.AmountPercent;
        //            }
        //            else if (calRow.Convert.Unit == "VE" && model.GeneralSetting.Convert.UnitNumber > 0)
        //            {
        //                calRow.AmountPercent = calRow.AmountPercent / model.GeneralSetting.Convert.UnitNumber;
        //                calRow.Total = (calRow.AmountPercent / 100) * iBaseAmount;
        //            }
        //        }
        //    }

        //    ////if currency needed
        //    //if (calRow.Currency != null && calRow.Currency.Currency != "CHF" && calRow.Currency.CurrencyBaseAmountField == "F")
        //    //{
        //    //    calRow.Currency.OriginalAmount = calRow.AmountPercent;
        //    //    if (model.GeneralSetting.Currency.Rate > 0)
        //    //    {
        //    //        calRow.AmountPercent = calRow.AmountPercent * model.GeneralSetting.Currency.Rate;
        //    //        calRow.Total = (iBaseAmount * (calRow.AmountPercent / 100));
        //    //    }
        //    //}
        //}
        //decimal GetCalculationBaseSummaryGroups(CalculationModel model, List<int> calculationGroups)
        //{
        //    decimal iSumCalRow = 0;

        //    foreach (int i in calculationGroups)
        //    {
        //        var oCalculationRows = model.CalculationViewItems.FindAll(item => !item.IsSummary && calculationGroups.Contains(item.Group));

        //        if (oCalculationRows != null)
        //        {
        //            iSumCalRow = (from calRow in oCalculationRows select calRow.Total).Sum();
        //        }
        //    }

        //    return iSumCalRow;
        //}
    }
}
