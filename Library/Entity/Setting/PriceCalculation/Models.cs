using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models
{

    public class PriceScaleComboboxItem
    {
        public int Value { get; set; }
        public string Caption { get; set; }
    }


    #region GeneralSetting Model
    public class GeneralProductDesc
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
    }
    public class GeneralPriceScale
    {
        public decimal Scale { get; set; }
        public string MarkUp { get; set; }
        public decimal MinProfit { get; set; }
        public decimal MaxProfit { get; set; }
    }
    public class GeneralConvert
    {
        public string Mode { get; set; }

        //ShopUnit
        public string ShopUnit { get; set; }

        //SaleUnit
        public string SaleUnit { get; set; }

        //ShopUnit
        public decimal EEUnitNumber { get; set; }

        //SaleUnit
        public decimal VEUnitNumber { get; set; }

        public decimal UnitNumber { get; set; }
    }
    public class GeneralCurrency
    {
        public string Mode { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
    public class GeneralSettingModel
    {
        public long ID { get; set; }
        public string CostType { get; set; }
        public List<string> Options { get; set; }
        public string Remark { get; set; }
        public string Supplier { get; set; }
        public string Info { get; set; }
        public string Employee { get; set; }
        public string CreateDate { get; set; }
        public GeneralProductDesc ProductDesc { get; set; }
        public GeneralPriceScale PriceScale { get; set; }
        public GeneralConvert Convert { get; set; }
        public GeneralCurrency Currency { get; set; }
        public List<string> TextLines { get; set; }
    }
    #endregion

    #region Calculation
    public class CalculationItemModel
    {
        public string Sign { get; set; }
        public string Description { get; set; }
        public decimal AmountPercent { get; set; }
        public decimal AmountFix { get; set; }
        public CurrencyModel Currency { get; set; }

        //keep scale unit
        //EE = original, VE = convert
        //get convert number from GeneralUnit model
        public ConvertModel Convert { get; set; }

        public decimal Total { get; set; }
        public string Tag { get; set; }
        public int Group { get; set; }
        public int Order { get; set; }
        public bool IsSummary { get; set; }
        public List<int> SummaryGroups { get; set; }
        public List<int> CalculationBaseGroupRows { get; set; }

        //keep margin
        public decimal VariableTotal { get; set; }

        //P percent, F fix, V variable total, T total
        public string EditedField { get; set; }

    }

    public class ConvertModel
    {
        public string Unit { get; set; }
        public decimal OriginalAmount { get; set; }

        [Description("P=Percent, F=Fix, T=Total, S=Special")]
        public String ConvertAmountField { get; set; }

        public override string ToString()
        {
            return Unit;
        }
    }

    public class CurrencyModel
    {
        public string Currency { get; set; }
        public decimal OriginalAmount { get; set; }

        [Description("P=Percent, F=Fix, T=Total, S=Special")]
        public String CurrencyBaseAmountField { get; set; }
        public override string ToString()
        {
            return Currency;
        }
    }

    //main model
    public class CalculationModel
    {
        //database ros id
        public long ID { get; set; }

        ////from calculation basic one
        //public decimal MasterAmount { get; set; }

        //from general's settings
        public GeneralSettingModel GeneralSetting { get; set; }

        //from module's settings
        public PriceSetting PriceSetting { get; set; }


        public List<CalculationNoteModel> CalculationNotes { get; set; }

        //basic calculation tempt view, not serialize to json
        public List<CalculationItemModel> CalculationViewItems { get; set; }

        public List<CalculationItemModel> CalculationMarginViewItems { get; set; }
        
        ////if scale more than 1
        //public List<CalculationScaleModel> ScaleCalculationItems { get; set; }
    }

    //calculation note
    public class CalculationNoteModel
    {
        //database ros id
        public long ID { get; set; }        

        //basic calculation
        public List<CalculationItemModel> CalculationItems { get; set; }

        public List<CalculationItemModel> CalculationMarginItems { get; set; }

        //keep scale unit number
        public decimal Quantity { get; set; }
    }

    //public class CalculationScaleModel
    //{
    //    public long ID { get; set; }
    //    public decimal Scale { get; set; }
    //    public List<CalculationItemModel> CalculationItems { get; set; }
    //}

    #endregion
}
