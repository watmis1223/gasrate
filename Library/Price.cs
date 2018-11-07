using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library
{
    class Price
    {
        private decimal priceIndex;
        private List<Rate> rates;

        public decimal PriceIndex
        {
            get { return priceIndex; }
            set { priceIndex = value; }
        }

        public List<Rate> Rates
        {
            get { return rates; }
            set { rates = value; }
        }
    }
}
