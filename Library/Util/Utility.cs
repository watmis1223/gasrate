using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CalculationOilPrice.Library.Util
{
    public class Utility
    {
        public static string ObjectToJson(object obj)
        {
            string sJson = string.Empty;
            try
            {
                if (obj != null)
                {
                    sJson = new JavaScriptSerializer().Serialize(obj);
                }
            }
            catch { }

            return sJson;
        }

        public static T JsonToObject<T>(string json, Type type)
        {
            string sJson = string.Empty;
            JavaScriptSerializer javaConvert = new JavaScriptSerializer()
            {
                MaxJsonLength = 2147483644
            };
            try
            {
                return (T)Convert.ChangeType(javaConvert.Deserialize<T>(json), type);
            }
            catch //(Exception ex)
            {
                //Logger.writeLog("JsonToObject<T> (ERROR): " + ex.Message);
            }

            return default(T);
        }        
    }
}
