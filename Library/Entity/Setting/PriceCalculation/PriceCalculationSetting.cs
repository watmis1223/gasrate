using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Entity.Setting.PriceCalculation
{
    [Serializable]
    public class PriceCalculationSetting
    {                
        public PriceSetting PriceSetting;
        public TextSetting TextSetting;
        public ReportPathSetting ReportPathSetting;
        public int CalculationCounter;
        public string ProffixConnection;

        public PriceCalculationSetting()
        {
            PriceSetting = new PriceSetting();
            TextSetting = new TextSetting();
            ReportPathSetting = new ReportPathSetting();
            CalculationCounter = 0;
            ProffixConnection = "";
        } 
    }
}
