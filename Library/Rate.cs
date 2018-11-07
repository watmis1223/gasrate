using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library
{
    class Rate
    {
        private Int16 percentRate;
        private float multiplyRate;

        public Int16 PercentRate
        {
            get { return percentRate; }
            set { percentRate = value; }
        }

        public float MultiplyRate
        {
            get { return multiplyRate; }
            set { multiplyRate = value; }
        }
    }
}
