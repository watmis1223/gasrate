using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Entity
{
    public class Commission
    {        
        public decimal CalculationSalesPriceAmount;
        public decimal CalculationSalesPricePercent;        
        public decimal WIRCorrection;
        public decimal WIRCorrectionAmount;
        public decimal CommissionLegitimate;
        public decimal CommissionLegitimateAmount;
        public decimal CommissionLegitimatePercent;
        public decimal TotalCommissionPercent;
        public decimal TotalCommissionAmount;

        public Commission()
        {
            CalculationSalesPriceAmount = 0;
            CalculationSalesPricePercent = 0;
            WIRCorrection = 0;
            WIRCorrectionAmount = 0;
            CommissionLegitimate = 0;
            CommissionLegitimateAmount = 0;
            CommissionLegitimatePercent = 0;
            TotalCommissionPercent = 0;
            TotalCommissionAmount = 0;
        }
    }
}
