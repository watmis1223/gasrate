using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CalculationOilPrice.Library.Entity
{
    public class Order
    {
        public Guid OrderID;
        public string OrderTime;
        public string OrderDate;
        public string OrderDatePrint;
        public int OilQuality;
        public decimal OilPrice;
        public decimal OilQuantity;
        public decimal UnloadPlace;
        public decimal UnloadCost;
        public decimal PartOfWIR;
        public decimal WIRFactor;        
        public string CustomerName;
        public string CustomerSurname;
        public string CustomerAddress;
        public string CustomerZipcode;
        public string CustomerPlace;
        public string OilQualityName;

        ///below are field for order detail
        public decimal TotalPricePer100KG;
        public decimal Total1Price;
        public decimal Total2Price;
        public decimal Total1WIRPrice;
        public decimal Total2CashPrice;
        public decimal AdditionalCosts;


        public decimal VpPer100KgWIRPrice;

        public decimal CommissionFactor;        

        public int UnloadPlaceNumber;

        public DataTable OrderDetailDt;

        public Commission Commission;

        public int OrderNumber;
        string ReportName;

        public Order()
        {
            OrderID = new Guid();
            SetOrderDateTimeInfo(DateTime.Now);
            
            OilQuality = 0;
            OilPrice = 0;
            OilQuantity = 0;
            UnloadPlace = 0;
            UnloadCost = 0;
            PartOfWIR = 0;
            WIRFactor = 0;
            CustomerName = string.Empty;
            CustomerSurname = string.Empty;
            CustomerAddress = string.Empty;
            CustomerZipcode = string.Empty;
            CustomerPlace = string.Empty;
            OilQualityName = string.Empty;

            TotalPricePer100KG = 0;
            Total1Price = 0;
            Total2Price = 0;
            Total1WIRPrice = 0;
            Total2CashPrice = 0;
            AdditionalCosts = 0;

            VpPer100KgWIRPrice = 0;

            CommissionFactor = 0;
            UnloadPlaceNumber = 0;

            OrderDetailDt = new DataTable();
            Commission = new Commission();

            OrderNumber = -1;
            //ReportName = string.Format("Kalkulation unknow {0}-{1}", OrderDatePrint, OrderTime);
        }

        public void SetOrderDateTimeInfo(DateTime dateTime)
        {
            OrderDate = string.Format(dateTime.ToString("dd/MM/yyyy"));
            OrderTime = string.Format(dateTime.ToString("HH.mm.ss"));
            OrderDatePrint = string.Format(dateTime.ToString("yyyy.MM.dd"));            
        }

        public string GetReportName()
        {
            ReportName = string.Format("Kalkulation {0} - {1}.{2} - {3} {4} - {5}",
                new object[]{OrderNumber, OrderDatePrint, OrderTime
                    , CustomerName, CustomerSurname, OrderID.ToString().ToUpper()});

            //ReportName = string.Format("Kalkulation {0} - {1} - {2} {3} - {4}",
            //    new object[]{OrderNumber, OrderDatePrint, CustomerName, CustomerSurname, OrderID.ToString().ToUpper()});

            return ReportName;
        }
    }
}
