using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.Global
{
    public class OilOrderDetailSummaryColumnCategory
    {
        public string Key { get; set; }
        public string Mask { get; set; }

        public static OilOrderDetailSummaryColumnCategory Col1Row1
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col1Row1", "Total Zuschläge = {0:n2}"); }
        }
        public static OilOrderDetailSummaryColumnCategory Col1Row2
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col1Row2", "Abladestellen = {0:n2}"); }
        }
        public static OilOrderDetailSummaryColumnCategory Col1Row3
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col1Row3", "VP für 100 kg in WIR = {0:n2}"); }
        }
        public static OilOrderDetailSummaryColumnCategory Col1Row4
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col1Row4", "VP für 100 kg in CHF = {0:n2}"); }
        }

        public static OilOrderDetailSummaryColumnCategory Col2Row1
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col2Row1", "Total = {0:n2}"); }
        }
        public static OilOrderDetailSummaryColumnCategory Col2Row2
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col2Row2", "Average = {0:n2}"); }
        }
        public static OilOrderDetailSummaryColumnCategory Col2Row3
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col2Row3", "Subtotal CHF = {0:n2}"); }
        }
        public static OilOrderDetailSummaryColumnCategory Col2Row4
        {
            get { return new OilOrderDetailSummaryColumnCategory("Col2Row4", "Subtotal WIR = {0:n2}"); }
        }
        private OilOrderDetailSummaryColumnCategory(string key, string mask)
        {
            Key = key;
            Mask = mask;
        }
    }
}
