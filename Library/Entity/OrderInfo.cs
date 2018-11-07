using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CalculationOilPrice.Library.Entity
{
    class OrderInfo
    {
        Guid orderInfoID;
        Order currentOrder;
        DataTable orderInfoDt;

        public OrderInfo(Order order)
        {
            orderInfoDt = new DataTable();
            currentOrder = order;
            orderInfoID = Guid.NewGuid();            

            DataColumn dc = new DataColumn("0");
            dc.DefaultValue = 1;
            orderInfoDt = Storage.StorageOperator.LoadTable("OrderInfo", null, new DataColumn[] { dc }, null);

            DataRow dr = orderInfoDt.NewRow();
            dr["OrderID"] = currentOrder.OrderID;
            dr["OrderInfo"] = getOrderInfo(currentOrder);            
            orderInfoDt.Rows.Add(dr);
        }

        string getOrderInfo(Order order)
        {
            StringBuilder orderInfo = new StringBuilder();
            
            orderInfo.AppendFormat(" {0},", order.Commission);
            orderInfo.AppendFormat(" {0},", order.CommissionFactor);
            orderInfo.AppendFormat(" {0},", order.CustomerAddress);
            orderInfo.AppendFormat(" {0},", order.CustomerName);
            orderInfo.AppendFormat(" {0},", order.CustomerPlace);
            orderInfo.AppendFormat(" {0},", order.CustomerSurname);
            orderInfo.AppendFormat(" {0},", order.CustomerZipcode);
            orderInfo.AppendFormat(" {0},", order.OilPrice);
            orderInfo.AppendFormat(" {0},", order.OilQuality);
            orderInfo.AppendFormat(" {0},", order.OilQualityName);
            orderInfo.AppendFormat(" {0},", order.OilQuantity);
            orderInfo.AppendFormat(" {0},", order.OrderDate);

            orderInfo.AppendFormat(" {0},", order.OrderID);
            orderInfo.AppendFormat(" {0},", order.OrderNumber);
            orderInfo.AppendFormat(" {0},", order.PartOfWIR);
            orderInfo.AppendFormat(" {0},", order.Total1Price);
            orderInfo.AppendFormat(" {0},", order.Total1WIRPrice);
            orderInfo.AppendFormat(" {0},", order.Total2CashPrice);
            orderInfo.AppendFormat(" {0},", order.Total2Price);
            orderInfo.AppendFormat(" {0},", order.TotalPricePer100KG);
            orderInfo.AppendFormat(" {0},", order.UnloadCost);

            orderInfo.AppendFormat(" {0},", order.UnloadPlace);
            orderInfo.AppendFormat(" {0},", order.WIRFactor);
            orderInfo.AppendFormat(" {0}", order.GetReportName());

            return orderInfo.ToString();
        }

        public void SaveOrderInfo()
        {
            Storage.StorageOperator.SaveTable(orderInfoDt, orderInfoDt.Columns["OrderID"], null);
        }
    }
}
