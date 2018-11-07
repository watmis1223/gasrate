using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models
{
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
    public class GeneralUnit
    {
        public string Mode { get; set; }
        public string ShopUnit { get; set; }
        public string SaleUnit { get; set; }
        public int UnitNumber { get; set; }
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
        public GeneralUnit Unit { get; set; }
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
        public string Currency { get; set; }
        public decimal Total { get; set; }
        public string Tag { get; set; }
        public int Group { get; set; }
        //public bool IsSummaryGroup { get; set; }
        //public bool IsSummaryGroupPlusTotalAmount { get; set; }
        public int Order { get; set; }
        public bool IsSummary { get; set; }
        public List<int> SummaryGroups { get; set; }
        public List<int> CalculationBaseGroupRows { get; set; }
    }
    public class CalculationModel
    {
        public long ID { get; set; }
        public decimal MasterAmount { get; set; }
        public List<CalculationItemModel> CalculationItems { get; set; }
        public GeneralSettingModel GeneralSetting { get; set; }
        public PriceSetting PriceSetting { get; set; }

        //if scale more than 1
        public List<CalculationScaleModel> CalculationScales { get; set; }
    }
    public class CalculationScaleModel
    {
        public decimal Scale { get; set; }
        public List<CalculationItemModel> CalculationItems { get; set; }
    }

    #endregion
}
