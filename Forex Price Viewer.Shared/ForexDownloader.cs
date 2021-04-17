using Forex_Price_Viewer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Forex_Price_Viewer.Shared
{
    public class ForexDownloader
    {
        public const string baseUrl = "https://www.forexite.com/free_forex_quotes/";

        public void Test()
        {
            var date = DateTime.Today.AddDays(-1).ToUrlFileName();
            //var date = DateToUrlFileName(DateTime.Now.AddHours(-1));

            /*string urlDt = DateToUrlFileName(DateTime.Today);
            string url = $"https://www.forexite.com/free_forex_quotes/{urlDt}.zip";*/

            
            Process.Start($"{baseUrl}{date}.zip");
        }
    }
}
