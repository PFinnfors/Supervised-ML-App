using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using BayesApp.Models;
using System;

namespace BayesApp.Controllers
{
    public class WeatherController : Controller
    {
        public async Task<string> Get()
        {
            //deserialize JSON directly from a file
            WeatherModel model = JsonConvert.DeserializeObject<WeatherModel>
                (System.IO.File.ReadAllText(@"C:\DarkSkyAPI\key.json"));
            
            WeatherDate wD = new WeatherDate();

            DateTime dt = new DateTime();
            dt = DateTime.UtcNow;
            dt = dt.AddDays(-13);

            for (int i = 0; i < 0; i++)
            {
                dt = dt.AddDays(-1);
            }
            
            //Get Weather Date
            wD = GetDate(dt, wD);

            using (var client = new HttpClient())
            {
                //
                HttpResponseMessage response = await client.GetAsync(
                    $"https://api.darksky.net/forecast/{model.key}/" +
                    $"57.782614,14.161788,{wD.Year}-{wD.Month}-{wD.Day}T{wD.Hour}:{wD.Minute}:{wD.Second}" +
                    $"?units=si" +
                    $"&exclude=hourly,daily,flags");

                if (response.IsSuccessStatusCode)
                {
                    string dataStr = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(dataStr);
                }
                else
                {
                    var status = response.StatusCode;
                    var phrase = response.ReasonPhrase;
                    var msg = response.RequestMessage;
                }
            }

            return null;
        }

        //
        public WeatherDate GetDate(DateTime dTime, WeatherDate wDate)
        {
            wDate.Year = dTime.Year.ToString();

            if (dTime.Month.ToString().Length == 2) wDate.Month = dTime.Month.ToString();
            else wDate.Month = "0" + dTime.Month.ToString();

            if (dTime.Day.ToString().Length == 2) wDate.Day = dTime.Day.ToString();
            else wDate.Day = "0" + dTime.Day.ToString();

            if (dTime.Hour.ToString().Length == 2) wDate.Hour = dTime.Hour.ToString();
            else wDate.Hour = "0" + dTime.Hour.ToString();

            if (dTime.Minute.ToString().Length == 2) wDate.Minute = dTime.Minute.ToString();
            else wDate.Minute = "0" + dTime.Minute.ToString();

            if (dTime.Second.ToString().Length == 2) wDate.Second = dTime.Second.ToString();
            else wDate.Second = "0" + dTime.Second.ToString();

            return wDate;
        }

        //
        public struct WeatherDate
        {
            public string Year;
            public string Month;
            public string Day;
            public string Hour;
            public string Minute;
            public string Second;
        }


    }
}