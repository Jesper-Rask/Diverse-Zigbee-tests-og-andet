using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strømpriser
{
    public static class DataProcessor
    {
        private static string urlPart1 = "https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start=";
        private static string urlPart2 = "T00:00&end=";
        private static string urlPart3 = "T00:00&filter=%7B%22PriceArea%22:[%22DK1%22]%7D&sort=HourUTC%20ASC&timezone=dk";
        private static string url = "https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start=2022-12-10T00:00&end=2022-12-12T00:00&filter=%7B%22PriceArea%22:[%22DK1%22]%7D&sort=HourUTC%20ASC&timezone=dk";
        public static async Task<StrømData> LoadData()
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            string tomorrow = date.AddDays(2).ToString("yyyy-MM-dd");
            string today = date.AddDays(0).ToString("yyyy-MM-dd");
            string yesterday = date.AddDays(-1).ToString("yyyy-MM-dd");

            url = urlPart1 + today + urlPart2 + tomorrow + urlPart3;

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    StrømData data = await response.Content.ReadAsAsync<StrømData>();
                    return data;
                }
                else
                {
                    return new StrømData();
                }
            }
        }
    }
}
