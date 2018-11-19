using CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Business.PriceCalculation
{
    public interface ICalculation
    {
        string GetCalculationRowUnitValue(CalculationModel model, int rowID);
        string GetCalculationRowCurrencyValue(CalculationModel model, int rowID);
        string GetCalculationRowCurrencyFieldEditable(CalculationModel model, int rowID);
        void UpdateCalculationRowUnit(CalculationModel model, int rowID, string unit);
        void UpdateCalculationRowCurrency(CalculationModel model, int rowID, string currency);
        void UpdateCalculationRowCurrencyField(CalculationModel model, int rowID, string field);
        void UpdateGroupAmountAll(CalculationModel model, bool updateGroupOnly);
        void UpdateCalculationRowAmount(CalculationModel model, int rowID, decimal value, bool isPercent, bool specialCalculation, bool isCellEdit);
        void UpdateRowAmountPercent(CalculationModel model, CalculationItemModel calRow, decimal value, bool skipBaseGroupRows = false);
        void UpdateRowAmountFix(CalculationModel model, CalculationItemModel calRow, decimal value, bool skipBaseGroupRows = false);

    }
}
