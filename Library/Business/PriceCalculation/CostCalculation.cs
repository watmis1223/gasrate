using CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Business.PriceCalculation
{
    public class CostCalculation : ICalculation
    {
        public string GetCalculationRowUnitValue(CalculationModel model, int rowID)
        {
            throw new NotImplementedException("Not support");
        }

        public string GetCalculationRowCurrencyValue(CalculationModel model, int rowID)
        {
            throw new NotImplementedException("Not support");
        }

        public string GetCalculationRowCurrencyFieldEditable(CalculationModel model, int rowID)
        {
            throw new NotImplementedException("Not support");
        }

        public void UpdateCalculationRowUnit(CalculationModel model, int rowID, string unit)
        {
        }

        public void UpdateCalculationRowCurrency(CalculationModel model, int rowID, string currency)
        {
        }

        public void UpdateCalculationRowCurrencyField(CalculationModel model, int rowID, string field)
        {
        }

        public void UpdateGroupAmountAll(CalculationModel model, bool updateGroupOnly)
        {
            var oModels = model.CalculationViewItems.FindAll(item => item.CostCalculatonGroup != null);

            //reverse group
            for (int i = oModels.Count(); i > 0; i--)
            {
                UpdateGroupAmount(model, oModels[i - 1], updateGroupOnly);
            }
        }

        void UpdateGroupAmount(CalculationModel model, CalculationItemModel groupRow, bool updateGroupOnly)
        {
            //CalculationItemModel oGroup = model.CalculationViewItems.Find(item => item.Group == group && item.Order == groupID && item.IsSummary);

            if (groupRow != null && groupRow.CostCalculatonGroup != null)
            {
                //get calculation items from group below (as see in gridview)
                //get items per group       

                decimal iItemSummary = 0;
                if (groupRow.CostCalculatonGroup.BaseCalculationGroupRows.Count > 0)
                {
                    var oModels = model.CalculationViewItems.FindAll(
                        item => !item.IsSummary &&
                        groupRow.CostCalculatonGroup.BaseCalculationGroupRows.Contains(item.Group));

                    foreach (CalculationItemModel item in oModels)
                    {
                        bool isSpecial = false;
                        if (item.Group == 0)
                        {
                            UpdateCalculationRowAmount(model, item.Order, item.AmountFix, false, isSpecial, false);
                        }
                        else
                        {
                            //if special row
                            if (item.Tag == "SKT" || item.Tag == "PV" || item.Tag == "RBT")
                            {
                                isSpecial = true;
                            }

                            if (item.EditedField == "P" || String.IsNullOrWhiteSpace(item.EditedField))
                            {
                                UpdateCalculationRowAmount(model, item.Order, item.AmountPercent, true, isSpecial, false);
                            }
                            else
                            {
                                UpdateCalculationRowAmount(model, item.Order, item.AmountFix, false, isSpecial, false);
                            }
                        }
                    }

                    //update group's total
                    iItemSummary = (from row in oModels select row.Total).Sum();
                }

                decimal iItemGroupSummary = 0;
                if (groupRow.CostCalculatonGroup.SummaryGroups.Count > 0)
                {
                    iItemGroupSummary = (from row in model.CalculationViewItems
                                         where
                                         row.IsSummary &&
                                         groupRow.CostCalculatonGroup.SummaryGroups.Contains(row.Group)
                                         select row.Total).Sum();

                    //update group's total only
                    groupRow.Total = iItemGroupSummary - iItemSummary;
                }
                else
                {
                    //if no summary group like VVK
                    //update group's total only
                    groupRow.Total = iItemSummary;
                }
            }
        }

        public void UpdateCalculationRowAmount(CalculationModel model, int rowID, decimal value, bool isPercent, bool specialCalculation, bool isCellEdit)
        {
            CalculationItemModel oCalRow = model.CalculationViewItems[rowID];

            //set edited column
            if (oCalRow != null)
            {
                oCalRow.EditedField = isPercent ? "P" : "F";
            }

            UpdateCalculationRowAmount(model, oCalRow, value, isPercent, specialCalculation);
        }

        void UpdateCalculationRowAmount(CalculationModel model, CalculationItemModel calRow, decimal value, bool isPercent, bool specialCalculation)
        {
            if (calRow != null)
            {
                //if not master amount row
                //master row's is VK(brutto)
                if (calRow.Tag != "VK(brutto)")
                {
                    if (isPercent)
                    {
                        if (specialCalculation)
                        {
                            UpdateRowAmountPercentSpecial(model, calRow, value);
                        }
                        else
                        {
                            UpdateRowAmountPercent(model, calRow, value);
                        }
                    }
                    else
                    {
                        if (specialCalculation)
                        {
                            UpdateRowAmountFixSpecial(model, calRow, value);
                        }
                        else
                        {
                            UpdateRowAmountFix(model, calRow, value);
                        }
                    }
                }
                else
                {
                    //set row's total amount
                    //master row
                    calRow.Total = value;
                }
            }
        }

        void UpdateRowAmountPercentSpecial(CalculationModel model, CalculationItemModel calRow, decimal value)
        {
            //decimal iBaseAmount = model.MasterAmount;
            decimal iBaseAmount = model.CalculationViewItems.Where(item => item.Group == calRow.Group && item.IsSummary).LastOrDefault().Total;

            calRow.AmountPercent = value;
            calRow.AmountFix = (value / 100) * iBaseAmount;
            calRow.Total = calRow.AmountFix;
        }

        void UpdateRowAmountFixSpecial(CalculationModel model, CalculationItemModel calRow, decimal value)
        {
            //decimal iBaseAmount = model.MasterAmount;
            decimal iBaseAmount = model.CalculationViewItems.Where(item => item.Group == calRow.Group && item.IsSummary).LastOrDefault().Total;

            calRow.AmountFix = value;
            if (iBaseAmount > 0)
            {
                //if special
                //(x / (99) * 100) = y
                //x is value
                //99 is base amount
                //y is resut (amount percent0
                calRow.AmountPercent = (value / (iBaseAmount)) * 100;
            }
            calRow.Total = calRow.AmountFix;
        }

        public void UpdateRowAmountPercent(CalculationModel model, CalculationItemModel calRow, decimal value, bool skipBaseGroupRows = false)
        {
            //decimal iBaseAmount = model.MasterAmount;
            decimal iBaseAmount = model.CalculationViewItems.Where(item => item.Group == calRow.Group && item.IsSummary).LastOrDefault().Total;

            //x*100 + 0.01*100 = 9900
            //x is value
            //100 is fix number (from 100 percent)
            //0.01 is x as percentage (eg 1/100)
            //9900 is base amount

            //101x = 9900
            //x = 9900 / 101 = 98.0198
            if (value > 0)
            {
                //must set value first
                calRow.AmountPercent = value;

                //get summary from all item by particular group                
                decimal iSummaryAllItems = model.CalculationViewItems.FindAll(item => item.Group == calRow.Group && !item.IsSummary).Sum(item => item.AmountPercent);

                //calculation
                decimal iCalculation = (iBaseAmount * 100) / (100 + iSummaryAllItems) * (value / 100);
                calRow.AmountFix = iCalculation;
                calRow.Total = iCalculation;
            }
        }

        public void UpdateRowAmountFix(CalculationModel model, CalculationItemModel calRow, decimal value, bool skipBaseGroupRows = false)
        {
            //decimal iBaseAmount = model.MasterAmount;
            decimal iBaseAmount = model.CalculationViewItems.Where(item => item.Group == calRow.Group && item.IsSummary).LastOrDefault().Total;

            //if (!skipBaseGroupRows)
            //{
            //    //get summary from group's total row
            //    iBaseAmount = GetCalculationBaseSummaryGroups(model, calRow);
            //}

            //calculate
            //must set value first
            calRow.AmountFix = value;
            if (iBaseAmount > 0)
            {
                //get summary from all item by particular group                
                decimal iSummaryAllItems = model.CalculationViewItems.FindAll(item => item.Group == calRow.Group && !item.IsSummary).Sum(item => item.AmountFix);

                //(x / (99 - x) * 100) = y
                //x is value
                //99 is base amount
                //y is resut (amount percent0
                calRow.AmountPercent = (value / (iBaseAmount - iSummaryAllItems)) * 100;
                //calRow.AmountPercent = (value / (iBaseAmount - value)) * 100;

            }
            calRow.Total = calRow.AmountFix;
        }

        decimal GetCalculationBaseSummaryGroups(CalculationModel model, CalculationItemModel calRow)
        {
            decimal iSumCalRow = 0;

            var oCalculationRows = model.CalculationViewItems.FindAll(item => item.IsSummary && item.Group == calRow.Group);

            if (oCalculationRows != null)
            {
                iSumCalRow = (from row in oCalculationRows select row.Total).Sum();
            }

            return iSumCalRow;
        }
    }
}
