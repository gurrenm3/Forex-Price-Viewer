using System;

namespace Forex_Price_Viewer.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToUrlFileName(this DateTime dt)
        {
            return $"{dt.Year}/" +
                $"{dt.Month.ToString().PadLeft(2, '0')}/" +
                $"{dt.Day.ToString().PadLeft(2, '0')}" +
                $"{dt.Month.ToString().PadLeft(2, '0')}" +
                $"{dt.Year.ToString().Substring(2, 2)}";
        }
    }
}