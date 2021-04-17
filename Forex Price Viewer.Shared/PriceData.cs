using System;

namespace Forex_Price_Viewer
{
    public class PriceData
    {
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
    }
}