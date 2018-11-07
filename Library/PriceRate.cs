using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library
{
    class PriceRate
    {        
        private Dictionary<int, Price> priceDictionary;

        public PriceRate()
        {
            priceDictionary = new Dictionary<int, Price>();
        }

        public void AddPrice(decimal price, List<Rate> rates)
        {
            priceDictionary.Add((int)price, new Price() { PriceIndex = price, Rates = rates });
        }

        public Price GetPrice(int priceIndex)
        {
            return priceDictionary[priceIndex];
        }
    }
}
