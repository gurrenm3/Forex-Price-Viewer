using System;
using System.Diagnostics;
using System.Linq;

namespace Forex_Price_Viewer
{
    public class PriceData
    {
        public string Ticker { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        //public DateTime Date { get; set; }
        //public int Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }

        public PriceData()
        {

        }

        public PriceData(string priceInfo)
        {
            var split = priceInfo.Split(',');
            Ticker = split[0];
            Date = split[1];
            Time = split[2];
            Open = Double.Parse(split[3]);
            High = Double.Parse(split[4]);
            Low = Double.Parse(split[5]);
            Close = Double.Parse(split[6]);
            
        }

        /*public PriceData(DateTime dateTime)
        {
            Date = dateTime;
        }

        public PriceData(DateTime dateTime, string priceInfo) : this(dateTime)
        {
            
        }*/

        public PriceData CreateFromPriceData(string line)
        {
            return null;
        }


        public override string ToString()
        {
            return $"{Ticker},{Date},{Time},{Open},{High},{Low},{Close}";
        }
    }
}