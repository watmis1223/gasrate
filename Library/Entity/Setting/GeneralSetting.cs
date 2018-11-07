using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Entity.Setting
{
    [Serializable]
    class GeneralSetting
    {                
        public OilPriceSetting OilPriceSetting;
        public ReportPathSetting ReportPathSetting;
        public int CalculationCounter;

        public GeneralSetting()
        {
            OilPriceSetting = new OilPriceSetting();
            ReportPathSetting = new ReportPathSetting();
            CalculationCounter = 0;
        } 
    }
}
