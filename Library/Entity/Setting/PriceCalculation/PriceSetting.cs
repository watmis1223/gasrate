using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CalculationOilPrice.Library.Entity.Setting.PriceCalculation
{
    [Serializable]
    public class PriceSetting
    {
        public decimal CashDiscount;
        public decimal SalesBonus;
        public decimal CustomerDiscount;
        public decimal VatTaxes;

        //[OptionalField]
        //public bool IsFixCHFPrices;
        
        //[OptionalField]
        //public decimal FixExtraLightOilCHFPrice;
        
        //[OptionalField]
        //public decimal FixEcoOilCHFPrice;
    }
}
