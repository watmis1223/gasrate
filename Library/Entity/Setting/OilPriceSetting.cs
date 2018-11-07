using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CalculationOilPrice.Library.Entity.Setting
{
    [Serializable]
    internal class OilPriceSetting
    {
        public decimal ExtraLightOilUSPrice;
        public decimal EcoOilUSPrice;
        public decimal CurrentCurrencyRate;
        public decimal FixCostForUnloadPlace;
        public decimal DensityOfHeatOil;
        public decimal CommissionFactor;

        [OptionalField]
        public bool IsFixCHFPrices;
        
        [OptionalField]
        public decimal FixExtraLightOilCHFPrice;
        
        [OptionalField]
        public decimal FixEcoOilCHFPrice;
    }
}
