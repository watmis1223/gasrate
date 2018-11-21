using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CalculationOilPrice.Library.Entity.PriceCalculation.Models
{

    public class ComboboxItemModel
    {
        public int Value { get; set; }
        public string Caption { get; set; }
    }


    #region GeneralSetting Model
    public class GeneralProductDesc
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line4 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line5 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Line6 { get; set; }
    }
    public class GeneralPriceScale
    {
        private decimal _Scale;
        public decimal Scale
        {
            get { return _Scale; }
            set { _Scale = decimal.Round(value, 4); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarkUp { get; set; }

        private decimal _MinProfit;
        public decimal MinProfit
        {
            get { return _MinProfit; }
            set { _MinProfit = decimal.Round(value, 4); }
        }

        private decimal _MaxProfit;
        public decimal MaxProfit
        {
            get { return _MaxProfit; }
            set { _MaxProfit = decimal.Round(value, 4); }
        }
    }
    public class GeneralConvert
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }

        //ShopUnit
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ShopUnit { get; set; }

        //SaleUnit
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SaleUnit { get; set; }

        //ShopUnit
        private decimal _EEUnitNumber;
        public decimal EEUnitNumber
        {
            get { return _EEUnitNumber; }
            set { _EEUnitNumber = decimal.Round(value, 4); }
        }

        //SaleUnit
        private decimal _VEUnitNumber;
        public decimal VEUnitNumber
        {
            get { return _VEUnitNumber; }
            set { _VEUnitNumber = decimal.Round(value, 4); }
        }

        private decimal _UnitNumber;
        public decimal UnitNumber
        {
            get { return _UnitNumber; }
            set { _UnitNumber = decimal.Round(value, 4); }
        }
    }
    public class GeneralCurrency
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        private decimal _Rate;
        public decimal Rate
        {
            get { return _Rate; }
            set { _Rate = decimal.Round(value, 4); }
        }
    }
    public class GeneralSettingModel
    {
        public long ID { get; set; }
        public string CostType { get; set; }
        public List<string> Options { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Supplier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Employee { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreateDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralProductDesc ProductDesc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralPriceScale PriceScale { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralConvert Convert { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralCurrency Currency { get; set; }

        [ScriptIgnore]
        public List<string> TextLines { get; set; }
    }
    #endregion

    #region Calculation
    public class CalculationItemModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sign { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }


        private decimal _AmountPercent;
        public decimal AmountPercent
        {
            get { return _AmountPercent; }
            set { _AmountPercent = decimal.Round(value, 4); }
        }

        private decimal _AmountFix;
        public decimal AmountFix
        {
            get { return _AmountFix; }
            set { _AmountFix = decimal.Round(value, 4); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CurrencyModel Currency { get; set; }

        //keep scale unit
        //EE = original, VE = convert
        //get convert number from GeneralUnit model
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ConvertModel Convert { get; set; }


        private decimal _Total;
        public decimal Total
        {
            get { return _Total; }
            set { _Total = decimal.Round(value, 4); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }
        public int Group { get; set; }
        public int ItemOrder { get; set; }
        public bool IsSummary { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> SummaryGroups { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BaseCalculationGroupRows { get; set; }

        //keep cost calculation
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CostCalculatonGroupModel CostCalculatonGroup { get; set; }

        //keep margin
        private decimal _VariableTotal;
        public decimal VariableTotal
        {
            get { return _VariableTotal; }
            set { _VariableTotal = decimal.Round(value, 4); }
        }

        //P percent, F fix, V variable total, T total
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EditedField { get; set; }

    }

    public class ConvertModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Unit { get; set; }

        private decimal _OriginalAmount;
        public decimal OriginalAmount
        {
            get { return _OriginalAmount; }
            set { _OriginalAmount = decimal.Round(value, 4); }
        }

        [Description("P=Percent, F=Fix, T=Total, S=Special")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String ConvertAmountField { get; set; }

        public override string ToString()
        {
            return Unit;
        }
    }

    public class CostCalculatonGroupModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> SummaryGroups { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BaseCalculationGroupRows { get; set; }
    }

    public class CurrencyModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        private decimal _OriginalAmount;
        public decimal OriginalAmount
        {
            get { return _OriginalAmount; }
            set { _OriginalAmount = decimal.Round(value, 4); }
        }

        [Description("P=Percent, F=Fix, T=Total, S=Special")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String CurrencyBaseAmountField { get; set; }
        public override string ToString()
        {
            return Currency;
        }
    }

    //main model
    public class CalculationModel
    {
        //database row id
        public long ID { get; set; }

        //from general's settings
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneralSettingModel GeneralSetting { get; set; }

        ////from module's settings
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public PriceSetting PriceSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CalculationNoteModel> CalculationNotes { get; set; }

        //basic calculation tempt view, not serialize to json
        [ScriptIgnore]
        public List<CalculationItemModel> CalculationViewItems { get; set; }

        //keep margin
        [ScriptIgnore]
        public List<CalculationItemModel> CalculationMarginViewItems { get; set; }        
    }

    //calculation note
    public class CalculationNoteModel
    {
        //database ros id
        public long ID { get; set; }

        //basic calculation
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CalculationItemModel> CalculationItems { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CalculationItemModel> CalculationMarginItems { get; set; }

        //keep scale unit number
        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set { _Quantity = decimal.Round(value, 4); }
        }
    }
    #endregion
}
