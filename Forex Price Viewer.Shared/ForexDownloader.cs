using Forex_Price_Viewer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Forex_Price_Viewer
{
    public class ForexDownloader
    {
        public const string baseUrl = "https://www.forexite.com/free_forex_quotes/";
        public static string downloadPath = $"{Environment.CurrentDirectory}\\price data";

        public ForexDownloader()
        {
            Directory.CreateDirectory(downloadPath);
        }


        public string GetFilePath(DateTime dateTime) => $"{downloadPath}\\{dateTime.ToZipFileName()}";

        public bool IsDataAlreadyDownloaded(DateTime dateTime) => File.Exists(GetFilePath(dateTime));


        public async Task DownloadPriceData(DateTime dateTime)
        {
            var url = $"{baseUrl}{dateTime.ToUrlFileName()}.zip";

            using (WebClient wc = new WebClient())
            {
                await wc.DownloadFileTaskAsync(new Uri(url), GetFilePath(dateTime));
            }
        }

        public async Task<PriceData> GetPriceData(DateTime dateTime)
        {
            if (!IsDataAlreadyDownloaded(dateTime))
            {
                await DownloadPriceData(dateTime);
                if (!IsDataAlreadyDownloaded(dateTime))
                {
                    throw new Exception("Failed to get price data for some reason");
                }
            }

            var text = await GetTextFromZipFile(GetFilePath(dateTime));

            var prices = GetPriceDataFromTime(dateTime, text);

            return null;
        }

        public async Task<string> GetAllPriceData(DateTime dateTime)
        {
            if (!IsDataAlreadyDownloaded(dateTime))
            {
                await DownloadPriceData(dateTime);
                if (!IsDataAlreadyDownloaded(dateTime))
                {
                    throw new Exception("Failed to get price data for some reason");
                }
            }

            return await GetTextFromZipFile(GetFilePath(dateTime));
        }




        public async Task<string> GetTextFromZipFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            using (ZipArchive zip = ZipFile.Open(filePath, ZipArchiveMode.Read))
            {
                using (StreamReader reader = new StreamReader(zip.Entries[0].Open()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public List<PriceData> GetPriceDataFromTime(DateTime dateTime, string allPriceDataForThisDay)
        {
            var lines = allPriceDataForThisDay.Split('\n');
            
            var newTime = $"{dateTime.Hour.ToString().PadLeft(2, '0')}{dateTime.Minute.ToString().PadLeft(2, '0')}00";
            var priceLines = lines.Where(l => l.Contains(newTime.ToString().Replace(":", "")));


            var results = new List<PriceData>();
            foreach (var line in priceLines)
            {
                results.Add(new PriceData(line));
            }

            return results;
        }
    }
}
